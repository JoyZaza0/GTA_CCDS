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
using UnityEngine.Events;

/// <summary>
/// UI informer panel.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/CCDS UI Informer")]
public class CCDS_UI_Informer : ACCDS_Component {

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_UI_Informer Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<CCDS_UI_Informer>();

            return instance;

        }

    }

    private static CCDS_UI_Informer instance;

    /// <summary>
    /// Content.
    /// </summary>
    public GameObject content;

    /// <summary>
    /// Info text.
    /// </summary>
    public TextMeshProUGUI infoText;

    /// <summary>
    /// Animator.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Timer used to disable the content.
    /// </summary>
	private float timer = 0f;
    
	//[SerializeField] private GameObject missionPopup;    
	//[SerializeField] private GameObject missionFailedPopup;    
	
	//[SerializeField] private TextMeshProUGUI missionText;
	//[SerializeField] private TextMeshProUGUI missionFailedText;
	
	//[SerializeField] private Button startMissionButton;
	//[SerializeField] private Button canselButton;
	[SerializeField] private MissionPopupUi missionPopup;
	[SerializeField] private MissionVictoryPopupUI missionVictoryPopup;
	[SerializeField] private MissionFailedPopup missionFailedPopup;

    private void Update() {

        //  Disable the content if timer hits to 0.
        if (timer > 0) {

            timer -= Time.deltaTime;

            if (!content.activeSelf)
                content.SetActive(true);

        } else {

            timer = 0f;

            if (content.activeSelf)
                content.SetActive(false);

        }

    }

    /// <summary>
    /// Displays the info with given string.
    /// </summary>
    /// <param name="info"></param>
    public void Info(string info) {

        //  Setting timer to 1.5 seconds.
        timer = 1.5f;

        //  Displaying the text as info.
        infoText.text = info;

        //  If animator found, play the animator.
        if (animator)
            animator.Play(0);

    }
    
	public void OpenMissionPoup(string info,UnityAction onClick)
	{
		missionPopup.gameObject.SetActive(true);
		missionPopup.Setup(info,onClick,() => missionPopup.gameObject.SetActive(false));
	}
    
	public void CloseMissionPopup()
	{
		missionPopup.gameObject.SetActive(false);
	}
	
	public void OpenVictoryPopup(string info,bool hasNextMission,UnityAction nextOnClick = null)
	{
		missionVictoryPopup.gameObject.SetActive(true);
		missionVictoryPopup.Setup(info,nextOnClick,hasNextMission);
	}
	
	public void CloseVictoryPopup()
	{
		missionVictoryPopup.gameObject.SetActive(false);
	}
	
	public void OpenFailedPopup(string info,UnityAction restartOnClick)
	{
		missionFailedPopup.gameObject.SetActive(true);
		missionFailedPopup.Setup(info,restartOnClick);
	}
	
	public void CloseFailedPopup()
	{
		missionFailedPopup.gameObject.SetActive(false);
	}
}
