using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuruComponents.CodeEditor.Library.Drawing.Shapes
{

	public interface IFillable  
	{

        Brush Brush{get;set;}
        void Fill(Graphics graphics);

	}
}