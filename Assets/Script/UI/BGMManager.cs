using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    public AudioSource BGM;

    public AudioClip[] BGMClips;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Start()
    {
        instance = this;
        BGM = GetComponent<AudioSource>();
        BGM.clip = BGMClips[0];
    }

    public void Play()
    {
        BGM.Play();
        BGM.volume = SoundSlider.BGMVolume;
    }


    public void ChangeBGM(int index)
    {
        StartCoroutine(FadeOutBGM(index));
    }

    IEnumerator FadeOutBGM(int index)
    {
        float Volume = SoundSlider.BGMVolume;

        for(float i = Volume; i >= 0f; i -= 0.1f)
        {
            BGM.volume = i;
            yield return waitTime;
        }

        BGM.clip = BGMClips[index];
        BGM.Play();
        yield return null;

        for (float i = 0f; i <= Volume; i += 0.1f)
        {
            BGM.volume = i;
            yield return waitTime;
        }
        yield return null;
    }

    IEnumerator FadeInBGM()
    {
        for (float i = 0f; i <= 1.0f; i += 0.1f)
        {
            BGM.volume = i;
            yield return waitTime;
        }
    }
}
