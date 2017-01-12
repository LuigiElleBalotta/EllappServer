using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LappaORM;

namespace EllappServer
{
    public static class Database
    {
        public static LappaORM.Database EllappDB = new LappaORM.Database();

        public static string CreateConnectionString(string host, string user, string password, string database, int port, int minPoolSize, int maxPoolSize, DatabaseType dbType)
        {
            if (dbType == DatabaseType.MySql)
                return $"Server={host};User Id={user};Port={port};Password={password};Database={database};Pooling=True;Min Pool Size={minPoolSize};Max Pool Size={maxPoolSize};CharSet=utf8";
            return null;
        }
    }
}
