using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;

    public bool isGrounded;
    private bool isMoving;
    
    
    public float sprintSpeed = 18f; // Add a sprint speed

    void Start()    
    {
        controller = GetComponent<CharacterController>();
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }
    
    void Update()
    {
        CheckGround();
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Получаем вектор движения на плоскости XZ
        Vector3 move = transform.right * x + transform.forward * z;

        // Нормализуем движение, чтобы скорость не зависела от угла наклона камеры
        move.y = 0; // игнорируем вертикальную составляющую движения
        move.Normalize(); // Нормализуем вектор, чтобы его длина всегда была равна 1

        if (isGrounded && velocity.y < 0) velocity.y = -2f;
        
        // Проверяем, если нажата клавиша Shift для бега
        if (Input.GetKey(KeyCode.LeftShift))
        {
            controller.Move(move * sprintSpeed * Time.deltaTime);
        }
        else
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Проверяем, если объект, с которым столкнулся игрок, имеет тег "Wall"
        if (hit.collider.CompareTag("Wall"))
        {
            Debug.Log("aboba hited");
            // Получаем нормаль столкновения
            Vector3 normal = hit.normal;

            // Применяем отклонение движения игрока от стены
            // Например, отклоняем движение игрока от стен
            controller.Move(normal * speed * Time.deltaTime);
        }
    }
}