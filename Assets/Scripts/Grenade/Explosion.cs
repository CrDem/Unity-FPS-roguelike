using UnityEngine;

public class Explosion: MonoBehaviour
{
    void Start()
    {
        // Удаляем объект через указанное время
        Destroy(gameObject, 0.1f);
    }
}
