using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace LogEx
{
    /// <summary>
    /// database repository
    /// </summary>
    internal class DBRepository
    {
        private static DBRepository instance;
        private static object synchronizationLock = new object();
        private IMongoDatabase db;

        internal DBRepository()
        {
        }

        public static DBRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (synchronizationLock)
                    {
                        if (instance == null)
                            instance = new DBRepository();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// check if database exists
        /// </summary>
        /// <returns></returns>
        public bool IsDatabaseExists()
        {
            db = connect();
            return (db != null);
        }

        /// <summary>
        /// insert exception
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>is insert successfull</returns>
        public async Task<bool> InsertExceptionAsync(Exception exception)
        {
            return await insertExceptionAsync(exception);
        }

        /// <summary>
        /// insert log
        /// </summary>
        /// <param name="log">Log</param>
        /// <returns>is insert successfull</returns>
        public async Task<bool> InsertLogAsync(Log log)
        {
            return await insertLogAsync(log);
        }

        private async Task<bool> insertExceptionAsync(Exception exception)
        {
            try
            {
                var collection = db.GetCollection<BsonDocument>("Exceptions");
                var doc = exception.ToBsonDocument();
                await collection.InsertOneAsync(doc);
                return true;
            }
            catch (System.Exception)
            {
            }
            return false;
        }

        private async Task<bool> insertLogAsync(Log log)
        {
            try
            {
                var collection = db.GetCollection<BsonDocument>("Logs");
                var doc = log.ToBsonDocument();
                await collection.InsertOneAsync(doc);
                return true;
            }
            catch (System.Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// connect to MongoDB database
        /// </summary>
        /// <returns></returns>
        private IMongoDatabase connect()
        {
            try
            {
                MongoClient client = new MongoClient(ConfigManager.Instance.ConnectionString);
                //update the state to check connection
                var databases = client.ListDatabases();

                return client.GetDatabase("LogExDB");
            }
            catch(System.Exception)
            {
            }
            return null;
        }
    }
}
