using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] private Text LevelText;
    [Header("Win/Lose Panel")]
    [SerializeField] float Time;
    [SerializeField] private Transform WinLosePanel;
    [SerializeField] private Image WinLoseImage;
    [SerializeField] private Text WinLoseText;
    [Space]
    [SerializeField] private Image NextButtonImage;
    [SerializeField] private Text NextButtonText;
    [SerializeField] private Image NextButtonArrow;
    [Space]
    [Header("Settings")]
    [SerializeField] private float time = 0.2f;
    [SerializeField] private float ySettingsMoving;
    [SerializeField] private Transform SettingsButton;
    [SerializeField] private Transform SFXButton;
    [SerializeField] private Transform MusicButton;
    [Space]
    [SerializeField] private Image WhiteImage;
    [SerializeField] private Image FadeImage;
    
    private Image SFXImage;
    private Image MusicImage;

    private float xOffset = 1;
    public float yOffset = 1;
    private bool isMoving;
    private bool stateOfSettings;

    private void Start()
    {
        instance = this;

        xOffset = Screen.width / 720;
        yOffset = Screen.height / 1280;
        
        SFXImage = SFXButton.GetComponent<Image>();
        MusicImage = MusicButton.GetComponent<Image>();

        if (PlayerPrefs.GetInt("levelCounts") == 0)
            PlayerPrefs.SetInt("levelCounts", 1);
        LevelText.text = "Level " + PlayerPrefs.GetInt("levelCounts");
        
        StartCoroutine(WhiteFadeRoutine(0, false));
    }

    public IEnumerator WhiteFadeRoutine(float value, bool state)
    {
        WhiteImage.gameObject.SetActive(true);
        yield return WhiteImage.DOFade(value, 1).WaitForCompletion();
        WhiteImage.gameObject.SetActive(state);
    }
    
    public void MoveSettingsButtons()
    {
        if (!isMoving)
            StartCoroutine(MoveButtonsRoutine());
    }
    
    private IEnumerator MoveButtonsRoutine()
    {
        isMoving = true;

        if (!stateOfSettings)
        {
            SFXButton.gameObject.SetActive(true);
            MusicButton.gameObject.SetActive(true);

            SFXImage.DOFade(1, time);
            MusicImage.DOFade(1, time);

            SFXButton.DOMoveY(SettingsButton.position.y - yOffset * ySettingsMoving, time);
            yield return MusicButton.DOMoveY(SettingsButton.position.y - yOffset * ySettingsMoving * 2, time).WaitForCompletion();
        }
        else
        {
            SFXImage.DOFade(0, time);
            MusicImage.DOFade(0, time);
            
            SFXButton.DOMoveY(SettingsButton.position.y, time);
            yield return MusicButton.DOMoveY(SettingsButton.position.y, time).WaitForCompletion();
            
            SFXButton.gameObject.SetActive(false);
            MusicButton.gameObject.SetActive(false);
        }
        
        stateOfSettings = !stateOfSettings;
        isMoving = false;
    }

    public IEnumerator ShowWinLosePanel()
    {
        WinLosePanel.gameObject.SetActive(true);
        FadeImage.DOFade(1, Time);

        float addTime = Time - Time / 3;
        yield return new WaitForSeconds(Time / 3);

        WinLoseImage.transform.DOMoveX(1, addTime).From();
        WinLoseImage.DOFade(1, addTime);
        WinLoseText.DOFade(1, addTime);

        NextButtonImage.transform.DOMoveX(-1, addTime).From();
        NextButtonImage.DOFade(1, addTime);
        NextButtonText.DOFade(1, addTime);
        NextButtonArrow.DOFade(1, addTime);
    }

    public void DeleteAllTween()
    {
        DOTween.KillAll();
    }
}
