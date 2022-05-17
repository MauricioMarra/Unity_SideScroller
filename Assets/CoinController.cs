using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public AudioClip coinGet;

    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioManagerController.audioManager.PlaySound(coinGet);

        if (other.CompareTag("Player"))
            Destroy(this.gameObject);
    }
}
