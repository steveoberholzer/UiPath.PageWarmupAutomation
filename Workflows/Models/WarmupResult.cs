using K2Warmup.Workflows.Parsing.Models;

namespace K2Warmup.Workflows.Models
{
    public class WarmupResult
    {
        public PageConfig PageConfig { get; set; }
        public bool Success { get; private set; }
        public System.Exception Error { get; private set; }
        public System.DateTime StartTime { get; private set; }
        public System.DateTime EndTime { get; private set; }
        public System.TimeSpan Duration { get { return EndTime - StartTime; }}

        public WarmupResult()
        {
            Success = false;
            Error = null;
            StartTime = System.DateTime.Now;
        }
        
        public void SaveEndTime()
        {
            Success = true;
            EndTime = System.DateTime.Now;
        }
        
        public void SaveEndTimeWithError(System.Exception error)
        {
            Success = false;
            Error = error;   
        }
    }
}