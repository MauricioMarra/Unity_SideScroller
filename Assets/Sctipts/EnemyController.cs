using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private IEnumerator coroutine;

    private Animator enemyAnimator;
    private BoxCollider2D enemyCollider;
    private EnemyController enemyController;
    private Rigidbody2D enemyRb;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 0.3f;
    [SerializeField] private LayerMask playerLayer;

    private Vector3 center;

    [Header("")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private int damage;

    private int animParamAnimState = Animator.StringToHash("AnimState");
    private int animParamGrounded = Animator.StringToHash("Grounded");

    private void Awake()
    {
        
        enemyAnimator = this.GetComponent<Animator>();
        enemyCollider = this.GetComponent<BoxCollider2D>();
        enemyController = this.GetComponent<EnemyController>();
        enemyRb = this.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DetectPlayer"); //No formato string é possível usar start e stop
    }

    // Update is called once per frame
    void Update()
    {
        center = new Vector3(this.transform.position.x, this.gameObject.transform.position.y + this.GetComponent<BoxCollider2D>().size.y, this.gameObject.transform.position.z);
        Roaming();
    }

    public void Hit()
    {
        AudioManagerController.audioManager.PlaySoundOneShot(hitSound);
    }

    public void Death()
    {
        enemyAnimator.SetTrigger("Death");
        enemyCollider.enabled = false;
        enemyRb.velocity = Vector2.zero;
        Destroy(enemyController);
    }

    IEnumerator DetectPlayer()
    {
        while (true)
        {
            var attackZone = Physics2D.OverlapCircle(center, detectionRange, playerLayer);
            if (attackZone != null && attackZone.gameObject.tag == "Hero")
            {
                if (attackZone.gameObject.transform.position.x < this.transform.position.x && this.transform.localScale.x > 0)
                {
                    Flip();
                    roamingDirection *= -1;
                }
                else if (attackZone.gameObject.transform.position.x > this.transform.position.x && this.transform.localScale.x < 0)
                {
                    Flip();
                    roamingDirection *= -1;
                }

                ToggleMove(); //Disable
                enemyAnimator.SetTrigger("Attack1");
                yield return new WaitForSeconds(1);
                ToggleMove(); //Enable
            }

            yield return null;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(this.transform.position.x, this.gameObject.transform.position.y + this.GetComponent<BoxCollider2D>().size.y, this.gameObject.transform.position.z), detectionRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(new Vector3(this.transform.position.x + (this.transform.localScale.x > 0 ? 1.5f : -1.5f), this.gameObject.transform.position.y, this.gameObject.transform.position.z), 0.2f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hero")
        {
            var direction = this.transform.position.x > collision.gameObject.transform.position.x ? -1 : 1;
            collision.GetComponent<sc_PlayerController>().Hit(direction, damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Hero")
        {
            var direction = this.transform.position.x > collision.gameObject.transform.position.x ? -1 : 1;
            collision.collider.GetComponent<sc_PlayerController>().Hit(direction, damage);
        }
    }

    private float roamingDirection = -1;
    [SerializeField] private LayerMask groundLayer;
    private float roamingSpeed = 4;
    
    public void Roaming()
    {
        var roamingDetector = new Vector3(this.transform.position.x + (this.transform.localScale.x > 0 ? 1.5f : -1.5f), this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        var roamingGround = Physics2D.OverlapCircle(roamingDetector, 0.2f, groundLayer);

        if (roamingGround == null)
        {
            roamingDirection *= -1;
            Flip();
        }

        if (isMoveEnabled)
        {
            enemyRb.velocity = new Vector2(roamingSpeed * roamingDirection, 0);
            enemyAnimator.SetInteger(animParamAnimState, 1);
            enemyAnimator.SetBool(animParamGrounded, true);
        }
        else
        {
            enemyRb.velocity = new Vector2(0, 0);
            enemyAnimator.SetInteger(animParamAnimState, 0);
            enemyAnimator.SetBool(animParamGrounded, true);
        }
    }

    private bool isMoveEnabled = true;
    public void ToggleMove()
    {
        isMoveEnabled = !isMoveEnabled;
    }

    private void Flip()
    {
        var facingDir = this.transform.localScale;
        this.transform.localScale = new Vector3(facingDir.x *= -1, facingDir.y, facingDir.z);
    }
}
