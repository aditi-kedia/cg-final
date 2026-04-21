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

        // Store original world scale before reparenting
        Vector3 originalScale = item.transform.localScale;
        
        // Parent to cart storage point
        item.transform.SetParent(cartStoragePoint);
        
        // Get the box collider to determine valid storage bounds
        BoxCollider storageCollider = cartStoragePoint.GetComponent<BoxCollider>();
        
        Vector3 localPosition;
        if (storageCollider != null)
        {
            // Position within the box collider bounds with some padding
            Vector3 colliderCenter = storageCollider.center;
            Vector3 colliderSize = storageCollider.size;
            
            // Add random offset within the collider bounds (with padding to prevent items from being too close to edges)
            float padding = 0.05f;
            localPosition = new Vector3(
                colliderCenter.x + Random.Range(-colliderSize.x / 2 + padding, colliderSize.x / 2 - padding),
                colliderCenter.y + Random.Range(0f, colliderSize.y - padding),
                colliderCenter.z + Random.Range(-colliderSize.z / 2 + padding, colliderSize.z / 2 - padding)
            );
        }
        else
        {
            // Fallback if no collider found
            Debug.LogWarning($"ShoppingCartTrigger: No BoxCollider found on cartStoragePoint {cartStoragePoint.name}. Using default offset.");
            localPosition = new Vector3(
                Random.Range(-0.15f, 0.15f),
                Random.Range(0.05f, 0.2f),
                Random.Range(-0.1f, 0.1f)
            );
        }
        
        item.transform.localPosition = localPosition;
        item.transform.localRotation = Quaternion.identity;
        
        // Compensate for parent scale so item appears at original size
        Vector3 parentScale = cartStoragePoint.lossyScale;
        item.transform.localScale = new Vector3(
            originalScale.x / parentScale.x,
            originalScale.y / parentScale.y,
            originalScale.z / parentScale.z
        );
    }
}