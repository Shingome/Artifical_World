using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    float height;
    string type;
    public bool isWalkable;

    public List<GameObject> tilesAround;

    public GameObject havingObject = null;

    [Serializable]
    public struct TerrainLevel
    {
        public string name;
        public float height;
        public Color color;
        public bool isWalkable;
    }
    public List<TerrainLevel> terrainLevel = new List<TerrainLevel>();

    private void Start()
    { 

    }
    public float Height
    {
        get
        {
            return Height;
        }
        set
        {
            float prev = 0;
            for(int i = 0; i < terrainLevel.Count;i++)
            {
                if(value > prev && value < terrainLevel[i].height)
                {
                    this.GetComponent<SpriteRenderer>().color = terrainLevel[i].color;
                    height = terrainLevel[i].height;
                    type = terrainLevel[i].name;
                    isWalkable = terrainLevel[i].isWalkable;
                    break;
                }
                prev = terrainLevel[i].height;
            }
        }

    }


    public void setObject(GameObject obj)
    {
        isWalkable = false;
        havingObject = obj;
    }

    public void deleteObject()
    {
        isWalkable = true;
        havingObject = null;
    }

    public void CheckAround()
    {
        Transform _transform = this.transform;
        gameObject.layer = 1;
        Collider2D[] mas = Physics2D.OverlapBoxAll(_transform.position, new Vector2(1.5f,1.5f), 0, LayerMask.GetMask("Ground"));
        foreach(var i in mas)
        {
            tilesAround.Add(i.gameObject);
        }
        gameObject.layer = 6;
    }

}
