using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskReciver : MonoBehaviour
{
    public GameObject botGenerator;

    public void GetData(string a)
    {
        string[]mas = a.Split(' ');
        botGenerator.GetComponent<GenerateBots>().bots[Convert.ToInt32(mas[0])].GetComponent<BotScript>().GetTask(Convert.ToSingle(mas[1]));
    }
}
