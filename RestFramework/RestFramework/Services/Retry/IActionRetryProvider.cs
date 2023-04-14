using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFramework.Services
{
    public interface IActionRetryProvider
    {
        void DoWithRetry(Action action, IEnumerable<Type> handledExceptions = null);
        T DoWithRetry<T>(Func<T> function, IEnumerable<Type> handledExceptions = null);
    }
}
