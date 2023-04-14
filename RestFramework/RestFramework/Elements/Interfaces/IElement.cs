using System;
using OpenQA.Selenium;

namespace RestFramework.Elements
{
    public interface IElement
    {
        By Locator { get; }
        IWebElement Element { get; }
        string Name { get; }

        IElementStateChecker State { get; }
        //Aquality.Selenium.Core.Elements.ElementFinder elementFinder;

        IVisualDetector VisualDetector { get; }

        string Text { get; }

        WebElement GetElement(TimeSpan? timeout = null);

        string GetAttribute(string attr);

        void SendKeys(string key);

        void Click();

        T FindChildElement<T>(By childLocator, string name = null, ElementState state = ElementState.Displayed) where T : IElement;
        IList<T> FindChildElements<T>(By childLocator, string name = null, ElementRangeCount expectedCount = ElementRangeCount.Any, ElementState state = ElementState.Displayed) where T : IElement;
    }

    public enum ElementState
    {
        Displayed,
        AnyState
    }

    public enum ElementRangeCount
    {
        Zero,
        MoreThenZero,
        Any
    }
}
