using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneStart : MonoBehaviour {

    public GameObject SetScene; //设置界面
    bool ShowSetScene = false;

    public Slider MusicSlider;
    public Slider SoundSlider;
    public AudioSource MusicAudio;
    public AudioSource ButtonSound;

    float MusicVolume;
    float SoundVolume;

    public SoundController soundController;

    void Start()
    {
        soundController = GameObject.Find("MusicFlag").GetComponent<SoundController>();
        if (!soundController.musicOpen)
        {
            MusicAudio.loop = true; //循环播放
            MusicAudio.volume = 0.5f;//设置初始音量大小，区间在0-1之间
            MusicSlider.value = 50f;
            MusicAudio.Play();
            soundController.musicOpen = true;
            DontDestroyOnLoad(MusicAudio);
        }
        else
        {
            Destroy(MusicAudio);
            MusicAudio = GameObject.Find("MusicAudio").GetComponent<AudioSource>();
            MusicSlider.value = MusicAudio.volume * 100;
        }
    }

    //开始游戏
    public void BeginGame()
    {
        ButtonSound.Play();
        SceneManager.LoadScene("Select");
    }

    //进入设置界面
    public void EnterSetScene()
    {
        ShowSetScene = !ShowSetScene;
        SetScene.SetActive(ShowSetScene);
    }

    //关闭设置界面
    public void CloseSetScene()
    {
        ShowSetScene = false;
        SetScene.SetActive(ShowSetScene);
    }

    //设置音乐和声音的大小
    public void OnValueChanged()
    {
        MusicSlider.onValueChanged.AddListener(delegate {
            MusicVolume = MusicSlider.value;
            MusicAudio.volume = MusicVolume / 100;
        });
        SoundSlider.onValueChanged.AddListener(delegate {
            SoundVolume = SoundSlider.value;
            ButtonSound.volume = SoundVolume / 100;
        });
    }
}
