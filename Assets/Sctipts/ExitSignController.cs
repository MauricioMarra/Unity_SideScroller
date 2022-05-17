using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSignController : MonoBehaviour
{
    PlayerInputAction playerInput;
    [SerializeField] GameObject player;
    [SerializeField] Canvas exitTextCanvas;

    private void Awake()
    {
        playerInput = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerInput.Character.PlayerInteraction.triggered)
        {
            var newPosition = TransitionController._instance.PlayerPositionOrigin;
            TransitionController._instance.TransitionTo(player, newPosition);
        }

        exitTextCanvas.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        exitTextCanvas.enabled = false;
    }
}
