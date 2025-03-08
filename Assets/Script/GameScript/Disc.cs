using UnityEngine;

/// <summary>
/// Class handling behavior of  dics in the game
/// </summary>
public class Disc : MonoBehaviour
{
    // DISC PROPERTIES

    /// <summary>
    /// The speed of the disc
    /// </summary>
    [Tooltip("The speed of the disc")] public float speed = 1000.0f;
    
    /// <summary>
    /// The rigidbody of the disc
    /// </summary>
    private Rigidbody _rb;

    /// <summary>
    /// The game manager instance
    /// </summary>
    private GameManager _gm;
    
    /// <summary>
    /// The audio manager instance
    /// </summary>
    private AudioManager _am;

    /// <summary>
    /// True if the disc is in player side, false otherwise.
    /// </summary>
    private bool _inPlayerSide = true;

    // PLAYER INTERACTION

    /// <summary>
    /// Tells if the disc can be moved or not by players
    /// </summary>
    [Tooltip("Tells if the disc can be moved or not by players")] public bool Freeze;

    /// <summary>
    /// Tells if the disc has been grabbed by the player
    /// </summary>
    [Tooltip("Tells if the disc has been grabbed by the player")] private bool _grabbed = false;

    /// <summary>
    /// Tells if the disc has been pushed by the player
    /// </summary>
    [Tooltip("Tells if the disc has been pushed by the player")] private bool _pushed = false;

    //BOT INTERACTION
    
    /// <summary>
    /// Targeted position of the bot
    /// </summary>
    private Vector3 _botTargetPosition;

    // UI TARGET DIRECTION ARROW

    /// <summary>
    /// The canvas of the arrow under the disc
    /// </summary>
    [Tooltip("The canvas of the arrow under the disc")] public RectTransform DirectionArrow;

    /// <summary>
    /// The panel transform of the arrow's main line
    /// </summary>
    [Tooltip("The panel transform of the arrow's main line")] public Transform DirectionArrowLine;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _gm = FindAnyObjectByType<GameManager>();
        _am = FindAnyObjectByType<AudioManager>();

        // Detect if disc is in player side or not
        _inPlayerSide = _gm.IsDiscInPlayerSide(transform.position.z, transform.localScale.z);
    }

    void OnMouseDown()
    {   
        // If disc is in player side, it can be grabbed by the player 
        if(_gm.DebugPushFromOppositeSide || _inPlayerSide){
            GrabDisc();
        }
    }

    void OnMouseUp()
    {
        // When user release left mouse button, if the disc has been grabbed, the disc is ready to be pushed in the targeted direction
        if(_grabbed){
            PushDisc();
        }
    }

    void Update()
    {
        // Update side properties
        bool wasInPlayerSide = _inPlayerSide;
        bool nowInPlayerSide = _gm.IsDiscInPlayerSide(transform.position.z, transform.localScale.z);

        if(!wasInPlayerSide && nowInPlayerSide){
            _gm.AddDiscInPlayerSide(this);
            _inPlayerSide = true;
        }

        if(wasInPlayerSide && !nowInPlayerSide){
            _gm.AddDiscInBotSide(this);
            _inPlayerSide = false;
        }
    }

    void FixedUpdate(){
        // Push the disc if possible
        if(!Freeze){
            if(_inPlayerSide){
                // - Handles player interactions

                // Compute the direction (that the disc will follow) according to the last mouse position
                Vector3 direction = transform.position - _gm.MousePosition;

                // If grabbed, display the arrow under the disc to help the user for choosing a direction
                if(_grabbed){
                    // Compute the arrow rotation according to the given input direction
                    Vector3 targetArrowRotation = Quaternion.LookRotation(direction, Vector3.up).eulerAngles;
                    targetArrowRotation.x = DirectionArrow.rotation.eulerAngles.x; // the x axis rotation value must be locked
                    DirectionArrow.rotation = Quaternion.Euler(targetArrowRotation); // rotate the arrow

                    // Compute the length of the middle line of the arrow, in order to always follow direction magnitude
                    DirectionArrowLine.localScale = new Vector3(DirectionArrowLine.localScale.x, direction.magnitude, DirectionArrowLine.localScale.z);
                    
                    DirectionArrow.gameObject.SetActive(true);
                }else{
                    DirectionArrow.gameObject.SetActive(false);
                }

                if(!_grabbed && _pushed){
                    // The user release the left mouse button, so the disc is ready to be pushed in the targeted direction
                    _rb.AddForce(speed * direction.normalized);
                    _am.Play("DiscPushed");
                    _pushed = false;
                }
            }else{
                // - Handles bot interactions
                if(!_grabbed && _pushed){
                    Vector3 direction = _botTargetPosition - transform.position;
                    _rb.AddForce(speed * direction.normalized);
                    _pushed = false;
                }
            }
        }
    }

    // PLAYER FUNCTIONS
    public void GrabDisc(){
        _grabbed = true;
    }

    public void PushDisc(){
        _grabbed = false;
        _pushed = true;
    }

    // BOT FUNCTIONS
    public void SetBotTarget(Vector3 position){
        _botTargetPosition = position;
    }
}