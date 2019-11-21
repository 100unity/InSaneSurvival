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

        public void SendString(string message)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            _stream.Write(data, 0, data.Length);

            // Console.WriteLine("Sent: {0}", message);
        }

        private void Disconnect()
        {
            _stream.Close();
            _client.Close();
        }
        
        //event listeners
        public void HeathUpdated(int newValue)
        {
            Debug.Log(newValue);
        }
    }
}