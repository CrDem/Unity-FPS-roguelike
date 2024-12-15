using UnityEngine;

public class Movie : MonoBehaviour
{
    // �������� �������� ���������
    public float _speed = 6.0f;

    // ���� ����������, �������������� �� ���������
    public float _gravity = -9.8f;

    // ������ �� ��������� CharacterController, ������� ��������� ��������� ���������
    private CharacterController _characterController;

    private void Start()
    {
        // �������� ��������� CharacterController, ������������� � ����� �������
        _characterController = GetComponent<CharacterController>();

        // ���������, ���������� �� ��������� CharacterController
        if (_characterController == null)
            // ���� ���������� ���, ������� ��������� �� ������ � �������
            Debug.Log("CharacterController �� ������");
    }

    private void Update()
    {
        // �������� �������� �������������� � ������������ ���� ����� (W, A, S, D ��� �������)
        float deltaX = Input.GetAxis("Horizontal") * _speed;
        float deltaZ = Input.GetAxis("Vertical") * _speed;

        // ������� ������ �������� �� ������ ���������� ��������
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        // ������������ �������� ������� �������� �� �������� ��������
        movement = Vector3.ClampMagnitude(movement, _speed);

        // ��������� ���������� � ������� ��������
        movement.y = _gravity;

        // �������� ������ �������� �� �����, ��������� � ���������� �����
        movement *= Time.deltaTime;

        // ����������� ������ �������� � ��������� ������� ��������� �������
        movement = transform.TransformDirection(movement);

        // ���������� ��������� � ������������ � �������� ��������
        _characterController.Move(movement);
    }
}
