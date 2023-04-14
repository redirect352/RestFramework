using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFramework.Waits
{
    public interface IConditionalWait
    {
        T WaitFor<T>(Func<IWebDriver, T> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string message = null, IList<Type> exceptionsToIgnore = null);

        bool WaitFor(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, IList<Type> exceptionsToIgnore = null);

        Task<bool> WaitForAsync(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, IList<Type> exceptionsToIgnore = null);

        void WaitForTrue(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string message = null, IList<Type> exceptionsToIgnore = null);

        Task WaitForTrueAsync(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string message = null, IList<Type> exceptionsToIgnore = null);
    }
}
