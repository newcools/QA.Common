namespace Automation.Common.Testing
{
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IO;

    /// <summary>
    /// Defines global test settings.
    /// </summary>
    public class TestSettings
    {
        #region Static Fields

        /// <summary>
        /// The singleton instance of <see cref="TestSettings"/>.
        /// </summary>
        private static TestSettings instance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="TestSettings"/> class from being created.
        /// </summary>
        private TestSettings()
        {
            this.Initialize();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the drop root.
        /// </summary>
        public string DropRoot { get; set; }

        /// <summary>
        /// Gets or sets the max allowed consecutive failures.
        /// </summary>
        public int MaxAllowedConsecutiveFailures { get; set; }

        /// <summary>
        /// Gets or sets the think time multiplier.
        /// </summary>
        public double ThinkTimeMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the number of milliseconds to wait for the application to be ready.
        /// </summary>
        public int WaitForReadyTimeout { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        internal static TestSettings Instance
        {
            get
            {
                return instance ?? (instance = new TestSettings());
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// To initialize the instance properties.
        /// </summary>
        public void Initialize()
        {
            this.ApplySettings(ConfigurationManager.AppSettings);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Apply settings from app configuration file.
        /// </summary>
        /// <param name="appSettings">
        /// The app settings.
        /// </param>
        private void ApplySettings(NameValueCollection appSettings)
        {
            this.MaxAllowedConsecutiveFailures = SettingsUtils.GetConfigOptionValueInt("MaxAllowedConsecutiveFailures", 3, appSettings);
            this.WaitForReadyTimeout = SettingsUtils.GetConfigOptionValueInt("WaitForReadyTimeout", 60000, appSettings);
            this.ThinkTimeMultiplier = SettingsUtils.GetConfigOptionValueDouble("ThinktimeMultiplier", 1.0, appSettings);
            this.DropRoot = SettingsUtils.GetConfigOptionValue("DropRoot", Path.GetTempPath(), appSettings);
        }

        #endregion
    }
}