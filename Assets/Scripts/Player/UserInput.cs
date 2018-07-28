using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mySpace;

public class UserInput : MonoBehaviour
{

    private IUserAction action;

    public int x, y;

    public void setXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void moveLeft()
    {
        x = -1;
        y = 0;
    }

    public void moveRight()
    {
        x = 1;
        y = 0;
    }

    public void moveUp()
    {
        x = 0;
        y = 1;
    }

    public void moveDown()
    {
        x = 0;
        y = -1;
    }

    // Use this for initialization
    void Start () {
        action = MainManager.GetInstance().currentSceneController as IUserAction;
	}
	
	// Update is called once per frame
	void Update () {
        action.move(x, y);
	}
}
