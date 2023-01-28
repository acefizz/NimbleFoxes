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


    [Header("--- Options Components ---")]
    public Slider main;
    public Slider music;
    public Slider sfx;

    public float[] options = new float[3];
    public float[] optionsSlider = new float[3];

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        source = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();

        LoadOptions();
    }

    public void SetVolume(float sliderValue)
    {
        gameAudio.SetFloat("MainVolume", Mathf.Log10(sliderValue) * 20);
        gameAudio.GetFloat("MainVolume", out options[0]);
        optionsSlider[0] = sliderValue;
        SaveOptions();
        //ExampleSound();
    }
    public void SetMusicVolume(float sliderValue)
    {
        gameAudio.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        gameAudio.GetFloat("MusicVolume", out options[1]);
        optionsSlider[1] = sliderValue;
        SaveOptions();
        //ExampleSound();
    }
    public void SetSfxVolume(float sliderValue)
    {
        gameAudio.SetFloat("SfxVolume", Mathf.Log10(sliderValue) * 20);
        gameAudio.GetFloat("SfxVolume", out options[2]);
        optionsSlider[2] = sliderValue;
        SaveOptions();
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

    public void LoadOptions()
    {
        OptionData data;

        if (GameDataSave.LoadOptionData() != null)
        {
            data = GameDataSave.LoadOptionData();

            for (int i = 0; i < options.Length; ++i)
            {
                options[i] = data.options[i];
            }

            for (int i = 0; i < optionsSlider.Length; ++i)
            {
                optionsSlider[i] = data.optionsSlider[i];
            }

            main.value = optionsSlider[0];
            music.value = optionsSlider[1];
            sfx.value = optionsSlider[2];
        }
    }

    public void SaveOptions()
    {
        GameDataSave.SaveOptionData(this);
    }
}
