using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFramework.Elements
{
    public interface IElementStateChecker
    {
        bool IsDisplayed { get; }

        bool IsExist { get; }


        bool IsClickable { get; }

        bool IsEnabled { get; }


        bool WaitForDisplayed(TimeSpan? timeout = null);
        bool WaitForNotDisplayed(TimeSpan? timeout = null);
        bool WaitForExist(TimeSpan? timeout = null);

        bool WaitForNotExist(TimeSpan? timeout = null);

        bool WaitForEnabled(TimeSpan? timeout = null);
        bool WaitForNotEnabled(TimeSpan? timeout = null);
        void WaitForClickable(TimeSpan? timeout = null);
    }

    
}
