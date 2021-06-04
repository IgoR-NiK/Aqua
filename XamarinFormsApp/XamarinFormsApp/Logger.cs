using Aqua.Core.IoC;
using Aqua.Core.Utils;

namespace XamarinFormsApp
{
    [AsSingleInstance]
    public interface ILogger : IResolvable
    {
        void Log(string message);
    }
    
    [Order(10)]
    public class XmlLogger : ILogger
    {
        public void Log(string message)
        {
            throw new System.NotImplementedException();
        }
    }
    
    [Order(1)]
    public class JsonLogger : ILogger
    {
        public void Log(string message)
        {
            throw new System.NotImplementedException();
        }
    }
    
    [Order(5)]
    public class TxtLogger : ILogger
    {
        public void Log(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}