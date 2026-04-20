// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;

// [RequireComponent(typeof(Collider))]
// public class ShoppingCartTrigger : MonoBehaviour
// {
//     public string cartTag = "ShoppingCart";

//     private void Reset()
//     {
//         GetComponent<Collider>().isTrigger = true;
//     }

//     // private void OnTriggerEnter(Collider other)
//     // {
//     //     GroceryItem item = other.GetComponent<GroceryItem>();
//     //     if (item == null)
//     //         item = other.GetComponentInParent<GroceryItem>();

//     //     if (item != null && !item.isCollected)
//     //     {
//     //         if (GameManager.Instance != null)
//     //         {
//     //             GameManager.Instance.CollectItem(item);
//     //         }
//     //         return;
//     //     }
//     // }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (!other.attachedRigidbody) return;

//         GroceryItem item = other.attachedRigidbody.GetComponent<GroceryItem>();
//         if (item == null)
//             item = other.attachedRigidbody.GetComponentInParent<GroceryItem>();

//         if (item != null && !item.isCollected)
//         {
//             GameManager.Instance?.CollectItem(item);
//         }
//     }
// }



using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Collider))]
public class ShoppingCartTrigger : MonoBehaviour
{
    public Transform cartStoragePoint; // where items go
    public int columns = 3;
    public Vector3 itemSpacing = new Vector3(0.25f, 0.15f, 0.25f);
    public Vector3 baseOffset = new Vector3(0.12f, 0.05f, 0.12f);

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.attachedRigidbody) return;

        GroceryItem item = other.attachedRigidbody.GetComponent<GroceryItem>();
        if (item == null)
            item = other.attachedRigidbody.GetComponentInParent<GroceryItem>();

        if (item == null || item.isCollected)
            return;

        bool collected = GameManager.Instance?.CollectItem(item) ?? false;
        if (!collected)
            return;

        PlaceItemInCart(item);
    }

    private void PlaceItemInCart(GroceryItem item)
    {
        if (cartStoragePoint == null)
        {
            Debug.LogWarning($"ShoppingCartTrigger: cartStoragePoint is not assigned on {gameObject.name}.");
            return;
        }

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Disable grabbing
        var grab = item.GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.enabled = false;
        }

        // Parent to cart
        item.transform.SetParent(cartStoragePoint, false);

        // Move to storage position
        item.transform.localPosition = GetNextStackPosition();
        item.transform.localRotation = Quaternion.identity;
    }

    private int itemCount = 0;

    private Vector3 GetNextStackPosition()
    {
        int column = itemCount % columns;
        int row = (itemCount / columns) % columns;
        int layer = itemCount / (columns * columns);

        Vector3 position = baseOffset + new Vector3(
            column * itemSpacing.x,
            layer * itemSpacing.y,
            row * itemSpacing.z
        );

        itemCount++;
        return position;
    }
}