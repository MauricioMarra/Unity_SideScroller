using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class sc_PlayerController : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] Transform pointOfRespawn;
    private Transform playerTr;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private RaycastHit2D rayCastPlayerHits;
    private bool movementDisabled = false;
    private bool invincible = false;
    private bool isDead = false;

    private float speed = 10f;
    private bool facingRight = true;
    private int health = 100;

    private int animParamDeath = Animator.StringToHash("death");

    [SerializeField] private bool isGrounded = true;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private float groundCheckRadius = 0.2f;

    private int jumpForce = 12;
    private int maxNumberOfExtraJumps = 1;
    [SerializeField] private int numberOfExtraJumps;

    PlayerInputAction playerInputAction;

    [Header("Audio")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip stepSound;
    [SerializeField] AudioClip landSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip hurt;

    [Header("Hit Param")]
    [SerializeField] float impactPowerX = 2;
    [SerializeField] float impactPowerY = 2;
    [SerializeField] ForceMode2D hitForceMode = ForceMode2D.Force;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.playerTr = GetComponent<Transform>();
        this.playerRb = GetComponent<Rigidbody2D>();
        this.playerAnimator = GetComponent<Animator>();
        numberOfExtraJumps = maxNumberOfExtraJumps;
        this.healthBar.GetComponent<HUDController>().SetFullHealth(health);
    }

    private void OnEnable()
    {
        playerInputAction.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Disable();
    }

    private void FixedUpdate()
    {
        rayCastPlayerHits = Physics2D.Raycast(playerRb.position, Vector2.down, 0.1f, groundLayer);

        //if (!movementDisabled && !EventSystem.current.currentSelectedGameObject.CompareTag("Inventory")) 
            Move();

        CheckGround();
        AirTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            Debug.Log($"Pointer over: {EventSystem.current.currentSelectedGameObject.name}");

        #region Debug do Event System vs. IsPointerOverGameObject
        Debug.Log($"Over game object?: {EventSystem.current.IsPointerOverGameObject()}");
        Debug.Log($"Point over current name: {EventSystem.current.name}"); //Point over event system
        Debug.Log($"Point over gameObject: {EventSystem.current.gameObject}");
        Debug.Log($"Input modeule: {EventSystem.current.currentInputModule}");
        Debug.Log($"First selected: {EventSystem.current.firstSelectedGameObject}");
        Debug.Log($"Already selecting: {EventSystem.current.alreadySelecting}");
        Debug.Log($"Current tag: {EventSystem.current.tag}");
        Debug.Log($"Teste: {EventSystem.current.IsPointerOverGameObject()}");
        //EventSystem.

        //Debug.Log($"Is it over inventory?: {EventSystem.current.currentSelectedGameObject.CompareTag("Inventory")}");

        //This line of code determines if my pointer is hovering over any UI element.
        //if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject.CompareTag("Inventory")) 
        //    return;
        #endregion

        if (GameManagerController.IsPaused()) 
            return;

        Attack();
        if (!movementDisabled) Jump();

        //if (Input.GetKey(KeyCode.LeftArrow))
        if (playerInputAction.Character.Move.ReadValue<Vector2>().x < 0)
        {
            if (facingRight) Flip();
            playerAnimator.SetBool("isRunning", true);
            playerAnimator.SetBool("isIdle", false);
        }
        //else if (Input.GetKey(KeyCode.RightArrow))
        else if (playerInputAction.Character.Move.ReadValue<Vector2>().x > 0)
        {
            if (!facingRight) Flip();
            playerAnimator.SetBool("isRunning", true);
            playerAnimator.SetBool("isIdle", false);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
            playerAnimator.SetBool("isIdle", true);
        }
    }

    void Flip()
    {
        playerTr.localScale = new Vector3(playerTr.localScale.x * -1, playerTr.localScale.y, playerTr.localScale.z);
        facingRight = !facingRight;
    }

    void Move()
    {
        float mov = 0;
        var input = playerInputAction.Character.Move.ReadValue<Vector2>();

        if (input.x > 0.05f)
            mov = 1;
        else if (input.x < -0.05f)
            mov = -1;
        else
            mov = 0;

        playerRb.velocity = new Vector2(speed * mov, playerRb.velocity.y);
        //playerRb.velocity = new Vector2(speed * Input.GetAxisRaw("Horizontal"), playerRb.velocity.y);
    }

    private void ToggleMove()
    {
        movementDisabled = !movementDisabled;
    }

    public void ToggleInvincibility()
    {
        this.invincible = !this.invincible;
    }

    void Attack()
    {
        if (playerInputAction.Character.Attack.triggered)
        //if (Input.GetButtonDown("Fire1"))
        {
            playerAnimator.SetTrigger("attack");
        }
    }

    void Jump()
    {
        //var hasJumped = Input.GetKeyDown(KeyCode.Space);
        //var hasJumped = playerInputAction.Character.Jump.ReadValue<float>() == 1; //ASSIM DÁ PROBLEMAS PARA O PULO DUPLO.
        var hasJumped = playerInputAction.Character.Jump.triggered;

        if (hasJumped && numberOfExtraJumps > 0 && isGrounded)
        {
            AudioManagerController.audioManager.PlaySound(jumpSound);
            isGrounded = false;
            playerRb.velocity = Vector2.up * jumpForce;
            playerAnimator.SetBool("isJumpAscending", true);
        }
        else if (hasJumped && numberOfExtraJumps > 0 && !isGrounded)
        {
            playerRb.velocity = Vector2.up * jumpForce;
            numberOfExtraJumps--;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }

    private void CheckGround()
    {
        isGrounded = rayCastPlayerHits.collider != null;

        if (isGrounded)
        {
            playerAnimator.SetBool("isGroundedAnim", true);
            playerAnimator.SetBool("isJumpDescending", false);
            playerAnimator.SetBool("isJumpAscending", false);
            numberOfExtraJumps = maxNumberOfExtraJumps;
        }
        else
        {
            playerAnimator.SetBool("isGroundedAnim", false);
        }
    }

    private void AirTime()
    {
        if (!isGrounded)
        {
            if (playerRb.velocity.y < 0)
            {
                playerAnimator.SetBool("isJumpAscending", false);
                playerAnimator.SetBool("isJumpDescending", true);

                //Demora menos a descida após o pulo
                playerRb.velocity = playerRb.velocity + Vector2.down * Time.deltaTime * 20f;
            }
            else if ((playerRb.velocity.y > 0))
            {
                playerAnimator.SetBool("isJumpAscending", true);
                playerAnimator.SetBool("isJumpDescending", false);

                //Demora menos a subida do pulo
                playerRb.velocity = playerRb.velocity + Vector2.down * Time.deltaTime * 20f;
            }
        }
    }

    public void Hit(float pointOfImpact, int damage)
    {
        if (isDead)
        {
            return;
        }

        if (!invincible)
        {
            TakeDamage(damage);

            playerAnimator.SetTrigger("hit");

            StartCoroutine("DisableMoveAfterHit");
            playerRb.AddForce(new Vector2(impactPowerX * pointOfImpact, impactPowerY), hitForceMode); //ForceMode = impulse does not work becaus X is being updated in Move()
        }
    }

    IEnumerator DisableMoveAfterHit()
    {
        //Disable
        ToggleMove(); 
        ToggleInvincibility(); 

        yield return new WaitForSeconds(0.3f);

        //Enable
        ToggleMove();
        ToggleInvincibility();
    }

    
    #region audio
    public void Respawn()
    {
        this.gameObject.transform.position = pointOfRespawn.position;
    }

    public void PlayStep()
    {
        AudioManagerController.audioManager.PlaySoundOneShot(stepSound, 0.4f);
    }

    public void PlayLand()
    {
        AudioManagerController.audioManager.PlaySoundOneShot(landSound, 0.4f);
    }

    public void PlayAttack()
    {
        AudioManagerController.audioManager.PlaySoundOneShot(attackSound, 0.4f);
    }
    #endregion

    public void TakeDamage(int damage)
    {
        health -= damage;
        this.healthBar.GetComponent<HUDController>().SetHealth(damage);
        AudioManagerController.audioManager.PlaySound(hurt);

        if (health <= 0)
        {
            isDead = true;
            playerAnimator.SetTrigger(animParamDeath);
            this.GetComponent<sc_PlayerController>().enabled = false;
            playerRb.simulated = false;
        }
    }

    public void RestoreHealth(int value)
    {
        health += value;
        Debug.Log($"Restore {value} points of health.");

        if (health > 100)
            health = 100;
    }

    public void RestoreMana(int value)
    {
        Debug.Log($"Restore {value} points of mana.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 15)
        {
            PlayLand();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var objScript = collision.GetComponent<EnemyController>();
            objScript.Hit();
            objScript.Death();
        }
    }
}
