using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManage : MonoBehaviour {

    public int Hp;//血量
    public int Ep;//精力
    public float Speed;//速度
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void move()
    {
        transform.Translate(-1, 0, 0);
    }
}
