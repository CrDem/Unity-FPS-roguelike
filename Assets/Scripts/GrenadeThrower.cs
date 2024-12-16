using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [Header("Grenade Settings")]
    public GameObject grenadePrefab;
    public Transform grenadeSpawnPoint;
    public float throwForce = 10f; // Сила броска
    public float upwardForce = 2f; // Вертикальная составляющая броска
    public int grenadeCount = 5; // Количество гранат

    [Header("Camera Settings")]
    public Camera playerCamera;

    private void Update()
    {
        // Бросок гранаты по нажатию средней кнопки мыши
        if (Input.GetMouseButtonDown(2) && grenadeCount > 0)
        {
            Throw();
        }
    }

    private void Throw()
    {
        // Проверка на наличие необходимых компонентов
        if (grenadePrefab == null || playerCamera == null || grenadeSpawnPoint == null)
        {
            Debug.LogWarning("GrenadeThrower is not fully set up. Make sure all public fields are assigned in the Inspector.");
            return;
        }


        GameObject grenade = Instantiate(grenadePrefab, grenadeSpawnPoint.position, Quaternion.identity);

        // Вычисление направления броска
        Vector3 throwDirection = playerCamera.transform.forward; // Направление по камере
        throwDirection.y += upwardForce / throwForce; // Добавляем вертикальную составляющую

        // Добавление силы к гранате
        Rigidbody grenadeRb = grenade.GetComponent<Rigidbody>();
        if (grenadeRb != null)
        {
            grenadeRb.AddForce(throwDirection.normalized * throwForce + Vector3.up * upwardForce, ForceMode.Impulse);
        }

        // Уменьшение количества гранат
        grenadeCount--;
    }
}
