using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestFramework.Waits;

namespace RestFramework.Elements
{
    public class ElementFinder : IElementFinder
    {
        private readonly IConditionalWait wait;
        public ElementFinder(IWebDriver webDriver, IConditionalWait conditionalWait) 
        {
            wait = conditionalWait;
        }
        public virtual IWebElement FindElement(By locator, ElementState state = ElementState.AnyState, TimeSpan? timeout = null, string name = null)
        {
            return FindElement(locator, GetState(state), state.ToString(), timeout, name);
        }

        public virtual IWebElement FindElement(By locator, Func<IWebElement, bool> elementStateCondition, TimeSpan? timeout = null, string name = null)
        {
            return FindElement(locator, elementStateCondition, "desired", timeout, name);
        }

        public virtual IWebElement FindElement(By locator, Func<IWebElement, bool> elementStateCondition, string stateName, TimeSpan? timeout = null, string name = null)
        {
            //DesiredState desiredState = new DesiredState(elementStateCondition, stateName)
            //{
            //    IsCatchingTimeoutException = false,
            //    IsThrowingNoSuchElementException = true
            //};
            return FindElements(locator, elementStateCondition, timeout, name).First();
        }

        public virtual ReadOnlyCollection<IWebElement> FindElements(By locator, ElementState state = ElementState.AnyState, TimeSpan? timeout = null, string name = null)
        {
            return FindElements(locator, GetState(state), timeout, name);
        }

        public virtual ReadOnlyCollection<IWebElement> FindElements(By locator, Func<IWebElement, bool> desiredState, TimeSpan? timeout = null, string name = null)
        {
            List<IWebElement> foundElements = new List<IWebElement>();
            List<IWebElement> resultElements = new List<IWebElement>();
            wait.WaitFor(delegate (IWebDriver driver)
            {
                foundElements = driver.FindElements(locator).ToList();
                resultElements = foundElements.Where(desiredState).ToList();
                return resultElements.Any();
            }, timeout);
            return resultElements.AsReadOnly();
        }

        protected virtual Func<IWebElement, bool> GetState(ElementState state)
        {
            return state switch
            {
                ElementState.Displayed => (IWebElement element) => element.Displayed,
                ElementState.AnyState => (IWebElement element) => true,
                _ => throw new InvalidOperationException($"{state} state is not recognized"),
            };
        }
    }
}
