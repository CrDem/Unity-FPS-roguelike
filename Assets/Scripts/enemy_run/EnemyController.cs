using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefab; //массив шаблонов
    private GameObject _enemy;

    private void Update()
    {   //новый враг, если их нет
        if (_enemy == null)
        {
            int randEnemy = Random.Range(1, enemyPrefab.Length); //выбираем врага случайно
            _enemy = Instantiate(enemyPrefab[randEnemy]) as GameObject; //клон
            _enemy.transform.position = new Vector3(0, 3, 0); //позиция появления
            float angle = Random.Range(0, 360);
            _enemy.transform.Rotate(0, angle, 0); //поворачиваем
        }
    }
}
