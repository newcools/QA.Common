namespace Automation.Common.Testing
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.Web.Configuration;

    /// <summary>
    ///     The configuration helper.
    /// </summary>
    public static class ConfigurationHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the value for the specified app setting key.
        /// </summary>
        /// <param name="key">
        /// The app setting key.
        /// </param>
        /// <returns>
        /// The app setting value.
        /// </returns>
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// To get the application setting value from web configuration file.
        /// </summary>
        /// <param name="key">
        /// The key name of the application setting element.
        /// </param>
        /// <param name="virtualPath">
        /// The virtual path of the application to retrieve information from.
        /// </param>
        /// <param name="site">
        /// The site name where the application installed.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetWebConfiguration(string key, string virtualPath, string site = "Default Web Site")
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(virtualPath, site);
            KeyValueConfigurationElement configuration = config.AppSettings.Settings[key];

            return configuration != null ? configuration.Value : null;
        }

        /// <summary>
        /// Sets the app setting.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetAppSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationElement mappingDataPathSetting = config.AppSettings.Settings[key];

            if (mappingDataPathSetting == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                mappingDataPathSetting.Value = value;
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }

        /// <summary>
        /// Sets the web configuration appSettings section.
        /// </summary>
        /// <param name="key">
        /// The appSettings key.
        /// </param>
        /// <param name="value">
        /// The appSettings value to be set.
        /// </param>
        /// <param name="virtualPath">
        /// The virtual path of the web application.
        /// </param>
        /// <param name="site">
        /// Optional parameter. The site of the web application.
        /// </param>
        public static void SetWebConfiguration(string key, string value, string virtualPath, string site = "Default Web Site")
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(virtualPath, site);

            KeyValueConfigurationElement mappingDataPathSetting = config.AppSettings.Settings[key];

            if (mappingDataPathSetting == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                mappingDataPathSetting.Value = value;
            }

            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// Get app setting value from <paramref name="appSettings"/> based on the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="appSettings">
        /// The app settings.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// The value type.
        /// </typeparam>
        /// <returns>
        /// Return true if successfully retrieves setting from the <paramref name="appSettings"/>. Otherwise false.
        /// </returns>
        public static bool GetValue<T>(NameValueCollection appSettings, string key, out T value)
        {
            if (appSettings == null)
                throw new ArgumentNullException("appSettings", "App settings cannot be null.");
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("A valid configuration key needs to be specified.", "key");
            
            bool success = false;
            value = default(T);
            string valueFromAppSettings = appSettings[key];
            if (string.IsNullOrEmpty(valueFromAppSettings))
            {
                return false;
            }

            try
            {
                value = (T)Convert.ChangeType(valueFromAppSettings, typeof(T), CultureInfo.CurrentCulture);
                success = true;
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine("Failed to set following option. Option {0}, Value {1}", (object)key, (object)valueFromAppSettings);
                Debug.WriteLine(ex);
            }
            catch (FormatException ex)
            {
                Debug.WriteLine("Failed to set following option. Option {0}, Value {1}", (object)key, (object)valueFromAppSettings);
                Debug.WriteLine(ex);
            }
            return success;
        }
        #endregion
    }
}