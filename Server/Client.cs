using Assets.Scripts.GamePlay;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    #region Declarations
    public GameObject host;
    public GameObject foreignPlayer;
    public GameObject yourPlayer;
    public GameObject Dice;

    public static Dictionary<int, GameObject> allPlayer = new Dictionary<int, GameObject>();
    
    public Transform Tiles;

    Packet _packet;
    TcpClient socket;
    NetworkStream stream;

    public string ip;
    byte[] receiveBuffer;
    public static string text1;
    float countdown=60;
    #endregion
    #region Setup
    void Start()
    {
        
        socket = new TcpClient
        {
            ReceiveBufferSize = 4096,
            SendBufferSize = 4096
        };
        socket.BeginConnect(IPAddress.Parse("127.0.0.1"), 26950, ConnectCallback, socket);
        

    }
    private void ConnectCallback(IAsyncResult _result)
    {
        try
        {
            socket.EndConnect(_result);
        }
        catch (Exception )
        {
            ThreadManager.ExecuteOnMainThread(() =>
            {
                Debug.Log("Initialize being the host...");

                host.SetActive(true);
            });
            socket.BeginConnect(IPAddress.Parse("127.0.0.1"), 26950, ConnectCallback, socket);
        }

        if (!socket.Connected) return;
        
        _packet = new Packet();
       receiveBuffer=new byte[4096];
        stream = socket.GetStream();
        stream.BeginRead(receiveBuffer, 0, 4096, ReceiveCallback, null);
    }
    private void ReceiveCallback(IAsyncResult _result)
    {
        try
        {
            int _byteLength = stream.EndRead(_result);

            if (_byteLength <= 0)
            {
                //Disconnect
                return;
            }
            _packet.Reset(HandleData(receiveBuffer));
            stream.BeginRead(receiveBuffer, 0, 4096, ReceiveCallback, null);    
        }
        catch
        {
            //Disconnect();
        }
    }
    private bool HandleData(byte[] _data)
    {
        _packet.SetBytes(_data);
        try
        {
            switch (_packet.ReadInt())
            {
                case 0:
                    //players inhere firsttime
                    if (_packet.ReadInt(false) != 0)
                    {
                        ownVar.myID = _packet.ReadInt();
                        int playersToAdd = _packet.ReadInt();


                        ThreadManager.ExecuteOnMainThread(() =>
                        {
                            allPlayer.Add(ownVar.myID, yourPlayer);
                            allPlayer[ownVar.myID].SetActive(true);
                            for (int i = 1; i <= playersToAdd; i++)
                            {
                                allPlayer.Add(i, Instantiate(foreignPlayer, Tiles.GetChild(0).position + Vector3.up * 17.5f, new Quaternion(0, 0, 0, 0)));
                            }
                            Dice.SetActive(true);
                        });
                    }
                    break;

                case 1:
                    int PlayerToAddID = _packet.ReadInt();
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        allPlayer.Add(PlayerToAddID, Instantiate(foreignPlayer, Tiles.GetChild(0).position + Vector3.up * 17.5f, new Quaternion(0, 0, 0, 0)));
                    });
                    break;
                case 2:
                    Debug.Log("inCase");
                    DiceRollSimple.InputPos=_packet.ReadVector2();
                    DiceRollSimple.mouseDown = true;
                    break;
                case 3:
                    DiceRollSimple.mouseUp = true;
                    DiceRollSimple.mouseDown = false;
                    break;
                case 4:
                    DiceRollSimple.mouseUp = false;
                    ownVar.isFirstPlayer();
                    ownVar.Turn++;
                    break;
            }

                            
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
            return true;
        }
    #endregion
  void Update()
    {
        try
        {
            if (socket.Connected)
            {
                SendData();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

     void SendData()
    {
        countdown =5;

        if (DiceRollSimple.dragging && (ownVar.Turn == 0||ownVar.yourTurn()))
        {
            Packet _packet = new Packet(2, ownVar.myID);
            _packet.Write(Input.mousePosition);

            stream.BeginWrite(_packet.buffer.ToArray(), 0, _packet.buffer.Count, null, null);
        }
        else if (DiceRollSimple.wasIn)
        {
            Packet _packet = new Packet(3, ownVar.myID);
            stream.BeginWrite(_packet.buffer.ToArray(), 0, _packet.buffer.Count, null, null);
        }
        else if (countdown <= 0 || ownVar.finishedLevels == 3)
        {
            Packet _packet = new Packet(4,ownVar.myID);
            stream.BeginWrite(_packet.buffer.ToArray(), 0, _packet.buffer.Count, null, null);
            countdown = 60f;
            ownVar.isFirstPlayer();
            ownVar.Turn++;
        }

    }
}


