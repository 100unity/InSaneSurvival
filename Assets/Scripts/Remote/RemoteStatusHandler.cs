using System;
using System.Net.Sockets;
using Player;
using UnityEngine;

namespace Remote
{
    public class RemoteStatusHandler : MonoBehaviour
    {
        
        private TcpClient _client;
        private NetworkStream _stream;

        [SerializeField] private string ip;
        [SerializeField] private int port;
        
    
        private void Start()
        {
            Connect(ip);
        }

        private void OnApplicationQuit()
        {
            Disconnect();
        }
        
        //server connection
        private void Connect(string server)
        {
            try
            {
                _client = new TcpClient(server, port);

                _stream = _client.GetStream();
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

        private void Disconnect()
        {
            _stream.Close();
            _client.Close();
        }
        
        //event listeners
        public void HeathUpdated(int newValue)
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