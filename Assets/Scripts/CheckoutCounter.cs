using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckoutCounter : MonoBehaviour
{
    [Tooltip("Tag assigned to the shopping cart object used for checkout detection.")]
    public string cartTag = "ShoppingCart";

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(cartTag))
            return;

        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.AllItemsCollected)
        {
            GameManager.Instance.CheckoutComplete();
        }
        else
        {
            GameManager.Instance.NotifyMissingItems();
        }
    }
}
