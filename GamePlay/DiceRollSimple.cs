using Assets.Scripts.GamePlay;
using UnityEngine;

public class DiceRollSimple : MonoBehaviour
{
    #region Declarations
    public Transform Tiles;
    public GameObject Dice1;
    new Transform transform;
    
    public static bool wasIn;
    public static bool dragging;
    public static bool mouseDown;
    public static bool mouseUp;

    bool firstTime = true;

    float distance;
    float SpeedThroughInput = 10;

    public static Vector2 InputPos;
    public static Vector3 speed;

    Vector3 setBefore;
    Vector3 setAfter;
    //DateTime timeToWait;
    // int TimeFirst;
    #endregion

    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
        transform = GetComponent<Transform>();
    }
    void OnMouseDown()
    {
        if (ownVar.yourTurn())
        {
           // transform.position = Tiles.position + Vector3.up * 50;
            distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            dragging = true;
            //TimeFirst = timeToWait.Second;
        }
    }
    void OnMouseUp()
    {
        if (firstTime && ownVar.yourTurn())
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().velocity -= speed*40;
            dragging = false;
            firstTime = false;
            wasIn = true;
        }
    }
    void Update()
    {
        if (mouseDown)
        {
            // transform.position = Tiles.position + Vector3.up * 50;
            distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            dragging = true;
            mouseDown = false;
            Debug.Log("InMousedown");
            //TimeFirst = timeToWait.Second;
        }else if (mouseUp)
        {
            
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().velocity -= speed * 40;
            dragging = false;
            mouseUp = false;
            wasIn = true;
        } else if (ownVar.yourTurn())
        {
            InputPos = Input.mousePosition;
        }
     
        if (dragging)
        {

            setBefore = transform.position;
            speed = setAfter - setBefore;
            Ray ray = Camera.main.ScreenPointToRay(InputPos);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = Vector3.Lerp(transform.position, rayPoint, SpeedThroughInput * Time.deltaTime);
            setAfter = setBefore;
        }
        else if (wasIn)
        {
            if (GetComponent<Rigidbody>().velocity == new Vector3(0, 0))
            {
                DiceRollSimple2.CountForOnce = getDiceNumber(transform);
                Dice1.SetActive(true);
                normalizeDice();
            }
        }
    }
     public int getDiceNumber(Transform _transform)
    {
        int highest = 0;

        for (int i = 1; i < 6; i++)
        {
            if (_transform.GetChild(highest).position.y < _transform.GetChild(i).position.y)
            {
                highest = i;
            }
            
        }
        return highest + 1;
    }
    public void normalizeDice()
    {
        wasIn = false;
        firstTime = true;
    }
}

