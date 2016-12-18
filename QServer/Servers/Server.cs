using QServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QServer.Servers
{
    class Server
    {
        protected QList<Client> clients;//All connected clients
        public Server()
        {
            clients = new QList<Client>();
        }
        public void AddClient(Client visitor)
        {
            clients.Add(visitor);
            visitor.received += new Client.ClientReceivedHandler(client_received);
            visitor.disconnected += new Client.ClientDisconnectedHandler(visitor_disconnected);
        }

        protected virtual void visitor_disconnected(Client sender)
        {
            RemoveClient(sender);
           // sender.disconnected -= new Client.ClientDisconnectedHandler(visitor_disconnected);
        }
        protected void RemoveAllClients()
        {
            while (clients.Count != 0)
            {
                RemoveClient(clients[0]);
            }
        }
        public void SendToAll(string package)
        {
            package = QEncryption.Encrypt(package);
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].SendWEncrypt(package);
            }
        }
        protected void RemoveClient(Client visitor)
        {
            clients.Remove(visitor);
            visitor.received -= new Client.ClientReceivedHandler(client_received);
        }
        protected virtual void client_received(Client sender, string[] packetStrings)
        {
            throw new Exception("Empty should be overwriten...");
        }
    }
}
