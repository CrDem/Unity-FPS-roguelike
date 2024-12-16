using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [Header("Grenade Settings")]
    public GameObject grenadePrefab; 
    public Transform grenadeSpawnPoint; 
    public float throwForce = 10f;
    public int grenadeCount = 5;

    [Header("Camera Settings")]
    public Camera playerCamera;

    public void Update()
    {
        if (Input.GetMouseButtonDown(2) && grenadeCount > 0)
        {
            Throw();
        }
    }

    public void Throw()
    {
        if (grenadePrefab == null || playerCamera == null || grenadeSpawnPoint == null)
        {
            Debug.LogWarning("GrenadeThrower is not fully set up. Make sure all public fields are assigned in the Inspector.");
            return;
        }

        GameObject grenade = Instantiate(grenadePrefab, grenadeSpawnPoint.position, Quaternion.identity);

        Vector3 throwDirection = playerCamera.transform.forward;
        throwDirection.y += 0.2f;

        Rigidbody grenadeRb = grenade.GetComponent<Rigidbody>();
        if (grenadeRb != null)
        {
            grenadeRb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
        }
        grenadeCount--;
    }
}
