using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    float speed = 10f;
    public GameObject origin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
    }

    public void Create()
    {
        Instantiate(this.gameObject, origin.transform.position, origin.transform.rotation);
    }

    public void SetOrigin(GameObject origin)
    {
        this.origin = origin;
    }
}
