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
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Soundtrack manager.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Managers/CCDS Soundtrack Manager")]
public class CCDS_SoundtrackManager : ACCDS_Manager {

    private static CCDS_SoundtrackManager instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_SoundtrackManager Instance;

    /// <summary>
    /// Audiosource.
    /// </summary>
    public AudioSource SoundtrackSource {

        get {

            if (soundtrackSource == null)
                soundtrackSource = GetComponent<AudioSource>();

            if (soundtrackSource == null)
                soundtrackSource = RCCP_AudioSource.NewAudioSource(gameObject, "CCDS_SountrackSource", 0f, 0f, CCDS_Settings.Instance.defaultMusicVolume, null, true, false, false);

            return soundtrackSource;

        }

    }

    private AudioSource soundtrackSource;

    /// <summary>
    /// Main menu soundtracks.
    /// </summary>
    public List<AudioClip> garageSountracks = new List<AudioClip>();

    /// <summary>
    /// Casual gameplay soundtracks.
    /// </summary>
    public List<AudioClip> casualSountracks = new List<AudioClip>();

    /// <summary>
    /// Pursuit gameplay soundtracks.
    /// </summary>
    public List<AudioClip> pursuitSoundtracks = new List<AudioClip>();

    /// <summary>
    /// Ignore AudioListener pause for this audiosource.
    /// </summary>
    public bool ignorePause = false;

    /// <summary>
    /// Maximum volume of this audiosource.
    /// </summary>
    [Range(.1f, 1f)] public float maximumVolume = .65f;

    private void Awake() {

        //  Getting static instance of the gameobject and marking as dont destroy.
        if (Instance != null) {

            Destroy(gameObject);
            return;

        } else {

            Instance = this;
            DontDestroyOnLoad(gameObject);

        }

        //  Setting ignoreListenerPause.
        SoundtrackSource.ignoreListenerPause = ignorePause;

        //  Stop the audiosource on awake.
        Stop();

        //  Setting the music volume level with latest saved value.
        //  Setting the general volume level with latest saved value.
        CCDS_Events_OnAudioChanged();

    }

    private void OnEnable() {

        //  Listening an event when audio has been changed by the player.
        CCDS_Events.OnAudioChanged += CCDS_Events_OnAudioChanged;

    }

    /// <summary>
    /// Listening an event when audio has been changed by the player.
    /// </summary>
    private void CCDS_Events_OnAudioChanged() {

        //  Setting the music volume level with latest saved value.
        SetMusicVolume(CCDS.GetMusicVolume());

        //  Setting the general volume level with latest saved value.
        SetGeneralVolume(CCDS.GetAudioVolume());

    }

    private void Update() {

        //  Limiting the volume level.
        if (SoundtrackSource.volume > maximumVolume)
            SoundtrackSource.volume = maximumVolume;

        //  If this scene is garage / main menu scene, play the main menu soundtracks.
        if (SceneManager.GetActiveScene().buildIndex == CCDS_Settings.Instance.mainMenuSceneIndex) {

            //  If audioclip has been selected...
            if (garageSountracks.Count > 0) {

                //  Random audioclip in the list.
                AudioClip randomClip = garageSountracks[Random.Range(0, garageSountracks.Count)];

                //  And playing the audioclip.
                if (!garageSountracks.Contains(SoundtrackSource.clip))
                    PlayClip(randomClip);
            }

            return;

        }

        //  If the scene is not a garage / main menu scene, continue.
        //  Finding the gameplay manager.
        CCDS_GameplayManager sceneManager = CCDS_GameplayManager.Instance;

        //  Return if it doesn't exist.
        if (!sceneManager)
            return;

        //  Finding the player.
        CCDS_Player player = sceneManager.player;

        //  Return if it doesn't exist.
        if (!player)
            return;

        //  If player is not in pursue, play the casual audioclips.
        if (!player.inPursue) {

            //  If audioclip has been selected...
            if (casualSountracks.Count > 0) {

                //  Random audioclip in the list.
                AudioClip randomClip = casualSountracks[Random.Range(0, casualSountracks.Count)];

                //  And playing the audioclip.
                if (!casualSountracks.Contains(SoundtrackSource.clip))
                    PlayClip(randomClip);

            }

        } else {

            //  If audioclip has been selected...
            if (pursuitSoundtracks.Count > 0) {

                //  Random audioclip in the list.
                AudioClip randomClip = pursuitSoundtracks[Random.Range(0, pursuitSoundtracks.Count)];

                //  And playing the audioclip.
                if (!pursuitSoundtracks.Contains(SoundtrackSource.clip))
                    PlayClip(randomClip);

            }

        }

    }

    /// <summary>
    /// Plays the target audioclip.
    /// </summary>
    /// <param name="newClip"></param>
    public void PlayClip(AudioClip newClip) {

        SoundtrackSource.clip = newClip;
        SoundtrackSource.Play();

    }

    /// <summary>
    /// Sets the music volume without saving it.
    /// </summary>
    /// <param name="newVolume"></param>
    public void SetMusicVolume(float newVolume) {

        SoundtrackSource.volume = newVolume;

    }

    /// <summary>
    /// Sets the general volume without saving it.
    /// </summary>
    /// <param name="newVolume"></param>
    public void SetGeneralVolume(float newVolume) {

        AudioListener.volume = newVolume;

    }

    /// <summary>
    /// Stop!
    /// </summary>
    public void Stop() {

        SoundtrackSource.clip = null;
        SoundtrackSource.Stop();

    }

    private void OnDisable() {

        CCDS_Events.OnAudioChanged -= CCDS_Events_OnAudioChanged;

    }

}

