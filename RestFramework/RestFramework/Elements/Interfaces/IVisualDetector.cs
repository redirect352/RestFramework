using SkiaSharp;
using System.Drawing;

namespace RestFramework.Elements
{
    public interface IVisualDetector
    {
        Size Size { get; }
        Point Location { get; }
        SKImage Image { get; }
    }
}
