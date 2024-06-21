using System.Collections;
using UnityEngine;

public class BgSoundsController : MonoBehaviour
{


    [Header("Ambient Sounds")]
    [SerializeField] private AudioSource ambientSource;

    [Header("Bg Music")]
    [SerializeField] private AudioSource bgMusicSource;

    [Header("Horror Sounds")]
    [Tooltip("задержка между хоррор звуками в секундах, от-до")]
    [SerializeField] private Vector2 horrorSoundDelay;
    [Tooltip("шанс проигрывания звука, 10 - 100%  0 - 0%")]
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
        //включаем музыку
        bgMusicSource.Play();
        //включаем фоновые звуки
        ambientSource.Play();
        //запускаем ожидание хоррор звуков
        StartCoroutine("WaitHorrorSound");
    }

    //сохраненное значение включенное музыки
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

    //сохраненное значение включенных звуков
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

    //обновляем значения музыки и звуков
    private void UpdateSounds()
    {
        bgMusicSource.mute = !Music;
        horrorSource.mute = !Sound;
        ambientSource.mute = !Sound;
    }

    //задержка между хоррор звуками
    private IEnumerator WaitHorrorSound()
    {
        //случайное количество секунд ожидания
        float sec = Random.Range(horrorSoundDelay.x,horrorSoundDelay.y);
        //Debug.Log($" sec {sec}");
        yield return new WaitForSeconds(sec);
        IsPlayHorrorSound();
    }

    //играем ли звук хоррор
    private void IsPlayHorrorSound()
    {
        int chance = Random.Range(0, (10 - horrorSoundChance) + 1);
        //Debug.Log($" chance {chance}");
        if (chance == 0 && horrorSoundChance != 0) 
        {
            //играем случайный звук
            horrorSource.PlayOneShot(horrorClips[Random.Range(0,horrorClips.Length)]);
        }
        //перезапусакаем карутину
        StartCoroutine("WaitHorrorSound");
    }


}
