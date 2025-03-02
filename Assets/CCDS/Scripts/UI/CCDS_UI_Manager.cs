//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright � 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gameplay UI manager.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/CCDS UI Gameplay Manager")]
public class CCDS_UI_Manager : ACCDS_Manager {

    private static CCDS_UI_Manager instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_UI_Manager Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<CCDS_UI_Manager>();

            return instance;

        }

    }

    /// <summary>
    /// Options / settings panel.
    /// </summary>
    public GameObject optionsPanel;

    /// <summary>
    /// Loading panel.
    /// </summary>
    public GameObject loadingPanel;

    /// <summary>
    /// Quality buttons in the options / settings menu.
    /// </summary>
    public Button lowQualityButton, medQualityButton, highQualityButton, ultraQualityButton;

    /// <summary>
    /// Damage label, not the text.
    /// </summary>
    public GameObject damageLabel;

    /// <summary>
    /// Time label, not the text.
    /// </summary>
    public GameObject timeLabel;

    /// <summary>
    /// Percentage label, not the text.
    /// </summary>
    public GameObject percentageLabel;

    /// <summary>
    /// Countdown label, not the text.
    /// </summary>
    public GameObject countDownLabel;

    /// <summary>
    /// Busting label, not the text.
    /// </summary>
    public GameObject bustingLabel;

    /// <summary>
    /// Busted label, not the text.
    /// </summary>
    public GameObject bustedLabel;

    /// <summary>
    /// Wrecked label, not the text.
    /// </summary>
    public GameObject wreckedLabel;

    /// <summary>
    /// Garage label, not the text.
    /// </summary>
    public GameObject garageLabel;

    /// <summary>
    /// Movable panels related to the player vehicle's rigidbody velocity.
    /// </summary>
    public RectTransform[] movablePanels;

    /// <summary>
    /// Money text.
    /// </summary>
    public TextMeshProUGUI moneyText;

    /// <summary>
    /// Time text.
    /// </summary>
    public TextMeshProUGUI timeText;

    /// <summary>
    /// Percentage text.
    /// </summary>
    public TextMeshProUGUI percentageText;

    /// <summary>
    /// Countdown text.
    /// </summary>
    public TextMeshProUGUI countDownText;

    /// <summary>
    /// Damage fill image.
    /// </summary>
    public Image damageFillImage;

    /// <summary>
    /// Felony fill image.
    /// </summary>
    public Image felonyFillImage;

    /// <summary>
    /// Time fill image.
    /// </summary>
    public Image timeFillImage;

    /// <summary>
    /// Percentage fill image.
    /// </summary>
    public Image percentageFillImage;

    /// <summary>
    /// Busting fill images.
    /// </summary>
    public Image[] bustingFillImages;

    /// <summary>
    /// Siren lights raw image.
    /// </summary>
    public GameObject sirenLightsImage;

    /// <summary>
    /// In-game panel text.
    /// </summary>
    public TextMeshProUGUI ingamePointsDriftText;

    /// <summary>
    /// In-game panel text.
    /// </summary>
    public TextMeshProUGUI ingamePointsStuntText;

    /// <summary>
    /// In-game panel text.
    /// </summary>
    public TextMeshProUGUI ingamePointsSpeedText;

    /// <summary>
    /// Police fine text.
    /// </summary>
    public TextMeshProUGUI policeFineText;

    /// <summary>
    /// Garage time counter text.
    /// </summary>
    public TextMeshProUGUI garageTimeCounterText;

    /// <summary>
    /// Fade UI image with animation.
    /// </summary>
    public Animator fade;

    /// <summary>
    /// Countdown timer.
    /// </summary>
    public float countDownTimer = 0f;

    private float lastTimer = 0f;

    private List<Vector2> defaultPositionsOfMovingPanels = new List<Vector2>();

    private void Awake() {

        if (movablePanels.Length > 0) {

            defaultPositionsOfMovingPanels = new List<Vector2>();

            for (int i = 0; i < movablePanels.Length; i++) {

                if (movablePanels[i] != null)
                    defaultPositionsOfMovingPanels.Add(movablePanels[i].anchoredPosition);
                else
                    defaultPositionsOfMovingPanels.Add(Vector3.zero);

            }

        }

    }

    private void OnEnable() {

        //  Listening events.
        CCDS_Events.OnMissionCountdownStarted += CCDS_Events_OnCountdownStarted;
        CCDS_Events.OnPaused += CCDS_Events_OnPaused;
        CCDS_Events.OnResumed += CCDS_Events_OnResumed;

    }

    /// <summary>
    /// When game paused.
    /// </summary>
    private void CCDS_Events_OnPaused() {

        if (optionsPanel)
            optionsPanel.SetActive(true);

    }

    /// <summary>
    /// When game resumed.
    /// </summary>
    private void CCDS_Events_OnResumed() {

        if (optionsPanel)
            optionsPanel.SetActive(false);

    }

    private void OnDisable() {

        //  Not listening events.
        CCDS_Events.OnMissionCountdownStarted -= CCDS_Events_OnCountdownStarted;
        CCDS_Events.OnPaused -= CCDS_Events_OnPaused;
        CCDS_Events.OnResumed -= CCDS_Events_OnResumed;

    }

    /// <summary>
    /// When countdown starts at the beginning of the game.
    /// </summary>
    private void CCDS_Events_OnCountdownStarted() {

        //  Fading off.
        Fade();

        //  Return if gameplay manager couldn't found.
        if (CCDS_GameplayManager.Instance == null)
            return;

        //  Setting countdown timer if player is on mission and that mission has countdown timer.
        if (CCDS_GameplayManager.Instance.OnMission)
            SetCountDownTimer(CCDS_GameplayManager.Instance.currentMission.countDown);
        else
            SetCountDownTimer(CCDS_GameplayManager.Instance.countdownToStart);

    }

    private void LateUpdate() {

        CCDS_Player player = CCDS_GameplayManager.Instance.player;

        // Setting text of the money.
        moneyText.text = "$ " + CCDS.GetMoney().ToString();

        //  If scene doesn't include gameplay manager, return by disabling panels and setting their values back to 0.
        if (!CCDS_GameplayManager.Instance) {

            //  Disabling the time label.
            if (timeLabel.activeSelf)
                timeLabel.SetActive(false);

            //  Disabling the damage label.
            if (damageLabel.activeSelf)
                damageLabel.SetActive(false);

            //  Cleaning the time text string;
            timeText.text = "";

            //  Setting fill image to 0.
            damageFillImage.fillAmount = 0f;

            //  Setting fill image to 0.
            felonyFillImage.fillAmount = 0f;

            //  Setting fill image to 0.
            percentageFillImage.fillAmount = 0f;

            //  Setting fill image to 0.
            timeFillImage.fillAmount = 0f;

            for (int i = 0; i < bustingFillImages.Length; i++) {

                if (bustingLabel.activeSelf)
                    bustingLabel.SetActive(false);

                bustingFillImages[i].fillAmount = 0f;

            }

            //if (bustedLabel.activeSelf)
            //    bustedLabel.SetActive(false);

            if (garageLabel.activeSelf)
                garageLabel.SetActive(false);

            //  Deactivating the siren lights.
            if (sirenLightsImage)
                sirenLightsImage.SetActive(false);

            for (int i = 0; i < movablePanels.Length; i++) {

                movablePanels[i].anchoredPosition = defaultPositionsOfMovingPanels[i];

            }

            return;

        }

        //  Time left.
        float timeLeft = CCDS_GameplayManager.Instance.timeLimit;

        int min;
        int sec;

        //  Enable the time label if time left is over 0 second. Disable on 0 second.
        if (timeLeft > 0) {

            min = Mathf.FloorToInt(timeLeft / 60);
            sec = Mathf.FloorToInt(timeLeft % 60);

            if (timeLeft > lastTimer)
                lastTimer = timeLeft;

        } else {

            min = 0;
            sec = 0;
            lastTimer = 0f;

        }

        //  Setting string of the time text.
        timeText.text = min.ToString("00") + ":" + sec.ToString("00");

        //  Enable damage label and set its fillamount depending on the player vehicle damage.
        if (player) {

            if (!damageLabel.activeSelf)
                damageLabel.SetActive(true);

            damageFillImage.fillAmount = Mathf.Clamp(player.damage, .01f, 100f) / 100f;
            felonyFillImage.fillAmount = Mathf.Clamp(player.felony, .01f, 100f) / 100f;

            //  Activating the siren lights if player is in pursue.
            if (sirenLightsImage)
                sirenLightsImage.SetActive(player.inPursue);

            for (int i = 0; i < bustingFillImages.Length; i++) {

                bustingFillImages[i].fillAmount = Mathf.InverseLerp(0f, 100f, player.busting);

                if (bustingFillImages[i].fillAmount > 0) {

                    if (!bustingLabel.activeSelf)
                        bustingLabel.SetActive(true);

                } else {

                    if (bustingLabel.activeSelf)
                        bustingLabel.SetActive(false);

                }

            }

            //if (bustedLabel.activeSelf != player.busted)
            //    bustedLabel.SetActive(player.busted);

            for (int i = 0; i < movablePanels.Length; i++) {

                movablePanels[i].anchoredPosition = defaultPositionsOfMovingPanels[i];
                Vector3 localVelocity = player.transform.InverseTransformDirection(player.CarController.Rigid.linearVelocity);
                movablePanels[i].anchoredPosition += Vector2.right * localVelocity.x * 3f;

            }

            if (player.timeForGarage > 0) {

                if (!garageLabel.activeSelf)
                    garageLabel.SetActive(true);

                garageTimeCounterText.text = (3f - player.timeForGarage).ToString("F0");

            } else {

                if (garageLabel.activeSelf)
                    garageLabel.SetActive(false);

                garageTimeCounterText.text = "";

            }

        } else {

            if (damageLabel.activeSelf)
                damageLabel.SetActive(false);

            damageFillImage.fillAmount = 0f;
            felonyFillImage.fillAmount = 0f;

            //  Deactivating the siren lights.
            if (sirenLightsImage)
                sirenLightsImage.SetActive(false);

            for (int i = 0; i < bustingFillImages.Length; i++) {

                if (bustingLabel.activeSelf)
                    bustingLabel.SetActive(false);

                bustingFillImages[i].fillAmount = 0f;

            }

            //if (bustedLabel.activeSelf)
            //    bustedLabel.SetActive(false);

            for (int i = 0; i < movablePanels.Length; i++) {

                movablePanels[i].anchoredPosition = defaultPositionsOfMovingPanels[i];

            }

        }

        if (player) {

            ingamePointsDriftText.text = player.score_Drift.ToString("F0");
            ingamePointsStuntText.text = player.score_Stunt.ToString("F0");
            ingamePointsSpeedText.text = player.score_Speed.ToString("F0");

            //if (bustedLabel.activeSelf)
            //    policeFineText.text = "Pay $" + player.policeFineMoney.ToString("F0") + "\nTo Be Free!";
            //else
            //    policeFineText.text = "";

        } else {

            ingamePointsDriftText.text = "";
            ingamePointsStuntText.text = "";
            ingamePointsSpeedText.text = "";
            policeFineText.text = "";

        }

    }

    private void Update() {

        if (countDownTimer > 0) {

            countDownTimer -= Time.deltaTime;

            countDownLabel.SetActive(true);
            countDownText.text = Mathf.CeilToInt(countDownTimer).ToString();

        } else {

            countDownTimer = 0f;

            countDownLabel.SetActive(false);
            countDownText.text = "";

        }

        //  If scene doesn't include gameplay manager, return by disabling panels and setting their values back to 0.
        if (!CCDS_GameplayManager.Instance) {

            if (percentageLabel.activeSelf)
                percentageLabel.SetActive(false);

            percentageText.text = "";

            return;

        }

        if (CCDS_GameplayManager.Instance.OnMission) {

            if (CCDS_GameplayManager.Instance.currentMission.percentage >= 0f) {

                if (!percentageLabel.activeSelf)
                    percentageLabel.SetActive(true);

                percentageText.text = Mathf.CeilToInt(CCDS_GameplayManager.Instance.currentMission.percentage).ToString() + " / " + CCDS_GameplayManager.Instance.currentMission.percentageOver.ToString();
                percentageFillImage.fillAmount = Mathf.Clamp(CCDS_GameplayManager.Instance.currentMission.percentage, .01f, Mathf.Infinity) / CCDS_GameplayManager.Instance.currentMission.percentageOver;

            } else {

                if (percentageLabel.activeSelf)
                    percentageLabel.SetActive(false);

                percentageText.text = "";
                percentageFillImage.fillAmount = 0f;

            }

            if (CCDS_GameplayManager.Instance.timeLimit > 0) {

                if (!timeLabel.activeSelf)
                    timeLabel.SetActive(true);

                //  Setting fill image.
                timeFillImage.fillAmount = CCDS_GameplayManager.Instance.timeLimit / lastTimer;

            } else {

                if (timeLabel.activeSelf)
                    timeLabel.SetActive(false);

                //  Setting fill image.
                timeFillImage.fillAmount = CCDS_GameplayManager.Instance.timeLimit / lastTimer;

            }

        } else {

            if (percentageLabel.activeSelf)
                percentageLabel.SetActive(false);

            percentageText.text = "";
            percentageFillImage.fillAmount = 0f;

            if (timeLabel.activeSelf)
                timeLabel.SetActive(false);

            timeText.text = "";
            timeFillImage.fillAmount = 0f;

        }
    }

    /// <summary>
    /// Fading the UI by animating the fade animator.
    /// </summary>
    public void Fade() {

        if (fade)
            fade.Play(0);

    }

    /// <summary>
    /// Sets the countdown timer.
    /// </summary>
    /// <param name="seconds"></param>
    public void SetCountDownTimer(float seconds) {

        countDownTimer = seconds;

    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void PauseGameButton() {

        CCDS.PauseGame();

    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void ResumeGameButton() {

        CCDS.ResumeGame();

    }

    /// <summary>
    /// Restarts the game.
    /// </summary>
    public void RestartGameButton() {

        CCDS.RestartGame();

    }

    /// <summary>
    /// Returns to the main menu.
    /// </summary>
    public void MainMenuButton() {

        //  Enabling the loading panel.
        if (loadingPanel)
	        loadingPanel.SetActive(true);
            
	    CCDS.SetPlayerCharacterPosition(BCG_EnterExitManager.Instance.activePlayer.transform.position);

        CCDS.MainMenu();

    }

    public void PlayerBusted(CCDS_Player player) {

        bustedLabel.SetActive(true);
        policeFineText.text = "Pay $" + player.policeFineMoney.ToString("F0") + "\nTo Be Free!";

    }

    public void PlayerReleased(CCDS_Player player) {

        policeFineText.text = "";
        bustedLabel.SetActive(false);

    }

    public void PlayerWrecked(CCDS_Player player) {

        wreckedLabel.SetActive(true);

    }

    /// <summary>
    /// Pays fine to be free.
    /// </summary>
    public void PayFine() {

        CCDS_GameplayManager.Instance.PayFineToBeFree();

    }

    /// <summary>
    /// Get back to the main menu.
    /// </summary>
    public void Garage() {

        CCDS.MainMenu();

    }

}
