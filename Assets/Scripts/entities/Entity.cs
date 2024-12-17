using UnityEngine;

public class Entity : MonoBehaviour
{
    // Здоровье персонажа
    public float health = 100f;

    // Скорость движения
    public float speed = 12f;

    // Метод получения урона
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("damage");
        if (health <= 0)
        {
            Die();
        }
    }

    // Метод, вызываемый при смерти
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // Удаляем объект
    }

    // Вычисление вектора движения
    protected virtual Vector3 CalculateMovement()
    {
        return Vector3.zero;
    }

    // Базовое передвижение
    protected virtual void Move(Vector3 moveDirection, float moveSpeed)
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}