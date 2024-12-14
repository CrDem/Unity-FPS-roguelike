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
        print(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("hit enemy");
            Destroy(gameObject);
        }
		if (collision.gameObject.CompareTag("Surface")) {
            print("hit surface");
            --reboundLeft;
            if(reboundLeft <= 0)
                Destroy(gameObject);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPos == transform.position && Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) > 5f)
        {
            Destroy(gameObject);
        }
       
        
        if (transform.position.y > 10f)
        {
            Destroy(gameObject);
        }
        
        lastPos = transform.position;

    }
}
