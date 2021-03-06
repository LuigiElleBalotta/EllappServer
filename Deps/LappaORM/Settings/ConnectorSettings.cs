﻿// Copyright (C) Arctium Software.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using LappaORM.Logging;
using LappaORM.Misc;

namespace LappaORM
{
    internal sealed class ConnectorSettings
    {
        public DatabaseType DatabaseType { get; private set; }
        string type;
        Assembly assembly;

        internal ConnectorSettings(DatabaseType dbType)
        {
            DatabaseType = dbType;

            if (dbType == DatabaseType.MySql)
            {
                var connectorPath = Environment.CurrentDirectory + "/MySql.Data.dll";

                if (!File.Exists(connectorPath))
                {
                    Helper.Log.Message(LogTypes.Error, $"{connectorPath} doesn't exist.");

                    return;
                }

                assembly = Assembly.LoadFile(connectorPath);
                type = "MySql.Data.MySqlClient.MySql";
            }
            else if (dbType == DatabaseType.MSSql)
            {
                assembly = Assembly.Load("System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                type = "System.Data.SqlClient.Sql";
            }
            else if (dbType == DatabaseType.SQLite)
            {
                var connectorPath = Environment.CurrentDirectory + "/System.Data.SQLite.dll";

                if (!File.Exists(connectorPath))
                {
                    Helper.Log.Message(LogTypes.Error, $"{connectorPath} doesn't exist.");

                    return;
                }

                assembly = Assembly.LoadFile(connectorPath);
                type = "System.Data.SQLite.SQLite";
            }
        }

        internal object CreateObject(string classPart)
        {
            return Activator.CreateInstance(assembly.GetType(type + classPart));
        }
    }
}
