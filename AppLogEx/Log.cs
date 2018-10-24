using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LogEx
{
    /// <summary>
    /// log class
    /// </summary>
    internal class Log
    {
        [BsonElement]
        [JsonProperty()]
        internal DateTime Time { get; set; }
        [BsonElement]
        [JsonProperty()]
        internal string Code { get; set; }
        [BsonElement]
        [JsonProperty()]
        internal string Type { get; set; }
        [BsonElement]
        [JsonProperty()]
        internal ICollection<LogParameter> Parameters { get; set; }
        [BsonElement]
        [JsonProperty()]
        internal string ControllerName { get; set; }
        [BsonElement]
        [JsonProperty()]
        internal string ActionName { get; set; }
        [BsonElement]
        [JsonProperty()]
        internal string User { get; set; }
        [BsonElement]
        [JsonProperty()]
        internal string IP { get; set; }
        [BsonElement]
        [JsonProperty()]
        internal string Agent { get; set; }

        //private void setClientParameters()
        //{
        //    this.IP = HttpContext.Current.Request.UserHostAddress;
        //    this.Browser = HttpContext.Current.Request.Browser.Browser + " " + HttpContext.Current.Request.Browser.Version;
        //    this.Platform = HttpContext.Current.Request.Browser.Platform;
        //    this.Agent = HttpContext.Current.Request.UserAgent;
        //    this.User = getUser();
        //}

        //private string getUser()
        //{
        //    string strUsername = HttpContext.Current.User.Identity.Name;
        //    if (!string.IsNullOrEmpty(strUsername))
        //        strUsername = strUsername.Substring(strUsername.LastIndexOf("\\") + 1);
        //    return strUsername;
        //}
    }
}
