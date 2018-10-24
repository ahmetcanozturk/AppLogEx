using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LogEx
{
    /// <summary>
    /// log definition class
    /// </summary>
    public class LogDefinition
    {
        /// <summary>
        /// log identifier
        /// </summary>
        public string LogID { get; set; }

        /// <summary>
        /// log type: Information, Warning, Error
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// log message
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// log definitions form logdefinitions.json
    /// </summary>
    internal class LogDefinitions
    {
        internal static List<LogDefinition> LogDefinitionList { get; set; }
    }
}