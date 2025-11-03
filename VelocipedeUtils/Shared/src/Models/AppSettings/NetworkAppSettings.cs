namespace VelocipedeUtils.Shared.Models.AppSettings
{
    /// <summary>
    /// Class for storing confidurations related to the core server.
    /// </summary>
    public sealed class NetworkAppSettings
    {
        /// <summary>
        /// Network address of the server application.
        /// </summary>
        public required string ServerAddress { get; set; }

        /// <summary>
        /// Network address of the client application.
        /// </summary>
        public required string ClientAddress { get; set; }
        
        /// <summary>
        /// String representation of the environment (e.g. test, production).
        /// </summary>
        public required string Environment { get; set; }
        
        /// <summary>
        /// HTTP paths.
        /// </summary>
        public required string[] HttpPaths { get; set; }
        
        /// <summary>
        /// HTTP paths used for testing and debugging the server.
        /// </summary>
        public required string[] HttpPathsDbg { get; set; }

        /// <summary>
        /// Allows to print web paths which API server uses to listen to requests.
        /// </summary>
        public bool PrintWebPaths { get; set; }

        /// <summary>
        /// Allows to print debug information about processing HTTP requests.
        /// </summary>
        public bool PrintHttpRequestProcInfo { get; set; }
    }
}
