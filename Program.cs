using System;
using System.Threading.Tasks;
using TL;
using System.Linq;
using System.Security.Cryptography;
//using WTelegram;

namespace TelegramClientTest
{
    class Program
    {

        static string Config1(string what)
        {
            switch (what)
            {
                case "api_id": return "19696018";
                case "api_hash": return "7e74cd6006ddef7ed43187d3ac564b40";
                case "phone_number": return "79164649009";
                case "verification_code": Console.Write("Code: "); return Console.ReadLine();
                case "first_name": return "Sasha";      // if sign-up is required
                case "last_name": return "Ivanov";        // if sign-up is required
                case "password": return "5883022";     // if user has enabled 2FA
                case "session_pathname": return "session1";
                default: return null;                  // let WTelegramClient decide the default config
            }
        }
        static string Config2(string what)
        {
            switch (what)
            {
                case "api_id": return "19696018";
                case "api_hash": return "7e74cd6006ddef7ed43187d3ac564b40";
                case "phone_number": return "79015396209";
                case "verification_code": Console.Write("Code: "); return Console.ReadLine();
                case "first_name": return "Sasha";      // if sign-up is required
                case "last_name": return "Ivanov";        // if sign-up is required
                case "password": return "5883022";     // if user has enabled 2FA
                case "session_pathname": return "session2";
                default: return null;                  // let WTelegramClient decide the default config
            }
        }
        static string Config3(string what)
        {
            switch (what)
            {
                case "api_id": return "19696018";
                case "api_hash": return "7e74cd6006ddef7ed43187d3ac564b40";
                case "phone_number": return "79255087106";
                case "verification_code": Console.Write("Code: "); return Console.ReadLine();
                case "first_name": return "Sasha";      // if sign-up is required
                case "last_name": return "Ivanov";        // if sign-up is required
                case "password": return "5883022";     // if user has enabled 2FA
                case "session_pathname": return "session_79255087106";
                default: return null;                  // let WTelegramClient decide the default config
            }
        }

        static async Task Main(string[] _)
        {
            using var client1 = new WTelegram.Client(Config1);
            var my1 = await client1.LoginUserIfNeeded();
            using var client2 = new WTelegram.Client(Config2);
            var my2 = await client2.LoginUserIfNeeded();
            using var client3 = new WTelegram.Client(Config3);
            var my3 = await client3.LoginUserIfNeeded();

            Console.WriteLine($"We are logged-in as {my1.username ?? my1.first_name + " " + my1.last_name} (id {my1.id})");
            Console.WriteLine($"And We are logged-in as {my2.username ?? my2.first_name + " " + my2.last_name} (id {my2.id})");

            var chats = await client3.Messages_GetAllChats();
            Console.WriteLine("This user has joined the following:");
            foreach (var (id, chat) in chats.chats)
                switch (chat) // example of downcasting to their real classes:
                {
                    case Chat smallgroup when smallgroup.IsActive:
                        Console.WriteLine($"{id}:  Small group: {smallgroup.title} with {smallgroup.participants_count} members");
                        break;
                    case Channel group when group.IsGroup:
                        Console.WriteLine($"{id}: Group {group.username}: {group.title}");
                        break;
                    case Channel channel:
                        Console.WriteLine($"{id}: Channel {channel.username}: {channel.title}");
                        break;
                }


            //var c2 = new InputUser(client2.UserId, client2.GetAccessHashFor<IObject>(client2.UserId));
            //= await (InputUserBase) client.se ;

            //var call01 = await client1.Phone_RequestCall(c2,10,null,null);
            //client2.hash
            //var u2info = await client2.Help_GetUserInfo(new InputUser(client2.UserId, client2.GetAccessHashFor<IObject>(client2.UserId)));

            var u1 = my1.ToInputPeer();
            var u2 = my2.ToInputPeer();
            var u3 = my3.ToInputPeer();

            
            var uid = my2.id;
            var ah= my2.access_hash;
            var c2 = new InputUser(uid, ah);

            var cont2 = await client2.Contacts_GetContacts();
            var GG = cont2.users.Where(x => x.Value.phone == "79252623619");

            //key = 1144464374  GG
            //Key = 977062115   Iosif

            //var newC = await client2.Contacts_ImportContacts(new[] { new InputPhoneContact() { phone = "79262001705" } });

            //var dhc = (Messages_DhConfig) await client3.Messages_GetDhConfig(1,10);
            //var gah_gen = SHA256.Create();
            
            //var g_a = dhc.p;

            //var g_a_hash = gah_gen.ComputeHash(g_a);
            //var php = new PhoneCallProtocol() { flags = (PhoneCallProtocol.Flags) 3, min_layer = 133, max_layer = 139, library_versions = new string[] { "library_version_001" } };

            //await client3.Phone_RequestCall(c2, 11, g_a_hash, php);

            //await client2.SendMessageAsync(cont2.users[977062115].ToInputPeer(), "Привет, Йося. Это Телеграмм-бот)");
            //await client3.SendMessageAsync(u2, "Hello, world! I'm a Client-3-2");

            Console.Write("Type a chat ID to send a message: ");
            long chatId = long.Parse(Console.ReadLine());
            var target = chats.chats[chatId];
            Console.WriteLine($"Sending a message in chat {chatId}: {target.Title}");
            await client3.SendMessageAsync(target, "Hello, world! I'm a Telegram bot");
        }
    }
}
