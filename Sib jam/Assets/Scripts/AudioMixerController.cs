using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [System.Serializable]
    public class AudioChannel
    {
        public string name;
        public Slider slider;
        public string mixerParameter;
    }

    [Header("���������")]
    public AudioMixer mixer;
    public AudioChannel[] channels;

    private void Start()
    {
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        foreach (var channel in channels)
        {
            if (channel.slider == null)
            {
                Debug.LogError($"������� ��� {channel.name} �� ��������!", this);
                continue;
            }

            // ��������� ������� �������� �� �������
            if (mixer.GetFloat(channel.mixerParameter, out float dbValue))
            {
                // ������������ dB � �������� 0-1
                channel.slider.value = DBToLinear(dbValue);
            }
            else
            {
                Debug.LogError($"�� ������� �������� �������� {channel.mixerParameter}", this);
                continue;
            }

            // ��������� ���������� ���������
            channel.slider.onValueChanged.AddListener((value) =>
            {
                SetVolume(channel.mixerParameter, value);
            });
        }
    }

    private void SetVolume(string parameter, float linearValue)
    {
        // ������������ 0-1 � dB
        float dbValue = LinearToDB(linearValue);

        // ������������� ���������
        if (!mixer.SetFloat(parameter, dbValue))
        {
            Debug.LogError($"�� ������� ���������� {parameter}", this);
        }

        Debug.Log($"{parameter} ���������� �� {dbValue}dB (input: {linearValue})");
    }

    private float LinearToDB(float linear)
    {
        return linear <= 0.0001f ? -80f : Mathf.Log10(linear) * 20f;
    }

    private float DBToLinear(float db)
    {
        return Mathf.Pow(10f, db / 20f);
    }

    // ����� ��� �������� �����������
    public void TestConnection()
    {
        foreach (var channel in channels)
        {
            if (mixer.GetFloat(channel.mixerParameter, out float value))
            {
                Debug.Log($"{channel.mixerParameter} = {value}dB");
            }
            else
            {
                Debug.LogError($"{channel.mixerParameter} �� ��������!");
            }
        }
    }
}