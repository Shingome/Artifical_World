using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotScript : MonoBehaviour
{
    public int index; // индекс
    public int generation = 1; // поколение
    public string status; // текущий статус
    public int foodType;

    [Header("Стаитистика")]

    public float health = 100f;
    public float dammage = 10f;

    public float food; // максимальное значение еды
    public float maxFood; // максимальное значение еды

    public float thirst; // текущее значение жажды
    public float maxThirst; // максимальное значение жажды

    public float needReproduction; // значение потребности размножения
    public float maxNeedReproduction; // максимальное значение потребности размножения

    [Header("Время действия")]
    public float actionTime; // период выполнения действия


    [Header("Объекты")]

    public GameObject waterObject = null; // все объекты воды которые были обнаружены
    public GameObject foodObject; // все объекты еды которые были обнаружены
    Vector3 foodPosition;
    Vector3 waterPosition;

    public float[] priority; // массив приоритетов бота
    public float botQuest;

    public string type;

    bool hasAimFood = false;
    bool isFoundFood = false;
    bool hasAimWater = false;
    bool isFoundWater = false;

    public bool partnerSeek = false;
    public bool isMain = false;
    public bool isDeath = false;

    public GameObject partner = null;
    public GameObject curentCell; // текущая клетка бота
    public GameObject generator;
    public GameObject conMeneger;

    
    public static void Shuffle(float[] arr)
        {
            System.Random rand = new System.Random();

            for (int i = arr.Length - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                float tmp = arr[j];
                arr[j] = arr[i];
                arr[i] = tmp;
            }
        }

    void Start()
    {
        actionTime = Random.Range(1f, 3f);

        foodType = Random.Range(0, 3);

        if(foodType == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        foodObject = null;

        conMeneger = GameObject.Find("ConnectionObject");

        priority = new float[3] {0f, 1f, 2f};
        Shuffle(priority);

        StartCoroutine(Action());
        StartCoroutine(Life());
    }

    IEnumerator Action()
    {
        conMeneger.GetComponent<ConnectionMeneger>().SendData(gameObject);
        yield return null;
    }

    IEnumerator Life()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            food -= 1f;
            thirst -= 1f;
            needReproduction += 0.5f;

            if(food == 0 || thirst == 0 || health == 0)
            {
                curentCell.GetComponent<Cell>().deleteObject();
                generator.GetComponent<GenerateBots>().bots.Remove(gameObject);
                isDeath = true;
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Move(string aim)
    {
        if(aim == "food")
        {
            status = "Идёт к еде";
            if (foodObject != null)
            {
                hasAimFood = true;

                float minDistance = float.MaxValue;
                GameObject moveTo = curentCell;
                foreach (GameObject i in curentCell.GetComponent<Cell>().tilesAround)
                {
                    if (i.GetComponent<Cell>().isWalkable == true)
                    {
                            
                            float newDistance = Mathf.Sqrt(Mathf.Pow((float)(foodPosition.x - i.transform.position.x), 2f) + Mathf.Pow((float)(foodPosition.y - i.transform.position.y), 2f));
                            if (newDistance < minDistance)
                            {
                                 minDistance = newDistance;
                                 moveTo = i;
                            }
                    }
                }
                moveTo.GetComponent<Cell>().setObject(gameObject);
                curentCell.GetComponent<Cell>().deleteObject();
                curentCell = moveTo;
                gameObject.transform.position = moveTo.transform.position;
                StopMoving("food");
            }
            else
            {
                GameObject moveTo;
                while (true)
                {
                    moveTo = curentCell.GetComponent<Cell>().tilesAround[Random.Range(0, curentCell.GetComponent<Cell>().tilesAround.Count - 1)];
                    if(moveTo.GetComponent<Cell>().isWalkable == true)
                    {
                        break;
                    }
                }
                moveTo.GetComponent<Cell>().setObject(gameObject);
                curentCell.GetComponent<Cell>().deleteObject();
                curentCell = moveTo;
                gameObject.transform.position = moveTo.transform.position;
                StopMoving("food");
            }
            yield return new WaitForSeconds(1f);
        }
        else if(aim == "drink")
        {
            status = "Идёт к воде";
            if (waterObject != null)
            {
                hasAimWater = true;

                float minDistance = float.MaxValue;
                GameObject moveTo = curentCell;
                foreach (GameObject i in curentCell.GetComponent<Cell>().tilesAround)
                {
                    if (i.GetComponent<Cell>().isWalkable == true)
                    {
                            float newDistance = Mathf.Sqrt(Mathf.Pow((float)(foodPosition.x - i.transform.position.x), 2f) + Mathf.Pow((float)(foodPosition.y - i.transform.position.y), 2f));
                            if (newDistance < minDistance)
                            {
                                minDistance = newDistance;
                                moveTo = i;
                            }
                    }
                }
                moveTo.GetComponent<Cell>().setObject(gameObject);
                curentCell.GetComponent<Cell>().deleteObject();
                curentCell = moveTo;
                gameObject.transform.position = moveTo.transform.position;
                StopMoving("drink");
            }
            else
            {
                GameObject moveTo;
                while (true)
                {
                    moveTo = curentCell.GetComponent<Cell>().tilesAround[Random.Range(0, curentCell.GetComponent<Cell>().tilesAround.Count - 1)];
                    if (moveTo.GetComponent<Cell>().isWalkable == true)
                    {
                        break;
                    }
                }
                moveTo.GetComponent<Cell>().setObject(gameObject);
                curentCell.GetComponent<Cell>().deleteObject();
                curentCell = moveTo;
                gameObject.transform.position = moveTo.transform.position;
                StopMoving("drink");
            }
            yield return new WaitForSeconds(1f);
        }
        else if (aim == "partner")
        {
            if (partner != null)
            {
                if(isMain)
                {
                    yield return new WaitForSeconds(1f);
                }
                float minDistance = float.MaxValue;
                GameObject moveTo = curentCell;
                foreach (GameObject i in curentCell.GetComponent<Cell>().tilesAround)
                {
                    if (i.GetComponent<Cell>().havingObject == null)
                    {
                        float newDistance = Mathf.Sqrt(Mathf.Pow((float)(partner.transform.position.x - i.transform.position.x), 2f) + Mathf.Pow((float)(partner.transform.position.y - i.transform.position.y), 2f));
                            if (newDistance < minDistance)
                            {
                                minDistance = newDistance;
                                moveTo = i;
                            }

                    }
                }
                moveTo.GetComponent<Cell>().setObject(gameObject);
                curentCell.GetComponent<Cell>().deleteObject();
                curentCell = moveTo;
                gameObject.transform.position = moveTo.transform.position;
                StopMoving("reproduction");
            }
            else
            {
                GameObject moveTo;
                while (true)
                {
                    moveTo = curentCell.GetComponent<Cell>().tilesAround[Random.Range(0, curentCell.GetComponent<Cell>().tilesAround.Count - 1)];
                    if (moveTo.GetComponent<Cell>().havingObject == null)
                    {
                        break;
                    }
                }
                moveTo.GetComponent<Cell>().setObject(gameObject);
                curentCell.GetComponent<Cell>().deleteObject();
                curentCell = moveTo;
                gameObject.transform.position = moveTo.transform.position;
                StopMoving("reproduction");
            }
            yield return new WaitForSeconds(1f);
        }

    }

    IEnumerator Eat()
    {
        status = "Решил искать еду";
        while (true)
        {
            foreach (var i in curentCell.GetComponent<Cell>().tilesAround)
            {
                if (i.GetComponent<Cell>().havingObject != null)
                {
                    if (foodType == 0)
                    {
                        if (i.GetComponent<Cell>().havingObject.tag == "Bush")
                        {
                            float[] _nums = i.GetComponent<Cell>().havingObject.GetComponent<CustScript>().getFood();

                            if (food + _nums[0] > maxFood)
                            {
                                food = maxFood;
                            }
                            else food += _nums[0];

                            if (thirst + _nums[1] > maxThirst)
                            {
                                thirst = maxThirst;
                            }
                            else thirst += _nums[1];

                            isFoundFood = true;
                            break;
                        }
                    }
                    else if(foodType == 1)
                    {
                        if (i.GetComponent<Cell>().havingObject.tag == "Bot")
                        {
                            i.GetComponent<Cell>().havingObject.GetComponent<BotScript>().health -= dammage * 2f;
                            float _food = dammage * 2f;
                            if (food + _food > maxFood)
                            {
                                food = maxFood;
                            }
                            else food += _food;
                            isFoundFood = true;
                            break;
                        }
                    }
                    else
                    {
                        if (i.GetComponent<Cell>().havingObject.tag == "Bush")
                        {
                            float[] _nums = i.GetComponent<Cell>().havingObject.GetComponent<CustScript>().getFood();

                            if (food + _nums[0] > maxFood)
                            {
                                food = maxFood;
                            }
                            else food += _nums[0];

                            if (thirst + _nums[1] > maxThirst)
                            {
                                thirst = maxThirst;
                            }
                            else thirst += _nums[1];

                            isFoundFood = true;
                            break;
                        }
                        else
                        {
                            if (i.GetComponent<Cell>().havingObject.tag == "Bot")
                            {
                                i.GetComponent<Cell>().havingObject.GetComponent<BotScript>().health -= dammage * 2f;
                                float _food = dammage * 2f;
                                if (food + _food > maxFood)
                                {
                                    food = maxFood;
                                }
                                else food += _food;
                                isFoundFood = true;
                                break;
                            }
                        }
                    }
                }
            }
            if(isFoundFood)
            {
                break;
            }
            else yield return StartCoroutine(Move("food"));
        }

        StopMoving("eat");
        isFoundFood = false;
        yield return null;

    }

    IEnumerator Drink()
    {
        status = "Решил искать воду";
        while (true)
        {
            foreach (var i in curentCell.GetComponent<Cell>().tilesAround)
            {
                if (i.GetComponent<Cell>().havingObject != null)
                {
                    if (i.GetComponent<Cell>().havingObject.tag == "Water")
                    {
                        float _nums = i.GetComponent<Cell>().havingObject.GetComponent<WaterScript>().GetWater();
                        if (thirst + _nums > maxThirst)
                        {
                            thirst = maxThirst;
                        }
                        else thirst += _nums;

                        isFoundWater = true;
                        break;
                    }
                }
            }
            if (isFoundWater)
            {
                break;
            }
            else yield return StartCoroutine(Move("drink"));
        }

        StopMoving("drink");
        isFoundWater = false;
        yield return null;
    }

    IEnumerator ReproductionSeek()
    {
        partnerSeek = true;
        status = "Размножение";
        while (true)
        {
            foreach (var i in curentCell.GetComponent<Cell>().tilesAround)
            {
                if (i.GetComponent<Cell>().havingObject != null)
                {
                    if (i.GetComponent<Cell>().havingObject.tag == "Bot" && i.GetComponent<Cell>().havingObject.gameObject == partner)
                    {
                        if(isMain)
                        {
                            float[] _priority = priority;
                            BotScript bot2 = i.GetComponent<Cell>().havingObject.GetComponent<BotScript>();
                            if(Random.Range(0,101) <= 5)
                            {
                                Shuffle(_priority);
                            }
                            generator.GetComponent<GenerateBots>().createNewBot(curentCell, generation + 1, ((maxFood / 2) + (bot2.maxFood / 2) * Random.Range(0.9f, 1.2f)), ((maxThirst / 2) + (bot2.maxThirst / 2) * Random.Range(0.9f, 1.2f)), ((maxNeedReproduction / 2) + (bot2.maxNeedReproduction / 2) * Random.Range(0.9f, 1.2f)), _priority);
                        }
                        needReproduction = 0;
                        partner = null;
                        partnerSeek = false;
                        break;
                    }
                }
            }
            if (partner == null && partnerSeek == false)
            {
                break;
            }
            else yield return StartCoroutine(Move("partner"));
        }

        StopMoving("reproduction");
        yield return null;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bush")
        {
            if (foodType != 1)
            {
                if (collision.gameObject != foodObject)
                {
                    if (hasAimFood == false)
                    {
                        foodObject = collision.gameObject;
                        foodPosition = collision.gameObject.transform.position;
                    }
                }
            }
        }

        else if(collision.gameObject.tag == "Water")
        {
            if (collision.gameObject != waterObject)
            {
                if (hasAimWater == false)
                {
                    waterObject = collision.gameObject;
                    waterPosition = collision.gameObject.transform.position;
                }
            }
        }

        else if(collision.gameObject.tag == "Bot")
        {
            if (partnerSeek)
            {
                if(partner == null)
                {
                    partner = collision.gameObject;
                    if(isMain == false)
                    {
                        collision.gameObject.GetComponent<BotScript>().SetMain();
                    }
                }
            }
            else if(foodType != 0)
            {
                if(collision.gameObject != foodObject)
                {
                    if (!hasAimFood)
                    {
                        foodObject = collision.gameObject;
                    }
                }
            }
        }
    }


    public void StopMoving(string type)
    {
        if (type == "eat")
        {
            hasAimFood = false;
        }
        else if (type == "drink")
        {
            hasAimWater = false;
        }
        else if (type == "reproduction")
        {
            partnerSeek = false;
            isMain = false;
        }
        StartCoroutine(Action());
    }

    public void SetMain()
    {
        isMain = true;
    }

    private void OnDestroy()
    {
        if(partner != null)
        {
            partner.GetComponent<BotScript>().StopMoving("reproduction");
        }
    }

    public void GetTask(float i)
    {
        botQuest = i;
        switch(i)
        {
            case 0:
                StartCoroutine(Eat());
                break;
            case 1:
                StartCoroutine(Drink());
                break;
            case 2:
                StartCoroutine(ReproductionSeek());
                break;
        }        
    }
}
