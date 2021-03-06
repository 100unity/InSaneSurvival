﻿using System;
using System.Net.Sockets;
using System.Text;
using Entity.Player;
using Managers;
using UnityEngine;

namespace Remote
{
    public class RemoteStatusHandler : MonoBehaviour
    {
        public delegate void PlayerStateChanged(int newValue);

        private TcpClient _client;
        private NetworkStream _stream;

        [SerializeField] private string ip;
        [SerializeField] private int port;
        [SerializeField] private bool use;

        public static event PlayerStateChanged OnPlayerHealthRemoteUpdate;
        public static event PlayerStateChanged OnPlayerSaturationRemoteUpdate;
        public static event PlayerStateChanged OnPlayerHydrationRemoteUpdate;
        public static event PlayerStateChanged OnPlayerSanityRemoteUpdate;

        private void OnEnable()
        {
            PlayerState.OnPlayerHealthUpdate += HealthUpdated;
            PlayerState.OnPlayerSaturationUpdate += SaturationUpdated;
            PlayerState.OnPlayerHydrationUpdate += HydrationUpdated;
            PlayerState.OnPlayerSanityUpdate += SanityUpdated;
            PlayerController.OnPlayerPositionUpdate += PositionUpdated;
        }

        private void OnDisable()
        {
            PlayerState.OnPlayerHealthUpdate -= HealthUpdated;
            PlayerState.OnPlayerSaturationUpdate -= SaturationUpdated;
            PlayerState.OnPlayerHydrationUpdate -= HydrationUpdated;
            PlayerState.OnPlayerSanityUpdate -= SanityUpdated;
            PlayerController.OnPlayerPositionUpdate -= PositionUpdated;
        }

        private void Start()
        {
            if (use)
                Connect(ip);
        }

        private void OnDestroy()
        {
            if (use)
                Disconnect();
        }

        //server connection
        private async void Connect(string server)
        {
            try
            {
                _client = new TcpClient(server, port);
                _stream = _client.GetStream();

                while (_client.Connected)
                {
                    byte[] buffer = new byte[_client.ReceiveBufferSize];
                    int read = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        string responseData = Encoding.ASCII.GetString(buffer, 0, read);
                        MessageReceived(responseData);
                    }
                }
            }
            catch (Exception e)
            {
                use = false;
                Debug.Log("Connection to Remote Server failed because: " + e.Message);
            }
        }

        private void SendString(string message)
        {
            if (use && _client?.Connected == true)
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
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
                    case "HP":
                        OnPlayerHealthRemoteUpdate?.Invoke(int.Parse(parameters[1]));
                        break;
                    case "HNG":
                        OnPlayerSaturationRemoteUpdate?.Invoke(int.Parse(parameters[1]));
                        break;
                    case "THR":
                        OnPlayerHydrationRemoteUpdate?.Invoke(int.Parse(parameters[1]));
                        break;
                    case "SNT":
                        OnPlayerSanityRemoteUpdate?.Invoke(int.Parse(parameters[1]));
                        break;
                    case "SAVE":
                        SaveManager.Save(parameters[1]);
                        SendString("saving initiated");
                        break;
                    case "LOAD":
                        SaveManager.Load(parameters[1]);
                        SendString("loading initiated");
                        break;
                    case "TIME":
                        DayNightManager.Instance.SetTimeOfDay(float.Parse(parameters[1]));
                        break;
                    case "ITEM":
                        GetComponent<ItemCheater>().AddItems(parameters[1]);
                        break;
                    default:
                        SendString("invalid command");
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
        private void HealthUpdated(int newValue)
        {
            SendString("HP/" + newValue);
        }

        private void SaturationUpdated(int newValue)
        {
            SendString("HNG/" + newValue);
        }

        private void HydrationUpdated(int newValue)
        {
            SendString("THR/" + newValue);
        }

        private void SanityUpdated(int newValue)
        {
            SendString("SNT/" + newValue);
        }

        private void PositionUpdated(Vector3 newPosition)
        {
            SendString("POS/" + newPosition.x + "/" + newPosition.y + "/" + newPosition.z);
        }
    }
}