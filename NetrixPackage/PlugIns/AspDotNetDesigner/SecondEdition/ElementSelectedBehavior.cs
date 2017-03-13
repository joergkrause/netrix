using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Comzept.Genesis.NetRix.ComInterop;

/// <summary>
/// Summary description for Class1
/// </summary>
public class ElementSelectedBehavior : BaseBehavior
{
      // Methods
      public ElementSelectedBehavior()
{
      this.BorderMargin = new Rectangle(2, 2, 2, 2);
      this.BorderPenStyle = new Pen(Color.Black);
      this.BorderPenStyle.DashPattern = new float[] { 1f, 1f, 1f, 1f };
}

 

      public override void Draw()
{
      Rectangle rectangle2;
      Graphics graphics1;
      Rectangle rectangle1 = new Rectangle(base.leftBounds - 2, base.topBounds - 2, (base.rightBounds - base.leftBounds) + 1, (base.bottomBounds - base.topBounds) + 1);
      goto Label_0165;
Label_005C:
      if ((0 == 0) && (0x7fffffff != 0))
      {
            goto Label_0073;
      }
Label_005F:
      if (base._isResizing)
      {
            graphics1.DrawRectangle(this.BorderPenStyle, rectangle1);
            Color color1 = Color.FromArgb(100, Color.Yellow);
            if (0 == 0)
            {
                  if (0 == 0)
                  {
                        if (1 == 0)
                        {
                              goto Label_0196;
                        }
                        Color color2 = Color.FromArgb(30, Color.Yellow);
                        LinearGradientBrush brush1 = new LinearGradientBrush(rectangle1, color1, color2, 90f);
                        graphics1.FillRectangle(brush1, rectangle1);
                        SolidBrush brush2 = new SolidBrush(Color.Black);
                        Interop.IHTMLElement element1 = (Interop.IHTMLElement) base.attachedElement;
                        do
                        {
                              string text1 = element1.GetStyle().GetWidth() + ", " + element1.GetStyle().GetHeight();
                              graphics1.DrawString(text1, new Font("Arial", 8f), brush2, 5f + base.leftBounds, 5f + base.topBounds);
                        }
                        while (1 == 0);
                        if (0 == 0)
                        {
                              goto Label_005C;
                        }
                        goto Label_0143;
                  }
                  return;
            }
            goto Label_0165;
      }
Label_0073:
      graphics1.DrawImage(Resources.top, base.leftBounds, base.topBounds);
      Pen pen1 = new Pen(Color.DarkGray);
      graphics1.DrawRectangle(pen1, rectangle2);
      graphics1.Dispose();
      if (0x7fffffff == 0)
      {
            goto Label_005C;
      }
Label_0143:
      if (0 == 0)
      {
            return;
      }
Label_0165:
      rectangle2 = new Rectangle(base.leftBounds, base.topBounds, (base.rightBounds - base.leftBounds) - 3, (base.bottomBounds - base.topBounds) - 3);
Label_0196:
      graphics1 = Graphics.FromHdc(base.pvDrawObject);
      graphics1.Clear(Color.Transparent);
      graphics1.PageUnit = GraphicsUnit.Pixel;
      goto Label_005F;
}

 


      // Properties
      public override string Name
{
      get
      {
            return ("ElementSelected#" + BaseBehavior.url);
      }
}
 

}
 
public class ElementWithViewLinkBehavior : ElementSelectedBehavior
{
      // Methods
     protected override void BehaviorNotify(int x1b313ede48d7e656, IntPtr xb007630f2bd4bf83)
{
      if (x1b313ede48d7e656 == 0)
      {
            this.x54bc2c7d165b00be = (Interop.IHTMLElement) base.attachedElement;
            Interop.HTMLDocument document1 = (Interop.HTMLDocument) this.x54bc2c7d165b00be.document;
            Interop.IHTMLElementDefaults defaults1 = ((Interop.IElementBehaviorSiteOM2) base.behaviorSite).GetDefaults();
            do
            {
                  this.BorderMargin = new Rectangle(100, 100, 100, 100);
                  Application.DoEvents();
                  defaults1.contentEditable = "false";
            }
            while ((((uint) xb007630f2bd4bf83) | 0xff) == 0);
      }
}



    protected override void BehaviorNotify(int x1b313ede48d7e656, IntPtr xb007630f2bd4bf83)
    {
    }

      // Fields
      private Interop.IHTMLElement x54bc2c7d165b00be;
}
 
