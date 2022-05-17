using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    float speed = 0.05f;
    public MeshRenderer skyRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 displacement = new Vector2(speed * Time.deltaTime, 0);
        skyRenderer.material.mainTextureOffset += displacement;
    }
}
