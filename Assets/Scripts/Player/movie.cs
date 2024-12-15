using UnityEngine;

public class Movie : MonoBehaviour
{
    // Скорость движения персонажа
    public float _speed = 6.0f;

    // Сила гравитации, воздействующая на персонажа
    public float _gravity = -9.8f;

    // Ссылка на компонент CharacterController, который управляет движением персонажа
    private CharacterController _characterController;

    private void Start()
    {
        // Получаем компонент CharacterController, прикрепленный к этому объекту
        _characterController = GetComponent<CharacterController>();

        // Проверяем, существует ли компонент CharacterController
        if (_characterController == null)
            // Если компонента нет, выводим сообщение об ошибке в консоль
            Debug.Log("CharacterController не найден");
    }

    private void Update()
    {
        // Получаем значения горизонтальной и вертикальной осей ввода (W, A, S, D или стрелки)
        float deltaX = Input.GetAxis("Horizontal") * _speed;
        float deltaZ = Input.GetAxis("Vertical") * _speed;

        // Создаем вектор движения на основе полученных значений
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        // Ограничиваем величину вектора движения до заданной скорости
        movement = Vector3.ClampMagnitude(movement, _speed);

        // Применяем гравитацию к вектору движения
        movement.y = _gravity;

        // Умножаем вектор движения на время, прошедшее с последнего кадра
        movement *= Time.deltaTime;

        // Преобразуем вектор движения в локальную систему координат объекта
        movement = transform.TransformDirection(movement);

        // Перемещаем персонажа в соответствии с вектором движения
        _characterController.Move(movement);
    }
}
