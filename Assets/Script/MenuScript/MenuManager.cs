using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject startMenu;
    private GameObject currentSubMenu;

    // Start is called before the first frame update
    void Awake()
    {
        currentSubMenu = startMenu;
    }

    public void LaunchGame(){
        SceneManager.LoadScene("GameScene");
    }

    public void ShowSubMenu(GameObject subMenu){
        currentSubMenu.SetActive(false);
        subMenu.SetActive(true);
        currentSubMenu = subMenu;
    }

}