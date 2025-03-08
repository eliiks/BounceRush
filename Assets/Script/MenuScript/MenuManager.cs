using UnityEngine;
using UnityEngine.SceneManagement;

// <summary>
/// Manages the menu of the game
/// @author Eliiks
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// The UI panel of the main menu
    /// </summary>
    public GameObject mainMenu;

    /// <summary>
    /// The UI panel of the current menu (main menu by default)
    /// </summary>
    private GameObject currentMenu;

    void Awake()
    {
        currentMenu = mainMenu;
    }

    /// <summary>
    /// Change scene and launch the game
    /// </summary>
    public void LaunchGame(){
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Change the current menu by another one
    /// </summary>
    /// <param name="menu">The UI menu to show</param>
    public void ShowMenu(GameObject newMenu){
        currentMenu.SetActive(false);
        newMenu.SetActive(true);
        currentMenu = newMenu;
    }
}