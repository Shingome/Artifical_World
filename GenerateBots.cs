using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBots : MonoBehaviour
{

    List<GameObject> tiles;

    public int botCounts;

    public GameObject bot;

    public List<GameObject> bots = new List<GameObject>();
    
    void Start()
    {

    }

    public void generateBots(List<GameObject> _tiles)
    {
        tiles = _tiles;
        System.Random random = new System.Random();
        int a;
        for(int  i = 0; i < botCounts;i++)
        {
            a = random.Next(0, tiles.Count);
            GameObject _bot = Instantiate(bot, new Vector2(tiles[a].transform.position.x, tiles[a].transform.position.y), Quaternion.Euler(0, 0, 0));
            bots.Add(_bot);
            tiles[a].GetComponent<Cell>().setObject(_bot);
            _bot.GetComponent<BotScript>().curentCell = tiles[a];
            _bot.GetComponent<BotScript>().index = bots.Count - 1;
            _bot.GetComponent<BotScript>().generator = gameObject;

        }
    }

    public void createNewBot(GameObject curentCell, int generation, float maxFood, float maxThirst, float maxNeedReproduction, float[] priority)
    {
        Debug.Log("Создание чудика, часть 2");
        GameObject newBotTile = null;
        foreach(GameObject i in curentCell.GetComponent<Cell>().tilesAround)
        {
            if (i.GetComponent<Cell>().havingObject == null)
            {
                i.GetComponent<Cell>().setObject(new GameObject());
                newBotTile = i;
                break;
            }
        }
        if(newBotTile == null)
        {
            return;
        }
        GameObject _bot = Instantiate(bot, newBotTile.transform.position, Quaternion.Euler(0, 0, 0));
        newBotTile.GetComponent<Cell>().setObject(_bot);
        _bot.GetComponent<BotScript>().curentCell = newBotTile;
        BotScript script = _bot.GetComponent<BotScript>();
        script.generation = generation;
        script.maxFood = maxFood;
        script.maxThirst = maxThirst;
        script.maxNeedReproduction = maxNeedReproduction;
        script.priority = priority;
        bots.Add(_bot);
        _bot.GetComponent<BotScript>().index = bots.Count - 1;
        _bot.GetComponent<BotScript>().generator = gameObject;
        botCounts++;
    }
}
