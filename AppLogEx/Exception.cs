using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Security.Claims;

namespace LogEx
{
    /// <summary>
    /// exception class
    /// </summary>
    internal class Exception
    {
        [BsonElement]
        [JsonProperty()]
        internal DateTime ExceptionTime { get; set; }
        [BsonElement]
        [JsonProperty()]
        internal string TypeName { get; set; }
        [BsonElement]
        [JsonProperty()]
        internal string Message { get; set; }
        [BsonElement]
        [JsonProperty()]
        internal string StackTrace { get; set; }

        internal Exception(System.Exception exc)
        {
            if (exc.InnerException != null)
                exc = exc.InnerException;

            this.ExceptionTime = DateTime.Now;
            this.TypeName = exc.GetType().ToString();
            this.Message = exc.Message;
            this.StackTrace = exc.StackTrace;
        }
    }
}