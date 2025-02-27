using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Discs
    
    public GameObject DiscPrefab;

    private Disc[] _discsObjs = new Disc[8];

    [Tooltip("Spawn disc points")]
    public Transform[] SpawnPoints = new Transform[8];

    public Transform TriggerASide;
    public Transform TriggerBSide;

    //Logic
    public int _nbDiscsInASide = 4;
    public int _nbDiscsInBSide = 4;
    

    // Plane on which the interaction will happen
    private Plane plane = new(Vector3.down, 0.7f);
    public Vector3 MousePosition {private set; get;}

    // Debug
    public bool DebugPushFromOppositeSide = false;

    public BotPlayer bot;

    public TMPro.TextMeshProUGUI textSideA;
    public TMPro.TextMeshProUGUI textSideB;

   
    void Awake(){
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //Spawn discs
        for(int i = 0; i < SpawnPoints.Length; i++){
            _discsObjs[i] = Instantiate(DiscPrefab, SpawnPoints[i].position, Quaternion.identity).GetComponent<Disc>();
            if(i >= 4){
                bot.AddDisc(_discsObjs[i]);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if(plane.Raycast(ray, out float distance)){
            MousePosition = ray.GetPoint(distance);
        }
    }

    public void AddDiscInSideA(Disc disc){
        _nbDiscsInASide++;
        _nbDiscsInBSide--;

        bot.RemoveDisc(disc);

        if(_nbDiscsInASide == 8 && _nbDiscsInBSide == 0){
            textSideB.text = "WINNER";
            textSideA.text = "LOSER";
            StartCoroutine(EndGame());
        }
    }

    public void AddDiscInSideB(Disc disc){
        _nbDiscsInBSide++;
        _nbDiscsInASide--;

        bot.AddDisc(disc);

        if(_nbDiscsInBSide == 8 && _nbDiscsInASide == 0){
            textSideA.text = "WINNER";
            textSideB.text = "LOSER";
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame(){
        textSideA.gameObject.SetActive(true);
        textSideB.gameObject.SetActive(true);
        foreach(Disc disc in _discsObjs){
            disc.Freeze = true;
        }
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("MenuScene");
    }
}