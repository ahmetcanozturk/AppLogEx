using Newtonsoft.Json;
using System.IO;

namespace LogEx
{
    /// <summary>
    /// exception manager to handle exceptions
    /// </summary>
    public class ExceptionManager
    {
        private static ExceptionManager instance;
        private static object synchronizationLock = new object();

        public ExceptionManager()
        {
        }

        /// <summary>
        /// exception manager instance
        /// </summary>
        public static ExceptionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (synchronizationLock)
                    {
                        if (instance == null)
                            instance = new ExceptionManager();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// handle the exception
        /// </summary>
        /// <param name="exception">exception</param>
        public void CatchException(System.Exception exception)
        {
            catchException(exception);
        }

        /// <summary>
        /// construct the chain to handle the exception
        /// </summary>
        /// <param name="ex">system exception</param>
        private void catchException(System.Exception ex)
        {
            ExceptionHandler exceptionHandler = new DatabaseHandler();
            ExceptionHandler exceptionHandlerJson = new JsonFileHandler();

            exceptionHandler.SetSuccessor(exceptionHandlerJson);

            Exception exception = new Exception(ex);

            exceptionHandler.HandleException(exception);
        }
    }

    /// <summary>
    /// abstract exception handler class
    /// </summary>
    abstract class ExceptionHandler
    {
        protected ExceptionHandler successor;

        internal void SetSuccessor(ExceptionHandler successor)
        {
            this.successor = successor;
        }

        internal abstract void HandleException(Exception exception);
    }

    /// <summary>
    /// concrete handler class DatabaseHandler
    /// </summary>
    class DatabaseHandler : ExceptionHandler
    {
        internal override void HandleException(Exception exception)
        {
            bool isHandled = false;
            if (isDataBaseExists())
                isHandled = handleException(exception);

            if (!isHandled && successor != null)
                successor.HandleException(exception);
        }

        private bool handleException(Exception exception)
        {
            return DBRepository.Instance.InsertExceptionAsync(exception).Result;
        }

        private bool isDataBaseExists()
        {
            return DBRepository.Instance.IsDatabaseExists();
        }
    }

    /// <summary>
    /// concrete handler class JsonFileHandler
    /// </summary>
    class JsonFileHandler : ExceptionHandler
    {
        internal override void HandleException(Exception exception)
        {
            bool isHandled = handleException(exception);
            if (!isHandled && successor != null)
                successor.HandleException(exception);
        }

        private bool handleException(Exception exception)
        {
            try
            {
                string path = ConfigManager.Instance.ExceptionFile;
                Directory.CreateDirectory(new FileInfo(path).DirectoryName);
                using (var streamWriter = new System.IO.StreamWriter(path, true))
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(streamWriter, exception);
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