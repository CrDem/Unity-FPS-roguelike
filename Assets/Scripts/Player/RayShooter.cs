using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;

public class RayShooter : MonoBehaviour
{
    //объект камеры
    private Camera _camera;
    private void Start()
    {
        //получаем данные о камере
        _camera = GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked; // Блокировка курсора
        Cursor.visible = false; // Сделать курсор невидимым
    }


    private void Update()
    {
        // Проверяем, была ли нажата левая кнопка мыши
        if (Input.GetMouseButtonDown(0))
        {
            // Определяем центр экрана
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

            // Создаем луч из центра экрана в 3D-пространство
            Ray ray = _camera.ScreenPointToRay(screenCenter);
            RaycastHit hit; // Переменная для хранения информации о попадании луча

            // Выполняем проверку на столкновение луча с объектами в сцене
            if (Physics.Raycast(ray, out hit))
            {
                // Получаем объект, с которым столкнулся луч
                GameObject hitObject = hit.transform.gameObject;

                // Пытаемся получить компонент Reactive с объекта, с которым мы столкнулись
                Reactive target = hitObject.GetComponent<Reactive>();

                // Если компонент Reactive найден, вызываем метод ReactToHit()
                if (target != null)
                {
                    target.ReactToHit();
                }
                else
                {
                    // Если компонент не найден, запускаем корутину для отображения сферы
                    StartCoroutine(SphereInicatorCoroutine(hit.point));
                    // Отображаем линию от игрока до точки попадания в течение 6 секунд
                    Debug.DrawLine(this.transform.position, hit.point, Color.green, 6);
                }
            }
        }
    }

    private void OnGUI()
    {
        // Определяем размер метки
        int size = 12;
        // Вычисляем позицию для размещения метки в центре экрана
        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight / 2 - size / 2;
        // Отображаем метку в виде звездочки в центре экрана
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }

    private IEnumerator SphereInicatorCoroutine(Vector3 pos)
    {
        // Создаем сферу в заданной позиции
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;

        // Ждем 6 секунд
        yield return new WaitForSeconds(6);
        // Удаляем сферу из сцены
        Destroy(sphere);
    }
}


