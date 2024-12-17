using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public int score = 0;              // Счётчик очков игрока
    public int spawnIndex = 0;         // Текущий индекс для спавна
    private List<int> blockQueue;      // Очередь индексов для блоков
    private int[] blockIndices = { 0, 1, 2, 3 }; // Набор индексов
    public Queue<GameObject> spawnedObjects = new Queue<GameObject>(); // Очередь объектов
    public GameObject startBlock;          // Ссылка на стартовый блок


    private void Start()
    {
        // Создаём очередь и перемешиваем индексы
        blockQueue = new List<int>();
        RefillQueue();
        UpdateSpawnIndex();
        
        // Добавляем стартовый блок в очередь
        if (startBlock != null)
        {
            spawnedObjects.Enqueue(startBlock);
        }
        else
        {
            Debug.LogWarning("StartBlock не назначен в PlayerStats!");
        }
    }

    public void IncrementScore()
    {
        // Увеличиваем счёт
        score++;

        // Удаляем первый элемент очереди
        if (blockQueue.Count > 0)
        {
            blockQueue.RemoveAt(0);
        }

        // Если очередь пустая, заполняем её снова
        if (blockQueue.Count == 0)
        {
            RefillQueue();
        }

        // Обновляем spawnIndex
        UpdateSpawnIndex();
    }

    public void AddSpawnedObject(GameObject spawnedObject)
    {
        // Добавляем объект в очередь
        spawnedObjects.Enqueue(spawnedObject);

        // Если в очереди больше 2 объектов, удаляем самый старый
        if (spawnedObjects.Count > 2)
        {
            GameObject oldestObject = spawnedObjects.Dequeue();
            Destroy(oldestObject);
        }
    }
    
    private void RefillQueue()
    {
        // Заполняем очередь случайной перестановкой индексов
        List<int> tempList = new List<int>(blockIndices);
        Shuffle(tempList);
        blockQueue.AddRange(tempList);
    }

    private void Shuffle(List<int> list)
    {
        // Перемешивание списка
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void UpdateSpawnIndex()
    {
        // Устанавливаем spawnIndex как первый элемент очереди
        if (blockQueue.Count > 0)
        {
            spawnIndex = blockQueue[0];
        }
    }
}
