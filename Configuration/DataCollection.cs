namespace Automation.Common.Configuration
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Define the table collection, which inherits ConfigurationElementCollection.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Justification = "Need to implement ConfigurationElementCollection.")]
    public class DataCollection : ConfigurationElementCollection
    {
        #region Public Properties

        /// <summary>
        /// Gets the type of the <see cref="T:System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Configuration.ConfigurationElementCollectionType"/> of this collection.
        /// </returns>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        /// Return an element via the index.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="DataElement"/>.
        /// </returns>
        public DataElement this[int index]
        {
            get
            {
                return (DataElement)this.BaseGet(index);
            }
            set
            {
                if (this.BaseGet(index) != null)
                    this.BaseRemoveAt(index);
                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Gets or sets a property, attribute, or child element of this configuration element.
        /// </summary>
        /// <param name="name">
        /// The name to be used for locating an element.
        /// </param>
        /// <returns>
        /// The specified property, attribute, or child element
        /// </returns>
        public new DataElement this[string name]
        {
            get
            {
                return (DataElement)this.BaseGet(name);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new DataElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">
        /// The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified
        ///     <see cref="T:System.Configuration.ConfigurationElement"/>
        ///     .
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DataElement)element).Name;
        }

        #endregion
    }
}