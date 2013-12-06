namespace Automation.Common.Configuration
{
    using System.Configuration;

    /// <summary>
    ///     Defines the properties for the Table Element.
    /// </summary>
    public class DataElement : ConfigurationElement
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        /// <value>
        ///     The connection string.
        /// </value>
        [ConfigurationProperty("connectionString", IsRequired = false)]
        public AssignableNameValue ConnectionString
        {
            get
            {
                return (AssignableNameValue)this["connectionString"];
            }

            set
            {
                this["connectionString"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the data file.
        /// </summary>
        /// <remarks>
        /// The data file is the file you either get data from, or save your data to it.
        /// </remarks>
        [ConfigurationProperty("dataFile", IsRequired = false)]
        public string DataFile
        {
            get
            {
                return (string)this["dataFile"];
            }

            set
            {
                this["dataFile"] = value;
            }
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }

            set
            {
                this["name"] = value;
            }
        }

        /// <summary>
        ///     Gets or sets the query. The query defines what information to get from database.
        /// </summary>
        /// <value>
        ///     The query.
        /// </value>
        [ConfigurationProperty("query", IsRequired = true)]
        public NameValueConfigurationElement Query
        {
            get
            {
                return (NameValueConfigurationElement)this["query"];
            }

            set
            {
                this["query"] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Allows assigning configuration values at run time.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public override bool IsReadOnly()
        {
            return false;
        }

        #endregion
    }
}