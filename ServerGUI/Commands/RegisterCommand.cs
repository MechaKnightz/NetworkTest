using System;
using System.Collections.Generic;
using System.Linq;
using Library;
using Lidgren.Network;
using MongoDB.Bson;
using MongoDB.Driver;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    public class RegisterCommand : ICommand
    {
        private static readonly List<Cooldown> Cooldowns = new List<Cooldown>();
        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player, List<Player> allPlayers, List<GameRoom> gameRooms)
        {
            for (int i = 0; i < Cooldowns.Count; i++)
            {
                if(Cooldowns[i].CreatedTime + TimeSpan.FromMinutes(5) <= DateTime.Now) Cooldowns.RemoveAt(i);
            }
            if (Cooldowns.Any(x => x.SenderConnection.RemoteEndPoint.Address.Equals(inc.SenderConnection.RemoteEndPoint.Address)))
            {
                inc.SenderConnection.Deny("You have created too many accounts within the last 5 minutes.");
                return;
            }
            
            var name = inc.ReadString();
            if (name.Length <= 3)
            {
                inc.SenderConnection.Deny("Username too short.");
                return;
            }
            if (name.Length >= 16)
            {
                inc.SenderConnection.Deny("Username too long.");
                return;
            }
            if (!name.All(char.IsLetterOrDigit))
            {
                inc.SenderConnection.Deny("Username contains invalid characters.");
            }
            var password = inc.ReadString();
            if (password.Length <= 3)
            {
                inc.SenderConnection.Deny("Password too short.");
                return;
            }
            if (password.Length >= 16)
            {
                inc.SenderConnection.Deny("Password too long.");
                return;
            }


            var database = mongoClient.GetDatabase("MapMaker");

            var collection = database.GetCollection<BsonDocument>("Logins");

            var filter = Builders<BsonDocument>.Filter.Eq("Username", name);
            var projection = Builders<BsonDocument>.Projection.Include("Username").Exclude("_id");

            var documentTest = collection.Find(filter).Project(projection).FirstOrDefault();

            //todo add registered date
            if (documentTest == null)
            {
                var document = new BsonDocument
                { 
                    { "Username", name},
                    { "Password", Hasher.ComputeHash(password, "SHA256", null) },
                    { "RegisterDate", DateTime.Now.Date }
                };
                collection.InsertOne(document);
                Cooldowns.Add(new Cooldown(inc.SenderConnection));
                inc.SenderConnection.Deny("Successfully registered account");
                loggerManager.ServerMsg("Registered account " +  name);
                return;
            }
            inc.SenderConnection.Deny("Account already exists");
        }
    }
}
