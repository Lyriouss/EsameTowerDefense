using UnityEngine;
using UnityEngine.SceneManagement;

//Manager used in MainMenu scene
public class MainMenuManager : MonoBehaviour
{
    //Loads GameScene
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    //Closes application in build
    public void QuitApplication()
    {
        Application.Quit();
    }
}