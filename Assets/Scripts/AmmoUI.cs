using UnityEngine;

public class PlayerUIManaget : MonoBehaviour
{
    public GameObject gunObject;
    private Weapon gun;
    private TextMesh ammoText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gun = gunObject.GetComponent<Weapon>();
        ammoText = gameObject.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gun.isReloading)
            ammoText.text = "R";
        else
            ammoText.text = gun.bulletLeft.ToString();
        //ammoText = gun.bulletLeft;
    }
}
