using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFramework.Services
{
    public class ActionRetryProvider : IActionRetryProvider
    {
        private readonly IRetryPolicy retryPolicy;
        public ActionRetryProvider( IRetryPolicy retryPolicy1) 
        {
            retryPolicy = retryPolicy1;
        }
        public virtual void DoWithRetry(Action action, IEnumerable<Type> handledExceptions = null)
        {
            DoWithRetry(delegate
            {
                action();
                return true;
            }, handledExceptions);
        }

        public virtual T DoWithRetry<T>(Func<T> function, IEnumerable<Type> handledExceptions = null)
        {
            IEnumerable<Type> handledExceptions2 = handledExceptions ?? new List<Type>();
            int num = retryPolicy.RetryCount;
            TimeSpan pollingInterval = retryPolicy.PollingInterval;
            T result = default(T);
            while (num >= 0)
            {
                try
                {
                    result = function();
                    return result;
                }
                catch (Exception exception)
                {
                    if (IsExceptionHandled(handledExceptions2, exception) && num != 0)
                    {
                        Thread.Sleep(pollingInterval);
                        num--;
                        continue;
                    }

                    throw;
                }
            }

            return result;
        }
        protected virtual bool IsExceptionHandled(IEnumerable<Type> handledExceptions, Exception exception)
        {
            return handledExceptions.Any((Type type) => type.IsAssignableFrom(exception.GetType()));
        }
    }
}
