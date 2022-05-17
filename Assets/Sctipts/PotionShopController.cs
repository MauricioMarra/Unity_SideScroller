using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionShopController : MonoBehaviour
{
    public Canvas hint;
    PlayerInputAction playerInput;

    [SerializeField] GameObject player;
    [SerializeField] Transform shopEntrance;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
            hint.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hint.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerInput.Character.PlayerInteraction.triggered)
        {
            TransitionController._instance.TransitionTo(player, shopEntrance.position);
        }
    }
}
