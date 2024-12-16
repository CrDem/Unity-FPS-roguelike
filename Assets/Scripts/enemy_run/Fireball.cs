using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // ��������
    public int damage = 1; // ����
    private Rigidbody rb;
    public float maxDistance = 20f; // ������������ ���������� ������
    //������ ���������� ����������� �������� � ����������
    private Vector3 spawnPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody �� ������ �� ������� " + gameObject.name);
            return;
        }

        if (speed <= 0)
        {
            Debug.LogError("�������� ��������� ���� ������ ���� ������ 0. ������� ��������: " + speed);
            return;
        }

        // ���������� ��������� �������
        spawnPosition = transform.position;

        // ���������� AddForce ��� ���������� ��������
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �������� ��� �������, � ������� ��������� ������������
        Debug.Log("�������� ��� ���������� �: " + collision.gameObject.name);

        /* ���������, ���������� �� �������� ��� � �������
        PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.Hurt(damage); // ������� ����, ���� ����������� � ����������
            Debug.Log("�������� ��� ����� ���� ������: " + damage);
        }*/

        // ���������� �������� ��� ��� ������������ � ������� ��� ������
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject); // ���������� �������� ���
            Debug.Log("�������� ��� ���������.");
        }
    }
    private void Update()
    {
        // ��������� ���������� �� ��������� �������
        if (Vector3.Distance(spawnPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject); // ���������� �������� ���, ���� �� �������� ������������ ����������
            Debug.Log("�������� ��� ���������, �������� ������������ ����������.");
        }
    }
}

