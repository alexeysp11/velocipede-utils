using System.Net;
using System.Threading;

namespace VelocipedeUtils.NetworkAPIs
{
    /// <summary>
    /// HTTP server.
    /// </summary>
    public class HttpServerWF
    {
        /// <summary>
        /// Reference to the method that adds Uniform Resource Identifier (URI) prefixes for HttpListener object.
        /// </summary>
        private System.Action<HttpListener> AddPrefixes { get; set; }

        /// <summary>
        /// Reference to the method that handles HTTP request.
        /// </summary>
        private System.Action<HttpListenerContext> ProcessRequest { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public HttpServerWF(
            System.Action<HttpListener> addPrefixes, 
            System.Action<HttpListenerContext> processRequest)
        {
            AddPrefixes = addPrefixes;
            ProcessRequest = processRequest;
        }

        /// <summary>
        /// Create web server as HttpListener.
        /// </summary>
        public void CreateWebServer()
        {
            // Start HttpListener 
            HttpListener listener = new HttpListener();
            AddPrefixes(listener);
            listener.Start();

            // Start the thread 
            new Thread(() => 
            {
                while (true)
                {
                    HttpListenerContext ctx = listener.GetContext();
                    ThreadPool.QueueUserWorkItem((_) => ProcessRequest(ctx));
                }
            }).Start();
        }
    }
}