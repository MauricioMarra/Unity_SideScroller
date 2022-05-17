using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDownAnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.enabled = false;
    }
}
