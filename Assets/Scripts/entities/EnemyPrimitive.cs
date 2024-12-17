using UnityEngine;

public class EnemyPrimitive : Entity
{
    public Transform pointA; // Первая точка маршрута
    public Transform pointB; // Вторая точка маршрута
    public float patrolSpeed = 5f; // Скорость патрулирования

    private CharacterController controller; // CharacterController для управления
    private Transform player; // Ссылка на объект игрока
    private Vector3 targetPoint; // Текущая цель движения

    void Start()
    {

        diffContoller = GameObject.FindGameObjectWithTag("Controller").GetComponent<DifficultyContoller>();

        // Инициализация CharacterController
        
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController is missing on EnemyPrimitive!");
            return;
        }

        // Устанавливаем начальную точку
        targetPoint = pointA.position;

        // Находим игрока по тегу "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found! Make sure the player has the 'Player' tag.");
        }
    }

    
    void Update()
    {
        if (controller == null) return;
        UpdateDifficulty();
        
        // Логика перемещения
        Vector3 move = CalculateMovement();
        Move(move, patrolSpeed);

        // Логика поворота к игроку
        LookAtPlayer();
    }

    // Рассчитываем вектор движения для патрулирования
    protected override Vector3 CalculateMovement()
    {
        // Вычисляем позицию без учёта высоты
        Vector3 flatPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatTarget = new Vector3(targetPoint.x, 0, targetPoint.z);

        // Если враг достиг текущей цели, меняем её на противоположную
        if (Vector3.Distance(flatPosition, flatTarget) < 0.1f)
        {
            targetPoint = targetPoint == pointA.position ? pointB.position : pointA.position;
        }

        // Вычисляем направление к текущей цели, игнорируя высоту
        Vector3 direction = (targetPoint - transform.position).normalized;
        direction.y = 0; // Убираем вертикальную составляющую

        return direction;
    }


    // Поворачиваем врага к игроку
    private void LookAtPlayer()
    {
        if (player == null) return;

        // Рассчитываем направление к игроку
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // Игнорируем высоту

        // Поворачиваем врага в сторону игрока
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
    }

    // Перемещение врага
    protected override void Move(Vector3 move, float moveSpeed)
    {
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    // Переопределение метода смерти
    protected override void Die()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject); // Удаляем объект врага
    }
}
