using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using EllappServer.definitions;
using EllappServer.Db;

namespace EllappServer.Classes
{
    class User
    {
        uint ID = 0;
        string username, last_ip, email;
        static Config_Manager conf = new Config_Manager();
        //static MySqlConnection staticconn = new MySqlConnection("Server=" + conf.getValue("mysql_host") + ";Database=" + conf.getValue("mysql_db") + ";Uid=" + conf.getValue("mysql_user") + ";Pwd=" + conf.getValue("mysql_password") + ";");
        //MySqlConnection conn = new MySqlConnection("Server=" + conf.getValue("mysql_host") + ";Database=" + conf.getValue("mysql_db") + ";Uid=" + conf.getValue("mysql_user") + ";Pwd=" + conf.getValue("mysql_password") + ";");
        public User(string _username, string _password)
        {
            username = _username;
            byte[] passwordbyte = Encoding.ASCII.GetBytes(username + ":" + _password);
            var sha_pass = SHA1.Create();
            byte[] bytehash = sha_pass.ComputeHash(passwordbyte);
            _password = Utility.HexStringFromBytes(bytehash);

            var res = Database.EllappDB.Where<Accounts>(r => r.username == username && r.password == _password);

            foreach(Accounts a in res)
            {
                ID = a.idAccount;
                last_ip = a.last_ip;
                email = a.email;
            }
        }

        public bool Validate()
        {
            if (ID > 0)
                return true;
            else
                return false;
        }

        public string GetUsername()
        {
            return username;
        }

        public uint GetID()
        {
            return ID;
        }

        public bool IsOnline()
        {
            var res = Database.EllappDB.Where<Accounts>(r => r.idAccount == ID);
            uint onlineBit = 0;
            foreach(Accounts a in res)
            {
                onlineBit = a.isOnline;
            }

            return onlineBit != 0;
        }

        public void SetOnline()
        {
            /*conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE accounts SET isOnline = 1 WHERE idAccount = @id;", conn);
            MySqlParameter idParameter = new MySqlParameter("@id", MySqlDbType.Int32, 0);
            idParameter.Value = ID;
            cmd.Parameters.Add(idParameter);
            cmd.ExecuteNonQuery();
            conn.Close();*/
        }

        public void SetOffline()
        {
            /*conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE accounts SET isOnline = 0 WHERE idAccount = @id;", conn);
            MySqlParameter idParameter = new MySqlParameter("@id", MySqlDbType.Int32, 0);
            idParameter.Value = ID;
            cmd.Parameters.Add(idParameter);
            cmd.ExecuteNonQuery();
            conn.Close();*/
        }

        public static List<Chat> GetChats(int AccountID, string ChatRequestID = "")
        {
            //staticconn.Open();
            List<Chat> chats = new List<Chat>();
            if (ChatRequestID == "") //I am requesting only all open chat, not the messages
            {/*
                MySqlCommand cmd = new MySqlCommand("SELECT ChatID as 'chatroom', `from`, content, `to`, `date` FROM log_chat WHERE to_type = 'CHAT_TYPE_USER_TO_USER' AND (`from` = @id or `to` = @id) AND `date` IN (SELECT MAX(`date`) FROM log_chat WHERE ChatID <> '' GROUP BY ChatID) ORDER BY `date` desc;", staticconn);
                MySqlParameter idParameter = new MySqlParameter("@id", MySqlDbType.Int32, 0);
                idParameter.Value = AccountID;
                cmd.Parameters.Add(idParameter);
                MySqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    Chat c = new Chat(ChatType.CHAT_TYPE_USER_TO_USER, r["chatroom"].ToString(), r["content"].ToString(), Misc.GetUsernameByID(Convert.ToInt32(r["from"])).ToString(), Misc.GetUsernameByID(Convert.ToInt32(r["to"])).ToString(), (long)Misc.DateTimeToUnixTimestamp(Convert.ToDateTime(r["date"].ToString())));
                    chats.Add(c);
                }
                r.Close();
                staticconn.Close();*/

                //var res = Database.EllappDB.Where<Log_Chat>(r => r.to_type == "CHAT_TYPE_USER_TO_USER" && (r.from == AccountID || r.to == AccountID));

                return chats;
            }
            else
            {
                /*MySqlCommand cmd = new MySqlCommand("SELECT ChatID as 'chatroom', `from`, content, `to`, `date` FROM log_chat WHERE ChatID = @chatid AND to_type = 'CHAT_TYPE_USER_TO_USER' ORDER BY `date` ASC;", staticconn);
                MySqlParameter chatidParameter = new MySqlParameter("@chatid", MySqlDbType.String, 0);
                chatidParameter.Value = ChatRequestID;
                cmd.Parameters.Add(chatidParameter);
                MySqlDataReader r = cmd.ExecuteReader();
                while(r.Read())
                {
                    Chat c = new Chat(ChatType.CHAT_TYPE_USER_TO_USER, r["chatroom"].ToString(), r["content"].ToString(), Misc.GetUsernameByID(Convert.ToInt32(r["from"])).ToString(), Misc.GetUsernameByID(Convert.ToInt32(r["to"])).ToString(), (long)Misc.DateTimeToUnixTimestamp(Convert.ToDateTime(r["date"].ToString())));
                    chats.Add(c);
                }
                r.Close();
                staticconn.Close();*/
                return chats;
            }
        }
    }
}
