using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public GameObject heavenSound;
    public GameObject hellSound;

    public GameObject menuSong;
    public List<GameObject> songs;

    private void Awake()
    {
        instance = this;
    }

    public void PlayMenuSong()
    {
        menuSong.SetActive(true);
    }

    public void PlayCharacterSong(int characterIndex)
    {
        songs[characterIndex].SetActive(true);
    }

    public void StopMusic(int characterIndex)
    {
        if (characterIndex <= 0)
        {
            menuSong.SetActive(false);
        }
        else
        {
            songs[characterIndex - 1].SetActive(false);
        }
    }

    public void PlayStampSound(StampType type)
    {
        switch (type)
        {
            case StampType.HEAVEN:
                heavenSound.GetComponent<StudioEventEmitter>().Play();
                break;
            case StampType.HELL:
                hellSound.GetComponent<StudioEventEmitter>().Play();
                break;
        }
    }

}
