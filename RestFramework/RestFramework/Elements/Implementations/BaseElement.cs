using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestFramework.Waits;
using RestFramework.Elements.Implementations;
using RestFramework.Services;

namespace RestFramework.Elements
{
    public class BaseElement : IElement
    {
        internal readonly ElementState elementState;
        
        public By Locator { get; }

        public string Name { get; }

        protected BaseElement(By locator, string name, ElementState state)
        {
            Locator = locator;
            Name = name;
            elementState = state;
        }

        public virtual IElementStateChecker State => new ElementStateCheker(Locator, ConditionalWait,Finder);


        public virtual IVisualDetector VisualDetector => new VisualDetector(this.GetElement()); 

        protected  IActionRetryProvider ActionRetrier { get; }

        protected  IConditionalWait ConditionalWait { get; }

        protected  IElementFinder Finder { get; }

        public IWebElement Element => this.GetElement();
        public string Text
        {
            get
            {
                string text = DoWithRetry(() => GetElement().Text);
                return text;
            }
        }

        protected virtual string ElementType => "BaseElement";

        public void Click()
        {
            DoWithRetry(delegate
            {
                GetElement().Click();
            });
        }

        public T FindChildElement<T>(By childLocator, string name = null, ElementState state = ElementState.Displayed) where T : IElement
        {
            //return Factory.FindChildElement(this, childLocator, name, supplier, state);
            return default(T);
        }

        public IList<T> FindChildElements<T>(By childLocator, string name = null, ElementRangeCount expectedCount = ElementRangeCount.Any, ElementState state = ElementState.Displayed) where T : IElement
        {
           // return Factory.FindChildElements(this, childLocator, name, supplier, expectedCount, state);
            return default(IList<T>);
        }

        public string GetAttribute(string attr)
        {
            string text = DoWithRetry(() => GetElement().GetAttribute(attr));
            return text;
        }

        public virtual WebElement GetElement(TimeSpan? timeout = null)
        {
            try
            {
                return ((WebElement)Finder.FindElement(Locator, elementState, timeout, Name));
            }
            catch (NoSuchElementException exception) //when (LoggerConfiguration.LogPageSource)
            {
               // лог;
                throw;
            }
        }

        public void SendKeys(string key)
        {
            DoWithRetry(delegate
            {
                GetElement().SendKeys(key);
            });
        }

        protected virtual void DoWithRetry(Action action)
        {
            ActionRetrier.DoWithRetry(action);
        }

        protected virtual T DoWithRetry<T>(Func<T> function)
        {
            return ActionRetrier.DoWithRetry(function);
        }
    }
}
