using System;
using System.Net.Sockets;
using Player;
using Polybrush;
using UnityEngine;
using UnityEngine.Events;

namespace Remote
{
    [System.Serializable] public class UnityEventInt:UnityEvent<int> {}
    public class RemoteStatusHandler : MonoBehaviour
    {
        
        private TcpClient _client;
        private NetworkStream _stream;

        [SerializeField] private string ip;
        [SerializeField] private int port;
        
        [Header("Event that will throw when the player's health has been changed via Remote")]
        public UnityEventInt playerHeathRemoteUpdate;
        
        [Header("Event that will throw when the player's hunger has been changed via Remote")]
        public UnityEventInt playerHungerRemoteUpdate;
        
        [Header("Event that will throw when the player's thirst has been changed via Remote")]
        public UnityEventInt playerThirstRemoteUpdate;
    
        private void Start()
        {
            Connect(ip);
        }

        private void OnApplicationQuit()
        {
            Disconnect();
        }
        
        //server connection
        private async void Connect(string server)
        {
            try
            {
                _client = new TcpClient(server, port);

                _stream = _client.GetStream();
                
                while (_client.Connected) {
                    byte[ ] buffer = new byte[_client.ReceiveBufferSize];
                    int read = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (read > 0 ){
                        string responseData = System.Text.Encoding.ASCII.GetString(buffer, 0, read);
                        MessageRecieved(responseData);
                    }
                }
                
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        private void SendString(string message)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            _stream.Write(data, 0, data.Length);
        }

        private void MessageRecieved(string message)
        {
            if (message.Contains("/"))
            {
                string[] parameters = message.Split('/');
                switch (parameters[0])
                {
                    case "HP": playerHeathRemoteUpdate.Invoke(int.Parse(parameters[1])); 
                        break;
                    case "HNG": playerHungerRemoteUpdate.Invoke(int.Parse(parameters[1]));
                        break;
                    case "THR": playerThirstRemoteUpdate.Invoke(int.Parse(parameters[1]));
                        break;
                }
                    
            }
        }

        private void Disconnect()
        {
            _stream.Close();
            _client.Close();
        }
        
        //event listeners
        public void HealthUpdated(int newValue)
        {
            SendString("HP/"+newValue);
        }

        public void HungerUpdated(int newValue)
        {
            SendString("HNG/"+newValue);
        }

        public void ThirstUpdated(int newValue)
        {
            SendString("THR/"+newValue);
        }

        public void PositionUpdated(Vector3 newPosition)
        {
            SendString("POS/"+newPosition.x+"/"+newPosition.y+"/"+newPosition.z);
        }
    }
}