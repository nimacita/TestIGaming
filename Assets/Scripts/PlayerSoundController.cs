using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{

    [Header("Whings Sounds Settings")]
    public AudioClip[] whingsSounds;
    public AudioSource whingSoundSource;

    [Header("Raven Sounds Settings")]
    public AudioClip[] ravenSounds;
    public AudioSource ravenSoundSource;

    [Header("Fall Sounds Settings")]
    public AudioSource fallSource;
    private bool isPlayFall = false;

    //проигрываем случайный звук взмаха крыльев
    public void PlayWhingSound()
    {
        if (BgSoundsController.instance.Sound) whingSoundSource.PlayOneShot(whingsSounds[Random.Range(0, whingsSounds.Length)]);
    }

    //проигрываем случайный звук вороны
    public void PlayRavenSound()
    {
       if(BgSoundsController.instance.Sound) ravenSoundSource.PlayOneShot(ravenSounds[Random.Range(0, ravenSounds.Length)]);
    }

    //звук падения
    public void PlayFallSound()
    {
        if (BgSoundsController.instance.Sound && !isPlayFall)
        {
            fallSource.Play();
            isPlayFall = true;
        }
    }
}
