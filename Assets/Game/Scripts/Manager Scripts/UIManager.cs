using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    private PlayerRunner playerRunner;

    [Header("UIS")]
    [SerializeField] private GameObject tapToPlayUI;
    [SerializeField] private GameObject nextLvlUI;
    [SerializeField] private GameObject restartLvlUI;

    [Header("Buttons")]
    [SerializeField] private GameObject pausedText;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle vibrationToggle;

    [Header("Texts")]
    [SerializeField] private TMP_Text currentLvl;
    [SerializeField] private TMP_Text totalMoneyText;
    [SerializeField] private TMP_Text collectedMoneyText;
    [SerializeField] private TMP_Text nextLvlButtonText;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private Button multiplierButton;
    [SerializeField] private TMP_Text multiplierButtonText;
    [SerializeField] private TMP_Text multiplierGameText;
    [SerializeField] private Image multiplierImage;
    private int multiplier;
    private int multiplierCounter = 0;

    [Header("Status Texts")]
    [SerializeField] private Image progressBarImage;
    [SerializeField] private GameObject poorText;
    [SerializeField] private GameObject averageText;
    [SerializeField] private GameObject richText;

    [SerializeField] private RectTransform arrowImage;
    private bool clickBonusCheck = false;

    public bool isPaused;

     void Awake()
    {
        if (PlayerPrefs.GetInt("vibrationOnOff") == 0) {
            vibrationToggle.GetComponent<Toggle>().isOn = false;
        }
        if (PlayerPrefs.GetInt("soundOnOff") == 0)
        {
            soundToggle.GetComponent<Toggle>().isOn = false;
        }
    }
    private void Start()
    {
        isPaused = true;
        DOTween.Init();
        LevelText();
    }
    public void PlayResButton() {
        if (tapToPlayUI.activeSelf) {
            tapToPlayUI.SetActive(false);
            isPaused = false;
            PlayerManagement.Instance.CanRun();
        }
        if (nextLvlUI.activeSelf) {
            nextLvlUI.SetActive(false);
            isPaused = false;
            ResMultiplierButton();

            LevelManager.Instance.LevelUp();
            LevelText();
            PlayerManagement.Instance.CharacterReset();
            LevelManager.Instance.GenerateCurrentLevel();
        }
        if (restartLvlUI.activeSelf) {
            restartLvlUI.SetActive(false);
            isPaused = false;

            PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoney")
                + PlayerManagement.Instance.currentLvlMoneyAmount);
            SetTotalMoney();

        }


    }//PlayResButton
    public void ClickMe() {
        Debug.Log("CLicked");
    }
    public void NextLvlUI() {
        if (!isPaused) {
            tapToPlayUI.SetActive(false);
            nextLvlUI.SetActive(true);
            isPaused = true;
         //   nextLvlButtonText.text = "" + PlayerManagement.Instance.currentLvlMoneyAmount + "$";
        }
        Invoke("MoveMultiplierArrow", 0.5f);
    }//NextLvlUI

    public void RestartButtonUI() {
        if (!isPaused) {
            restartLvlUI.SetActive(true);
            isPaused = true;
        }
    }//restartButton
    public void PauseButtonUI() {
        if (!isPaused) {
            pausedText.SetActive(true);
            tapToPlayUI.SetActive(true);
            isPaused = true;
        }
    }//pauseButton

    public void UIVibrationToggle(bool checkOnOff) {
        if (checkOnOff) {
            vibrationToggle.GetComponent<Toggle>().isOn = true;
            PlayerPrefs.SetInt("vibrationOnOff", 1);
        }
        else {
            vibrationToggle.GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetInt("vibrationOnOff", 0);
        }
    }//vibrationToggle
    public void UISoundToggle(bool checkOnOff)
    {
        if (checkOnOff)
        {
            soundToggle.GetComponent<Toggle>().isOn = true;
            PlayerPrefs.SetInt("soundOnOff", 1);
        }
        else
        {
            soundToggle.GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetInt("soundOnOff", 0);
        }
    }


    public void LevelText() {
         int LevelInt = LevelManager.Instance.GetGlobalLevelIndex() + 1;
         currentLvl.text = "Level " + LevelInt;
    }

    public void SetProgress(float progress) {
        progressBarImage.fillAmount = progress;
    }
    public void SetCollectedMoney() {
        collectedMoneyText.text = "" + PlayerManagement.Instance.currentLvlMoneyAmount;
    }
    public void SetTotalMoney() {
        totalMoneyText.text = "" + PlayerPrefs.GetInt("TotalMoney", 0) + "$";
    }
    public void ChangeStatusText(int status) {
        if (status == 0) {
            poorText.SetActive(true);
            averageText.SetActive(false);
            richText.SetActive(false);
            progressBarImage.color = Color.red;
        }
        if (status == 1) {
            poorText.SetActive(false);
            averageText.SetActive(true);
            richText.SetActive(false);
            progressBarImage.color = new Color(1.0f, 0.64f, 0.0f);
        }
        if (status == 2) {
            poorText.SetActive(false);
            averageText.SetActive(false);
            richText.SetActive(true);
            progressBarImage.color = Color.green;
        }
    }//changeStatus

     void MoveMultiplierArrow() {
        arrowImage.DORotate(arrowImage.forward * 90, 0.01f);
        arrowImage.DORotate(arrowImage.forward * -90, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        StartCoroutine(HandTransform());
    }
    IEnumerator HandTransform() {
        float arrowAngle = arrowImage.eulerAngles.z;
        if (arrowAngle > 45 && arrowAngle < 90)
            SetMultiplier("Get 1X", 1);
        if (arrowAngle > 0 && arrowAngle < 45)
            SetMultiplier("Get 2X", 2);
        if (arrowAngle > 315 && arrowAngle < 360)
            SetMultiplier("Get 3X", 3);
        if (arrowAngle > 270 && arrowAngle < 315)
            SetMultiplier("Get 4X", 4);
        if (!clickBonusCheck) {
            yield return new WaitForFixedUpdate();
            StartCoroutine(HandTransform());
        }
        else {
            MultiplierButton(true);
        }
    }

    void SetMultiplier(string textString,int multiplierInt) {
        multiplierButtonText.text = textString;
        multiplier = multiplierInt;

        if (multiplierInt == 1)
            multiplierImage.color = new Color32(241, 12, 12, 255);
        if (multiplierInt == 2)
            multiplierImage.color = new Color32(255, 153, 21, 255);
        if (multiplierInt == 3)
            multiplierImage.color = new Color32(250, 205, 51, 255);
        if (multiplierInt == 4)
            multiplierImage.color = new Color32(105, 179, 76, 255);

    }
    public void MultiplierButton(bool coroutineCheck) {
        clickBonusCheck = true;
        StopCoroutine(HandTransform());
        DOTween.Kill(arrowImage.transform);
        multiplierButton.interactable = false;

        if (coroutineCheck) {
            multiplierGameText.text = "You Won";
            multiplierButtonText.text = PlayerManagement.Instance.currentLvlMoneyAmount * multiplier + "$";
            PlayerManagement.Instance.currentLvlMoneyAmount *= multiplier;
            nextLevelButton.SetActive(true);
            PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoney")
                + PlayerManagement.Instance.currentLvlMoneyAmount);
            SetTotalMoney();
        }

    }//multiplierbutton

    public void ResMultiplierButton() {
        clickBonusCheck = false;
        nextLevelButton.SetActive(false);
        multiplierGameText.text = "Tap to Win";
        multiplierButtonText.text = "Get 1X";
        multiplierButton.interactable = true;
    }//resmultiplierButton
    public void UIQuitGame() {
        Application.Quit();
    }



}
