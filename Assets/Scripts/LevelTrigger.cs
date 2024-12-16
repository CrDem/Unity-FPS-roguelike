using UnityEngine;

public class LevelTrigger: MonoBehaviour
{
    public GameObject[] objectsToSpawn; // Список объектов для спавна
    public Transform spawnLocation;    // Место для спавна
    public int scoreIncrement = 1;     // Сколько прибавляется к счётчику
    
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что триггер активируется только игроком
        if (other.CompareTag("Player"))
        {
            // Получаем компонент игрока, который содержит счётчик и очередь
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                // Получаем объект для спавна по текущему индексу
                int index = playerStats.spawnIndex;

                // Проверяем, что индекс не выходит за пределы массива
                if (index >= 0 && index < objectsToSpawn.Length)
                {
                    // Создаём объект по индексу
                    GameObject spawnedObject = Instantiate(objectsToSpawn[index], spawnLocation.position, spawnLocation.rotation);
                    
                    // Добавляем созданный объект в очередь
                    playerStats.AddSpawnedObject(spawnedObject);
                }
                else
                {
                    Debug.LogWarning("Индекс спавна выходит за пределы массива объектов.");
                }
                
                // Увеличиваем счётчик игрока и обновляем очередь
                playerStats.IncrementScore();
            }

            // Удаляем триггер после использования (если не нужен повторно)
            Destroy(gameObject);
        }
    }
}