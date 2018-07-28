using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private const float width = 13;
    private const float height = 7.4f;

    public Animator mAnimator;
    public float speed;

    int positionX;
    int positionY;

    int HP;
    int Coins;

    void Start()
    {
        
    }

    public bool isMoving
    {
        get;
        set;
    }

    public int[] getPostion()
    {
        return new int[] { positionX, positionY };
    }

    public void setPostion(int x, int y)
    {
        positionX = x;
        positionY = y;
        transform.position = new Vector3(x * width, y * height, 0);
    }

    public int getHP()
    {
        return HP;
    }

    public void setHP(int h)
    {
        HP = h;
    }

    public int getCoins()
    {
        return Coins;
    }

    public void setCoins(int coins)
    {
        Coins = coins;
    }

    public void init(int x, int y, int h, int coins)
    {
        setPostion(x, y);
        setHP(h);
        setCoins(coins);
        speed = 0.1f;
    }

    public bool move(int x, int y)
    {
        if (x == 0 && y == 1 && !isMoving && positionY <= 3)
        {
            positionY++;
            StartCoroutine("uprun");
            return true;
        }
        if (x == -1 && y == 0 && !isMoving && positionX >= 1)
        {
            positionX--;
            StartCoroutine("leftrun");
            return true;
        }
        if (x == 0 && y == -1 && !isMoving && positionY >= 1)
        {
            positionY--;
            StartCoroutine("downrun");
            return true;
        }
        if (x == 1 && y == 0 && !isMoving && positionX <= 3)
        {
            positionX++;
            StartCoroutine("rightrun");
            return true;
        }
        return false;
    }

    IEnumerator uprun()
    {
        GetComponent<Animator>().SetInteger("dirY", 1);
        isMoving = true;//状态变为移动状态
        while (transform.position.y <= (positionY ) * height)
        {//map.moveY为两个格子之间y的距离，positionY表示目前角色的y坐标
            transform.Translate(0, speed, 0);
            yield return null;
        }
        GetComponent<Animator>().SetInteger("dirY", 0);
        isMoving = false;
        GameObject.Find("Canvas").GetComponent<UserInput>().setXY(0, 0);
    }
    IEnumerator downrun()
    {
        GetComponent<Animator>().SetInteger("dirY", -1);
        isMoving = true;
        while (transform.position.y >= (positionY ) * height)
        {
            transform.Translate(0, -speed, 0);
            yield return null;
        }
        GetComponent<Animator>().SetInteger("dirY", 0);
        isMoving = false;
        GameObject.Find("Canvas").GetComponent<UserInput>().setXY(0, 0);
    }

    IEnumerator leftrun()
    {
        GetComponent<Animator>().SetInteger("dirX", -1);
        isMoving = true;
        while (transform.position.x >= (positionX ) * width)
        {
            transform.Translate(-speed, 0, 0);
            yield return null;
        }
        GetComponent<Animator>().SetInteger("dirX", 0);
        isMoving = false;
        GameObject.Find("Canvas").GetComponent<UserInput>().setXY(0, 0);
    }

    IEnumerator rightrun()
    {
        GetComponent<Animator>().SetInteger("dirX", 1);
        isMoving = true;
        while (transform.position.x <= (positionX ) * width)
        {
            transform.Translate(speed, 0, 0);
            yield return null;
        }
        GetComponent<Animator>().SetInteger("dirX", 0);
        isMoving = false;
        GameObject.Find("Canvas").GetComponent<UserInput>().setXY(0, 0);
    }
}
