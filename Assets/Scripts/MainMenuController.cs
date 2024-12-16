using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Метод для кнопки "Играть"
    public void PlayGame()
    {
        SceneManager.LoadScene("MAZE");
    }

    // Метод для кнопки "Выход"
    public void QuitGame()
    {
        // Закрывает приложение
        Application.Quit();

        // На случай запуска в редакторе
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

