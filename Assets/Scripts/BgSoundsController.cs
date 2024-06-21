using System.Collections;
using UnityEngine;

public class BgSoundsController : MonoBehaviour
{


    [Header("Ambient Sounds")]
    [SerializeField] private AudioSource ambientSource;

    [Header("Bg Music")]
    [SerializeField] private AudioSource bgMusicSource;

    [Header("Horror Sounds")]
    [Tooltip("�������� ����� ������ ������� � ��������, ��-��")]
    [SerializeField] private Vector2 horrorSoundDelay;
    [Tooltip("���� ������������ �����, 10 - 100%  0 - 0%")]
    [SerializeField, Range(0,10)] private int horrorSoundChance;
    [SerializeField] private AudioSource horrorSource;
    [SerializeField] private AudioClip[] horrorClips;

    public static BgSoundsController instance;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        UpdateSounds();
        PlayOnStart();
    }

    public void PlayOnStart()
    {
        //�������� ������
        bgMusicSource.Play();
        //�������� ������� �����
        ambientSource.Play();
        //��������� �������� ������ ������
        StartCoroutine("WaitHorrorSound");
    }

    //����������� �������� ���������� ������
    public bool Music
    {
        get
        {
            if (PlayerPrefs.HasKey("music"))
            {
                if (PlayerPrefs.GetInt("music") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                PlayerPrefs.SetInt("music", 1);
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("music", 1);
            }
            else
            {
                PlayerPrefs.SetInt("music", 0);
            }
            UpdateSounds();
        }
    }

    //����������� �������� ���������� ������
    public bool Sound
    {
        get
        {
            if (PlayerPrefs.HasKey("sound"))
            {
                if (PlayerPrefs.GetInt("sound") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                PlayerPrefs.SetInt("sound", 1);
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("sound", 1);
            }
            else
            {
                PlayerPrefs.SetInt("sound", 0);
            }
            UpdateSounds();
        }

    }

    //��������� �������� ������ � ������
    private void UpdateSounds()
    {
        bgMusicSource.mute = !Music;
        horrorSource.mute = !Sound;
        ambientSource.mute = !Sound;
    }

    //�������� ����� ������ �������
    private IEnumerator WaitHorrorSound()
    {
        //��������� ���������� ������ ��������
        float sec = Random.Range(horrorSoundDelay.x,horrorSoundDelay.y);
        //Debug.Log($" sec {sec}");
        yield return new WaitForSeconds(sec);
        IsPlayHorrorSound();
    }

    //������ �� ���� ������
    private void IsPlayHorrorSound()
    {
        int chance = Random.Range(0, (10 - horrorSoundChance) + 1);
        //Debug.Log($" chance {chance}");
        if (chance == 0 && horrorSoundChance != 0) 
        {
            //������ ��������� ����
            horrorSource.PlayOneShot(horrorClips[Random.Range(0,horrorClips.Length)]);
        }
        //�������������� ��������
        StartCoroutine("WaitHorrorSound");
    }


}
