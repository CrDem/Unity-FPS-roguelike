using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // скорость
    public int damage = 1; // урон
    private Rigidbody rb;
    public float maxDistance = 20f; // максимальное рассто€ние полета
    //можете установить максдистанс значение в инспекторе
    private Vector3 spawnPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody не найден на объекте " + gameObject.name);
            return;
        }

        if (speed <= 0)
        {
            Debug.LogError("—корость огненного шара должна быть больше 0. “екуща€ скорость: " + speed);
            return;
        }

        // «апоминаем начальную позицию
        spawnPosition = transform.position;

        // »спользуем AddForce дл€ начального движени€
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ћогируем им€ объекта, с которым произошло столкновение
        Debug.Log("ќгненный шар столкнулс€ с: " + collision.gameObject.name);

        // ѕровер€ем, столкнулс€ ли огненный шар с игроком
        PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.Hurt(damage); // Ќаносим урон, если столкнулись с персонажем
            Debug.Log("ќгненный шар нанес урон игроку: " + damage);
        }

        // ”ничтожаем огненный шар при столкновении с игроком или стеной
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject); // ”ничтожаем огненный шар
            Debug.Log("ќгненный шар уничтожен.");
        }
    }
    private void Update()
    {
        // ѕровер€ем рассто€ние от начальной позиции
        if (Vector3.Distance(spawnPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject); // ”ничтожаем огненный шар, если он пролетел максимальное рассто€ние
            Debug.Log("ќгненный шар уничтожен, пролетев максимальное рассто€ние.");
        }
    }
}

