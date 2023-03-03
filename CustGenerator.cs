using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustGenerator : MonoBehaviour
{
    public GameObject cust;

    public float generatorTime;

    public int startCustCount;

    List<GameObject> tiles;

    public void startGenerator(List<GameObject> _tiles)
    {
        StartCoroutine(custSpawner(_tiles));
    }
    IEnumerator custSpawner(List<GameObject> _tiles)
    {
        tiles = _tiles;

        while(true)
        {
            System.Random random = new System.Random();
            int a;
            for (int i = 0; i < startCustCount; i++)
            {
                a = random.Next(0, tiles.Count);
                if (tiles[a].GetComponent<Cell>().havingObject == null)
                {
                    GameObject _cust = Instantiate(cust, tiles[a].transform.position, Quaternion.Euler(0, 0, 0));
                    tiles[a].GetComponent<Cell>().setObject(cust);
                    _cust.GetComponent<CustScript>().cell = tiles[a];
                }
            }
            startCustCount = random.Next(startCustCount / 3, startCustCount * 2 - startCustCount / 2);

            yield return new WaitForSeconds(generatorTime);
        }
    }
}
