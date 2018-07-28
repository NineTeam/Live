using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{

    public static SaveData instance;

    //要保存使用的数据; 
    public int[] lastPosition;
    public int[] position;
    public int HP;
    public int[,] blocks;
    public int[,] status;
    public int Coins;

    public int hintTimes;
    public Dictionary<int[], string> hints;

    public bool isWin;
    public bool isRebuild = false;
    public int chapter;


    //初始化
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }
}
