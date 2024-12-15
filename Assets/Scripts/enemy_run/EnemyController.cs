using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefab; //������ ��������
    private GameObject _enemy;

    private void Update()
    {   //����� ����, ���� �� ���
        if (_enemy == null)
        {
            int randEnemy = Random.Range(1, enemyPrefab.Length); //�������� ����� ��������
            _enemy = Instantiate(enemyPrefab[randEnemy]) as GameObject; //����
            _enemy.transform.position = new Vector3(0, 3, 0); //������� ���������
            float angle = Random.Range(0, 360);
            _enemy.transform.Rotate(0, angle, 0); //������������
        }
    }
}
