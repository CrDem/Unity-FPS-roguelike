using System.Collections;
using UnityEngine;

public class Reactive : MonoBehaviour
{
    // ������ �� ��������� AI, ������� ����� ��������� ���������� �������
    private AI _AI;

    private void Start()
    {
        // �������� ��������� AI, ������������� � ����� �������
        _AI = GetComponent<AI>();
    }

    // �����, ������� ���������� ��� ��������� � ������
    public void ReactToHit()
    {
        // ���� ��������� AI ����������, �������� ��� ��������� �� "�����"
        if (_AI != null)
            _AI.SetAlive(false);

        // ��������� ��������, ������� ������������ ������ �������
        StartCoroutine(DieCorountine(3));
    }

    // �������� ��� ��������� ������ �������
    private IEnumerator DieCorountine(float waitSecond)
    {
        // ������������ ������ �� 45 �������� �� ��� X
        this.transform.Rotate(45, 0, 0);

        // ���� ��������� ���������� ������ ����� ������������ �������
        yield return new WaitForSeconds(waitSecond);

        // ���������� ������� ������
        Destroy(this.transform.gameObject);
    }
}

