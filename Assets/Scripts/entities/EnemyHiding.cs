using Unity.VisualScripting;
using UnityEngine;

public class EnemyHiding : Entity
{
    public Transform[] patrolPoints; // Массив точек маршрута
    public float patrolSpeed = 5f; // Скорость патрулирования

    private CharacterController controller; // CharacterController для управления
    private Transform player; // Ссылка на объект игрока
    private int currentPatrolIndex = 0; // Индекс текущей точки маршрута
    private Vector3 targetPoint; // Текущая цель движения

    public float detectionRange = 10f; // Радиус обнаружения игрока
    public Transform rayOrigin; // Точка, откуда выпускается луч
    public Transform rayDirection; // Направление выпуска луча
    public LayerMask obstacleMask; // Маска для определения препятствий (например, стены)

    private bool running = true;
    
    void Start()
    {
        // Инициализация CharacterController
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController is missing on EnemyPrimitive!");
            return;
        }

        // Проверка наличия точек патруля
        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points assigned!");
            return;
        }

        // Устанавливаем начальную точку маршрута
        targetPoint = patrolPoints[currentPatrolIndex].position;

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
        if (controller == null || patrolPoints.Length == 0) return;

        // Логика перемещения
        // Проверяем расстояние до игрока (только по XZ)
        float distanceToPlayer = Vector3.Distance(ProjectToXZ(player.position), ProjectToXZ(transform.position));
        if (distanceToPlayer < detectionRange && CanSeePlayer() && currentPatrolIndex <= 3)
        {
            running = true;
        }
        if (running)
        {
            Vector3 move = CalculateMovement();
            Move(move, patrolSpeed);
        }

        // Логика поворота к игроку
        LookAtPlayer();
    }

    // Рассчитываем вектор движения для патрулирования
    protected override Vector3 CalculateMovement()
    {
        // Вычисляем позицию без учёта высоты
        Vector3 flatPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatTarget = new Vector3(targetPoint.x, 0, targetPoint.z);

        // Если враг достиг текущей цели, переходим к следующей точке маршрута
        if (Vector3.Distance(flatPosition, flatTarget) < 0.1f)
        {
            if (currentPatrolIndex < patrolPoints.Length - 1)
            {
                ++currentPatrolIndex;
            }
            targetPoint = patrolPoints[currentPatrolIndex].position; // Обновляем цель
            running = false;
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

    bool CanSeePlayer()
    {
        // Рассчитываем направление от врага к игроку, проецируя на XZ
        Vector3 direction = player.position - rayOrigin.position;

        // Рисуем отладочный луч
        Debug.DrawRay(rayOrigin.position, direction * detectionRange, Color.red);

        // Пускаем луч и проверяем, что он пересекается с игроком
        if (Physics.Raycast(rayOrigin.position, direction, out RaycastHit hit, detectionRange, ~obstacleMask))
        {
            // Отладка: проверим, что мы попали в игрока
            if (hit.transform == player)
            {
                //Debug.Log("Player detected!");
                return true;
            }
            //else
            //{
                //Debug.Log("Hit something else: " + hit.transform.name);
            //}
        }

        return false;
    }

    
    // Переопределение метода смерти
    protected override void Die()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject); // Удаляем объект врага
    }
    
    // Проекция позиции на XZ
    private Vector3 ProjectToXZ(Vector3 position)
    {
        return new Vector3(position.x, 0, position.z);
    }
}
