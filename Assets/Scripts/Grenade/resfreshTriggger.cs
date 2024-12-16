using UnityEngine;

public class resfreshTriggger: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что триггер активируется только игроком
        if (other.CompareTag("Player"))
        {
            GrenadeThrower thrower = other.GetComponent<GrenadeThrower>();

            if (thrower != null)
            {
                thrower.grenadeCountUpdate();
            }
            
            // Удаляем триггер после использования (если не нужен повторно)
            Destroy(gameObject);
        }
    }
}
