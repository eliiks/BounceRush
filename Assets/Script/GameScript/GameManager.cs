using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager is the main class of the game. It manages the discs and the interaction between the player and the discs.
/// </summary>
public class GameManager : MonoBehaviour
{
    // GAME PROPERTIES
    /// <summary>
    /// Discs prefab to instantiate
    /// </summary>
    [Tooltip("Disc prefab to instantiate")] public GameObject DiscPrefab;

    /// <summary>
    /// Parent transform listing spawn points transform for discs
    /// </summary>
    [Tooltip("Spawn points for the discs")] public Transform SpawnPointsParent;

    /// <summary>
    /// Trigger point to detect if a disc has entered player side (bottom side from camera pov)
    /// </summary>
    [Tooltip("Trigger point to detect if a disc has entered player side (bottom side from camera pov)")] public Transform PlayerSideAccessTriggerPoint;

    /// <summary>
    /// Trigger point to detect if a disc has entered bot side (top side from camera pov)
    /// </summary>
    [Tooltip("Trigger point to detect if a disc has entered bot side (top side from camera pov)")] public Transform BotSideAccessTriggerPoint;
    
    /// <summary>
    /// Discs objects in the game
    /// </summary>
    private Disc[] _discsObjs;

    /// <summary>
    /// Number of discs in player side
    /// </summary>
    private int _nbDiscsInPlayerSide;

    /// <summary>
    /// Number of discs in bot side
    /// </summary>
    private int _nbDiscsInBotSide;

    // BOT PROPERTIES
    public BotPlayer bot;
    
    // INTERACTION PROPERTIES
    /// <summary>
    /// Plane on which the interaction will happen
    /// </summary>
    private Plane plane = new(Vector3.down, 0.7f);

    /// <summary>
    /// Mouse position in the platform plane
    /// </summary>
    public Vector3 MousePosition {private set; get;}

    // UI PROPERTIES
    public TMPro.TextMeshProUGUI GUI_PlayerSideText;
    public TMPro.TextMeshProUGUI GUI_BotSideText;

    // DEBUG PROPERTIES
    public bool DebugPushFromOppositeSide = false;

    void Awake(){
        // List the spawn points and compute the repartition of discs
        _discsObjs = new Disc[SpawnPointsParent.childCount];
        int nbDiscsPerSide = SpawnPointsParent.childCount / 2;
        _nbDiscsInPlayerSide = nbDiscsPerSide;
        _nbDiscsInBotSide = nbDiscsPerSide;

        //Spawn discs
        for(int i = 0; i < SpawnPointsParent.childCount; i++){
            _discsObjs[i] = Instantiate(DiscPrefab, SpawnPointsParent.GetChild(i).position, Quaternion.identity).GetComponent<Disc>();
            if(i >= nbDiscsPerSide){
                bot.AddDisc(_discsObjs[i]);
            }
        }
    }

    void Update()
    {
        // Convert screen mouse position to a ray
        Vector3 screenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        // Compute mouse position in the platform plane
        if(plane.Raycast(ray, out float distance)){
            MousePosition = ray.GetPoint(distance);
        }
    }

    /// <summary>
    /// Tells if the disc is in the player side or the bot side
    /// </summary>
    /// <param name="zDiscPosition">the z position of the disc</param>
    /// <param name="discWidth">the width of the disc (localScale.z)</param>
    /// <returns>True if the disc is in the player side, false otherwise</returns>
    public bool IsDiscInPlayerSide(float zDiscPosition, float discWidth){
        if((zDiscPosition - (discWidth / 2)) > BotSideAccessTriggerPoint.position.z){
            return false;
        }
        return true;
    }

    /// <summary>
    /// Add a disc in player side
    /// Called when a disc has entered the player side
    /// Update properties and check if all discs are in player side (if yes, bot wins)
    /// </summary>
    /// <param name="disc">The disc object that entered the player side</param>
    public void AddDiscInPlayerSide(Disc disc){
        _nbDiscsInPlayerSide++;
        _nbDiscsInBotSide--;

        bot.RemoveDisc(disc);

        if(_nbDiscsInPlayerSide == SpawnPointsParent.childCount && _nbDiscsInBotSide == 0){
            GUI_BotSideText.text = "WINNER";
            GUI_PlayerSideText.text = "LOSER";
            StartCoroutine(EndGame());
        }
    }

    /// <summary>
    /// Add a disc in side of the bot
    /// Called when a disc has entered the bot side
    /// Update properties and check if all discs are in bot side (if yes, player wins)
    /// </summary>
    /// <param name="disc">The disc object that entered the side B</param>
    public void AddDiscInBotSide(Disc disc){
        _nbDiscsInBotSide++;
        _nbDiscsInPlayerSide--;

        bot.AddDisc(disc);

        if(_nbDiscsInBotSide == SpawnPointsParent.childCount && _nbDiscsInPlayerSide == 0){
            GUI_PlayerSideText.text = "WINNER";
            GUI_BotSideText.text = "LOSER";
            StartCoroutine(EndGame());
        }
    }

    /// <summary>
    /// End the game and reload the menu scene after 7 seconds
    /// </summary>
    IEnumerator EndGame(){
        // Display the result on GUI
        GUI_PlayerSideText.gameObject.SetActive(true);
        GUI_BotSideText.gameObject.SetActive(true);

        // Disable interaction with discs
        foreach(Disc disc in _discsObjs){
            disc.Freeze = true;
        }

        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("MenuScene");
    }
}