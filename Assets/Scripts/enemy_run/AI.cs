using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float speed = 5.0f;
    public float obstacleRandle = 5.0f;
    public bool _alive = true;
    public int health = 10; // �������� �����

    [SerializeField]
    private GameObject[] _fireballsPrefab;

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
            if (player != null)
            {
                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            transform.Translate(0, 0, speed * Time.deltaTime);

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;

                if (hitObject.GetComponent<CharacterController>() == null && hit.distance < obstacleRandle)
                {
                    float angleRotation = Random.Range(-100, 100);
                    transform.Rotate(0, angleRotation, 0);
                }
            }
        }
    }

    private IEnumerator ShootFireballs()
    {
        while (_alive)
        {
            yield return new WaitForSeconds(1.5f);

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

        // ����� ����� �������� �������� ������, �������, ����� � �.�.

        Destroy(gameObject); // ���������� ������ �����
    }
}
