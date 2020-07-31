using Assets.Scripts.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollSimple2 : MonoBehaviour
{

    public Transform Tiles;
    public GameObject Dice1;


    public static bool wasIn;
    public static bool dragging;
    bool firstTime = true;

    float distance;
    private float SpeedThroughInput = 10;
    public static int CountForOnce = 0;

    Vector2 setBefore;
    Vector2 setAfter;
    public static Vector2 speed;
    //DateTime timeToWait;
    // int TimeFirst;

    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
    }
    private void OnMouseDown()
    {
        if (firstTime && ownVar.yourTurn())
        {
            transform.position = Tiles.position + Vector3.up * 50;
            distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            dragging = true;
            //TimeFirst = timeToWait.Second;
        }
    }
    private void OnMouseUp()
    {
        if (firstTime && ownVar.yourTurn())
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().velocity -= new Vector3(speed.x, 30, speed.y);
            dragging = false;
            firstTime = false;
            wasIn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {

            setBefore = Input.mousePosition;
            speed = setAfter - setBefore;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = Vector3.Lerp(transform.position, rayPoint, SpeedThroughInput * Time.deltaTime);
            setAfter = setBefore;
        }
        else if (wasIn && GetComponent<Rigidbody>().velocity == new Vector3(0, 0))
        {
                CountForOnce += getDiceNumber(transform);
                ownVar.OnField[ownVar.myID-1] += CountForOnce;
                CountForOnce = 0;
                Client.allPlayer[ownVar.Turn].transform.position = Tiles.GetComponentInChildren<Transform>().GetChild(ownVar.OnField[ownVar.myID-1]).position;
                ownVar.finishedLevels = 2;
                Dice1.SetActive(false);
                gameObject.SetActive(false);
            
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

}
