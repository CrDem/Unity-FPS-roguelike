using UnityEngine;

public class DifficultyContoller : MonoBehaviour
{

    public int difficultyLevel;
    private float timerLevelUp;
    private float timeToLevelUp = 50;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        difficultyLevel = 1;
        ResetTimer();
    }

    public int GetLevel()
    {
        return difficultyLevel;
    }

    void ResetTimer()
    {
        timerLevelUp = timeToLevelUp;
    }

    // Update is called once per frame
    void Update()
    {
        timerLevelUp -= Time.deltaTime;
        if (timerLevelUp <= 0f)
        {
            ++difficultyLevel;
            ResetTimer();
        }
        
    }
}
