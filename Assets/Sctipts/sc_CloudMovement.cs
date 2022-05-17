using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_CloudMovement : MonoBehaviour
{
    private float speed = 0.1f;
    private float offset;
    private RawImage rawImage;

    // Start is called before the first frame update
    void Awake()
    {
        rawImage = this.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        offset = Time.deltaTime * speed;
        rawImage.material.mainTextureOffset = new Vector2(rawImage.material.mainTextureOffset.x + offset, 0);
    }
}
