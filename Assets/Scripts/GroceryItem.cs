using UnityEngine;

public enum GroceryItemType
{
    Apples,
    Bananas,
    Breads,
    Broccolis,
    Cans,
    Chips,
    Dough,
    Mealbox,
    Milk
}

[RequireComponent(typeof(Collider))]
public class GroceryItem : MonoBehaviour
{
    [Tooltip("Which grocery list entry this object represents.")]
    public GroceryItemType itemType;

    [HideInInspector]
    public bool isCollected;
}
