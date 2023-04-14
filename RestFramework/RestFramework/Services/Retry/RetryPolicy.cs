using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFramework.Services
{
    public class RetryPolicy : IRetryPolicy
    {
        public int RetryCount { get; }
        public TimeSpan PollingInterval { get; }
        public RetryPolicy(int count, TimeSpan interval) 
        {
            RetryCount = count;
            PollingInterval = interval;
        }
    }
}
