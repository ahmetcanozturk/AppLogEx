using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace LogEx
{
    /// <summary>
    /// log manager to record logs
    /// </summary>
    public class LogManager
    {
        private static LogManager instance;
        private static object synchronizationLock = new object();

        public LogManager()
        {
        }

        public static LogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (synchronizationLock)
                    {
                        if (instance == null)
                            instance = new LogManager();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// record the log
        /// </summary>
        /// <param name="logCode">log identifier</param>
        /// <param name="parameters">log parameters</param>
        /// <param name="controllerName">controller name of the log</param>
        /// <param name="actionName">action name of the log</param>
        /// <param name="user">user if there is any authentication</param>
        /// <param name="ip">ip of the client</param>
        /// <param name="agent">browser agent of the client</param>
        /// <returns>log definition</returns>
        public LogDefinition CreateLog(string logCode, ICollection<KeyValuePair<string, object>> parameters, string controllerName, string actionName, string user, string ip, string agent)
        {
            return createLog(logCode, parameters, controllerName, actionName, user, ip, agent);
        }

        /// <summary>
        /// construct the chain to handle the exception
        /// </summary>
        private LogDefinition createLog(string logCode, ICollection<KeyValuePair<string, object>> parameters, string controllerName, string actionName, string user, string ip, string agent)
        {
            LogHandler logHandler = new DatabaseLogHandler();
            LogHandler logHandlerJson = new JsonFileLogHandler();

            logHandler.SetSuccessor(logHandlerJson);

            var logparams = parameters.Select(p => new LogParameter() { Key = p.Key, Value = p.Value }).ToList();
            var definition = getLogDefinition(logCode);
            Log log = new Log() { Code = logCode, Type = definition.LogType, Time = DateTime.Now, Parameters = logparams, ControllerName = controllerName, ActionName = actionName, User = user, IP = ip, Agent = agent };

            logHandler.HandleLog(log);

            return definition;
        }

        private LogDefinition getLogDefinition(string logCode)
        {
            try
            {
                if (LogDefinitions.LogDefinitionList == null)
                {
                    string path = ConfigManager.Instance.LogDefinitionPath;
                    using (System.IO.TextReader streamReader = new System.IO.StreamReader(path))
                    {
                        string json = streamReader.ReadToEnd();
                        var definitions = JsonConvert.DeserializeObject<List<LogDefinition>>(json);
                        LogDefinitions.LogDefinitionList = definitions;
                    }
                }

                LogDefinition ld = LogDefinitions.LogDefinitionList.Where(l => l.LogID.Equals(logCode)).FirstOrDefault();
                if (ld != null)
                    return ld;
            }
            catch (System.Exception)
            {
                
            }
            return new LogDefinition() { LogID = logCode, LogType = "Information", Text = logCode };
        }
    }

    /// <summary>
    /// abstract log handler class
    /// </summary>
    abstract class LogHandler
    {
        protected LogHandler successor;

        internal void SetSuccessor(LogHandler successor)
        {
            this.successor = successor;
        }

        internal abstract void HandleLog(Log log);
    }

    /// <summary>
    /// concrete handler class DatabaseLogHandler
    /// </summary>
    class DatabaseLogHandler : LogHandler
    {
        internal override void HandleLog(Log log)
        {
            bool isHandled = false;
            if (isDataBaseExists())
                isHandled = handleLog(log);

            if (!isHandled && successor != null)
                successor.HandleLog(log);
        }

        private bool handleLog(Log log)
        {
            return DBRepository.Instance.InsertLogAsync(log).Result;
        }

        private bool isDataBaseExists()
        {
            return DBRepository.Instance.IsDatabaseExists();
        }
    }

    /// <summary>
    /// concrete handler class JsonFileLogHandler
    /// </summary>
    class JsonFileLogHandler : LogHandler
    {
        internal override void HandleLog(Log log)
        {
            bool isHandled = handleLog(log);
            if (!isHandled && successor != null)
                successor.HandleLog(log);
        }

        private bool handleLog(Log log)
        {
            try
            {
                string path = ConfigManager.Instance.LogFile;
                Directory.CreateDirectory(new FileInfo(path).DirectoryName);
                using (var streamWriter = new System.IO.StreamWriter(path, true))
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(streamWriter, log);
                    }
                }

                return true;
            }
            catch (System.Exception)
            {
            }
            return false;
        }
    }
}