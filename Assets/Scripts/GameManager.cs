using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Grocery List")]
    [Tooltip("Which items the player must collect.")]
    public GroceryItemType[] requiredItems = new GroceryItemType[]
    {
        GroceryItemType.Apples,
        GroceryItemType.Bananas,
        GroceryItemType.Cans,
        GroceryItemType.Breads,
        GroceryItemType.Broccolis
    };

    [Header("UI References")]
    public TextMeshProUGUI groceryListText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI successText;
    public Button exitButton;

    private readonly Dictionary<GroceryItemType, bool> collected = new Dictionary<GroceryItemType, bool>();

    public bool AllItemsCollected => collected.Count > 0 && collected.Values.All(value => value);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitializeList();
    }

    private void Start()
    {
        if (groceryListText != null)
            groceryListText.richText = true;
        else
            Debug.LogWarning("GameManager: groceryListText is not assigned in the Inspector.");

        if (statusText == null)
            Debug.LogWarning("GameManager: statusText is not assigned in the Inspector.");

        if (successText == null)
            Debug.LogWarning("GameManager: successText is not assigned in the Inspector.");

        if (exitButton == null)
            Debug.LogWarning("GameManager: exitButton is not assigned in the Inspector.");
        else
        {
            // Initially hide the exit button
            exitButton.gameObject.SetActive(false);
            // Add click listener to exit button
            exitButton.onClick.AddListener(ExitGame);
        }

        UpdateListUI();
        ShowStatus("Pick up the cart and collect the items on your list.");
        if (successText != null)
            successText.text = string.Empty;
    }

    private void InitializeList()
    {
        collected.Clear();

        foreach (var itemType in requiredItems)
        {
            if (!collected.ContainsKey(itemType))
                collected[itemType] = false;
        }
    }

    public bool IsRequiredItem(GroceryItemType itemType)
    {
        return collected.ContainsKey(itemType);
    }

    public bool IsItemCollected(GroceryItemType itemType)
    {
        return collected.TryGetValue(itemType, out bool value) && value;
    }

    public bool CollectItem(GroceryItem item)
    {
        if (item == null)
            return false;

        if (!IsRequiredItem(item.itemType))
        {
            ShowStatus("This item is not on the list.");
            return false;
        }

        if (item.isCollected || IsItemCollected(item.itemType))
        {
            ShowStatus("This item has already been collected.");
            return false;
        }

        item.isCollected = true;
        collected[item.itemType] = true;
        // item.gameObject.SetActive(false);

        UpdateListUI();

        if (AllItemsCollected)
        {
            ShowStatus("All items collected! Take the cart to checkout.");
        }
        else
        {
            int remaining = collected.Values.Count(value => !value);
            ShowStatus($"Collected {item.itemType}. {remaining} item(s) remaining.");
        }

        return true;
    }

    public void NotifyMissingItems()
    {
        int remaining = collected.Values.Count(value => !value);
        if (remaining > 0)
            ShowStatus($"You still need to collect {remaining} item(s) before checkout.");
        else
            ShowStatus("Bring the cart to the checkout.");
    }

    public void CheckoutComplete()
    {
        ShowStatus("Checkout complete! Well done.");
        if (successText != null)
            successText.text = "Success! Shopping task finished.";
        
        // Show exit button when success text is displayed
        if (exitButton != null)
            exitButton.gameObject.SetActive(true);
    }

    private void UpdateListUI()
    {
        if (groceryListText == null)
            return;

        var lines = new List<string>();
        foreach (var itemType in requiredItems)
        {
            bool crossed = IsItemCollected(itemType);
            if (crossed)
            {
                lines.Add($"<color=green><s>{FormatItemName(itemType)}</s></color>");
            }
            else
            {
                lines.Add($"{FormatItemName(itemType)}");
            }
        }

        // groceryListText.supportRichText = true;
        groceryListText.text = string.Join("\n", lines);
    }

    private void ShowStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
        else
            Debug.Log(message);
    }

    private static string FormatItemName(GroceryItemType itemType)
    {
        switch (itemType)
        {
            case GroceryItemType.Apples:
                return "Apples";
            case GroceryItemType.Bananas:
                return "Bananas";
            case GroceryItemType.Cans:
                return "Cans";
            case GroceryItemType.Breads:
                return "Breads";
            case GroceryItemType.Broccolis:
                return "Broccolis";
            default:
                return itemType.ToString();
        }
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
