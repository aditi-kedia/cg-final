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

        // Tell GameManager
        GameManager.Instance?.CollectItem(item);

        PlaceItemInCart(item);
    }

    private void PlaceItemInCart(GroceryItem item)
    {
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
        item.transform.SetParent(cartStoragePoint);

        // Move to storage position
        item.transform.localPosition = GetNextStackPosition();
        item.transform.localRotation = Quaternion.identity;
    }

    private int itemCount = 0;

    private Vector3 GetNextStackPosition()
    {
        float x = (itemCount % 3) * 0.3f;
        float z = (itemCount / 3) * 0.3f;
        float y = 0;

        itemCount++;

        return new Vector3(x, y, z);
    }
}