using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��� �����
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
    public int health = 10; // �������� �����

    [SerializeField]
    private GameObject[] _fireballsPrefab;

    [SerializeField]
    private Transform[] hidingSpots; // ����� ������� ��� ������� �����
    public EnemyType enemyType; // ��� �����
    private bool isHiding = true; // ����, �����������, �������� �� ����
    private int currentHidingSpotIndex = 0; // ������ �������� �������

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
        // �������� �� ���������� �� ������
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer < 10f && isHiding) // ���� ����� ������ � ���� ��������
        {
            isHiding = false; // ���� �������� ���������
            MoveToNextHidingSpot();
        }
        else if (distanceToPlayer >= 10f && !isHiding) // ���� ����� ������, ���� ������������ � �������
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
        }
    }
    private void MoveToNextHidingSpot()
    {
        if (hidingSpots.Length > 0)
        {
            // ������� � ���������� �������
            currentHidingSpotIndex = (currentHidingSpotIndex + 1) % hidingSpots.Length; // ����������� �������
            Vector3 targetPosition = hidingSpots[currentHidingSpotIndex].position;
            StartCoroutine(MoveTowards(targetPosition));
        }
    }
   
    private IEnumerator MoveTowards(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // ������������ ����������� � ����
            Vector3 direction = (targetPosition - transform.position).normalized;

            // �������� �� ������������ � �������������
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, obstacleRandle))
            {
                // ���� ������������ ���������, �������� ������ �����������
                Vector3 avoidanceDirection = Vector3.Cross(direction, Vector3.up); // ���������������� �����������
                Vector3 newDirection = Vector3.Lerp(direction, avoidanceDirection, 0.5f).normalized; // �������� �����������

                // ���������� ����� � ����� �����������
                transform.position += newDirection * speed * Time.deltaTime;
            }
            else
            {
                // ���� ��� ������������, ���������� ����� � ����������� ����
                transform.position += direction * speed * Time.deltaTime;
            }

            yield return null; // ���� ���������� �����
        }

        // ������������� ������� ����� � �������, ����� �������� ��������� ������
        transform.position = targetPosition;
        Debug.Log("���� �������� � �������: " + hidingSpots[currentHidingSpotIndex].name);
    }


    private IEnumerator ShootFireballs()
    {
        while (_alive)
        {
            yield return new WaitForSeconds(3.5f);

            // �������� ��������� ������ ��������� ���� �� �������
            int randFireball = Random.Range(0, _fireballsPrefab.Length);
            GameObject _fireball = Instantiate(_fireballsPrefab[randFireball]);

            // ������������� ������� ��������� ���� ������� ������ �� AI
            _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
            _fireball.transform.rotation = transform.rotation;

            // ���������� �������� ��� � ������� ������
            Vector3 directionToPlayer = (player.transform.position - _fireball.transform.position).normalized;
            _fireball.GetComponent<Rigidbody>().linearVelocity = directionToPlayer * 10f; // ���������� linearVelocity

            // ��������� �������� ��� ����������� ��������� ���� ����� 5 ������
            StartCoroutine(DestroyFireballAfterTime(_fireball, 5f));
        }
    }

    private IEnumerator DestroyFireballAfterTime(GameObject fireball, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(fireball);
        Debug.Log("�������� ��� ���������.");
    }

    public void SetAlive(bool alive)
    {
        _alive = alive;
    }

    // ����� ��� ��������� �����
    public void TakeDamage(int damage)
    {
        health -= damage; // ��������� �������� �� �������� �����
        Debug.Log("���� ������� ����: " + damage + ". ������� ��������: " + health);

        // ���������, ��� �� ����
        if (health <= 0)
        {
            Die(); // �������� ����� ������
        }
    }

    // �����, ���������� ��� ������ �����
    private void Die()
    {
        Debug.Log("���� ����!");
        Destroy(gameObject); // ���������� ������ �����
    }
} 
