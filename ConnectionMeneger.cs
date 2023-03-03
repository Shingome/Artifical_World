using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class ConnectionMeneger : MonoBehaviour
{
    [HideInInspector] public bool isTxStarted = false;

    [SerializeField] string IP = "localhost"; 
    [SerializeField] int rxPort = 8000; 
    [SerializeField] int txPort = 8001; 

    UdpClient client;
    IPEndPoint remoteEndPoint;
    Thread receiveThread;

    List<int> indexes = new List<int>();

    public GameObject taskReciver;

    public void SendData(GameObject obj) 
    {
        string message;
        if (indexes.Contains(obj.GetComponent<BotScript>().index))
        {
            message = obj.GetComponent<TrainManager>().ToGetAction(obj.GetComponent<BotScript>().botQuest);
        }
        else
        {
            indexes.Add(obj.GetComponent<BotScript>().index);
            message = obj.GetComponent<TrainManager>().GetObservation();
        }
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }

    void Awake()
    {
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), txPort);

        client = new UdpClient(rxPort);

        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
  
    }

    private void ReceiveData()
    {
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                taskReciver.GetComponent<TaskReciver>().GetData(text);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    private void ProcessInput(string input)
    {

        if (!isTxStarted) 
        {
            isTxStarted = true;
        }
    }

    void OnDisable()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        client.Close();
    }

}
