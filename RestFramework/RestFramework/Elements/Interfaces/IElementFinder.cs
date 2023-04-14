using Aquality.Selenium.Configurations;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFramework.Elements
{
    public interface IElementFinder
    {
        IWebElement FindElement(By locator, ElementState state = ElementState.AnyState, TimeSpan? timeout = null, string name = null);

        IWebElement FindElement(By locator, Func<IWebElement, bool> elementStateCondition, TimeSpan? timeout = null, string name = null);

        ReadOnlyCollection<IWebElement> FindElements(By locator, ElementState state = ElementState.AnyState, TimeSpan? timeout = null, string name = null);

        ReadOnlyCollection<IWebElement> FindElements(By locator, Func<IWebElement, bool> elementStateCondition, TimeSpan? timeout = null, string name = null);

    }
}
