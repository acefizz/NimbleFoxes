using UnityEngine;
using UnityEngine.Audio;

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

    [SerializeField] private AudioMixer gameAudio;
    [SerializeField] private AudioClip exampleVolume;
    [SerializeField] private AudioSource source;

    private void Awake()
    {
        DontDestroyOnLoad(GameManager.instance.GetComponent<SoundManager>());
        source = GameManager.instance.GetComponent<AudioSource>();
    }
    public void SetVolume(float sliderValue)
    {
        gameAudio.SetFloat("MainVolume", Mathf.Log10(sliderValue) * 20);
        //ExampleSound();
    }
    public void SetMusicVolume(float sliderValue)
    {
        gameAudio.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        //ExampleSound();
    }
    public void SetSfxVolume(float sliderValue)
    {
        gameAudio.SetFloat("SfxVolume", Mathf.Log10(sliderValue) * 20);
        ExampleSound();
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
}
