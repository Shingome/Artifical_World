using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    List<GameObject> tiles;

    public int waterCount;

    public GameObject water;

    void Start()
    {

    }

    public void generateWater(List<GameObject> _tiles)
    {
        tiles = _tiles;
        System.Random random = new System.Random();
        int a;
        for (int i = 0; i < waterCount; i++)
        {
            a = random.Next(0, tiles.Count);
            GameObject _bot = Instantiate(water, tiles[a].transform.position, Quaternion.Euler(0, 0, 0));
            tiles[a].GetComponent<Cell>().setObject(_bot);

        }
    }

}
