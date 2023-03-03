using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainManager : MonoBehaviour
{
    BotScript bot;
    void Start()
    {
        bot = gameObject.GetComponent<BotScript>();
    }

    public string GetObservation()
    {
        return $"{GetGroup()} {GetIndex()} {bot.food / bot.maxFood} {bot.thirst / bot.maxThirst} {bot.needReproduction / bot.maxNeedReproduction}";
    }

    public string GetGroup()
    {
        if (Enumerable.SequenceEqual(bot.priority, new float[] { 1, 2, 3 }))
            return "0";
        else if (Enumerable.SequenceEqual(bot.priority, new float[] { 1, 3, 2 }))
            return "1";
        else if (Enumerable.SequenceEqual(bot.priority, new float[] { 2, 1, 3 }))
            return "2";
        else if (Enumerable.SequenceEqual(bot.priority, new float[] { 2, 3, 1 }))
            return "3";
        else if (Enumerable.SequenceEqual(bot.priority, new float[] { 3, 1, 2 }))
            return "4";
        else if (Enumerable.SequenceEqual(bot.priority, new float[] { 3, 2, 1 }))
            return "5";
        return "5";
    }

    public string GetIndex()
    {
        return Convert.ToString(bot.index);
    }

    public string GetReward(float action)
    {
        if (bot.isDeath)
            return "-100";
        else if (action == 0)
            return $"{(10 * (1 + bot.food / bot.maxFood) / (Array.IndexOf(bot.priority, action) + 1))}";
        else if (action == 1)
            return $"{(10 * (1 + bot.thirst / bot.maxThirst) / (Array.IndexOf(bot.priority, action) + 1))}";
        else if (action == 2)
            return $"{(10 * (1 + bot.needReproduction / bot.maxNeedReproduction) / (Array.IndexOf(bot.priority, action) + 1))}";
        return "0";
    }

    public string ToGetAction(float action_prev)
    {
        return GetGroup() + GetIndex() + GetReward(action_prev) + GetObservation() + $"{Convert.ToInt32(bot.isDeath)}";
    }
}