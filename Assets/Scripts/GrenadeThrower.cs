using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [Header("Grenade Settings")]
    public GameObject grenadePrefab;
    public Transform grenadeSpawnPoint;
    public float throwForce = 10f; // ���� ������
    public float upwardForce = 2f; // ������������ ������������ ������
    public int grenadeCount = 5; // ���������� ������

    [Header("Camera Settings")]
    public Camera playerCamera;

    private void Update()
    {
        // ������ ������� �� ������� ������� ������ ����
        if (Input.GetMouseButtonDown(2) && grenadeCount > 0)
        {
            Throw();
        }
    }

    private void Throw()
    {
        // �������� �� ������� ����������� �����������
        if (grenadePrefab == null || playerCamera == null || grenadeSpawnPoint == null)
        {
            Debug.LogWarning("GrenadeThrower is not fully set up. Make sure all public fields are assigned in the Inspector.");
            return;
        }


        GameObject grenade = Instantiate(grenadePrefab, grenadeSpawnPoint.position, Quaternion.identity);

        // ���������� ����������� ������
        Vector3 throwDirection = playerCamera.transform.forward; // ����������� �� ������
        throwDirection.y += upwardForce / throwForce; // ��������� ������������ ������������

        // ���������� ���� � �������
        Rigidbody grenadeRb = grenade.GetComponent<Rigidbody>();
        if (grenadeRb != null)
        {
            grenadeRb.AddForce(throwDirection.normalized * throwForce + Vector3.up * upwardForce, ForceMode.Impulse);
        }

        // ���������� ���������� ������
        grenadeCount--;
    }
}
