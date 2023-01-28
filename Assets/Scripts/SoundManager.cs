using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region FirstAttempt
    //[Header("___| Main Volume Settings |___")]
    //public Slider volumeSlider;
    //float volume;
    //AudioSource[] audioSourceList;

    //[Header("___| Music Volume Settings |___")]
    //public Slider musicSlider;
    //float musicVolume;
    //[SerializeField] AudioClip[] musicList;

    //[Header("___| SFX Volume Settings |___")]
    //public Slider sfxSlider;
    //float sfxVolume;
    //[SerializeField] AudioClip[] sfxList;


    //// Start is called before the first frame update
    //void Start()
    //{
    //    sfxList = FindObjectsOfType<AudioClip>();
    //    audioSourceList = FindObjectsOfType<AudioSource>();

    //    musicSlider.value = musicVolume;
    //    sfxSlider.value = sfxVolume;
    //    volumeSlider.value = volume;
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    //public void Apply()
    //{

    //}
    //public void AdjustAll()
    //{
    //    volumeSlider.value = volume;
    //    musicSlider.value = musicVolume;
    //    sfxSlider.value = sfxVolume;

    //    AdjustVolume();
    //    AdjustSFX();
    //    AdjustMusic();
    //}
    //public void AdjustSFX()
    //{
    //    sfxVolume = sfxSlider.value;
    //    for (int i = 0; i < sfxList.Length; i++)
    //    {
    //        AudioSource temp = sfxList[i].GetComponent<AudioSource>();
    //        temp.volume = (volume + sfxVolume) / 2;
    //    }
    //}
    //public void AdjustMusic()
    //{
    //    musicVolume = musicSlider.value;
    //    for (int i = 0; i < musicList.Length; i++)
    //    {
    //        AudioSource temp = musicList[i].GetComponent<AudioSource>();
    //        temp.volume = (volume + musicVolume) / 2;
    //    }
    //}
    //public void AdjustVolume()
    //{
    //    volume = volumeSlider.value;
    //    for (int i = 0; i < audioSourceList.Length; i++)
    //    {
    //        audioSourceList[i].volume = (volume + audioSourceList[i].volume) / 2;
    //    }
    //} 
    #endregion

    public AudioMixer gameAudio;
    [SerializeField] private AudioClip exampleVolume;
    [SerializeField] private AudioSource source;

    public Slider main;
    public Slider music;
    public Slider sfx;

    float mainVol;
    float musicVol;
    float sfxVol;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        source = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();
    }

    public void SetVolume(float sliderValue)
    {
        gameAudio.SetFloat("MainVolume", Mathf.Log10(sliderValue) * 20);
        gameAudio.GetFloat("MainVolume", out mainVol);
        //ExampleSound();
    }
    public void SetMusicVolume(float sliderValue)
    {
        gameAudio.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        gameAudio.GetFloat("MusicVolume", out musicVol);
        //ExampleSound();
    }
    public void SetSfxVolume(float sliderValue)
    {
        gameAudio.SetFloat("SfxVolume", Mathf.Log10(sliderValue) * 20);
        ExampleSound();
        gameAudio.GetFloat("SfxVolume", out sfxVol);
    }
    void ExampleSound()
    {
        source.PlayOneShot(exampleVolume);
    }
    public void PlayMusic()
    {
        if(source.isPlaying) { return; }
        source.Play();
    }
    public void LoadVolume()
    {
        gameAudio.SetFloat("MusicVolume", Mathf.Log10(musicVol) * 20);
        music.value = musicVol;
        gameAudio.SetFloat("SfxVolume", Mathf.Log10(sfxVol) * 20);
        sfx.value = sfxVol;
        gameAudio.SetFloat("MainVolume", Mathf.Log10(mainVol) * 20);
        main.value = mainVol;
    }
}
