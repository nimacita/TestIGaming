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

    //����������� ��������� ���� ������ �������
    public void PlayWhingSound()
    {
        if (BgSoundsController.instance.Sound) whingSoundSource.PlayOneShot(whingsSounds[Random.Range(0, whingsSounds.Length)]);
    }

    //����������� ��������� ���� ������
    public void PlayRavenSound()
    {
       if(BgSoundsController.instance.Sound) ravenSoundSource.PlayOneShot(ravenSounds[Random.Range(0, ravenSounds.Length)]);
    }

    //���� �������
    public void PlayFallSound()
    {
        if (BgSoundsController.instance.Sound && !isPlayFall)
        {
            fallSource.Play();
            isPlayFall = true;
        }
    }
}
