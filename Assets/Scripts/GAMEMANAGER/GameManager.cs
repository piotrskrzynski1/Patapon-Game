using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using drumquality = RhythmManager.drumquality;
using static GameManager;
using System.Collections;
using UnityEditor;
public class GameManager : MonoBehaviour
{
    public RhythmManager rhythmManager;
    public PlayerController playerController;
    public bool isGameRunning = false;
    private List<drums> drumArray;
    private bool alreadyPressed = false;

    public int feverLevel = 0;


    [Header("UI")]
    public GameObject rhytmPanel;
    private Image rhytmPanelImage;
    public GameObject PataImage;
    public GameObject PonImage;
    public GameObject ChakaImage;
    public GameObject DonImage;
    [Header("AudioConfig")]
    public GameObject SoundeffectGO;
    public AudioClip errorclip;

    public AudioClip PataBest;
    public AudioClip PataGood;
    public AudioClip PataMissed;

    public AudioClip PonBest;
    public AudioClip PonGood;
    public AudioClip PonMissed;

    public AudioClip ChakaBest;
    public AudioClip ChakaGood;
    public AudioClip ChakaMissed;

    public AudioClip DonBest;
    public AudioClip DonGood;
    public AudioClip DonMissed;

    [Header("MusicProgression")]
    public AudioSource musicSource;
    public AudioClip noFever;
    public AudioClip noFeverFirstRhytm;
    public AudioClip feverStart;
    public AudioClip feverLoop;
    private AudioClip[] musicClips;
    public int rhytmCounter = 0;
    private List<drums> inputted;
    private int command = -1;

    [Header("PataponCommands")]
    public CommandState commandState;
    private RhytmState rhytmState = RhytmState.During;

    [Header("Yaripon settings")]
    public GameObject yariponManager;
    //singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Makes the instance persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        rhythmManager.OnBeat += HandleBeat; // Subscribe to beat events
        rhythmManager.BeatEnded += BeatEnded;
        StartGame();
        drumArray = new List<drums>();
        musicSource.volume = 0.4f;
        musicSource.clip = noFever;
        musicSource.Play();
        musicClips = new AudioClip[] { noFever, noFeverFirstRhytm, feverStart, feverLoop };
        inputted = new List<drums>();
        rhytmPanelImage = rhytmPanel.GetComponent<Image>();
    }

    public void StartGame()
    {
        isGameRunning = true;
        rhythmManager.StartRhythm();
    }

    public void StopGame()
    {
        isGameRunning = false;
        rhythmManager.StopRhythm();
    }

    private void HandleBeat(drumquality quality)
    {
        if (isGameRunning)
        {
            //je¿eli ostatnia klatka by³a klatk¹ z beatem w dodatku wykonalismy ju¿ komende to dodaj ¿e rhytm ju¿ min¹³
            if (rhytmState == RhytmState.After && rhytmPanelImage.color.a == 0) { rhytmPanelImage.color = new Vector4(0.3f, 0.3f, 0.3f, rhytmPanelImage.color.a); } else if (rhytmState == RhytmState.During && rhytmPanelImage.color.a == 0) { rhytmPanelImage.color = new Vector4(1f, 1f, 1f, rhytmPanelImage.color.a); }
            if (rhytmCounter == 4 && rhytmState == RhytmState.After)
            {
                rhytmCounter = 0;
                rhytmState = RhytmState.During;
                commandState = CommandState.IDLE;
            }

            HandleDrums(quality);
        }
    }

    private void HandleDrums(drumquality q)
    {

        if (playerController.IsPataPressed())
        {
            if (alreadyPressed || rhytmState == RhytmState.After) { Debug.Log("You don fucked up the rhytm bro"); q = drumquality.MISSED; feverLevel = 0; drumArray.Clear(); }
            Debug.Log("Pata pressed on beat, nice!");
            ShowDrumImageAnimation(drums.PATA);
            EvalulateDrum(q, drums.PATA, PataBest, PataGood, PataMissed);

        }
        if (playerController.IsPonPressed())
        {
            if (alreadyPressed || rhytmState == RhytmState.After) { Debug.Log("You don fucked up the rhytm bro"); q = drumquality.MISSED; feverLevel = 0; ; drumArray.Clear(); }
            Debug.Log("Pon pressed on beat");
            ShowDrumImageAnimation(drums.PON);
            EvalulateDrum(q, drums.PON, PonBest, PonGood, PonMissed);
        }
        if (playerController.IsChakaPressed())
        {
            if (alreadyPressed || rhytmState == RhytmState.After) { Debug.Log("You don fucked up the rhytm bro"); q = drumquality.MISSED; feverLevel = 0; ; drumArray.Clear(); }
            Debug.Log("Chaka pressed on beat");
            ShowDrumImageAnimation(drums.CHAKA);
            EvalulateDrum(q, drums.CHAKA, ChakaBest, ChakaGood, ChakaMissed);
        }
        if (playerController.IsDonPressed())
        {
            if (alreadyPressed || rhytmState == RhytmState.After) { Debug.Log("You don fucked up the rhytm bro"); q = drumquality.MISSED; feverLevel = 0; ; drumArray.Clear(); }
            Debug.Log("Don pressed on beat");
            ShowDrumImageAnimation(drums.DON);
            EvalulateDrum(q, drums.DON, DonBest, DonGood, DonMissed);
        }
    }

    void ShowDrumImageAnimation(drums drumType)
    {
        if (drumType == drums.PATA)
        {
            Color color = PataImage.GetComponent<Image>().color;
            color.a = 1f;
            PataImage.GetComponent<Image>().color = color;
        }
        if (drumType == drums.PON)
        {
            Color color = PonImage.GetComponent<Image>().color;
            color.a = 1f;
            PonImage.GetComponent<Image>().color = color;
        }
        if (drumType == drums.CHAKA)
        {
            Color color = ChakaImage.GetComponent<Image>().color;
            color.a = 1f;
            ChakaImage.GetComponent<Image>().color = color;
        }
        if (drumType == drums.DON)
        {
            Color color = DonImage.GetComponent<Image>().color;
            color.a = 1f;
            DonImage.GetComponent<Image>().color = color;
        }
    }
    public void EvalulateDrum(drumquality q, drums type, AudioClip Best, AudioClip Good, AudioClip Miss)
    {
        AudioSource drumsounds = this.AddComponent<AudioSource>();
        drumsounds.volume = 0.5f;
        if (rhytmState == RhytmState.After) { BreakFever();
            MistakeSoundEffect(); 
            rhytmCounter = 0; 
            rhytmState = RhytmState.During;
            drumsounds.clip = Miss;
            drumsounds.transform.parent = this.transform;
            drumsounds.Play();
            StartCoroutine(DestroyAfterPlaying(drumsounds));
            return; 
        }
        if (q == drumquality.BEST)
        {
            drumsounds.clip = Best;
            drumsounds.transform.parent = this.transform;
            drumsounds.Play();
            StartCoroutine(DestroyAfterPlaying(drumsounds));
            drumArray.Add(type);
            alreadyPressed = true;
            SendDrumSignalsToPataponTroops(type);
        }
        else if (q == drumquality.GOOD)
        {
            drumsounds.clip = Good;
            drumsounds.transform.parent = this.transform;
            drumsounds.Play();
            StartCoroutine(DestroyAfterPlaying(drumsounds));
            drumArray.Add(type);
            alreadyPressed=true;
            SendDrumSignalsToPataponTroops(type);
        }
        else if (q == drumquality.MISSED)
        {
            drumsounds.clip = Miss;
            drumsounds.transform.parent = this.transform;
            drumsounds.Play();
            StartCoroutine(DestroyAfterPlaying(drumsounds));
            if (drumArray.Count > 0)
            {
                BreakFever();
            }
            if (feverLevel > 0)
            {
                MistakeSoundEffect();
                BreakFever();
            }
            drumArray.Clear();
        }
    }

    int DetectCommand()
    {
        if (drumArray.Count >= 4) // Ensure we have enough input for a valid command
        {
            for (int i = 0; i < commands.GetLength(0); i++) // Iterate over rows of commands
            {
                bool foundCommand = true;

                // Iterate over columns, ensuring the index doesn't exceed the array bounds
                for (int j = 0; j < commands.GetLength(1); j++)
                {
                    if (j >= drumArray.Count || commands[i, j] != drumArray[j])
                    {
                        foundCommand = false;
                        break; // Mismatch found; break out of the loop
                    }
                }

                if (foundCommand) // Command match found
                {
                    Debug.LogWarning(i);
                    return i;
                }
            }
        }
        return -1; // No matching command found
    }

    private void BreakFever()
    {
        //used when no drums are struck in rhytm
        feverLevel = 0;
        drumArray.Clear();
        inputted.Clear();
        CheckFeverMusic();
    }
    private void MistakeSoundEffect()
    {
        AudioSource errorSound = SoundeffectGO.AddComponent<AudioSource>();
        errorSound.loop = false;
        errorSound.clip = errorclip;
        errorSound.Play();
        DestroyAfterPlaying(errorSound);
    }
    private bool AreDrumSequencesEqual(List<drums> list1, List<drums> list2)
    {
        if (list1.Count != list2.Count) return false;

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i]) return false;
        }

        return true;
    }

    void CheckFeverMusic()
    {
        if (feverLevel == 0 && musicSource.clip != noFever)
        {
            musicSource.clip = noFever;
            musicSource.loop = true;
            musicSource.Play();
        }
        if ((feverLevel > 0 && feverLevel < 8) && musicSource.clip != noFeverFirstRhytm)
        {
            musicSource.clip = noFeverFirstRhytm;
            musicSource.loop = false;
            musicSource.Play();
        }
        if (feverLevel == 8 && musicSource.clip != feverStart)
        {
            musicSource.clip = feverStart;
            musicSource.loop = false;
            musicSource.Play();
        }
        if (feverLevel > 8 && musicSource.clip != feverLoop)
        {
            musicSource.clip = feverLoop;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private void SendDrumSignalsToPataponTroops(drums drum)
    {
        YariponStateMachine[] yaripons = yariponManager.GetComponentsInChildren<YariponStateMachine>();
        if (drum == drums.PATA)
        {
            
            foreach(YariponStateMachine a in yaripons)
            {
                a.pataTrigger();
            }
        } else if(drum == drums.PON)
        {
            foreach (YariponStateMachine a in yaripons)
            {
                a.ponTrigger();
            }
        } else if (drum == drums.CHAKA)
        {

        } else if (drum == drums.DON)
        {

        }
    }
    private void BeatEnded()
    {
        command = DetectCommand();
        if (rhytmState == RhytmState.After)
        {
            inputted.Clear();
        }
        rhytmCounter++;

        if (inputted.Count > 0 || feverLevel > 0)
        {
            if (feverLevel == 0 && AreDrumSequencesEqual(inputted, drumArray))
            {
                Debug.LogWarning("Broken drum sequence");
                BreakFever();
                MistakeSoundEffect();
            }
            if (feverLevel > 0 && rhytmState == RhytmState.During && rhytmCounter != 0)
            {
                if (AreDrumSequencesEqual(inputted, drumArray))
                {
                    BreakFever();
                    MistakeSoundEffect();
                }
            }
        }
        if (command == 0) { Debug.Log("pata,pata,pata,pon!!"); drumArray.Clear(); rhytmState = RhytmState.After; feverLevel++; command = -1; rhytmCounter = 0; commandState = CommandState.PATA; }
        if (command == 1) { Debug.Log("pon,pon,pata,pon!!"); drumArray.Clear(); rhytmState = RhytmState.After; feverLevel++; command = -1; rhytmCounter = 0; commandState = CommandState.PON; }

        StartCoroutine(CheckMusicAfterDelay(1.8f));
        alreadyPressed = false;
        inputted = new List<drums>(drumArray); // Create a new copy of drumArray
    }

    private void OnDestroy()
    {
        rhythmManager.OnBeat -= HandleBeat; // Unsubscribe to prevent memory leaks
    }

    public enum drums
    {
        PATA,
        PON,
        DON,
        CHAKA
    }
    public drums[,] commands =
   {
        { drums.PATA,drums.PATA,drums.PATA,drums.PON }, //move command
        { drums.PON,drums.PON,drums.PATA,drums.PON }
    };
    public enum RhytmState
    {
        During,
        After
    }
    public enum CommandState
    {
        IDLE,
        PATA,
        PON
    }
    

    private System.Collections.IEnumerator DestroyAfterPlaying(AudioSource audioSource)
    {
        // Wait until the audio finishes playing
        while (audioSource.isPlaying)
        {
            yield return null; // Wait for the next frame
        }

        // Destroy the GameObject after the audio ends
        Destroy(audioSource);
    }

    private IEnumerator CheckMusicAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for 1.8 seconds
        CheckFeverMusic(); // Call the function after the delay
    }
}