using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LappaORM;
using MySql.Data.MySqlClient;
using EllappServer.Classes;
using System.Security.Cryptography;
using EllappServer.definitions;

namespace EllappServer.Db
{
    public class Account : Entity
    {
        [AutoIncrement]
        public uint idAccount { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public long data_creazione { get; set; }
        public DateTime last_connection { get; set; }
        public string last_ip { get; set; }
        public bool isOnline { get; set; }

        static Config_Manager conf = new Config_Manager();
        static MySqlConnection staticconn = new MySqlConnection("Server=" + conf.getValue("mysql_host") + ";Database=" + conf.getValue("mysql_db") + ";Uid=" + conf.getValue("mysql_user") + ";Pwd=" + conf.getValue("mysql_password") + ";");

        public Account() { }

        public Account(uint _id)
        {
            var res = Database.EllappDB.Where<Account>(r => r.idAccount == _id);
            foreach(Account a in res)
            {
                idAccount = a.idAccount;
                username = a.username;
                password = a.password;
                email = a.email;
                data_creazione = a.data_creazione;
                last_connection = a.last_connection;
                last_ip = a.last_ip;
                isOnline = a.isOnline;
            }
        }

        public Account(string _username, string _password)
        {
            username = _username;
            byte[] passwordbyte = Encoding.ASCII.GetBytes(username + ":" + _password);
            var sha_pass = SHA1.Create();
            byte[] bytehash = sha_pass.ComputeHash(passwordbyte);
            _password = Utility.HexStringFromBytes(bytehash);

            var res = Database.EllappDB.Single<Account>(r => r.username == username && r.password == _password);
            idAccount = res.idAccount;
            username = res.username;
            password = res.password;
            email = res.email;
            data_creazione = res.data_creazione;
            last_connection = res.last_connection;
            last_ip = res.last_ip;
            isOnline = res.isOnline;
        }

        public bool Validate()
        {
            if (idAccount > 0)
                return true;
            else
                return false;
        }

        public void CreateAccount()
        {
            Database.EllappDB.Add(this);
        }

        public bool IsOnline()
        {
            var res = Database.EllappDB.Single<Account>(r => r.idAccount == idAccount);
            bool onlineBit = res.isOnline;

            return onlineBit;
        }

        public void SetOnline()
        {
            isOnline = true;
            Database.EllappDB.Update(this);
        }

        public void SetOffline()
        {
            isOnline = false;
            Database.EllappDB.Update(this);
        }

        public static List<Chat> GetChats(uint AccountID, string ChatRequestID = "")
        {
            staticconn.Open();
            List<Chat> chats = new List<Chat>();
            if (ChatRequestID == "") //I am requesting only all open chat, not the messages
            {
                MySqlCommand cmd = new MySqlCommand("SELECT ChatID as 'chatroom', `from`, content, `to`, `date` FROM log_chat WHERE to_type = 'CHAT_TYPE_USER_TO_USER' AND (`from` = @id or `to` = @id) AND `date` IN (SELECT MAX(`date`) FROM log_chat WHERE ChatID <> '' GROUP BY ChatID) ORDER BY `date` desc;", staticconn);
                MySqlParameter idParameter = new MySqlParameter("@id", MySqlDbType.Int32, 0);
                idParameter.Value = AccountID;
                cmd.Parameters.Add(idParameter);
                MySqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    Chat c = new Chat(ChatType.CHAT_TYPE_USER_TO_USER, r["chatroom"].ToString(), r["content"].ToString(), Misc.GetUsernameByID(Convert.ToUInt16(r["from"])).ToString(), Misc.GetUsernameByID(Convert.ToUInt16(r["to"])).ToString(), (long)Misc.DateTimeToUnixTimestamp(Convert.ToDateTime(r["date"].ToString())));
                    chats.Add(c);
                }
                r.Close();
                staticconn.Close();

                //var res = Database.EllappDB.Where<Log_Chat>(r => r.to_type == "CHAT_TYPE_USER_TO_USER" && (r.from == AccountID || r.to == AccountID));

                return chats;
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand("SELECT ChatID as 'chatroom', `from`, content, `to`, `date` FROM log_chat WHERE ChatID = @chatid AND to_type = 'CHAT_TYPE_USER_TO_USER' ORDER BY `date` ASC;", staticconn);
                MySqlParameter chatidParameter = new MySqlParameter("@chatid", MySqlDbType.String, 0);
                chatidParameter.Value = ChatRequestID;
                cmd.Parameters.Add(chatidParameter);
                MySqlDataReader r = cmd.ExecuteReader();
                while(r.Read())
                {
                    Chat c = new Chat(ChatType.CHAT_TYPE_USER_TO_USER, r["chatroom"].ToString(), r["content"].ToString(), Misc.GetUsernameByID(Convert.ToUInt16(r["from"])).ToString(), Misc.GetUsernameByID(Convert.ToUInt16(r["to"])).ToString(), (long)Misc.DateTimeToUnixTimestamp(Convert.ToDateTime(r["date"].ToString())));
                    chats.Add(c);
                }
                r.Close();
                staticconn.Close();
                return chats;
            }
        }

    }
}
