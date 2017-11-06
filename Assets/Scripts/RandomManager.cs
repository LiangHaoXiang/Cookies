using UnityEngine;
using System.Collections;
using System;

public class RandomManager
{
    private static RandomManager instance = null;

    private System.Random r = new System.Random();
    public int num1Random;          //第一个数随机
    public int num2Random;          //第二个数随机
    public string operatorRandom;   //运算符随机


    private RandomManager() { }

    public static RandomManager Instance()
    {
        if (instance == null)
            instance = new RandomManager();
        return instance;
    }

    public void createRandom(int level)
    {
        num1Random = r.Next(6, 10);
        num2Random = r.Next(1, 5);
        int x = r.Next(0, 2);
        if (x == 0)
            operatorRandom = "+";
        else
            operatorRandom = "-";

        //fromLevel(level);
    }

    //public void fromLevel(int level)
    //{
    //    switch (level)
    //    {
    //        case 1:
                
    //            break;
    //        case 2:
                
    //            break;
    //        case 3:
    //            break;
    //        case 4:
    //            break;
    //        case 5:
    //            break;
    //        case 6:
    //            break;
    //        case 7:
    //            break;
    //        case 8:
    //            break;
    //        case 9:
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
