using UnityEngine;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject levelSelectionPanel; // Panel containing the level buttons
    // [SerializeField] private TextMeshProUGUI selectedLevelText;
    [SerializeField] private TextMeshProUGUI textObject1; // First text to show after selection
    [SerializeField] private TextMeshProUGUI textObject2; // Second text to show after selection

    private int selectedLevel = 2; // Default to level 2
    private CanvasGroup panelCanvasGroup;

    private void Start()
    {
        // Get CanvasGroup from the panel, or create one if it doesn't exist
        if (levelSelectionPanel != null)
        {
            panelCanvasGroup = levelSelectionPanel.GetComponent<CanvasGroup>();
            if (panelCanvasGroup == null)
            {
                panelCanvasGroup = levelSelectionPanel.AddComponent<CanvasGroup>();
            }
        }

        // Make sure the text objects are visible at the start (optional)
        if (textObject1 != null)
            textObject1.gameObject.SetActive(true);
        if (textObject2 != null)
            textObject2.gameObject.SetActive(true);

        // UpdateDisplay();
    }

    public void SelectLevel(int level)
    {
        if (level < 1 || level > 3)
        {
            Debug.LogWarning("Level must be between 1 and 3");
            return;
        }

        selectedLevel = level;
        // UpdateDisplay();

        // Set the audio level in AudioManager
        AudioManager.Instance?.SetLevel(level);
        DistractionManager.Instance?.SetLevel(level); 

        // Hide the level selection panel
        HideLevelPanel();

        // Show the text objects
        ShowTextObjects();
    }

    private void HideLevelPanel()
    {
        if (levelSelectionPanel != null)
        {
            levelSelectionPanel.SetActive(false);
        }
    }

    private void ShowTextObjects()
    {
        if (textObject1 != null)
            textObject1.gameObject.SetActive(true);
        if (textObject2 != null)
            textObject2.gameObject.SetActive(true);
    }

    // private void UpdateDisplay()
    // {
    //     string levelName = selectedLevel switch
    //     {
    //         1 => "LOW",
    //         2 => "MEDIUM",
    //         3 => "HIGH",
    //         _ => "MEDIUM"
    //     };

    //     // if (selectedLevelText != null)
    //     // {
    //     //     selectedLevelText.text = $"Selected: {levelName} (Level {selectedLevel})";
    //     // }

    //     Debug.Log($"Level selected: {selectedLevel} - {levelName}");
    // }
}
