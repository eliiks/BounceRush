using UnityEngine;

public class Disc : MonoBehaviour
{
    

    // Properties
    [Tooltip("The speed of the disc")]
    public float speed = 1000.0f;
    private GameManager gm;
    private Rigidbody rb;
    
    // Interaction
    [Tooltip("Tells if the disc has been grabbed by the player")]
    private bool grabbed = false;

    [Tooltip("Tells if the disc has been pushed by the player")]
    private bool pushed = false;

    public bool Freeze { get; internal set; }

    public bool ASide = true;

    public RectTransform DragArrow;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = FindAnyObjectByType<GameManager>();

        if((transform.position.z - (transform.localScale.z / 2)) > gm.TriggerBSide.position.z){
            ASide = false;
        }
    }

    void OnMouseDown()
    {
        if(gm.DebugPushFromOppositeSide || ASide){
            GrabDisc();
        }
    }

    void OnMouseUp()
    {
        if(grabbed){
            PushDisc();
        }
    }

    void Update()
    {
        if(!ASide && (transform.position.z - (transform.localScale.z / 2)) < gm.TriggerASide.position.z){
            gm.AddDiscInSideA(this);
            ASide = true;
        }

        if(ASide && (transform.position.z - (transform.localScale.z / 2)) > gm.TriggerBSide.position.z){
            gm.AddDiscInSideB(this);
            ASide = false;
        }
    }

    void FixedUpdate(){
        if(!Freeze){
            if(ASide){
                Vector3 direction = transform.position - gm.MousePosition;

                if(grabbed){
                    //Debug.DrawRay(gm.MousePosition, direction, Color.red);

                    Vector3 targetArrowRotation = Quaternion.LookRotation(direction, Vector3.up).eulerAngles;
                    targetArrowRotation.x = DragArrow.rotation.eulerAngles.x;
                    DragArrow.rotation = Quaternion.Euler(targetArrowRotation);

                    DragArrow.sizeDelta = new Vector2(DragArrow.sizeDelta.x, direction.magnitude);
                }else{
                    DragArrow.sizeDelta = new Vector2(DragArrow.sizeDelta.x, 0.0f);
                }

                if(!grabbed && pushed){
                    rb.AddForce(speed * direction.normalized);
                    pushed = false;
                }
            }else{
                if(!grabbed && pushed){
                    Vector3 direction = BotTarget - transform.position;
                    rb.AddForce(speed * direction.normalized);
                    pushed = false;
                }
            }
        }
    }


    public void GrabDisc(){
        grabbed = true;
    }

    public void PushDisc(){
        grabbed = false;
        pushed = true;
    }


    //Bot
    public Vector3 BotTarget;
    public void SetBotTarget(Vector3 position){
        BotTarget = position;
    }
}