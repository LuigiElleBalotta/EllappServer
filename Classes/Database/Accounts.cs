using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LappaORM;

namespace EllappServer.Db
{
    public class Accounts : Entity
    {
        [AutoIncrement]
        public uint idAccount { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public long data_creazione { get; set; }
        public DateTime last_connection { get; set; }
        public string last_ip { get; set; }
        public uint isOnline { get; set; }

    }
}
