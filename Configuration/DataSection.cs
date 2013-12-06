namespace Automation.Common.Configuration
{
    using System.Configuration;

    /// <summary>
    ///     Defines the section, which is a collection of table elements.
    /// </summary>
    public class DataSection : ConfigurationSection
    {
        #region Public Properties

        /// <summary>
        ///     Gets the tables.
        /// </summary>
        [ConfigurationProperty("Data", IsDefaultCollection = true)]
        public DataCollection Data
        {
            get
            {
                return (DataCollection)base["Data"];
            }
        }

        #endregion
    }
}