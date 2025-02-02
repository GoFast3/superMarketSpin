using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get { return instance; }
    }

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip correctCollectSound;
    [SerializeField] private AudioClip wrongCollectSound;

    [Header("UI")]
    [SerializeField] private Button muteButton;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite muteIcon;
    [SerializeField] private Sprite unmuteIcon;

    private bool isMuted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
        }

        if (muteButton != null)
        {
            if (buttonImage != null)
            {
                buttonImage.sprite = unmuteIcon;
            }
        }
    }

    private void ToggleMute()
    {
        Debug.Log("muutee?? " + isMuted);
        isMuted = !isMuted;
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? muteIcon : unmuteIcon;
        }
    }

    public void PlayCorrectCollect()
    {
        if (!isMuted && correctCollectSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(correctCollectSound);
        }
    }

    public void PlayWrongCollect()
    {
        if (!isMuted && wrongCollectSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(wrongCollectSound);
        }
    }
}