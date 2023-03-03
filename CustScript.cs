using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustScript : MonoBehaviour
{
    public int stage = 1;
    public int lifeTime = 0;
    public float lifeTimeCycle;
    public int endTime;

    public float food = 10;
    public float thirst = 5;

    bool hasBeries = false;
    public GameObject cell = null;

    public Sprite stage1;
    public Sprite stage2;
    public Sprite stage3;

    void Start()
    {
        StartCoroutine(Life());
    }

    IEnumerator Life()
    {
        while(true)
        {
            yield return new WaitForSeconds(lifeTimeCycle);
            if(lifeTime == endTime)
            {
                yield return null;
            }
            if(stage != 3)
            {
                stage++;
                NextStage(stage);
            }
            lifeTime++;
        }
    }

    void NextStage(int stage)
    {
        if(stage == 0)
        {
            if (cell != null)
            {
                cell.GetComponent<Cell>().deleteObject();
            }
            Destroy(gameObject);
        }
        if(stage == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = stage1;
            food = 5;
            thirst = 1;
        }
        if(stage == 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = stage2;
            food = 15;
            thirst = 3;
        }
        if(stage == 3)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = stage3;
            food = 25;
            thirst = 5;
            hasBeries = true;
        }
    }


    public float[] getFood()
    {
        float[] _food = new float[2] { food, thirst };
        stage--;
        NextStage(stage);
        return _food;
    }

    /*public void SetToAim(GameObject bot)
    {
        inAim.Add(bot);
    }*/

    /*private void OnDestroy()
    {
        foreach(GameObject i in inAim)
        {
            i.GetComponent<BotScript>().StopWalkingToAim();
        }
    }*/
}
