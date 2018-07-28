using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManage : MonoBehaviour {
    public int Monster_State = 0;
    public int Player_State = 0;
    public GameObject Monster;
    public GameObject Player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Change_State();
	}
    
    void Change_State()
    {
        Monster.GetComponent<Animator>().SetInteger("state", Monster_State);
        Player.GetComponent<Animator>().SetInteger("state", Player_State);
    }


}
