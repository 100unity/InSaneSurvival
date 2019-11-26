using System;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Remote
{
    [System.Serializable] public class UnityEventInt:UnityEvent<int> {}
    public class RemoteStatusHandler : MonoBehaviour
    {
        
        private TcpClient _client;
        private NetworkStream _stream;

        [SerializeField] private string ip;
        [SerializeField] private int port;
        [SerializeField] private bool use;
        
        [Header("Event that will throw when the player's health has been changed via Remote")]
        public UnityEventInt playerHealthRemoteUpdate;
        
        [Header("Event that will throw when the player's saturation has been changed via Remote")]
        public UnityEventInt playerSaturationRemoteUpdate;
        
        [Header("Event that will throw when the player's hydration has been changed via Remote")]
        public UnityEventInt playerHydrationRemoteUpdate;
    
        private void Start()
        {
            if (use)
            {
                Connect(ip);
            }
        }

        private void OnDestroy()
        {
            if (use)
            {
                Disconnect();
            }
            
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
                        MessageReceived(responseData);
                    }
                }
                
            }
            catch (Exception e)
            {
                use = false;
                Debug.Log("Connection to Remote Server failed");
            }
        }

        private void SendString(string message)
        {
            if (use)
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                _stream.Write(data, 0, data.Length);
            }
        }

        private void MessageReceived(string message)
        {
            if (message.Contains("/"))
            {
                string[] parameters = message.Split('/');
                switch (parameters[0])
                {
                    case "HP": playerHealthRemoteUpdate.Invoke(int.Parse(parameters[1])); 
                        break;
                    case "HNG": playerSaturationRemoteUpdate.Invoke(int.Parse(parameters[1]));
                        break;
                    case "THR": playerHydrationRemoteUpdate.Invoke(int.Parse(parameters[1]));
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

        public void SaturationUpdated(int newValue)
        {
            SendString("HNG/"+newValue);
        }

        public void HydrationUpdated(int newValue)
        {
            SendString("THR/"+newValue);
        }

        public void PositionUpdated(Vector3 newPosition)
        {
            SendString("POS/"+newPosition.x+"/"+newPosition.y+"/"+newPosition.z);
        }
    }
}