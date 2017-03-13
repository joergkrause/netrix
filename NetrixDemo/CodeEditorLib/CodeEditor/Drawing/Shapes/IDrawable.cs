using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuruComponents.CodeEditor.Library.Drawing.Shapes
{

	public interface IDrawable  
	{

        Pen Pen { get;set;}
        void Draw(Graphics graphics);
        void Draw(Graphics graphics, SmoothingMode smoothingMode);

	}
}