namespace Automation.Common.Configuration
{
    using System.Configuration;

    /// <summary>
	/// The assignable name value configuration element.
	/// </summary>
	public sealed class AssignableNameValue : ConfigurationElement
	{
		/// <summary>
		/// Allows assigning configuration values at run time.
		/// </summary>
		/// <returns>
		/// The <see cref="bool"/>.
		/// </returns>
		public override bool IsReadOnly()
		{
			return false;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[ConfigurationProperty("name")]
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
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[ConfigurationProperty("value", IsRequired = true)]
		public string Value
		{
			get
			{
				return (string)this["value"];
			}
			set
			{
				this["value"] = value;
			}
		}
	}
}
