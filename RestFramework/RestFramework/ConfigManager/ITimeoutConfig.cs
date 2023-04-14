using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFramework.ConfigManager
{
    public interface ITimeoutConfig
    {
        TimeSpan ImplicitTimeout { get; }

        TimeSpan ConditionTimeout { get; }

        TimeSpan PollingIntervalTimeout { get; }

        TimeSpan CommandTimeout { get; }

        TimeSpan ScriptTimeout { get; }
        TimeSpan PageLoadTimeout { get; }
    }
}
