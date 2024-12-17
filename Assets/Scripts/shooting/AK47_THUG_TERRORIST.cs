using UnityEngine;
public class AK47: MonoBehaviour
{
	public bool readyToShoot = true;
	public float shootingDelay = 0.5f;
	public float spreadIntensity;
	
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public Transform handleGun;
	public float bulletVelocity = 30f;

	// Update is called once per frame
    void Update()
    {
	    if (readyToShoot)
	    {
		    FireWeapon();
	    }
    }

    private void FireWeapon()
	{
		Vector3 shootingDirection = CalculateSpreadedDir().normalized;
		GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
		bullet.transform.forward = shootingDirection;
		bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
		
		readyToShoot = false;
		Invoke("ResetShot", shootingDelay);
	}
    
    private void ResetShot()
    {
	    readyToShoot = true;
    }

    private Vector3 CalculateSpreadedDir()
	{
		Vector3 direction = bulletSpawn.position - handleGun.position;

		float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
		float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
		
		return direction + new Vector3(x, y, 0);
	}
	
}
