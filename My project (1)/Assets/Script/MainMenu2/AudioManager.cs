using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;

    public void PlaySFX(string sfxName)
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            if (audioClips[i].name == sfxName)
            {
                AudioSource.PlayClipAtPoint(audioClips[i], transform.position);
            }
        }
    }
}
