namespace VelocipedeUtils.Shared.Models.Network
{
    /// <summary>
    /// Email configuration.
    /// </summary>
    public class EmailConfiguration
    {
        /// <summary>
        /// From.
        /// </summary>
        public required string From { get; set; }
        
        /// <summary>
        /// SMTP server.
        /// </summary>
        public required string SmtpServer { get; set; }
        
        /// <summary>
        /// Port.
        /// </summary>
        public int Port { get; set; }
        
        /// <summary>
        /// User name.
        /// </summary>
        public required string UserName { get; set; }
        
        /// <summary>
        /// Password.
        /// </summary>
        public required string Password { get; set; }
    }
}