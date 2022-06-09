using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManagement : MonoSingleton<PlayerManagement>
{
    [SerializeField] private GameObject poorUI;
    [SerializeField] private GameObject averageUI;
    [SerializeField] private GameObject richUI;
    [Space]
    [SerializeField] private GameObject localPosition;
    [SerializeField] private GameObject localMoverTarget;
    [SerializeField] private GameObject character;
    [SerializeField] private PlayerRunner playerRunner;

    private bool isStarted = false;
    private bool canRun = true;
    public float characterProgress = 0;
    public int currentLvlMoneyAmount;
    private int status = 0;

    private void Start()
    {
        DOTween.Init();
        currentLvlMoneyAmount = 0;
        playerRunner.idleAnimation();
        
    }
     void Update()
    {
        if (Input.GetMouseButton(0) && canRun) {
            playerRunner.running = true;
            playerRunner.SetRotateEnabled(true);
            playerRunner.SetEnabled(true);
            playerRunner.StartToRun();
            isStarted = true;
            canRun = false;
        }

    }
    public void AddMoney(int colledtedAmount) {
        currentLvlMoneyAmount += colledtedAmount;
        if (0 > currentLvlMoneyAmount + colledtedAmount) {
            currentLvlMoneyAmount = 0;
        }
        UIManager.Instance.SetCollectedMoney();
        characterProgress += colledtedAmount;
        SetUIProgress();

        if(status == 0 && (characterProgress + colledtedAmount)<=0) {
            characterProgress = 0;
        }
        if (status == 2 && (characterProgress + colledtedAmount) >= 100) {
            characterProgress = 100;
        }
        if (status == 1) { //average
            if (characterProgress > 100) { //turn to rich form
                characterProgress -= 100;
                status = 2;
                PlayParticle();
                playerRunner.richSpinAnimation();
                ChangeApperance(status);
                SetUIProgress();
                return;
            }
            if (characterProgress < 0) { // turn to poor form
                characterProgress = 100 + colledtedAmount;
                status = 0;
                PlayParticle();
                playerRunner.sadSpinAnimation();
                ChangeApperance(status);
                SetUIProgress();
                return;
            }
            
        }
        if (status == 2) { //rich
            if (characterProgress < 0) { //turn to average form
                characterProgress = 100 + colledtedAmount;
                status = 1;
                PlayParticle();
                playerRunner.sadSpinAnimation();
                ChangeApperance(status);
                SetUIProgress();
                return;
            }

        }
        if (status == 0) {//poor
            if (characterProgress > 100) { //turn to average form
                characterProgress -= 100;
                status = 1;
                PlayParticle();
                playerRunner.richSpinAnimation();
                ChangeApperance(status);
                SetUIProgress();
                return;
            }
        }

    }
    public void StartToDance() {
        Debug.Log("Finished LEVEL");
        playerRunner.running = false;
        playerRunner.SetRotateEnabled(false);
        playerRunner.SetEnabled(false);
        canRun = false;
        if (status == 0) {
            playerRunner.sadFinishAnimation();
        }
        else { 
        playerRunner.richFinishAnimation();
        }
        
        character.transform.DORotate(new Vector3(0, 180, 0), 1);
        UIManager.Instance.NextLvlUI();
    }
    void PlayParticle() {
        var particle = ObjectPooler.Instance.GetPooledObject("ApperanceParticle");
        particle.transform.position = localPosition.transform.position + new Vector3(0f, 1f, 1f);
        particle.transform.rotation = localPosition.transform.rotation;
        particle.SetActive(true);
        particle.GetComponent<ParticleSystem>().Play();

    }
    void SetUIProgress() {
        float floatCharacterProgress = characterProgress / 100;
         UIManager.Instance.SetProgress(floatCharacterProgress);
    }
    void ChangeApperance(int statusValue) {
        if (statusValue == 0) {
            averageUI.SetActive(false);
            richUI.SetActive(false);
            poorUI.SetActive(true);

            playerRunner.sadWalkAnimation();
            UIManager.Instance.ChangeStatusText(status);
        }
        if (status == 1) {
            averageUI.SetActive(true);
            richUI.SetActive(false);
            poorUI.SetActive(false);

            playerRunner.averageWalkAnimation();
            UIManager.Instance.ChangeStatusText(status);

        }
        if (status == 2) {
            averageUI.SetActive(false);
            richUI.SetActive(true);
            poorUI.SetActive(false);

            playerRunner.richWalkAnimation();
            UIManager.Instance.ChangeStatusText(status);
        }
    }
    public void CanRun() {
        canRun = true;
    }
    public void CharacterReset() {
        characterProgress = 0;
        status = 0;
        isStarted = false;
        currentLvlMoneyAmount = 0;

        character.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        localMoverTarget.transform.localPosition = new Vector3(0f, 0f, 0.9f);

        ChangeApperance(status);
        SetUIProgress();
        playerRunner.idleAnimation();
        playerRunner.distance = 0;
        UIManager.Instance.PauseButtonUI();

    }



}
