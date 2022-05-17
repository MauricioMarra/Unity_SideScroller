using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public GameObject player;
    public GameObject bullet;
    public GameObject bulletOrigin;
    float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Pega o momento em que a tecla deixa de ser apertada.
        //Input.GetKeyUp(KeyCode.RightArrow);

        //Disparado enquanto a tecla for pressionada.
        //Input.GetKeyDown(KeyCode.RightArrow);

        //Disparado enquanto a tecla estiver sendo pressionada.
        //Input.GetKey(KeyCode.RightArrow);

        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector3(h * speed * Time.deltaTime, v * speed * Time.deltaTime, 0));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bullet.GetComponent<BulletController>().SetOrigin(bulletOrigin);
            bullet.GetComponent<BulletController>().Create();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
}
