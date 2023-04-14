using Aquality.Selenium.Core.Logging;
using Aquality.Selenium.Core.Utilities;
using Aquality.Selenium.Core.Visualization;
using OpenQA.Selenium;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFramework.Elements.Implementations
{
    public class VisualDetector : IVisualDetector
    {
        private readonly WebElement element;

        public Size Size => element.Size;

        public Point Location => element.Location;

        public SKImage Image => element.GetScreenshot().AsImage();

        public VisualDetector( WebElement webElement)
        {
            element = webElement ;
        }
    }
}
