using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("����� ���������")]
    [SerializeField] private AudioClip[] appearSounds;
    [SerializeField] private AudioSource appearAudioSource;
    //[SerializeField] private float appearVolume = 0.7f;

    [Header("����� ������������")]
    [SerializeField] private AudioClip[] disappearSounds;
    [SerializeField] private AudioSource disappearAudioSource;

    private bool isCanPlaySound;
    //[SerializeField] private float disappearVolume = 0.7f;

    private void Awake()
    {
        // ������������� AudioSource ��� ���������
        if (appearAudioSource == null)
        {
            appearAudioSource = gameObject.AddComponent<AudioSource>();
            appearAudioSource.playOnAwake = false;
        }

        // ������������� AudioSource ��� ������������
        if (disappearAudioSource == null)
        {
            disappearAudioSource = gameObject.AddComponent<AudioSource>();
            disappearAudioSource.playOnAwake = false;
        }
    }


    public void SetActivePlatform(bool active)
    {
        isCanPlaySound = true;
        gameObject.SetActive(active);
    }


    private void OnEnable()
    {
        if (isCanPlaySound)
        {
            PlayRandomAppearSound();
            isCanPlaySound = false;
        }
       
    }

    private void OnDisable()
    {
        print("asdnkasnYYYYY");
        if (isCanPlaySound)
        {
            PlayRandomDisappearSound();
            isCanPlaySound = false;
        }
    }

    public void PlayRandomAppearSound()
    {
        if (appearSounds.Length > 0 && appearAudioSource != null)
        {
            AudioClip clip = appearSounds[Random.Range(0, appearSounds.Length)];
            appearAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("����� ��������� �� ��������� ��� AudioSource �����������!");
        }
    }

    public void PlayRandomDisappearSound()
    {
        if (disappearSounds.Length > 0 && disappearAudioSource != null)
        {
            AudioClip clip = disappearSounds[Random.Range(0, disappearSounds.Length)];
            disappearAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("����� ������������ �� ��������� ��� AudioSource �����������!");
        }
    }
}