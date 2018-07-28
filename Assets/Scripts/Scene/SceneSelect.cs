using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{
    public Image CommonSingle;
    public Image CommonMulti;

    public GameObject PleaseExpect; //设置界面
    bool ShowExpectScene = false;

    AudioSource music;
    AudioSource sound;

    // Use this for initialization
    void Start()
    {
    }

    public void SinglePattern()
    {
        //切换到单人模式的游戏场景中...
        SceneManager.LoadScene("Explore");
    }

    public void MultiPattern()
    {
        //切换到多人模式的游戏场景中
        //...
        ShowExpectScene = !ShowExpectScene;
        PleaseExpect.SetActive(ShowExpectScene);
    }

    //返回开始界面
    public void ReturnScene1()
    {
        SceneManager.LoadScene("Start");
    }
}
