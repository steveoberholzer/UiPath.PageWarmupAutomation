using WebApplicationWarmup.Workflows.Parsing.Models;

namespace WebApplicationWarmup.Workflows.Models
{
    public class WarmupResult 
    {
        public PageConfig PageConfig { get; private set; }
        public bool Success { get; private set; }
        public System.Exception Error { get; private set; }
        
        public WarmupResult(PageConfig pageConfig, bool success, System.Exception error)
        {
            PageConfig = pageConfig;
            Success = success;
            if (error != null) Error = error;
        }
        
        public WarmupResult(PageConfig pageConfig, bool success)
        {
            PageConfig = pageConfig;
            Success = success;
            Error = null;
        }
        
        public WarmupResult(PageConfig pageConfig)
        {
            PageConfig = pageConfig;
            Success = true;
            Error = null;
        }
    }
}