using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alchemy;
using Alchemy.Classes;
using Newtonsoft.Json;
using EllappServer.Db;

namespace EllappServer.Classes
{
    class Session
    {
        uint ID;
        Account account;
        UserContext context;

        public Session(uint _id, Account _account, UserContext _context)
        {
            ID = _id;
            account = _account;
            context = _context;
        }

        public UserContext GetContext()
        {
            return context;
        }

        public Account GetAccount()
        {
            return account;
        }

        public void SendMessage(MessagePacket pkt)
        {
            var msg = JsonConvert.SerializeObject(pkt);
            Console.WriteLine(msg);
            context.Send(msg);
        }
    }
}
