using System.Collections;
using UnityEngine;

public class Reactive : MonoBehaviour
{
    // Ссылка на компонент AI, который будет управлять состоянием объекта
    private AI _AI;

    private void Start()
    {
        // Получаем компонент AI, прикрепленный к этому объекту
        _AI = GetComponent<AI>();
    }

    // Метод, который вызывается при попадании в объект
    public void ReactToHit()
    {
        // Если компонент AI существует, изменяем его состояние на "мертв"
        if (_AI != null)
            _AI.SetAlive(false);

        // Запускаем корутину, которая обрабатывает смерть объекта
        StartCoroutine(DieCorountine(3));
    }

    // Корутину для обработки смерти объекта
    private IEnumerator DieCorountine(float waitSecond)
    {
        // Поворачиваем объект на 45 градусов по оси X
        this.transform.Rotate(45, 0, 0);

        // Ждем указанное количество секунд перед уничтожением объекта
        yield return new WaitForSeconds(waitSecond);

        // Уничтожаем игровой объект
        Destroy(this.transform.gameObject);
    }
}

