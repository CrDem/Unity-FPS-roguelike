using UnityEngine;

public class Weapon : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletVelocity = 30f;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Mouse0) ) {
			FireWeapon();		
		}
    }

	private void FireWeapon() {
		GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
		bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
	}

	
}
