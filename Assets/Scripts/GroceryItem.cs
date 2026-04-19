using UnityEngine;

public enum GroceryItemType
{
    Apples,
    Bananas,
    Basil,
    Breads,
    Broccolis
}

[RequireComponent(typeof(Collider))]
public class GroceryItem : MonoBehaviour
{
    [Tooltip("Which grocery list entry this object represents.")]
    public GroceryItemType itemType;

    [HideInInspector]
    public bool isCollected;
}
