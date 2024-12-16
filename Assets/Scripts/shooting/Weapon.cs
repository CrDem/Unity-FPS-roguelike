using UnityEngine;



public class Weapon : MonoBehaviour
{
	
	public Camera playerCamera;

	public bool isShooting, readyToShoot;
	bool allowReset = true;
	public float shootingDelay = 0.5f;
	public float reloadingTime = 2f;
	public bool isReloading;

	public float reloadingTimeLeft;

	public float spreadIntensity;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletVelocity = 30f;

	public int bulletMax = 30;
	public int bulletLeft;


	public enum ShootingMode {
		Single,
		Auto,
	}

	public ShootingMode currentShootingMode;

	private void Awake()
	{
		isReloading = false;
		readyToShoot = true;
		bulletLeft = bulletMax;
		reloadingTimeLeft = reloadingTime;
	}
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

	void SwitchShootingMode() {
		if(Input.GetKey(KeyCode.Alpha1)) {
			currentShootingMode = ShootingMode.Auto;
		}
		else if (Input.GetKey(KeyCode.Alpha2)) {
			currentShootingMode = ShootingMode.Single;
}
	}

    // Update is called once per frame
    void Update()
    {
		SwitchShootingMode();
	    if (isReloading)
		    UpdateReloading();
	    else
	    {
		    if (currentShootingMode == ShootingMode.Auto)
			    isShooting = Input.GetKey(KeyCode.Mouse0);
		    else if (currentShootingMode == ShootingMode.Single)
			    isShooting = Input.GetKeyDown(KeyCode.Mouse0);

		    if (isShooting && readyToShoot)
		    {
			    FireWeapon();
		    }

		    if (Input.GetKeyDown(KeyCode.R) && !isShooting)
			    ReloadWeapon();
	    }
    }

    void UpdateReloading()
    {
	    reloadingTimeLeft -= Time.deltaTime;
	    if (reloadingTimeLeft < 0)
	    {
		    isReloading = false;
		    bulletLeft = bulletMax;
	    }
    }
    
    void ReloadWeapon()
    {
	    reloadingTimeLeft = reloadingTime;
	    isReloading = true;
    }

	private void FireWeapon()
	{
		if (bulletLeft <= 0) return;

		--bulletLeft;
		readyToShoot = false;
		Vector3 shootingDirection = CalculateSpreadedDir().normalized;
		GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
		bullet.transform.forward = shootingDirection;
		bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

		if (allowReset)
		{
			Invoke("ResetShot", shootingDelay);
			allowReset = false;
		}
	}

	private void ResetShot()
	{
		readyToShoot = true;
		allowReset = true;
	}

	private Vector3 CalculateSpreadedDir()
	{
		Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		
		Vector3 targetPoint;
		if (Physics.Raycast(ray, out hit) && hit.distance > 1f)
		{
			targetPoint = hit.point;
		}
		else
			targetPoint = ray.GetPoint(100f);
		
		Vector3 direction = targetPoint - bulletSpawn.position;

		float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
		float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
		
		return direction + new Vector3(x, y, 0);
	}
	
}
