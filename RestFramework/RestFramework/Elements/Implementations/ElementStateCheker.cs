using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestFramework.Waits;

namespace RestFramework.Elements
{
    public class ElementStateCheker : IElementStateChecker
    {
        private readonly By elementLocator;

        private IConditionalWait ConditionalWait { get; }

        private IElementFinder ElementFinder { get; }

        public bool IsDisplayed => WaitForDisplayed(TimeSpan.Zero);

        public bool IsExist => WaitForExist(TimeSpan.Zero);

        public bool IsEnabled => WaitForEnabled(TimeSpan.Zero);

        public bool IsClickable => IsElementClickable(TimeSpan.Zero);

        public ElementStateCheker(By elementLocator, IConditionalWait conditionalWait, IElementFinder elementFinder)
        {
            this.elementLocator = elementLocator;
            ConditionalWait = conditionalWait;
            ElementFinder = elementFinder;
        }

        public bool WaitForDisplayed(TimeSpan? timeout = null)
        {
            return DoAndLogWaitForState(() => IsAnyElementFound(timeout, ElementState.Displayed), "displayed", timeout);
        }

        public bool WaitForNotDisplayed(TimeSpan? timeout = null)
        {
            return DoAndLogWaitForState(() => ConditionalWait.WaitFor(() => !IsDisplayed, timeout), "not.displayed", timeout);
        }

        public bool WaitForExist(TimeSpan? timeout = null)
        {
            return DoAndLogWaitForState(() => IsAnyElementFound(timeout, ElementState.AnyState), "exist", timeout);
        }

        public bool WaitForNotExist(TimeSpan? timeout = null)
        {
            return DoAndLogWaitForState(() => ConditionalWait.WaitFor(() => !IsExist, timeout), "not.exist", timeout);
        }

        private bool IsAnyElementFound(TimeSpan? timeout, ElementState state)
        {
            return ElementFinder.FindElements(elementLocator, state, timeout).Any();
        }

        public bool WaitForEnabled(TimeSpan? timeout = null)
        {
            return DoAndLogWaitForState(() => IsElementInState((IWebElement element) => IsElementEnabled(element), "ENABLED", timeout), "enabled", timeout);
        }

        public bool WaitForNotEnabled(TimeSpan? timeout = null)
        {
            return DoAndLogWaitForState(() => IsElementInState((IWebElement element) => !IsElementEnabled(element), "NOT ENABLED", timeout), "not.enabled", timeout);
        }

        protected virtual bool IsElementEnabled(IWebElement element)
        {
            return element.Enabled;
        }

        private bool IsElementInState(Func<IWebElement, bool> elementStateCondition, string state, TimeSpan? timeout)
        {
            return IsElementInCondition(timeout, elementStateCondition,state);
        }

        public void WaitForClickable(TimeSpan? timeout = null)
        {
            //string stateKey = "loc.el.state.clickable";
            try
            {
                //logElementState("loc.wait.for.state", stateKey);
                IsElementClickable(timeout);
            }
            catch
            {
                //logElementState("loc.wait.for.state.failed", stateKey);
                throw;
            }
        }

        private bool IsElementClickable(TimeSpan? timeout)
        {
            return IsElementInCondition(timeout, (IWebElement element) => element.Displayed && element.Enabled, "CLICKABLE");
        }

        private bool IsElementInCondition(TimeSpan? timeout, Func<IWebElement, bool> elementStateCondition, string state)
        {
            return ElementFinder.FindElements(elementLocator, elementStateCondition, timeout,state).Any();
        }

        private bool DoAndLogWaitForState(Func<bool> waitingAction, string conditionKeyPart, TimeSpan? timeout = null)
        {
            if (TimeSpan.Zero == timeout)
            {
                return waitingAction();
            }

            //string stateKey = "loc.el.state." + conditionKeyPart;
            //logElementState("loc.wait.for.state", stateKey);
            bool num = waitingAction();
            if (!num)
            {
                //logElementState("loc.wait.for.state.failed", stateKey);
            }

            return num;
        }
    }
}

