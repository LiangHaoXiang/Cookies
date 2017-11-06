using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class JsonManager : MonoBehaviour {

#region 题库的属性
    [HideInInspector]
    public List<TestItem> testCookies;
    [Serializable]
    public class TestItem
    {
        public int ID;          //题号
        public int num1;        
        public int num2;
        public string operation;//运算法则
    }
    [Serializable]
    public class Test
    {
        public List<TestItem> testCookies;
    }
    #endregion


    void Awake()
    {
        string json = File.ReadAllText("H:\\云孩科技/Json/testCookies.txt");  //读取json数据
        JsonTestCookies(json);
    }

    void Start () {
        
	}

    public void JsonTestCookies(string json)
    {
        if (json != string.Empty)
        {
            Test item = JsonUtility.FromJson<Test>(json);     //反序列化后存储到类或结构体
            testCookies = item.testCookies;                     //获取类的对象拥有的属性列表
        }
    }
}

//{
//	"testFruits1":[
//		{"ID": 1,"Size": "the smallest","Fruit1": "greenApple","Fruit2": "eggplant","Who":"first"},
//		{"ID": 2,"Size": "the biggest","Fruit1": "greenBanana","Fruit2": "lemon","Who":"second"},
//		{"ID": 3,"Size": "the middle","Fruit1": "greenApple","Fruit2": "orange","Who":"second"},
//		{"ID": 4,"Size": "the biggest","Fruit1": "greenBanana","Fruit2": "eggplant","Who":"second"},
//		{"ID": 5,"Size": "the smallest","Fruit1": "melon","Fruit2": "carrot","Who":"first"}
//	]
//}


//{
//	"testFruits1":[
//		{"ID": 1,"Size": "the smallest","Fruit1": "greenApple","Fruit2": "eggplant","Who":"first"},
//		{"ID": 2,"Size": "the biggest","Fruit1": "greenBanana","Fruit2": "lemon","Who":"second"}
//	],
//	"testFruits2":[
//		{"ID": 1,"Size1": "the smallest","Size2": "the biggest","Fruit1": "greenApple","Fruit2": "eggplant"},
//		{"ID": 2,"Size1": "the biggest","Size2": "the smallest","Fruit1": "greenBanana","Fruit2": "lemon"}
//	]
//}








//public class JsonManager : MonoBehaviour
//{

//    public List<TestItem> tests;
//    [Serializable]
//    public class TestItem
//    {
//        public int ID;
//        public int initCount;
//        public int addCount;
//        public int ranCount;
//    }
//    [Serializable]
//    public class Test
//    {
//        public List<TestItem> test;
//    }

//    void Start()
//    {
//        JsonMy();
//    }

//    public void JsonMy()
//    {
//        string json = File.ReadAllText("H:\\jsonmy4.txt");
//        if (json != string.Empty)
//        {
//            Test item = JsonUtility.FromJson<Test>(json);
//            tests = item.test;
//        }
//    }

//}

////{
////    "test": [
////        {"ID": 1,"initCount": 3,"addCount": 2,"ranCount": 3},
////        {"ID": 2,"initCount": 1,"addCount": 3,"ranCount": 4},
////        {"ID": 3,"initCount": 3,"addCount": 2,"ranCount": 1},
////        {"ID": 4,"initCount": 1,"addCount": 2,"ranCount": 3},
////        {"ID": 5,"initCount": 6,"addCount": 1,"ranCount": 4},
////        {"ID": 6,"initCount": 3,"addCount": 3,"ranCount": 5},
////        {"ID": 7,"initCount": 4,"addCount": 0,"ranCount": 0},
////        {"ID": 8,"initCount": 7,"addCount": 0,"ranCount": 4},
////        {"ID": 9,"initCount": 2,"addCount": 2,"ranCount": 0},
////        {"ID": 10,"initCount": 5,"addCount": 1,"ranCount": 1}
////    ]
////}

