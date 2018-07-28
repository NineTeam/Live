using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManage : MonoBehaviour
{
    public GameObject Monster;//怪物
    public GameObject Player;//主角
    //public GameObject Leftmove;
    //public GameObject Rightmove;
    //public GameObject Leftmove_Highlight;
    //public GameObject Rightmove_Highlight;
    public GameObject Airwall;//空气墙，防止玩家脱离屏幕
    public GameObject Animator_Manage;//动画管理器
    public GameObject Hpbar;//血条
    public GameObject Epbar;//精力条
    public GameObject Weapon_Closecombat;//近战武器
    public GameObject Weapon_Farcombat;//远程武器
    public GameObject[] Weapon_Data;//武器数据
    public GameObject Sprite_Playerdead;
    public GameObject Sprite_Monsterdead;
    public GameObject Button_Leftmove;
    public GameObject Button_Rightmove;
    public float Player_Speed;//玩家移动速度
    public float Monster_Speed;
    public float Cloest_Player_Monster;//怪物与玩家的最近距离
    public float Attack_Monster;//怪物有效攻击范围
    public float Attack_Player;//玩家有效攻击范围
    public float Close_Angle_Z;//近战武器旋转z轴
    public int Own_Weapon;//玩家拥有的武器
    public int Player_State;//玩家状态
    public int Monster_State = 1;//怪物状态
    public float Helptime;//辅助时间操作设置
    public float Monster_Attack_Waiting = 0.4f;//攻击延迟时间
    public float Monster_Fullhp;//怪物血量总量
    public bool isAddforce = false;
    public bool isAttrack = false;
    public bool isPlayer_Alive = true;//是否活着
    public bool isMonster_Alive = true;
    const int Movestate = 1;
    const int Attackstate = -1;
    const int Waitstate = 0;
    private CharacterManage Monster_Attribute;
    private CharacterManage Player_Attribute;
    private SpriteRenderer Weapon_Sprite;
    private SpriteRenderer Monster_Sprite;
    private SpriteRenderer Player_Sprite;
    private Animator Monster_Animator;
    private Animator Player_Animator;
    private AnimatorManage All_Animator;
    
    // Use this for initialization
    void Start()
    {
        Data_Initialization();
    }

    void Data_Initialization()
    {
        Weapon_Sprite = GameObject.FindGameObjectWithTag("weapon").GetComponent<SpriteRenderer>();
        Monster_Attribute = Monster.GetComponent<CharacterManage>();
        Player_Attribute = Player.GetComponent<CharacterManage>();
        Monster_Sprite = Monster.GetComponent<SpriteRenderer>();
        Player_Sprite = Player.GetComponent<SpriteRenderer>();
        Monster_Animator = Monster.GetComponent<Animator>();
        Player_Animator = Player.GetComponent<Animator>();
        All_Animator = Animator_Manage.GetComponent<AnimatorManage>();
        Weapon_Sprite.sprite = Weapon_Data[Own_Weapon].GetComponent<SpriteRenderer>().sprite;//武器图片初始化
        Monster_Fullhp = Monster_Attribute.Hp;//怪物总血量的初始化
        Far_Attrack_Initialization();
    }
    // Update is called once per frame
    void Update()
    {
        if (isPlayer_Alive && isMonster_Alive)
        {
            Player_Move();
            Monster_Move();
            Change_State();
            Change_Hp_Ep_Bar();
        }
        else if (!isPlayer_Alive && isMonster_Alive)
        {
            SaveData.instance.position = SaveData.instance.lastPosition;
            SaveData.instance.HP--;
            if (SaveData.instance.HP <= 0)
            {
                SaveData.instance.isWin = false;
                StartCoroutine(LoadEnd(0.5f));
            }
            StartCoroutine(LoadExplore(0.5f));
        }
        else
        {
            StartCoroutine(LoadExplore(0.5f));
        }

    }
    IEnumerator LoadExplore(float t)
    {
        yield return new WaitForSeconds(t);
        SaveData.instance.isRebuild = true;
        SceneManager.LoadScene("Explore");
    }

    IEnumerator LoadEnd(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("End");
    }

    private void Far_Attrack_Initialization()
    {
        if (Own_Weapon == 3)
        {
            Weapon_Closecombat.transform.localPosition = new Vector3(-0.2f, -0.8f, 0);
            Weapon_Closecombat.transform.localScale = new Vector3(1, 1, 1.5f);
            Weapon_Closecombat.transform.Rotate(0, 0, 20);
        }
    }

    public void Attack_Button()
    {
        if (!isAttrack)
        {
            StartCoroutine("I_Attack_Button");
        }
    }

    IEnumerator I_Attack_Button()
    {
        isAttrack = true;
        yield return null;
        isAttrack = false;
    }
    public void Addforce_Button()
    {
        StartCoroutine("I_Addforce_Button");
    }

    IEnumerator I_Addforce_Button()
    {
        isAddforce = true;
        yield return null;
        isAddforce = false;
    }

    void Player_Move()
    {
        if ((/*Input.GetMouseButtonDown(1)|| */ isAttrack) && Player.GetComponent<CharacterManage>().Ep == 100)
        {
            if (Own_Weapon != 3)
            {
                Player_Attribute.Ep = 0;
                Player_State = Attackstate;
                StartCoroutine("Close_Weapon_Attack");
                if (Player.transform.position.x - Monster.transform.position.x <= Attack_Player)
                {
                    Monster_Attribute.Hp -= 20;
                    if (Monster_Attribute.Hp <= 0)
                    {
                        Monster_Attribute.Hp = 0;
                        isMonster_Alive = false;
                        //Player_Animator.enabled = false;
                        Monster_Animator.enabled = false;
                        Weapon_Closecombat.SetActive(false);
                        Monster_Sprite.sprite = Sprite_Monsterdead.GetComponent<SpriteRenderer>().sprite;
                    }
                }
            }
            else
            {
                Player_Attribute.Ep = 0;
                Player_State = Attackstate;
                Monster_Attribute.Hp -= 20;
                if (Monster_Attribute.Hp <= 0)
                {
                    Monster_Attribute.Hp = 0;
                    isMonster_Alive = false;
                    //Player_Animator.enabled = false;
                    Monster_Animator.enabled = false;
                    Weapon_Closecombat.SetActive(false);
                    Monster_Sprite.sprite = Sprite_Monsterdead.GetComponent<SpriteRenderer>().sprite;
                }
            }


        }
        else
        {
            if ((Input.GetKey(KeyCode.A) || Button_Leftmove.GetComponent<ButtomEvents>().isDown) && Player.transform.position.x > Monster.transform.position.x + Cloest_Player_Monster)
            {
                Player.transform.Translate(-Time.deltaTime * Player_Speed, 0, 0);
                //Leftmove.GetComponent<SpriteRenderer>().sprite = Leftmove_Highlight.GetComponent<SpriteRenderer>().sprite;
                Player_State = Movestate;

            }
            else if ((Input.GetKey(KeyCode.D) || Button_Rightmove.GetComponent<ButtomEvents>().isDown) && Player.transform.position.x < Airwall.transform.position.x - 1.5)
            {
                Player.transform.Translate(Time.deltaTime * Player_Speed, 0, 0);
                //Rightmove.GetComponent<SpriteRenderer>().sprite = Rightmove_Highlight.GetComponent<SpriteRenderer>().sprite;
                Player_State = Movestate;
            }

            else
            {
                Player_State = Waitstate;

            }
        }



        if (/*Input.GetMouseButtonDown(0)||*/isAddforce && Player.GetComponent<CharacterManage>().Ep < 100)
        {
            Player_Attribute.Ep += 20;
            if (Player_Attribute.Ep >= 98)
            {
                Player_Attribute.Ep = 100;
            }
        }
    }//主角移动及攻击

    IEnumerator Close_Weapon_Attack()
    {
        Quaternion originRotation = Weapon_Closecombat.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, Close_Angle_Z) * Quaternion.identity;
        while (Weapon_Closecombat.transform.rotation.eulerAngles.z < Close_Angle_Z)
        {

            Weapon_Closecombat.transform.rotation = Quaternion.Slerp(Weapon_Closecombat.transform.rotation, targetRotation, Time.deltaTime * 15);

            if (Weapon_Closecombat.transform.rotation.eulerAngles.z > Close_Angle_Z - 1)
                break;
            yield return null;

        }
        while (Weapon_Closecombat.transform.rotation.eulerAngles.z > 0)
        {
            Weapon_Closecombat.transform.rotation = Quaternion.Slerp(Weapon_Closecombat.transform.rotation, originRotation, Time.deltaTime * 15);
            if (Weapon_Closecombat.transform.rotation.eulerAngles.z < 1)
                yield break;
            yield return null;
        }

    }
    void Monster_Move()
    {
        if (Player.transform.position.x - Monster.transform.position.x > Cloest_Player_Monster)
        {
            if (Monster_Animator.enabled)
            {
                Monster.transform.position = Vector3.Lerp(Monster.transform.position, Player.transform.position, Monster_Speed * Time.deltaTime);
                Monster_State = Movestate;
            }
        }
        else
        {
            StartCoroutine("Monster_Attack");
        }
    }
    IEnumerator Monster_Attack()
    {
        yield return new WaitForSeconds(Monster_Attack_Waiting);
        Monster_State = Attackstate;
        yield return new WaitForSeconds(0.2f);
        if (Player.transform.position.x - Monster.transform.position.x < Attack_Monster)
        {
            Player_Animator.enabled = false;
            //Monster_Animator.enabled = false;
            Player_Sprite.sprite = Sprite_Playerdead.GetComponent<SpriteRenderer>().sprite;
            Weapon_Closecombat.SetActive(false);
            yield return new WaitForSeconds(1);
            isPlayer_Alive = false;
        }
    }


    void Change_Hp_Ep_Bar()//血条改变
    {
        Epbar.transform.localScale = new Vector3(Player_Attribute.Ep / 100f, 1, 1);
        Hpbar.transform.localScale = new Vector3(Monster_Attribute.Hp / (float)Monster_Fullhp, 1, 1);
    }

    void Change_State()//状态机
    {
        if (Player_State == Waitstate)
        {
            All_Animator.Player_State = Waitstate;
        }
        if (Player_State == Attackstate)
        {
            All_Animator.Player_State = Attackstate;
        }
        if (Player_State == Movestate)
        {
            All_Animator.Player_State = Movestate;
        }
        if (Monster_State == Movestate)
        {
            All_Animator.Monster_State = Movestate;
        }
        if (Monster_State == Attackstate)
        {
            All_Animator.Monster_State = Attackstate;
        }
        if (Monster_State == Waitstate)
        {
            All_Animator.Monster_State = Waitstate;
        }
    }

}
