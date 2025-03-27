using UnityEngine;
using UnityEngine.UI;
public class RhythmManager : MonoBehaviour
{
    [SerializeField]
    private float bpm = 120f; // Beats per minute
    public float beatWindow = 0.2f; // Time window for input accuracy
    private float beatInterval; // Time between beats
    private float nextBeatTime; // Time for the next beat
    private float lastBeatTime; // Time of the last beat
    private bool isPlaying = false;
    public GameObject rhytmPanel;
    private Image rhytmPanelImage;
    bool panelAlphaIncreased = false;

    public delegate void OnBeatEvent(drumquality quality);
    public event OnBeatEvent OnBeat; // Triggered on every beat
    public delegate void BeatEndedEvent();
    public event BeatEndedEvent BeatEnded;

    public float BPM
    {
        get => bpm;
        set
        {
            bpm = value;
            UpdateBeatInterval();
        }
    }

    private void Start()
    {
        rhytmPanelImage = rhytmPanel.GetComponent<Image>();
        UpdateBeatInterval();
    }

    private void Update()
    {
        if (!isPlaying) return;

        float currentTime = Time.unscaledTime;

        if (IsWithinBeatWindow(currentTime, nextBeatTime))
        {
            float timeToBeat = Mathf.Abs(currentTime - nextBeatTime);
            if (timeToBeat <= beatWindow / 2)
            {
                OnBeat?.Invoke(drumquality.BEST);
            }
            else if (timeToBeat <= beatWindow)
            {
                OnBeat?.Invoke(drumquality.GOOD);
            }
        }
        else
        {
            OnBeat?.Invoke(drumquality.MISSED);
        }

        if (currentTime >= nextBeatTime)
        {
            //make rhytm panel POP POP POP YEAAAH
            if (!panelAlphaIncreased)
            {
                Color color = rhytmPanelImage.color;
                color.a = 1f;
                rhytmPanelImage.color = color;
                panelAlphaIncreased = true;
            }
            if (currentTime >= nextBeatTime + beatWindow)
            {
                lastBeatTime = nextBeatTime;
                nextBeatTime += beatInterval;
                panelAlphaIncreased = false;
                BeatEnded?.Invoke();
            }
        }
    }

    public void StartRhythm()
    {
        isPlaying = true;
        lastBeatTime = Time.time;
        nextBeatTime = lastBeatTime + beatInterval;
    }

    public void StopRhythm()
    {
        isPlaying = false;
    }

    private bool IsWithinBeatWindow(float currentTime, float beatTime)
    {
        return currentTime >= (beatTime - beatWindow) && currentTime <= (beatTime + beatWindow);
    }


    private void UpdateBeatInterval()
    {
        beatInterval = 60f / bpm;
    }
    public enum drumquality
    {
        BEST,
        GOOD,
        MISSED
    }
}
