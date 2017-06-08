using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using MongoDB.Bson;
using MongoDB.Driver;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    class LoginCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player, List<Player> allPlayers, List<GameRoom> gameRooms)
        {
            var name = inc.ReadString();
            var password = inc.ReadString();

            var database = mongoClient.GetDatabase("MapMaker");

            var collection = database.GetCollection<BsonDocument>("Logins");

            var filter = Builders<BsonDocument>.Filter.Eq("Username", name);
            var projection = Builders<BsonDocument>.Projection.Include("Password").Exclude("_id");

            var documentTest = collection.Find(filter).Project(projection).FirstOrDefault();

            if (documentTest == null)
            {
                inc.SenderConnection.Deny("Incorrect username or password");
                loggerManager.ServerMsg("Incorrect username or password from: " + inc.SenderConnection);
                return;
            }

            string passHash = "";
            foreach (var value in documentTest.Values)
            {
                passHash = (string)value;
            }
            if (Hasher.VerifyHash(password, "SHA256", passHash))
            {
                inc.SenderConnection.Approve();
            }
            else
            {
                inc.SenderConnection.Deny("Incorrect username or password");
                loggerManager.ServerMsg("Incorrect username or password from: " + inc.SenderConnection);
            }

            loggerManager.ServerMsg("Incoming login");

            if (allPlayers.Any(x => x.Username == name))
            {
                var deniedReason = "Denied connection, duplicate client.";
                inc.SenderConnection.Deny(deniedReason);
                loggerManager.ServerMsg(deniedReason);
                return;
            }
            inc.SenderConnection.Approve();
            loggerManager.ServerMsg("Approved client connection");

            allPlayers.Add(CreatePlayer(loggerManager, inc, name, allPlayers));

            NetOutgoingMessage outmsg = server.CreateMessage();

            outmsg.Write((byte)PacketTypes.StartState);

            outmsg.Write(gameRooms.Count);
            for (int i = 0; i < gameRooms.Count; i++)
            {
                outmsg.Write(gameRooms[i].Name);
            }

            //connectionmessage:
            //packet
            //circle count
            //all circle info
            //player count
            //all player info

            System.Threading.Thread.Sleep(500);

            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

            loggerManager.ServerMsg("Approved new connection and updated the world status");
        }

        private static Player CreatePlayer(LoggerManager loggerManager, NetIncomingMessage inc, string name, List<Player> allPlayers)
        {
            return new Player(name, new Vector2(0, 0), 10f, 0f, 5f, 50, inc.SenderConnection);
        }
    }
}
