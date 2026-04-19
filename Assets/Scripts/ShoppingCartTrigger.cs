using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShoppingCartTrigger : MonoBehaviour
{
    [Tooltip("If the cart is not tagged, assign the correct tag here.")]
    public string cartTag = "ShoppingCart";

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        GroceryItem item = other.GetComponent<GroceryItem>();
        if (item == null)
            item = other.GetComponentInParent<GroceryItem>();

        if (item != null && !item.isCollected)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectItem(item);
            }
            return;
        }
    }
}
