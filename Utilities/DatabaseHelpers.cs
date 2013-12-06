namespace Automation.Common.Utilities
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;

    /// <summary>
    /// The database helpers.
    /// </summary>
    public static class DatabaseHelpers
    {
        #region Public Methods and Operators

        /// <summary>
        /// Backups the Database.
        /// </summary>
        /// <param name="sqlConnection">
        /// The SQL connection of the database to be backup.
        /// </param>
        /// <param name="destinationPath">
        /// The backup file path.
        /// </param>
        public static void BackupDatabase(SqlConnection sqlConnection, string destinationPath)
        {
            if (sqlConnection == null)
                throw new ArgumentNullException("sqlConnection", "The sql connection cannot be null.");
            if (string.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentException("A valid back up file name is required.");

            if (Path.IsPathRooted(destinationPath))
            {
                string directoryName = destinationPath.Substring(0, destinationPath.LastIndexOf('\\'));
                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);
            }

            var serverConnection = new ServerConnection(sqlConnection);
            var server = new Server(serverConnection);

            var backup =
                new Backup
                    {
                        Action = BackupActionType.Database,
                        BackupSetDescription = string.Format("ArchiveDataBase: {0}", DateTime.Now.ToShortDateString()),
                        BackupSetName = sqlConnection.Database,
                        Database = sqlConnection.Database,
                        Initialize = true,
                        Checksum = true,
                        ContinueAfterError = true,
                        Incremental = false
                    };

            var deviceItem = new BackupDeviceItem(destinationPath, DeviceType.File);
            backup.Devices.Add(deviceItem);

            backup.SqlBackup(server);

            backup.Devices.Remove(deviceItem);
            serverConnection.Disconnect();
        }

        /// <summary>
        /// Backups the Database.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string of the database to be backup.
        /// </param>
        /// <param name="destinationPath">
        /// The backup file path.
        /// </param>
        public static void BackupDatabase(string connectionString, string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("A valid sql connection is required. It cannot be null or white spaces.");
            if (string.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentException("A valid back up file path is required. It cannot be null or white spaces.");

            var builder = new SqlConnectionStringBuilder(connectionString);
            using (var connection = new SqlConnection(builder.ConnectionString))
                BackupDatabase(connection, destinationPath);
        }

        /// <summary>
        /// Drop data base.
        /// </summary>
        /// <param name="dbConnectionString">
        /// The database connection string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string DropDatabase(string dbConnectionString)
        {
            string message = String.Empty;
            var serverstring = new SqlConnectionStringBuilder(dbConnectionString)
                                   {
                                       InitialCatalog = "master"
                                   };
            var connectionstring = new SqlConnectionStringBuilder(dbConnectionString);
            string dbName = connectionstring.InitialCatalog;
            string cmdText = String.Format("Select name from master.dbo.sysdatabases where name='{0}'", dbName);
            try
            {
                string[] rows = ExecuteSqlQueries(serverstring.ConnectionString, cmdText);

                if (rows.Length > 0)
                {
                    using (var connection = new SqlConnection(dbConnectionString))
                    {
                        string script =
                            String.Format(
                                "USE master;\r\nGO\r\nALTER DATABASE {0}\r\nSET SINGLE_USER \r\nWITH ROLLBACK IMMEDIATE;\r\nGO\r\nDROP DATABASE {0};\r\n\r\n",
                                dbName);
                        var server = new Server(new ServerConnection(connection));
                        server.ConnectionContext.ExecuteNonQuery(script);
                    }
                }
            }
            catch (Exception e)
            {
                message += e.Message;
            }
            return message;
        }

        /// <summary>
        /// Executes the SQL file.
        /// </summary>
        /// <param name="dbConnectionString">
        /// The database connection string.
        /// </param>
        /// <param name="sqlFile">
        /// The SQL file.
        /// </param>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        public static int ExecuteSqlFile(string dbConnectionString, string sqlFile)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                var file = new FileInfo(sqlFile);

                connection.Open();
                string script = file.OpenText().ReadToEnd();
                using (var command = new SqlCommand(script, connection))
                {
                    command.CommandTimeout = 1800;
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executes  the specified query.
        /// </summary>
        /// <param name="connectionString">
        /// The database connection string.
        /// </param>
        /// <param name="querystring">
        /// The sql query string.
        /// </param>
        /// <returns>
        /// A string array will be returned, and each string stores column values
        ///     for each row in the query results and the value of individual column is separated by a semicolon
        /// </returns>
        public static string[] ExecuteSqlQueries(string connectionString, string querystring)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(querystring, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var dataList = new ArrayList();
                    while (reader.Read())
                    {
                        string data = Convert.ToString(reader.GetValue(0));
                        for (int j = 1; j < reader.FieldCount; j++)
                            data = data + ";" + Convert.ToString(reader.GetValue(j));
                        dataList.Add(data);
                    }
                    return dataList
                        .Cast<string>()
                        .ToArray();
                }
            }
        }

        /// <summary>
        /// Ensure that the specified value either represents the name of a SQL server connection string.
        /// </summary>
        /// <param name="nameOrConnectionString">
        /// The connection string name or connection string.
        /// </param>
        /// <returns>
        /// The connection string.
        /// </returns>
        public static string GetConnectionString(string nameOrConnectionString)
        {
            if (string.IsNullOrWhiteSpace(nameOrConnectionString))
                throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'nameOrConnectionString'.", "nameOrConnectionString");

            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[nameOrConnectionString];
            string connectionString = connectionStringSettings != null ?
                                          connectionStringSettings.ConnectionString : nameOrConnectionString;
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            return sqlConnectionStringBuilder.ConnectionString;
        }

        /// <summary>
        /// Get the Data table that filled with data retrieved from database by executing the SQL query.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection String Settings.
        /// </param>
        /// <param name="query">
        /// The prepared statement.
        /// </param>
        /// <param name="sqlParameters">
        /// The sql Parameters.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        public static DataTable GetTableResults(ConnectionStringSettings connectionStringSettings, string query, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new InvalidArgumentException("SQL query cannot be null or white spaces.");
            if (connectionStringSettings == null)
                throw new ArgumentNullException("connectionStringSettings", "Connection string settings cannot be null.");

            DataTable dataResult;
            DataTable dataTable = default(DataTable);

            try
            {
                DbProviderFactory provider = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);

                using (DbConnection connection = provider.CreateConnection())
                {
                    Debug.Assert(connection != null, "connection != null");
                    connection.ConnectionString = connectionStringSettings.ConnectionString;
                    connection.Open();
                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;
                        if (sqlParameters != null)
                        {
                            foreach (SqlParameter sqlParameter in sqlParameters)
                                command.Parameters.Add(sqlParameter);
                            command.Prepare();
                        }

                        // SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int, 0);
                        // idParam.Value = 20;
                        // command.Parameters.Add(idParam);
                        using (DbDataAdapter dataAdapter = provider.CreateDataAdapter())
                        {
                            Debug.Assert(dataAdapter != null, "dataAdapter != null");
                            dataAdapter.SelectCommand = command;
                            dataTable = new DataTable();
                            dataAdapter.Fill(dataTable);
                            dataResult = dataTable;
                            dataTable = null;
                        }
                    }
                }
            }
            finally
            {
                if (dataTable != null)
                    dataTable.Dispose();
            }
            return dataResult;
        }

        /// <summary>
        /// Get the Data table that filled with data retrieved from database by executing the SQL query.
        /// </summary>
        /// <param name="connectionString">
        /// The SQL database connection string.
        /// </param>
        /// <param name="query">
        /// The prepared statement.
        /// </param>
        /// <param name="sqlParameters">
        /// The sql Parameters.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        public static DataTable GetTableResults(string connectionString, string query, params SqlParameter[] sqlParameters)
        {
            ConnectionStringSettings settings = new ConnectionStringSettings("sqlDB", connectionString, "System.Data.SqlClient");
            return GetTableResults(settings, query, sqlParameters);
        }

        /// <summary>
        /// Restores a Database from backup file.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string provides the information for the database to be restored.
        /// </param>
        /// <param name="restoreFrom">
        /// The backup file path.
        /// </param>
        public static void RestoreDatabase(string connectionString, string restoreFrom)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("A valid sql connection is required. It cannot be null or white spaces.");
            if (string.IsNullOrWhiteSpace(restoreFrom))
                throw new ArgumentException("A valid back up file name is required. It cannot be null or white spaces.");

            var sqlConnectionString = new SqlConnectionStringBuilder(connectionString);
            string restoreDatabaseName = sqlConnectionString.InitialCatalog;

            sqlConnectionString.InitialCatalog = "master";
            using (var masterConnection = new SqlConnection(sqlConnectionString.ConnectionString))
            {
                var serverConnection = new ServerConnection(masterConnection);
                var server = new Server(serverConnection);

                Database restoreDatabase = server.Databases[restoreDatabaseName];
                if (restoreDatabase != null)
                {
                    // server.KillAllProcesses(restoreDatabaseName);
                    server.KillDatabase(restoreDatabaseName);
                }

                var restore = new Restore
                                  {
                                      NoRecovery = false,
                                      ReplaceDatabase = true,
                                      Action = RestoreActionType.Database,
                                      Database = restoreDatabaseName,
                                      PercentCompleteNotification = 10
                                  };

                var backupDeviceItem = new BackupDeviceItem(restoreFrom, DeviceType.File);
                restore.Devices.Add(backupDeviceItem);

                restore.SqlRestore(server);

                restore.Devices.Remove(backupDeviceItem);
                server.Refresh();
                serverConnection.Disconnect();
            }
        }

        /// <summary>
        /// Saves the table data.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="file">
        /// The file.
        /// </param>
        public static void SaveTableData(string connectionString, string query, string file)
        {
            DataTable table = GetTableResults(connectionString, query);

            var sb = new StringBuilder();
            int tableColumnCount = table.Columns.Count;
            int count = 0;
            foreach (DataColumn column in table.Columns)
            {
                sb.Append(column.ColumnName);
                if (count++ < (tableColumnCount - 1))
                    sb.Append("|");
            }
            sb.AppendLine();
            count = 0;
            foreach (DataRow row in table.Select())
            {
                foreach (DataColumn column in table.Columns)
                {
                    sb.Append(Convert.ToString(row[column.ColumnName]));

                    if (count++ < (tableColumnCount - 1))
                        sb.Append("|");
                }
                sb.AppendLine();
                count = 0;
            }
            File.WriteAllText(file, sb.ToString());
        }

        #endregion
    }
}
