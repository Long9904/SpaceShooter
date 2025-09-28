using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    public static AudioManagement instance;
    private AudioSource source;
    [Header("Audio Clips")]
    public AudioClip buttonClick;
    //public AudioClip gameOver;
    public AudioClip gameWin;
    public AudioClip shooter;
    public AudioClip hit;
    public AudioClip lootItem;
    public AudioClip cutsceneBoss;
    public AudioClip hitEn;
    public AudioClip backgroundMusic;
    public AudioClip backgroundBossMusic;
    public AudioClip bossSumon;
    public AudioClip bossDie;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        source = GetComponent<AudioSource>();
    }

    public void PlayButtonClick()
    {
        source.PlayOneShot(buttonClick);
    }

    //public void PlayGameOver()
    //{
    //    source.PlayOneShot(gameOver);
    //}
    public void PlayGameWin()
    {
        source.PlayOneShot(gameWin);
    }
    public void PlayShooter()
    {
        source.PlayOneShot(shooter);
    }
    public void PlayHit()
    {
        source.PlayOneShot(hit);
    }
    public void PlayLootItem()
    {
        source.PlayOneShot(lootItem);
    }
    public void PlayCutsceneBoss()
    {
        source.PlayOneShot(cutsceneBoss);
    }
    public void PlayHitEn()
    {
        source.PlayOneShot(hitEn);
    }
    public void PlayBossSumon()
    {
        source.PlayOneShot(bossSumon);
    }
    public void PlayBackgroundMusic()
    {
        source.clip = backgroundMusic;
        source.loop = true;
        source.Play();
    }
   
    public void StopMusic()
    {
        source.Stop();
    }
    public void PlayBackgroundBossMusic()
    {
        source.clip = backgroundBossMusic;
        source.loop = true;
        source.Play();
    }
    public void PlayBossDie()
    {
        source.clip = bossDie;
        source.loop = false;
        source.volume = 0.4f;
        source.Play();
    }
}
