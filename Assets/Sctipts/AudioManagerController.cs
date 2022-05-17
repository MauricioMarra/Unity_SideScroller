using UnityEngine;

public class AudioManagerController : MonoBehaviour
{
    public AudioSource sounds;
    public static AudioManagerController audioManager = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (audioManager == null)
            audioManager = this;
    }

    public void PlaySound(AudioClip sound)
    {
        sounds.clip = sound;
        sounds.Play();
    }

    public void PlaySoundOneShot(AudioClip sound, float volume = 1.0f)
    {
        sounds.PlayOneShot(sound, volume);
    }
}
