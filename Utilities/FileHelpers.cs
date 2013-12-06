namespace Automation.Common.Utilities
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///     Define method to read or write files.
    /// </summary>
    public static class FileHelpers
    {
        #region Public Methods and Operators

        /// <summary>
        /// Back up file to the back up folder.
        /// </summary>
        /// <param name="path">
        /// The current file path.
        /// </param>
        /// <param name="file">
        /// The file to be backed up.
        /// </param>
        /// <param name="backupFolder">
        /// The backup folder, where you backup your file.
        /// </param>
        /// <returns>
        /// Returns true if back up completes successfully.
        /// </returns>
        public static bool Backup(string path, string file, string backupFolder = "bak")
        {
            try
            {
                string sourceFileFullName = Path.Combine(path, file);
                if (!Path.IsPathRooted(backupFolder))
                {
                    backupFolder = Path.Combine(path, backupFolder);
                }

                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                string destinationFileName = Path.Combine(backupFolder, file);
                File.Copy(sourceFileFullName, destinationFileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Copy items from source directory to destination directory.
        /// </summary>
        /// <param name="source">
        /// The source directory.
        /// </param>
        /// <returns>
        /// The destination directory name <see cref="string"/>.
        /// </returns>
        public static string CopyAll(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException("Source directory cannot be null or empty.", "source");
            }

            if (!Directory.Exists(source))
            {
                throw new DirectoryNotFoundException("Source direcory was not found.");
            }

            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(source);
            string copyDirectoryName = string.Format("Copy - {0}", sourceDirectoryInfo.Name);
            DirectoryInfo parentDirectory = sourceDirectoryInfo.Parent;
            string copyDirectoryFullName = Path.Combine(parentDirectory.FullName, copyDirectoryName);
            if (Directory.Exists(copyDirectoryFullName))
            {
                Directory.Delete(copyDirectoryFullName, true);
            }

            DirectoryInfo destinationDirectoryInfo = parentDirectory.CreateSubdirectory(copyDirectoryName);

            CopyAll(sourceDirectoryInfo, destinationDirectoryInfo);
            return copyDirectoryFullName;
        }

        /// <summary>
        /// Copy items from source directory to destination directory.
        /// </summary>
        /// <param name="source">
        /// The source directory.
        /// </param>
        /// <param name="destination">
        /// The destination directory.
        /// </param>
        public static void CopyAll(string source, string destination)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException("Source directory cannot be null or empty.", "source");
            }

            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentException("Destination directory cannot be null or empty.", "destination");
            }

            if (!Directory.Exists(source))
            {
                throw new DirectoryNotFoundException("Source direcory was not found.");
            }

            if (!Directory.Exists(destination))
            {
                throw new DirectoryNotFoundException("Destination direcory was not found.");
            }

            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(source);
            DirectoryInfo destinationDirectoryInfo = new DirectoryInfo(destination);
            CopyAll(sourceDirectoryInfo, destinationDirectoryInfo);
        }

        /// <summary>
        /// Copy items from source directory to destination directory.
        /// </summary>
        /// <param name="source">
        /// The source directory.
        /// </param>
        /// <param name="destination">
        /// The destination directory.
        /// </param>
        public static void CopyAll(DirectoryInfo source, DirectoryInfo destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "Source directory needs to be specified.");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination", "Target directory needs to be specified.");
            }

            if (source.FullName.Equals(destination.FullName, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            // Check if the target directory exists, if not, create it.
            if (!Directory.Exists(destination.FullName))
            {
                Directory.CreateDirectory(destination.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(destination.ToString(), file.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo sourceSubDirectory in source.GetDirectories())
            {
                DirectoryInfo targetSubDirectory = destination.CreateSubdirectory(sourceSubDirectory.Name);
                CopyAll(sourceSubDirectory, targetSubDirectory);
            }
        }

        /// <summary>
        /// Get data from file and store in a data table object.
        /// </summary>
        /// <param name="fileFullName">
        /// The source file full name.
        /// </param>
        /// <param name="separator">
        /// The separator used to get columns delimited.
        /// </param>
        /// <param name="quoted">
        /// True if the separated strings are double-quoted. Default is true.
        /// </param>
        /// <returns>
        /// The System.Data.DataTable.
        /// </returns>
        public static DataTable GetDataFromFile(string fileFullName, string separator = "|", bool quoted = true)
        {
            DataTable dataTableResult;
            DataTable dataTable = default(DataTable);

            try
            {
                dataTable = new DataTable();
                string[] rows = File.ReadAllLines(fileFullName);

                string[] columns = rows[0].Split(new[] { separator }, StringSplitOptions.None);

                string[][] dataRows =
                    rows.Skip(1)
                        .Select(
                            row =>
                            {
                                string trimmedRow = row.Trim('"');
                                string quotedSeparator = quoted ? string.Format("\"{0}\"", separator) : separator;
                                return trimmedRow.Split(new[] { quotedSeparator }, StringSplitOptions.None);
                            })
                        .ToArray();

                foreach (string column in columns)
                {
                    dataTable.Columns.Add(column, typeof(string));
                }

                foreach (string[] r in dataRows)
                {
                    DataRow row = dataTable.NewRow();

                    if (dataTable.Columns.Count != r.Length)
                    {
                        throw new InvalidDataException("Number of columns in test data is incorrect.");
                    }

                    for (int i = 0; i < r.Length; i++)
                    {
                        row[i] = r[i];
                    }

                    dataTable.Rows.Add(row);
                }

                dataTableResult = dataTable;
                dataTable = null;
            }
            finally
            {
                if (dataTable != null)
                {
                    dataTable.Dispose();
                }
            }

            return dataTableResult;
        }

        /// <summary>
        /// Restore file from the back up folder to the current path.
        /// </summary>
        /// <param name="path">
        /// The current path.
        /// </param>
        /// <param name="file">
        /// The file needs to be restored.
        /// </param>
        /// <param name="backupFolder">
        /// The backup folder from where to restore.
        /// </param>
        /// <returns>
        /// Returns true if restore completes successfully.
        /// </returns>
        public static bool RestoreFile(string path, string file, string backupFolder = "bak")
        {
            try
            {
                string restoreToFile = Path.Combine(path, file);
                if (!Path.IsPathRooted(backupFolder))
                {
                    backupFolder = Path.Combine(path, backupFolder);
                }

                string restoreFromFile = Path.Combine(backupFolder, file);
                if (File.Exists(restoreFromFile))
                {
                    File.Copy(restoreFromFile, restoreToFile, true);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Save data table to a file.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <param name="quoted">
        /// The quoted.
        /// </param>
        public static void SaveDataTable(DataTable data, string fileName, string separator = ",", bool quoted = true)
        {
            if (data == null)
            {
                return;
            }

            DataColumnCollection columns = data.Columns;
            StringBuilder stringBuilder = new StringBuilder();

            int columnCount = columns.Count;
            for (int i = 0; i < columnCount; i++)
            {
                string columnName = columns[i].ColumnName;
                stringBuilder.AppendFormat("{0}{1}", columnName, i == (columnCount - 1) ? string.Empty : separator);
            }

            DataRow[] rows = data.Select();
            foreach (DataRow dataRow in rows)
            {
                if (dataRow == null)
                {
                    continue;
                }

                stringBuilder.AppendLine();
                stringBuilder.Append(dataRow.DelimitedRow(separator, quoted));
            }

            File.WriteAllText(fileName, stringBuilder.ToString());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the delimited row with separator and quote mark if required.
        /// </summary>
        /// <param name="row">
        /// The data row.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <param name="quoted">
        /// The quoted.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string DelimitedRow(this DataRow row, string separator = ",", bool quoted = true)
        {
            if (row == null)
            {
                return string.Empty;
            }

            object[] columnData = row.ItemArray;
            string delimitedString = columnData.Aggregate(
                string.Empty,
                (result, current) =>
                string.Concat(result, StringHelper.QuoteAndDelimite(current.ToString(), quoted, separator: separator)));

            return delimitedString.TrimEnd(separator.ToCharArray());
        }

        #endregion
    }
}