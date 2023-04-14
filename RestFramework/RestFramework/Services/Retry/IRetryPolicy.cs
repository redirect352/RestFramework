using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFramework.Services
{
    public interface IRetryPolicy
    {
        public int RetryCount { get; }
        public TimeSpan PollingInterval { get; }
    }
}
