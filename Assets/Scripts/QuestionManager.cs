using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public static RandomManager rm = RandomManager.Instance();
    private JsonManager jm;
    public GameObject blackPre1;
    public GameObject blackPre2;
    public GameObject yellowPre1;
    public GameObject yellowPre2;

    public Transform destinationPoints;//饼干目标点总物体
    public List<Vector3> DestinationPointsPosition = new List<Vector3>();//饼干各个目标点原始位置的列表集合
    private string createpointStr1;     //生成点名称
    private string createpointStr2;     //生成点名称
    private int pre_x1;                 //预设号,会传到GetCookiesPrefab()用
    private int pre_x2;                 //预设号

    [HideInInspector]
    public List<GameObject> Cookies = new List<GameObject>(); //饼干列表
    [HideInInspector]
    public int questionCount = 0;//出题数
    [HideInInspector]
    public string operatorStr;  //运算符
    [HideInInspector]
    public int num1;            //第一个数
    [HideInInspector]
    public int num2;            //第二个数
    [HideInInspector]
    public Text NumberText1;    //第一个数文本框
    [HideInInspector]           
    public Text OperatorText;   //运算符文本框
    [HideInInspector]          
    public Text NumberText2;    //第二个数文本框
    [HideInInspector]         
    public Text EqualText;      //等号文本框
    [HideInInspector]        
    public Text ResultText;     //结果文本框

    void Awake()
    {
        jm = GameObject.Find("jsonManager").GetComponent<JsonManager>();
        NumberText1 = GameObject.Find("NumberText1").GetComponent<Text>();
        OperatorText = GameObject.Find("OperatorText").GetComponent<Text>();
        NumberText2 = GameObject.Find("NumberText2").GetComponent<Text>();
        EqualText = GameObject.Find("EqualText").GetComponent<Text>();
        ResultText = GameObject.Find("ResultText").GetComponent<Text>();
    }

    void Start()
    {
        destinationPoints = GameObject.Find("DestinationPoints").transform;
        for(int i = 0; i < 18; i++)
        {
            DestinationPointsPosition.Add(destinationPoints.GetChild(i).localPosition);
        }
    }
    /// <summary>
    /// 饼干进入方式，包括上下，黑黄的选择
    /// </summary>
    void EnterStyle()
    {
        System.Random r = new System.Random();
        int x = r.Next(0, 2);
        int y = r.Next(0, 2);
        if (x == 0)
        {
            createpointStr1 = "createPoint1";
            createpointStr2 = "createPoint2";
            if (y == 0)
            {
                pre_x1 = 0; //下黑
                pre_x2 = 3; //上黄
            }
            else
            {
                pre_x1 = 2; //下黄
                pre_x2 = 1; //上黑
            }
        }
        else
        {
            createpointStr1 = "createPoint2";
            createpointStr2 = "createPoint1";
            if (y == 0)
            {
                pre_x1 = 1; //上黑
                pre_x2 = 2; //下黄
            }
            else
            {
                pre_x1 = 3; //上黄
                pre_x2 = 0; //下黑
            }
        }
    }

    public IEnumerator createQuestion(int level)
    {
        chooseQuestionStyle(level); //选择出题方式,
        if (operatorStr.Equals("+"))        //若是加法，居中显示最终数量的饼干
        {
            CenterDisplay(num1 + num2);
        }
        else if (operatorStr.Equals("-"))   //若是减法，居中显示第一个数的饼干
        {
            CenterDisplay(num1);
        }
        EnterStyle();   //随机进入饼干的方式
        yield return CreateCookies(num1, createpointStr1, pre_x1);//生成第一个数的饼干
        DisplayText(num1, NumberText1);//显示第一个数
        yield return new WaitForSeconds(1.5f);
        if (operatorStr.Equals("+"))    //若是加法
        {
            DisplayText("+", OperatorText);//显示加号
            yield return AddCookies(num2, createpointStr2, pre_x2);//生成第二个数的饼干
        }
        else if (operatorStr.Equals("-"))   //若是减法
        {
            DisplayText("-", OperatorText);//显示减号
            yield return SubCookies(num2, createpointStr1);//减去相应数量的饼干
        }
        
        yield return new WaitForSeconds(1.5f);
        DisplayText(num2, NumberText2);         //无论前面加减法如何，都一样显示第二个数
        yield return new WaitForSeconds(1.5f);
        DisplayText("=", EqualText);            //显示等号
        yield return new WaitForSeconds(1.5f);
        DisplayText("?", ResultText);           //显示问号
    }

    /// <summary>
    /// 显示题目的每个文本框
    /// </summary>
    /// <param name="obj">任意类型的数据</param>
    /// <param name="text">对应的文本框</param>
    void DisplayText(System.Object obj,Text text)
    {
        text.text = obj.ToString();
        iTween.PunchScale(text.gameObject, iTween.Hash("time", 1.0f, "x", 1.1f, "y", 1.1f, "looptype", iTween.LoopType.none));
    }

    /// <summary>
    /// 生成饼干
    /// </summary>
    /// <param name="num">数量</param>
    /// <param name="createpointStr">生成点</param>
    /// <param name="pre_x">预设号</param>
    /// <returns></returns>
    IEnumerator CreateCookies(int num, string createpointStr, int pre_x)
    {
        GameObject pre = GetCookiesPrefab(pre_x);
        for (int i = 0; i < num; i++)
        {
            Cookies.Add(Instantiate(pre));
            
            if (Cookies.Count <= num1)
            {
                //每个生成点在其父子目标点的本地坐标位置
                Vector3 createPos = destinationPoints.GetChild(i).FindChild(createpointStr).localPosition;
                Cookies[Cookies.Count - 1].transform.parent = destinationPoints.GetChild(i);
                //将饼干置于生成点的位置
                Cookies[Cookies.Count - 1].transform.localPosition = createPos;
                EnterCookies(Cookies[Cookies.Count - 1], createPos.y);   //饼干进入，传入生成点本地坐标的y值
            }
            else  //否则当前面已生成num1个后，就在第num1后的生成点生成，这用于加法
            {
                Vector3 createPos = destinationPoints.GetChild(num1 + i).FindChild(createpointStr).localPosition;
                Cookies[Cookies.Count - 1].transform.parent = destinationPoints.GetChild(num1 + i);
                Cookies[Cookies.Count - 1].transform.localPosition = createPos;
                EnterCookies(Cookies[Cookies.Count - 1], createPos.y);   //饼干进入
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    /// <summary>
    /// 饼干加法
    /// </summary>
    /// <param name="num">数量</param>
    /// <param name="createpointStr">生成点</param>
    /// <param name="pre_x">预设号</param>
    /// <returns></returns>
    IEnumerator AddCookies(int num, string createpointStr, int pre_x)
    {
        if (num > 0)
        {
            yield return new WaitForSeconds(1.0f);

            yield return CreateCookies(num, createpointStr, pre_x);
        }
    }

    /// <summary>
    /// 饼干减法
    /// </summary>
    /// <param name="num">减数</param>
    /// <param name="createpointStr">被减数的生成点，即createpointStr1</param>
    /// <returns></returns>
    IEnumerator SubCookies(int num, string createpointStr)
    {
        if (num > 0)
        {
            yield return new WaitForSeconds(1.0f);

            for (int i = 0; i < num; i++)
            {
                GameObject hand = Cookies[Cookies.Count - 1 - i].transform.FindChild("hand").gameObject;
                //第一个parent是cookies，第二个parent是目标piont
                Vector3 createPos = hand.transform.parent.parent.FindChild(createpointStr).localPosition;

                GameObject cooky = Cookies[Cookies.Count - 1 - i].transform.FindChild("cooky").gameObject;
                hand.SetActive(true);
                //手移动到饼干处
                iTween.MoveBy(hand, iTween.Hash("time", 0.2f, "easetype", iTween.EaseType.linear,
                                                "y", -createPos.y));
                //手和饼干缩回去
                iTween.MoveBy(cooky, iTween.Hash("time", 0.2f, "easetype", iTween.EaseType.linear,
                                                "delay", 0.2f, "y", createPos.y));
                iTween.MoveBy(hand, iTween.Hash("time", 0.2f, "easetype", iTween.EaseType.linear,
                                                "delay", 0.2f, "y", createPos.y));

                Cookies[Cookies.Count - 1 - i].transform.FindChild("dotted").gameObject.SetActive(true);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
   
    /// <summary>
    /// 饼干进入动画
    /// </summary>
    /// <param name="cookies">饼干物体，包括饼干，虚线，手</param>
    /// <param name="createPos_Y">生成点在目标点的本地坐标y值</param>
    void EnterCookies(GameObject cookies, float createPos_Y)
    {
        GameObject hand = cookies.transform.FindChild("hand").gameObject;
        iTween.MoveTo(cookies, iTween.Hash("time", 0.2f, "position", cookies.transform.parent.position,
                                           "easetype", iTween.EaseType.linear));
        //当饼干从生成点移动到目标点时，之前的生成点本地坐标y值就成了手缩回去的y值，因为此时饼干与目标点重合
        //这是手缩回去的动画
        iTween.MoveBy(hand, iTween.Hash("delay", 0.2f, "time", 0.2f, "y", createPos_Y,
                                       "easetype", iTween.EaseType.linear,
                                       "oncomplete", "DisableHand",
                                       "oncompletetarget", gameObject,  //指定目标是函数所在的物体
                                       "oncompleteparams", hand));
    }

    /// <summary>
    /// 隐藏手
    /// </summary>
    /// <param name="hand">负责放那只饼干的那只手</param>
    void DisableHand(GameObject hand)
    {
        hand.SetActive(false);
    }

    /// <summary>
    /// 获取饼干预设
    /// </summary>
    /// <param name="x">随机数字</param>
    /// <returns></returns>
    GameObject GetCookiesPrefab(int x)
    {
        switch (x)
        {
            case 0:
                return blackPre1;
            case 1:
                return blackPre2;
            case 2:
                return yellowPre1;
            case 3:
                return yellowPre2;
            default:
                return null;
        }
    }

    /// <summary>
    /// 居中显示饼干
    /// </summary>
    /// <param name="num">饼干数目</param>
    void CenterDisplay(int num)
    {
        if (num <= 6)
        {
            for(int i = 0; i < num; i++)
            {
                destinationPoints.GetChild(i).transform.localPosition = destinationPoints.GetChild(i + 6).transform.localPosition;
            }
        }
        else if (num > 6 && num <= 12)
        {
            for (int i = 0; i < num; i++)
            {
                destinationPoints.GetChild(i).transform.localPosition = destinationPoints.GetChild(i + 6).transform.localPosition;
            }
        }
    }

    /// <summary>
    /// 出题方式，题库or随机
    /// </summary>
    /// <param name="level">等级</param>
    void chooseQuestionStyle(int level)
    {
        //若有题库，优先使用题库出题
        if (jm.testCookies.Count > 0)
        {
            questionCount = jm.testCookies[0].ID;   //获取题号
            num1 = jm.testCookies[0].num1;              //获取第一个数
            num2 = jm.testCookies[0].num2;              //获取第二个数
            operatorStr = jm.testCookies[0].operation;  //获取运算符号
            jm.testCookies.RemoveAt(0);
        }
        //否则,若题库用完了或没有题库，那么使用随机出题
        else
        {
            rm.createRandom(level);
            System.Random r = new System.Random();
            questionCount++;
            num1 = rm.num1Random;              //获取第一个数
            num2 = rm.num2Random;              //获取第二个数
            operatorStr = rm.operatorRandom;  //获取运算符号
        }
    }
}
