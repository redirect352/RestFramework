using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using RestFramework.ConfigManager;
using RestFramework.Services.Browser;

namespace RestFramework.Waits
{
    public class ConditionalWait: IConditionalWait
    {
        private readonly ITimeoutConfig timeoutConfiguration;
        

        public ConditionalWait(ITimeoutConfig timeoutConfiguration)
        {
            this.timeoutConfiguration = timeoutConfiguration;
        }

        public T WaitFor<T>(Func<IWebDriver, T> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string message = null, IList<Type> exceptionsToIgnore = null)
        {

            Browser browser = BrowserService.Browser;
            browser.SetImplicitWaitTimeout(TimeSpan.Zero);
            TimeSpan timeout2 = ResolveConditionTimeout(timeout);
            TimeSpan pollingInterval2 = ResolvePollingInterval(pollingInterval);
            WebDriverWait webDriverWait = new WebDriverWait(browser.Driver, timeout2);
            webDriverWait.Message = message;
            webDriverWait.PollingInterval = pollingInterval2;
            IList<Type> source = exceptionsToIgnore ?? new List<Type> { typeof(StaleElementReferenceException) };
            webDriverWait.IgnoreExceptionTypes(source.ToArray());
            T result = webDriverWait.Until(condition);
            browser.SetImplicitWaitTimeout(timeoutConfiguration.ImplicitTimeout);
            return result;
        }

        public bool WaitFor(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, IList<Type> exceptionsToIgnore = null)
        {
            return IsConditionSatisfied(delegate
            {
                WaitForTrue(condition, timeout, pollingInterval, null, exceptionsToIgnore);
                return true;
            }, new List<Type> { typeof(TimeoutException) });
        }

        public Task<bool> WaitForAsync(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, IList<Type> exceptionsToIgnore = null)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition", "condition cannot be null");
            }

            TimeSpan waitTimeout = ResolveConditionTimeout(timeout);
            TimeSpan checkInterval = ResolvePollingInterval(pollingInterval);
            return WaitForAsyncCore(condition, exceptionsToIgnore, waitTimeout, checkInterval);
        }

        public async Task WaitForTrueAsync(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string message = null, IList<Type> exceptionsToIgnore = null)
        {
            TimeSpan waitTimeout = ResolveConditionTimeout(timeout);
            if (!(await WaitForAsync(condition, waitTimeout, pollingInterval, exceptionsToIgnore)))
            {
                throw GetTimeoutException(waitTimeout, message);
            }
        }

        public void WaitForTrue(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string message = null, IList<Type> exceptionsToIgnore = null)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition", "condition cannot be null");
            }

            TimeSpan timeSpan = ResolveConditionTimeout(timeout);
            TimeSpan timeout2 = ResolvePollingInterval(pollingInterval);
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (IsConditionSatisfied(condition, exceptionsToIgnore ?? new List<Type>()))
                {
                    return;
                }

                if (stopwatch.Elapsed > timeSpan)
                {
                    break;
                }

                Thread.Sleep(timeout2);
            }

            throw GetTimeoutException(timeSpan, message);
        }

        private TimeoutException GetTimeoutException(TimeSpan waitTimeout, string message)
        {
            string text = $"Timed out after {waitTimeout.TotalSeconds} seconds";
            if (!string.IsNullOrEmpty(message))
            {
                text = text + ": " + message;
            }

            return new TimeoutException(text);
        }

        private bool IsConditionSatisfied(Func<bool> condition, IList<Type> exceptionsToIgnore)
        {
            try
            {
                return condition();
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                if (exceptionsToIgnore.Any((Type type) => type.IsAssignableFrom(exception.GetType())))
                {
                    return false;
                }

                throw;
            }
        }

        private TimeSpan ResolveConditionTimeout(TimeSpan? timeout)
        {
            return timeout ?? timeoutConfiguration.ConditionTimeout;
        }

        private TimeSpan ResolvePollingInterval(TimeSpan? pollingInterval)
        {
            return pollingInterval ?? timeoutConfiguration.PollingIntervalTimeout;
        }

        private async Task<bool> WaitForAsyncCore(Func<bool> condition, IList<Type> exceptionsToIgnore, TimeSpan waitTimeout, TimeSpan checkInterval)
        {
            using CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Task waitTask = Task.Run(async delegate
            {
                while (!IsConditionSatisfied(condition, exceptionsToIgnore ?? new List<Type>()))
                {
                    await Task.Delay(checkInterval);
                }
            }, token);
            Task obj = await Task.WhenAny(new Task[2]
            {
                waitTask,
                Task.Delay(waitTimeout, token)
            });
            cts.Cancel();
            bool num = obj == waitTask;
            if (num && waitTask.Exception != null)
            {
                throw (waitTask.Exception!.InnerExceptions.Count == 1) ? waitTask.Exception!.InnerException : waitTask.Exception;
            }

            return num;
        }
    }
}

