using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int reboundLeft = 2;

    private Vector3 lastPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
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
        float distToPlayer = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position,
            transform.position);
        
        if (lastPos == transform.position && distToPlayer > 5f)
        {
            Destroy(gameObject);
        }
        
        
        if (transform.position.y > 10f || distToPlayer > 100f)
        {
            Destroy(gameObject);
        }
        
        lastPos = transform.position;

    }
}
