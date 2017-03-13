using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Comzept.Library.Drawing.Shapes
{

	public interface IDrawable  
	{

        Pen Pen { get;set;}
        void Draw(Graphics graphics);
        void Draw(Graphics graphics, SmoothingMode smoothingMode);

	}
}