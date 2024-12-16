using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Второй тип врага
public enum EnemyType
{
    Fire,
    Stealth
}

public class AI : MonoBehaviour
{
    public float speed = 5.0f;
    public float obstacleRandle = 5.0f;
    public bool _alive = true;
    public int health = 10; // Здоровье врага

    [SerializeField]
    private GameObject[] _fireballsPrefab;

    [SerializeField]
    private Transform[] hidingSpots; // Точки укрытия для второго врага
    public EnemyType enemyType; // Тип врага
    private bool isHiding = true; // Флаг, указывающий, прячется ли враг
    private int currentHidingSpotIndex = 0; // Индекс текущего укрытия

    private GameObject player;

    private void Start()
    {
        _alive = true;
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ShootFireballs());
    }

  
    private void Update()
    {
        if (_alive)
        {
            if (enemyType == EnemyType.Fire)
            {
                MoveTowardsPlayer();
            }
            else if (enemyType == EnemyType.Stealth)
            {
                HandleStealthBehavior();
            }
        }
    }
    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
    private void HandleStealthBehavior()
    {
        // Проверка на расстояние до игрока
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer < 10f && isHiding) // Если игрок близко и враг прячется
        {
            isHiding = false; // Враг начинает двигаться
            MoveToNextHidingSpot();
        }
        else if (distanceToPlayer >= 10f && !isHiding) // Если игрок далеко, враг возвращается в укрытие
        {
            isHiding = true;
            MoveToCurrentHidingSpot();
        }
    }
    private void MoveToCurrentHidingSpot()
    {
        if (hidingSpots.Length > 0)
        {
            Vector3 targetPosition = hidingSpots[currentHidingSpotIndex].position;
            StartCoroutine(MoveTowards(targetPosition));
            //transform.position = hidingSpots[currentHidingSpotIndex].position;
            //Debug.Log("Враг вернулся в укрытие: " + hidingSpots[currentHidingSpotIndex].name);
        }
    }
    private void MoveToNextHidingSpot()
    {
        if (hidingSpots.Length > 0)
        {
            // Переход к следующему укрытию
            currentHidingSpotIndex = (currentHidingSpotIndex + 1) % hidingSpots.Length; // Циклический переход
            Vector3 targetPosition = hidingSpots[currentHidingSpotIndex].position;
            StartCoroutine(MoveTowards(targetPosition));
            //currentHidingSpotIndex = (currentHidingSpotIndex + 1) % hidingSpots.Length; // Циклический переход
            //transform.position = hidingSpots[currentHidingSpotIndex].position;
            //Debug.Log("Враг переместился в следующее укрытие: " + hidingSpots[currentHidingSpotIndex].name);
        }
    }
    //private IEnumerator MoveTowards(Vector3 targetPosition)
    //{
    //    while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
    //    {
    //        // Рассчитываем направление к цели
    //        Vector3 direction = (targetPosition - transform.position).normalized;

    //        // Перемещаем врага
    //        transform.position += direction * speed * Time.deltaTime;

    //        // Проверка на столкновение с препятствиями
    //        RaycastHit hit;
    //        if (Physics.Raycast(transform.position, direction, out hit, obstacleRandle))
    //        {
    //            // Если столкновение произошло, останавливаем движение
    //            break;
    //        }

    //        yield return null; // Ждем следующего кадра
    //    }

    //    // Устанавливаем позицию точно в укрытие, чтобы избежать небольших ошибок
    //    transform.position = targetPosition;
    //    Debug.Log("Враг вернулся в укрытие: " + hidingSpots[currentHidingSpotIndex].name);
    //}
    private IEnumerator MoveTowards(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Рассчитываем направление к цели
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Проверка на столкновение с препятствиями
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, obstacleRandle))
            {
                // Если столкновение произошло, пытаемся обойти препятствие
                Vector3 avoidanceDirection = Vector3.Cross(direction, Vector3.up); // Перпендикулярное направление
                Vector3 newDirection = Vector3.Lerp(direction, avoidanceDirection, 0.5f).normalized; // Изменяем направление

                // Перемещаем врага в новом направлении
                transform.position += newDirection * speed * Time.deltaTime;
            }
            else
            {
                // Если нет столкновения, перемещаем врага в направлении цели
                transform.position += direction * speed * Time.deltaTime;
            }

            yield return null; // Ждем следующего кадра
        }

        // Устанавливаем позицию точно в укрытие, чтобы избежать небольших ошибок
        transform.position = targetPosition;
        Debug.Log("Враг вернулся в укрытие: " + hidingSpots[currentHidingSpotIndex].name);
    }


    private IEnumerator ShootFireballs()
    {
        while (_alive)
        {
            yield return new WaitForSeconds(1.5f);

            // Выбираем случайный префаб огненного шара из массива
            int randFireball = Random.Range(0, _fireballsPrefab.Length);
            GameObject _fireball = Instantiate(_fireballsPrefab[randFireball]);

            // Устанавливаем позицию огненного шара немного вперед от AI
            _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
            _fireball.transform.rotation = transform.rotation;

            // Направляем огненный шар в сторону игрока
            Vector3 directionToPlayer = (player.transform.position - _fireball.transform.position).normalized;
            _fireball.GetComponent<Rigidbody>().linearVelocity = directionToPlayer * 10f; // Используем linearVelocity

            // Запускаем корутину для уничтожения огненного шара через 5 секунд
            StartCoroutine(DestroyFireballAfterTime(_fireball, 5f));
        }
    }

    private IEnumerator DestroyFireballAfterTime(GameObject fireball, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(fireball);
        Debug.Log("Огненный шар уничтожен.");
    }

    public void SetAlive(bool alive)
    {
        _alive = alive;
    }

    // Метод для получения урона
    public void TakeDamage(int damage)
    {
        health -= damage; // Уменьшаем здоровье на величину урона
        Debug.Log("Враг получил урон: " + damage + ". Текущее здоровье: " + health);

        // Проверяем, жив ли враг
        if (health <= 0)
        {
            Die(); // Вызываем метод смерти
        }
    }

    // Метод, вызываемый при смерти врага
    private void Die()
    {
        Debug.Log("Враг умер!");
        Destroy(gameObject); // Уничтожаем объект врага
    }
} 
