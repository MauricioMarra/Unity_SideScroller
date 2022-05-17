using UnityEngine;
using UnityEngine.SceneManagement;

public class BottomDeathController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //var player = other.GetComponent<sc_PlayerController>();

        //player.Respawn();

        SceneManager.LoadScene("MetalSlug_01");
    }
}
