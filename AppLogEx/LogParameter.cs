using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace LogEx
{
    /// <summary>
    /// log parameter
    /// </summary>
    internal class LogParameter
    {
        /// <summary>
        /// key of the parameter
        /// </summary>
        [BsonElement]
        [JsonProperty()]
        internal string Key { get; set; }

        /// <summary>
        /// value of the parameter
        /// </summary>
        [BsonElement]
        [JsonProperty()]
        internal object Value { get; set; }
    }
}
