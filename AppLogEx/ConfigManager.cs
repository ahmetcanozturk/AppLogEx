using Microsoft.Extensions.Configuration;
using System.IO;

namespace LogEx
{
    /// <summary>
    /// configuration manager to read app settings configuration
    /// </summary>
    internal class ConfigManager
    {
        private static ConfigManager instance;
        private static object synchronizationLock = new object();
        private IConfigurationRoot configuration;

        internal ConfigManager()
        {
            var appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            configuration = new ConfigurationBuilder()
                .AddJsonFile("settings.json", optional: false, reloadOnChange: true).Build();
        }

        /// <summary>
        /// config manager instance
        /// </summary>
        internal static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (synchronizationLock)
                    {
                        if (instance == null)
                            instance = new ConfigManager();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// MongoDB connection string
        /// </summary>
        internal string ConnectionString
        {
            get => getConnectionString("mongodb://localhost:27017");
        }

        /// <summary>
        /// exceptions file path
        /// </summary>
        internal string ExceptionFile
        {
            get => getSettingFilePath("exceptionFile", "Temp\\Exceptions.json");
        }

        /// <summary>
        /// logs file path
        /// </summary>
        internal string LogFile
        {
            get => getSettingFilePath("logFile", "Temp\\Logs.json");
        }

        /// <summary>
        /// log definitions file path
        /// </summary>
        internal string LogDefinitionPath
        {
            get => getSettingFilePath("logDefinitions", "Resources\\LogDefinitions.json");
        }

        private string getSettingFilePath(string key, string defaultValue)
        {
            try
            {
                string pathRoot = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                return string.Format(@"{0}\{1}", pathRoot, getAppSetting(key, defaultValue));
            }
            catch
            {

            }
            return defaultValue;
        }

        private string getAppSetting(string key, string defaultValue)
        {
            try
            {
                string sectionkey = string.Format("appSettings:{0}", key);
                return configuration[sectionkey];
            }
            catch (System.Exception)
            {

            }
            return defaultValue;
        }

        private string getConnectionString(string defaultValue)
        {
            try
            {
                return configuration["connectionStrings:MongoContext"];
            }
            catch (System.Exception)
            {

            }
            return defaultValue;
        }
    }
}
