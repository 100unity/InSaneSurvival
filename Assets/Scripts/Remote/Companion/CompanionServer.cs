using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Managers;
using Remote.Companion.Packets;
using UnityEngine;

namespace Remote.Companion
{
    public class CompanionServer : MonoBehaviour
    {
        private const int PORT = 7999;

        private TcpListener _server;
        private NetworkStream _stream;
        private Dictionary<string, Inventory.Item> _itemMap;

        private void Start()
        {
            Inventory.Item[] items = Resources.FindObjectsOfTypeAll<Inventory.Item>();
            _itemMap = items
                .Select(i => new KeyValuePair<string, Inventory.Item>(i.name, i))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _server = new TcpListener(IPAddress.Parse("127.0.0.1"), PORT);
            _server.Start();

            Listen().ConfigureAwait(false);
        }

        private async Task Listen()
        {
            byte[] bytes = new byte[65536];

            while (true)
            {
                TcpClient client = await _server.AcceptTcpClientAsync();
                _stream = client.GetStream();

                // Subscribe to inventory update events for updating remove inventory view
                InventoryManager.Instance.ItemHandler.ItemAdded += SendAddOne;
                InventoryManager.Instance.ItemHandler.ItemRemoved += SendDeleteOne;

                IEnumerable<string> itemNames =
                    InventoryManager.Instance.GetInvController().GetItems().Select(item => item.name);
                Dictionary<string, Item> items = new Dictionary<string, Item>();
                foreach (string itemName in itemNames)
                {
                    if (items.TryGetValue(itemName, out Item item))
                        item.amount++;
                    else
                        items[itemName] = new Item
                        {
                            name = itemName,
                            amount = 1
                        };
                }

                string json = JsonUtility.ToJson(new InventoryPacket {data = items.Values.ToArray()});
                byte[] inventoryContent = Encoding.UTF8.GetBytes(json);

                await _stream.WriteAsync(inventoryContent, 0, inventoryContent.Length);

                while (client.Connected)
                {
                    int i;
                    while ((i = await _stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                    {
                        string data = Encoding.UTF8.GetString(bytes, 0, i);
                        HandlePacket(data);
                    }
                }

                // Unsubscribe from inventory update events for updating remove inventory view
                InventoryManager.Instance.ItemHandler.ItemAdded -= SendAddOne;
                InventoryManager.Instance.ItemHandler.ItemRemoved -= SendDeleteOne;
            }
        }

        private void SendAddOne(Inventory.Item item) => SendAddOneAsync(item).ConfigureAwait(false);

        private async Task SendAddOneAsync(Inventory.Item item)
        {
            AddOnePacket packet = new AddOnePacket
            {
                data = new Item
                {
                    name = item.name
                }
            };
            byte[] data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(packet));
            await _stream.WriteAsync(data, 0, data.Length);
        }

        private void SendDeleteOne(Inventory.Item item) => SendDeleteOneAsync(item).ConfigureAwait(false);

        private async Task SendDeleteOneAsync(Inventory.Item item)
        {
            DeleteOnePacket packet = new DeleteOnePacket
            {
                data = new Item
                {
                    name = item.name
                }
            };
            byte[] data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(packet));
            await _stream.WriteAsync(data, 0, data.Length);
        }

        private void HandlePacket(string packet)
        {
            ReceivedPacket receivedPacket = JsonUtility.FromJson<ReceivedPacket>(packet);
            switch (receivedPacket.type)
            {
                case "addOne":
                    AddOnePacket addOnePacket = JsonUtility.FromJson<AddOnePacket>(packet);
                    InventoryManager.Instance.AddItem(_itemMap[addOnePacket.data.name]);
                    break;
                case "deleteOne":
                    DeleteOnePacket deleteOnePacket = JsonUtility.FromJson<DeleteOnePacket>(packet);
                    InventoryManager.Instance.RemoveItem(_itemMap[deleteOnePacket.data.name]);
                    break;
                case "deleteAll":
                    DeleteAllPacket deleteAllPacket = JsonUtility.FromJson<DeleteAllPacket>(packet);
                    InventoryManager.Instance.RemoveItem(_itemMap[deleteAllPacket.data.name], 1000);
                    break;
                default:
                    Debug.LogError($"Received unknown packet type: {receivedPacket.type}");
                    break;
            }
        }
    }
}