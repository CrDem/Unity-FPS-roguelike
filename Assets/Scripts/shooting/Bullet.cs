using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int reboundLeft = 2;
    private Vector3 lastPos;
    public int damage;

    // Время задержки перед удалением объекта
    private float delay = 4f;

    void Start()
    {
        Destroy(gameObject, delay);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            // Получаем компонент Entity на объекте, с которым столкнулись
            Entity entity = collision.gameObject.GetComponent<Entity>();

            // Если компонент найден, наносим урон
            if (entity != null)
            {
                Debug.Log(damage);
                entity.TakeDamage(damage);
            }
            
            Destroy(gameObject);
        }
		if (collision.gameObject.CompareTag("Surface")) {
            --reboundLeft;
            if(reboundLeft <= 0)
                Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 10 || transform.position.y < -10)
        {
            Destroy(gameObject);
        }
        /*float distToPlayer = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position,
                transform.position);

            if (lastPos == transform.position && distToPlayer > 5f)
            {
                Destroy(gameObject);
            }

            if (transform.position.y > 10f || distToPlayer > 100f)
            {
                Destroy(gameObject);
            }

            lastPos = transform.position;*/
    }
}
