using System.Net.Sockets;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using System.Collections.Generic;

public class Server : MonoBehaviour
{
    #region Declarations
    private static TcpListener tcpListener;
    static Packet _packet;
    static Stream _stream;
    static Dictionary<int, TcpClient> clients;

    public static readonly int _maxPlayer = 1;
    static int _clientNumber=0;
    bool _isHost;

    #endregion
    #region Setup
    void Start()
    {
        if (_clientNumber <= _maxPlayer) { 
        tcpListener = new TcpListener(IPAddress.Any, 26950);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            clients = new Dictionary<int, TcpClient>();
     }

    }
    private static void TCPConnectCallback(IAsyncResult _result)
    {
        _clientNumber++;

        _packet = new Packet(0);
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            clients.Add(_clientNumber, _client);
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
        Debug.Log($"Incoming connection from: {_client.Client.RemoteEndPoint}");
        #region FirstPacket
        //tcpListener.EndAcceptTcpClient(_result).GetStream().BeginWrite(...)=
        _packet.Write(_clientNumber);
        //shows how many player to add
        _packet.Write(_clientNumber - 1);
        _packet.Write(StartGame());
        _stream = clients[_clientNumber].GetStream();
        _stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
        #endregion 
        
            Packet _packetforothers = new Packet(1);
            _packetforothers.Write(_clientNumber);
              SentToAllExcept(_packetforothers, _clientNumber);
        

    }
   static bool StartGame()
    {
        if (_clientNumber == _maxPlayer)
        {
            return true;
        }
        return false;
    }
    #endregion
    
    void Update()
    {
    BasicUp();
    }
    
  

    static void UpdateFunction()
    {
        //only sent if maxplayer+1
        if (_clientNumber > 1)
        {
            try
            {
                Packet packet = new Packet();
                byte[] receiveBuffer = new byte[4096];
                for (int i = 1; i < _clientNumber; i++)
                {
                    clients[i].GetStream().Read(receiveBuffer, 0, 4096);
                    packet.SetBytes(receiveBuffer);
                    if (packet.ReadInt(true) != 0)
                    {
                        SentToAllExcept(packet, packet.ReadInt(false));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
    static void BasicUp()
    {
        if (clients.ContainsKey(_clientNumber)) 
        {

            try
            {
                Packet packet = new Packet();
                byte[] receiveBuffer = new byte[4096];
                for (int i = 1; i <= _maxPlayer; i++)
                {
                        clients[i].GetStream().Read(receiveBuffer, 0, 4096);
                   

                    packet.SetBytes(receiveBuffer);

                    int pak = packet.ReadInt();
                    if (pak !=0)
                    {
                        Debug.LogError(pak);
                    }
                    SentToAll(receiveBuffer);
                }



            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }

        }
    }
    #region send
    static void SentToAll(byte[] _ArrToSend)
    {
        try
        {
            for (int i = 1; i <= _clientNumber; i++)
            {
                clients[i].GetStream().BeginWrite(_ArrToSend, 0, _ArrToSend.Length, null, null);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }

    }
    static void SentToAllExcept(byte[] arrayToSend, int ClientToExcept)
    {
        try
        {
            for (int i = 1; i <= _clientNumber; i++)
            {
                if (i != ClientToExcept)
                {
                    clients[i].GetStream().BeginWrite(arrayToSend, 0, arrayToSend.Length, null, null);
                }
            }
        }
        catch
        {
            throw new Exception("your momma");
        }
    }
    static void SentToAllExcept(Packet _packetToSend, int ClientToExcept)
    {
        try
        {
            for (int i = 1; i <= _clientNumber; i++)
            {
                if (i != ClientToExcept)
                {
                    clients[i].GetStream().BeginWrite(_packetToSend.ToArray(), 0, _packetToSend.Length(), null, null);
                }
            }
        }
        catch
        {
            throw new Exception("your momma");
        }
    }
    #endregion
}

