using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int level = 0;  //关卡
    int correctCount = 0;
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (SceneManager.GetActiveScene().name == "Scene2")
        {
            AnswerManager answerManager;
            QuestionManager questionManager;
            //若是在场景2才能找到answerManager和questionManager的引用
            answerManager = GameObject.Find("answerManager").GetComponent<AnswerManager>();
            questionManager = GameObject.Find("questionManager").GetComponent<QuestionManager>();

            GameObject.Find("PanelUI").transform.FindChild("TextLevel").GetComponent<Text>().text = "第" + level + "关";
            correctCount = answerManager.correctCount;
            PassLevel();
        }
    }

    //检测是否通关
    void PassLevel()
    {
        if (correctCount == 5)
        {
            GameObject.Find("Canvas").transform.FindChild("PanelUI").FindChild("PassLevel").gameObject.SetActive(true);
            correctCount = -1;//防止意外
        }
        else
        {
            GameObject.Find("Canvas").transform.FindChild("PanelUI").FindChild("TestID").GetComponent<Text>().text = "";
            GameObject.Find("Canvas").transform.FindChild("PanelUI").FindChild("Demystify").gameObject.SetActive(false);
        }
    }
}

//public class GameManager : MonoBehaviour {

//    int correctCount = 0;

//    void Update()
//    {
//        if (SceneManager.GetActiveScene().name == "Scene2")
//        {
//            AnswerManager answerManager;
//            //若是在场景2才能找到answerManager的引用
//            answerManager = GameObject.Find("answerManager").GetComponent<AnswerManager>();

//            //GameObject.Find("PanelUI").transform.FindChild("TextLevel").GetComponent<Text>().text = "第" + level + "关";

//            correctCount = answerManager.correctCount;
//            PassLevel();
//        }
//    }

//    void PassLevel()
//    {
//        if (correctCount == 5)
//        {
//            GameObject.Find("Canvas").transform.FindChild("PanelUI").FindChild("PassLevel").gameObject.SetActive(true);

//            correctCount = -1;//防止意外
//        }
//    }

//    public void Restart()
//    {
//        SceneManager.LoadScene("Scene2");
//    }

//}
