using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private Button backButton;
    public Button[] buttons;
    public TextMeshProUGUI[] buttonsText;

    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;
    // [SerializeField] private GameObject lockGo;

    [SerializeField] private string gameSceneName = "GameScene";
    private int _unlockedLevels;

    void Start()
    {
        _unlockedLevels = PlayerPrefs.GetInt("unlockedLevels", 0);
        for (int i = 0; i < buttons.Length; i++)
        {
            var levelNumber = i + 1;
            var i1 = i;

            var unlocked = i1 <= _unlockedLevels;

            buttonsText[i].text = unlocked ? levelNumber.ToString() : "";
            
            // lockGo.SetActive(true);
            
            buttons[i].onClick.AddListener(() => OnLevelButtonClick(i1));
            buttons[i].interactable = unlocked;
            buttons[i].GetComponent<Image>().sprite = unlocked ? unlockedSprite : lockedSprite;
        }
    }

    public void OnLevelButtonClick(int levelIndex)
    {
        if (levelIndex > _unlockedLevels) return;

        PlayerPrefs.SetInt("level", levelIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}