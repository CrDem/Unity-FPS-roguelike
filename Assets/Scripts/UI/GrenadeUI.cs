using UnityEngine;

public class GrenadeUI : MonoBehaviour
{
    public GameObject player;
    private GrenadeThrower grenadeThrower;
    private TextMesh grenadeText;
    
    void Start()
    {
        grenadeThrower = player.GetComponent<GrenadeThrower>();
        grenadeText = gameObject.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        grenadeText.text =grenadeThrower.grenadeCount.ToString();
    }
}
