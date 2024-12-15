using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private int _health = 5;
    private void Start()
    {
        _health = 5;
    }

    public void Hurt(int damage)
    {
        _health -= damage;
        Debug.Log("Player health: " + _health);
    }
}
