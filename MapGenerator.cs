using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public float scale;

    public int octaves;
    public float persistence;
    public float lacunarity;
    public int seed;

    public GameObject cell;
    public GameObject wood;
    public GameObject botGenerator;
    public GameObject custGenerator;
    public GameObject waterGenerator;

    Quaternion quaternion = Quaternion.Euler(0, 0, 0);

    public Vector2 offset;

    public List<GameObject> tiles;
    void Start()
    {
        GenerateMap();
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            seed = Random.Range(0, 9999);
            GenerateMap();
        }
    }

    void GenerateMap()
    {
        Vector2[] octavesOffset = new Vector2[octaves];

        System.Random rand = new System.Random(seed);

        for (int i = 0; i < octaves; i++)
        {
            
            float xOffset = rand.Next(-100000, 100000) + offset.x;
            float yOffset = rand.Next(-100000, 100000) + offset.y;
            octavesOffset[i] = new Vector2(xOffset / width, yOffset / height);
        }

        if (scale < 0)
        {
            scale = 0.0001f;
        }

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        // Генерируем точки на карте высот
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Задаём значения для первой октавы
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                float superpositionCompensation = 0;

                // Обработка наложения октав
                for (int i = 0; i < octaves; i++)
                {
                    // Рассчитываем координаты для получения значения из Шума Перлина
                    float xResult = (x - halfWidth) / scale * frequency + octavesOffset[i].x * frequency;
                    float yResult = (y - halfHeight) / scale * frequency + octavesOffset[i].y * frequency;

                    // Получение высоты из ГСПЧ
                    float generatedValue = Mathf.PerlinNoise(xResult, yResult);
                    // Наложение октав
                    noiseHeight += generatedValue * amplitude;
                    // Компенсируем наложение октав, чтобы остаться в границах диапазона [0,1]
                    noiseHeight -= superpositionCompensation;

                    // Расчёт амплитуды, частоты и компенсации для следующей октавы
                    amplitude *= persistence;
                    frequency *= lacunarity;
                    superpositionCompensation = amplitude / 2;
                }

                // Сохраняем точку для карты высот
                // Из-за наложения октав есть вероятность выхода за границы диапазона [0,1]
                GameObject obj = Instantiate(cell, new Vector2(y, x), quaternion);
                cell.GetComponent<Cell>().Height = Mathf.Clamp01(noiseHeight);
                if (Mathf.Clamp01(noiseHeight) > 0.31f)
                {
                    if (Random.Range(0, 7) == 0 && Mathf.Clamp01(noiseHeight) > 0.4f)
                    {
                        obj.GetComponent<Cell>().setObject(Instantiate(wood, new Vector2(y, x + 0.9f), quaternion));
                    }
                    else
                    {
                        tiles.Add(obj);
                        obj.layer = 6;
                    }
                }
            }
        }

        custGenerator.GetComponent<CustGenerator>().startGenerator(tiles);

        waterGenerator.GetComponent<WaterGenerator>().generateWater(tiles);

        botGenerator.GetComponent<GenerateBots>().generateBots(tiles);
        foreach(var i in tiles)
        {
            i.GetComponent<Cell>().CheckAround();
        }

    }
}
