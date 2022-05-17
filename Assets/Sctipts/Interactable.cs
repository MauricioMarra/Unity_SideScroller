using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] float interactRadius;
    public bool isInteractable = false;

    public Transform player;

    private void Update()
    {
        Interact(this.gameObject, player);
    }

    public virtual void Interact(GameObject item, Transform player)
    {
        var overlap = Physics2D.OverlapCircle(item.transform.position, interactRadius, playerLayer);

        if (overlap != null)
        {
            if (overlap.gameObject.CompareTag("Hero"))
            {
                isInteractable = true;
            }
            else
            {
                isInteractable = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, interactRadius);
    }

}
