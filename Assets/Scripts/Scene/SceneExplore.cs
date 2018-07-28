using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using mySpace;
using System.Collections;

public class SceneExplore : MonoBehaviour, ISceneController, IUserAction
{
    public GameObject Hint;         //线索面板
    public GameObject HintBtn;      //线索按钮
    public GameObject Goods;        //物资面板
    public GameObject GoodsBtn;     //物资按钮
    public GameObject MiniMap;      //小地图按钮

    bool IsShowHint = false;        //是否显示线索
    bool IsFindGoods = false;       //是否搜寻物资
    bool IsMiniMap = false;         //是否显示小地图

    public Image DeadHeart;
    public Image[] Lifes;           //生命数
    public Text[] PlayerData;       //金币、钻石...等的数量

    public GameObject map;
    public GameObject player;

    MapStatus mapStatus;
    PlayerStatus playerStatus;

    int hintTimes;

    string[] enemyHint = new string[]
    {
        "你已进入丧尸巡逻区，请注意安全。",
        "丧尸在你的周围蓄势待发，需谨慎行动。",
        "附近迷雾笼罩，隐隐传来丧尸的嘶吼声。",
        "跌跌撞撞跑来一个人，“快跑！”",
        "你已踏入了丧尸的包围圈。",
        "周围灯光朦胧，影影绰绰映出丧尸的倒影。",
        "天空阴云密布，似乎是一种不详的征兆。",
        "附近寂静的反常，需谨慎行动。",
        "丧尸在你的周围虎视眈眈，小心！",
        "隐约有大批丧尸朝你前来，快走！",
        "一大批丧尸已朝你前来，请尽快离开。",
        "在丧尸发现你前，快走！",
        "前方荒凉寂静，请小心行事。",
        "四周的断壁残垣上有血的印迹，这里在不久前发生过战斗。",
        "不远处传来了惨叫声，请提高警惕！",
        "远处传来枪声，可能有战斗发生。",
        "奇怪，这里看上去荒无人烟，为什么会有悉悉索索的声音？",
        "空气中传来一丝腐烂的味道。",
        "不远处的草丛耸动了一下。",
        "有乌鸦在我头上盘旋。"
    };

    string[] gateHint = new string[]
    {
        "西边有人在奔跑，跟上他们！",
        "西方隐隐显出生机。",
        "想知道怎么离开这里吗？朝西方走吧。",
        "大门在西方！",
        "西方可能隐藏出路",
        "跟着西方的火光，就能逃离这里！",
        "西方隐约传来人类的声音，“这里有大门！”"
    };

    string[] worldHint = new string[]
    {
        "5月25日，A城一位夫妇在淋雨狂欢后的归家途中，突发车祸身亡。法医检验后称，发现司机的脖子上有被猛兽撕咬的痕迹，警察猜测是山间的猛兽闯入车中咬伤司机导致汽车撞上山体。但是汽车的车窗完好，警察对于这一点没有合理解释。",
        "在世界卫生组织的努力攻关下，医学专家表示他们已成功提取病毒，将其命名为“bite7”。经研究，“bite7”是冠状病毒的一个变种，具有高度传染性，可通过液体传播。在临床试验上，被感染“bite7”的生物会在较短时间内呈现出精神亢奋、思维混乱、四肢抽搐等症状，逐渐丧失生命体征及原本的生物形态，且极具攻击性。目前，医学方面对“bite7“尚无治疗手段。",
        "联合国人口司在2050年6月的世界人口普查中发现，在最近半年，世界人口增长处于停滞状态。在进一步的调研中，人口司发现人口出生率基本正常，而死亡率增长了8.23‰。死亡率增长的主要地区为东亚、南亚和南美洲地区，具体的原因人口司仍在调查中。",
        "杰克·富勒（Jack Fuller）是废弃已久的猎人小屋的主人，他身手矫健，随身携带一把锋利的斧头，能劈开一切敢于挡住他路的事物，山林中的猛兽无一是他的对手。自他离世后，他的斧头也失去了踪迹。坊间传闻他的斧头就在他的小屋中，然而几十年过去了，没有人找到过。",
        "据说，隔壁B城住着一位有八年爆破经验的FBI防爆专家——米洛·马森（Milo Masson），他富有爱心，拥有出色的武力和极强的责任心。或许他能带领我们突出重围，寻得生路。",
        "玛丽亚教堂，始建于1880年，是典型的罗曼式建筑。在教堂婚礼盛行的年代，它曾经风靡一时。但在一场凶杀案之后，该教堂迅速被废弃，变得荒凉，时不时传出闹鬼的传闻。然而近几月，一群人被目击频繁出入这个地方。他们是谁？他们来这里干什么？"
    };

    // Use this for initialization
    void Start()
    {
        MainManager main = MainManager.GetInstance();
        main.currentSceneController = this;

        mapStatus = map.GetComponent<MapStatus>();
        playerStatus = player.GetComponent<PlayerStatus>();

        //初始化场景
        if (SaveData.instance.isRebuild)
        {
            rebuildScene();
        }
        else
        {
            main.currentSceneController.loadScene();
        }
    }

    void Update()
    {
        checkStatus();

        IsSpriteExistInCurrentScene(); //判断是否存在副界面，若存在，再点击就返回主界面

        //UpdateLifeNum(); //接口1，在最后，需要完善（根据是否打败僵尸，增加/减少界面上显示的生命个数）

        //UpdatePlayerData(); //接口2，根据游戏进度，更新玩家拥有的金币数、钻石数...等等  

    }

    void checkStatus()
    {
        int[,] s = mapStatus.getStatus();
        int[] p = playerStatus.getPostion();

        if (s[p[0], p[1]] == 0)
        {
            SaveData.instance.lastPosition = playerStatus.getPostion();
            HintBtn.SetActive(false);
            GoodsBtn.SetActive(false);
        }
        else if (s[p[0], p[1]] == 1)
        {
            //保存玩家数据
            SaveData.instance.position = playerStatus.getPostion();
            SaveData.instance.HP = playerStatus.getHP();
            SaveData.instance.Coins = playerStatus.getCoins();

            //保存地图数据
            SaveData.instance.blocks = mapStatus.getBlocks();
            SaveData.instance.status = mapStatus.getStatus();

            //保存提示次数
            SaveData.instance.hintTimes = hintTimes;

            //保存提示记录
            SaveData.instance.hints = mapStatus.getHints();

            StartCoroutine(LoadBattle(1.5f));
        }
        else if (s[p[0], p[1]] == 2)
        {
            SaveData.instance.lastPosition = playerStatus.getPostion();
            HintBtn.SetActive(true);
            GoodsBtn.SetActive(true);
        }
        else if (s[p[0], p[1]] == 3)
        {

        }
        else if (s[p[0], p[1]] == 4)
        {
            SaveData.instance.isWin = true;
            StartCoroutine(LoadEnd(0.5f));
        }
    }
    
    IEnumerator LoadEnd(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("End");
    }

    IEnumerator LoadBattle(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("Battle");
    }

    public void SelectHint(int times)
    {
        if (times < 3)
        {
            if (Random.Range(0, 100) < 95)
            {
                string hint = enemyHint[Random.Range(0, enemyHint.Length)]
                            + "\n（周围 8 格内有 " + getEnemyNum() + " 只丧尸）";
                Hint.GetComponentInChildren<Text>().text = hint;
                mapStatus.addHintInBlock(playerStatus.getPostion(), hint);
            }
            else
            {
                string hint = worldHint[Random.Range(0, worldHint.Length)];
                Hint.GetComponentInChildren<Text>().text = hint;
                mapStatus.addHintInBlock(playerStatus.getPostion(), hint);
            }
        }
        else
        {
            int i = Random.Range(0, 100);
            if (i < 75)
            {
                string hint = enemyHint[Random.Range(0, enemyHint.Length)]
                            + "\n（周围 8 格内有 " + getEnemyNum() + " 只丧尸）";
                Hint.GetComponentInChildren<Text>().text = hint;
                mapStatus.addHintInBlock(playerStatus.getPostion(), hint);
            }
            else if (i < 95 && i >= 75)
            {
                string hint = gateHint[Random.Range(0, gateHint.Length)];
                Hint.GetComponentInChildren<Text>().text = hint;
                mapStatus.addHintInBlock(playerStatus.getPostion(), hint);
            }
            else
            {
                string hint = worldHint[Random.Range(0, worldHint.Length)];
                Hint.GetComponentInChildren<Text>().text = hint;
                mapStatus.addHintInBlock(playerStatus.getPostion(), hint);
            }
        }

    }

    //显示线索界面
    public void DisplayHint()
    {
        IsShowHint = !IsShowHint;
        Hint.SetActive(IsShowHint);
        hintTimes++;

        SelectHint(hintTimes);

        int[] position = playerStatus.getPostion();
        mapStatus.clearStatus(position[0], position[1]);
    }

    //显示物资界面
    public void SearchGoods()
    {
        IsFindGoods = !IsFindGoods;
        Goods.SetActive(IsFindGoods);

        int[] position = playerStatus.getPostion();
        mapStatus.clearStatus(position[0], position[1]);
    }

    public void ShowMiniMap()
    {
        IsMiniMap = !IsMiniMap;
        MiniMap.SetActive(IsMiniMap);
    }

    //如果当前处于以下3种状态之一：阅读线索、查看物资、小地图，
    //则点击屏幕任何区域就会返回界面。否则什么也不干。
    public void IsSpriteExistInCurrentScene()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsShowHint)
            {
                IsShowHint = !IsShowHint;
                Hint.SetActive(IsShowHint);
            }
            if (IsFindGoods)
            {
                IsFindGoods = !IsFindGoods;
                Goods.SetActive(IsFindGoods);
            }
            if (IsMiniMap)
            {
                IsMiniMap = !IsMiniMap;
                MiniMap.SetActive(IsMiniMap);
            }
        }
    }

    public int getEnemyNum()
    {
        int[] position = playerStatus.getPostion();
        int[,] status = mapStatus.getStatus();

        int sum = 0;

        int x = position[0];
        int y = position[1];

        if ((x - 1) >= 0 && y - 1 >= 0)
            if (status[x - 1, y - 1] == 1)
                sum++;
        if ((x - 1) >= 0)
            if (status[x - 1, y] == 1)
                sum++;
        if (x - 1 >= 0 && y + 1 < 5)
            if (status[x - 1, y + 1] == 1)
                sum++;
        if (y - 1 >= 0)
            if (status[x, y - 1] == 1)
                sum++;
        if (y + 1 < 5)
            if (status[x, y + 1] == 1)
                sum++;
        if (x + 1 < 5 && y - 1 >= 0)
            if (status[x + 1, y - 1] == 1)
                sum++;
        if (x + 1 < 5)
            if (status[x + 1, y] == 1)
                sum++;
        if (x + 1 < 5 && y + 1 < 5)
            if (status[x + 1, y + 1] == 1)
                sum++;

        return sum;
    }

    public void rebuildScene()
    {

        int[] position = SaveData.instance.position;
        int HP = SaveData.instance.HP;
        int coins = SaveData.instance.Coins;
        //设置玩家数据
        playerStatus.init(position[0], position[1], HP, coins);

        //设置地图数据
        mapStatus.init(SaveData.instance.blocks, SaveData.instance.status);

        //设置提示次数
        hintTimes = SaveData.instance.hintTimes;

        //设置消息记录
        mapStatus.setHints(SaveData.instance.hints);

        //加载地图
        Instantiate(map);
    }

    public void loadScene()
    {
        //初始化玩家状态
        int x = 4;
        int y = 1;
        int HP = 3;
        int coins;
        if (PlayerPrefs.HasKey("Coins"))
        {
            coins = PlayerPrefs.GetInt("Coins");
        }
        else
        {
            coins = 0;
        }
        playerStatus.init(x, y, HP, coins);

        //初始化地图状态
        int[,] blocks = mapStatus.CreateBlocks();
        int[,] status = mapStatus.CreateStatus();
        mapStatus.init(blocks, status);

        //初始化提示次数
        hintTimes = 0;

        Instantiate(map);
    }

    #region IUserAction
    public void move(int x, int y)
    {
        if (playerStatus.move(x, y))
        {
            int[] position = playerStatus.getPostion();
            mapStatus.explore(position[0], position[1]);
        }
    }

    public void quit()
    {
        //玩家退出游戏
        PlayerPrefs.SetInt("Coins", playerStatus.getCoins());
        PlayerPrefs.SetInt("Chapter", SaveData.instance.chapter);
    }

    public void restart()
    {
        //TODO 玩家重新开始游戏
    }

    public void sceneSwitch()
    {
        throw new System.NotImplementedException();
    }
    #endregion

}
