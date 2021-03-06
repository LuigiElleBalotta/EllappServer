﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EllappServer.definitions;
using MySql.Data.MySqlClient;

namespace EllappServer.Classes
{
    class MessagePacket
    {
        public MessageType MessageType;
        public string from;
        public uint to;
        public string data;

        public MessagePacket(MessageType _msgtype, uint _from, uint _to, string _data)
        {
            MessageType = _msgtype;
            from = GetAccountname(_from);
            to = _to;
            data = _data;
        }

        private string GetAccountname(uint id)
        {
            string _from = "";
            if (id != 0)
            {
                Config_Manager conf = new Config_Manager();
                MySqlConnection conn = new MySqlConnection("Server=" + conf.getValue("mysql_host") + ";Database=" + conf.getValue("mysql_db") + ";Uid=" + conf.getValue("mysql_user") + ";Pwd=" + conf.getValue("mysql_password") + ";");
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT username FROM accounts WHERE idAccount = @id;", conn);
                MySqlParameter idParameter = new MySqlParameter("@id", MySqlDbType.Int32, 0);
                idParameter.Value = id;
                cmd.Parameters.Add(idParameter);
                MySqlDataReader row = cmd.ExecuteReader();
                while (row.Read())
                {
                    from = row["username"].ToString();
                }
                row.Close();
                conn.Close();
            }
            else
                _from = "Server Message";
            return _from;
        }
    }
}
