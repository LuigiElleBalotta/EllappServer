using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LappaORM;

namespace EllappServer.Db
{
    class Log_Chat : Entity
    {
        [AutoIncrement]
        public uint idLog { get; set; }
        public string content { get; set; }
        public uint from { get; set; }
        public string to_type { get; set; }
        public uint to { get; set; }
        public DateTime date { get; set; }
    }
}
