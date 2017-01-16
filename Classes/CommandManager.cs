using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace EllappServer.Classes
{
    class CommandManager
    {
        static Config_Manager conf = new Config_Manager();
        static MySqlConnection conn = new MySqlConnection("Server=" + conf.getValue("mysql_host") + ";Database=" + conf.getValue("mysql_db") + ";Uid=" + conf.getValue("mysql_user") + ";Pwd=" + conf.getValue("mysql_password") + ";");
        public static void CreateAccount(string username, string password, string email)
        {
            byte[] passwordbyte = Encoding.ASCII.GetBytes(username + ":" + password);
            var sha_pass = SHA1.Create();
            byte[] bytehash = sha_pass.ComputeHash(passwordbyte);
            var hashedpsw = Utility.HexStringFromBytes(bytehash);

            User u = new User();
            u.username = username.ToUpper();
            u.password = hashedpsw.ToUpper();
            u.email = email;

            u.CreateAccount();
        }
    }
}
