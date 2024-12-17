using UnityEngine;

public class HPUI : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerStats;
    private TextMesh hpText;
    
    void Start()
    {
        playerStats = player.GetComponent<PlayerMovement>();
        hpText = gameObject.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.health <= 0)
            hpText.text = "0";
        else
            hpText.text = playerStats.health.ToString();

    }
}
