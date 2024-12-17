using Unity.VisualScripting;
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

	public int damage = 50;
	private int additionalDamage;

	private DifficultyContoller _contoller;


	void Start()
	{
		_contoller = GameObject.FindGameObjectWithTag("Controller").GetComponent<DifficultyContoller>();
	}
	
	// Update is called once per frame
    void Update()
    {
	    UpdateDifficulty();
	    
	    if (readyToShoot)
	    {
		    FireWeapon();
	    }
    }

    void UpdateDifficulty()
    {
	    additionalDamage = _contoller.GetLevel() * 10;
    }

    private void FireWeapon()
	{
		Vector3 shootingDirection = CalculateSpreadedDir().normalized;
		GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
		bullet.GetComponent<Bullet>().damage = damage + additionalDamage;
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
