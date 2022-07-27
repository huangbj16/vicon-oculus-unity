using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class TcpSender : MonoBehaviour
{

    public String Host = "localhost";
    public Int32 Port = 9051;

    TcpClient client = null;
    NetworkStream stream = null;
    StreamWriter writer = null;
    public bool isConnected = false;

    // Start is called before the first frame update
    void Start()
    {
        client = new TcpClient();
        if (SetupSocket())
        {
            isConnected = true;
            Debug.Log("socket is set up");
        }
        else
        {
            isConnected = false;
            Debug.Log("socket setup failed");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendData(String data)
    {
        Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(data);
        client.GetStream().Write(sendBytes, 0, sendBytes.Length);
        Debug.Log("socket is sent");
    }

    public bool SetupSocket()
    {
        try
        {
            client.Connect(Host, Port);
            stream = client.GetStream();
            writer = new StreamWriter(stream);
            Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes("1");
            client.GetStream().Write(sendBytes, 0, sendBytes.Length);
            Debug.Log("socket is sent");
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
            return false;
        }
    }

    private void OnApplicationQuit()
    {
        if (client != null && client.Connected)
            client.Close();
    }

}
