using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStatus : MonoBehaviour
{
    private const int row = 5;
    private const int col = 5;
    int[,] blocks = new int[row, col];
    int[,] status = new int[row, col];

    Dictionary<int[], string> hintOfBlocks;

    public int[,] getBlocks()
    {
        return blocks;
    }

    public void setBlocks(int[,] blocks)
    {
        this.blocks = blocks;
    }

    public int[,] getStatus()
    {
        return status;
    }

    public void setStatus(int[,] s)
    {
        status = s;
    }

    public string getHintByBlock(int[] xy)
    {
        return hintOfBlocks[xy];
    }

    public void addHintInBlock(int[] xy, string hint)
    {
        hintOfBlocks.Add(xy, hint);
    }

    public Dictionary<int[], string> getHints()
    {
        return hintOfBlocks;
    }

    public void setHints(Dictionary<int[], string> hints)
    {
        hintOfBlocks = hints;
    }

    public void init(int[,] b, int[,] s)
    {
        blocks = b;
        status = s;
    }

    public int[,] CreateBlocks()
    {
        int[,] result = new int[5, 5];
        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                result[i, j] = 1;
            }
        }
        return result;
    }

    public int[,] CreateStatus()
    {
        int[,] result = new int[5, 5];
        int count = 0;
        //根据随机产生的概率值确定，地图上怪物的位置
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (i == 4 && j == 1)           //设置入口
                {
                    result[i, j] = 0;
                }
                else if ((i == 0 && j == 3))    //设置出口
                {
                    result[i, j] = 4;
                }
                else if ((i == 1 && j == 1) || (i == 3 && j == 3))
                {
                    result[i, j] = 3;
                }
                else if (1 == Random.Range(0, 5))
                {
                    result[i, j] = 1;
                    ++count;
                }
                else
                {
                    result[i, j] = 2;
                }
            }
            if (count >= 7)
                break;
            if (count < 3)
            {
                result[Random.Range(0, 5), Random.Range(0, 5)] = 1;
                result[Random.Range(0, 5), Random.Range(0, 5)] = 1;
            }
        }
        return result;
    }


    public void explore(int x, int y)
    {
        blocks[x, y] = 0;
    }

    public void clearStatus(int x, int y)
    {
        status[x, y] = 0;
    }

}
