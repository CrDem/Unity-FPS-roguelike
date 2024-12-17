using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : Entity
{
    private CharacterController controller;

    public float sprintSpeed = 18f; // Скорость спринта

    void Start()
    {
        // Получаем компонент CharacterController
        controller = GetComponent<CharacterController>();
    }

    protected override Vector3 CalculateMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Получаем вектор движения на плоскости XZ
        Vector3 move = transform.right * x + transform.forward * z;

        // Нормализуем вектор, чтобы длина всегда была равна 1
        move.y = 0; // Убираем вертикальную составляющую
        return move.normalized;
    }
    
    void Update()
    {
        // Вычисляем вектор движения
        Vector3 move = CalculateMovement();

        // Проверяем, если нажата клавиша Shift для спринта
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Move(move, sprintSpeed);
        }
        else
        {
            Move(move, speed);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Проверяем, если объект с которым столкнулся игрок, имеет тег "Wall"
        if (hit.collider.CompareTag("Wall"))
        {
            //Debug.Log("Hit the Wall!");

            // Получаем нормаль столкновения
            Vector3 normal = hit.normal;

            // Применяем отклонение движения игрока от стены
            controller.Move(normal * speed * Time.deltaTime);
        }
    }

    // Переопределение смерти для игрока
    protected override void Die()
    {
        Debug.Log("Player has died!");
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.Confined;
    }
}