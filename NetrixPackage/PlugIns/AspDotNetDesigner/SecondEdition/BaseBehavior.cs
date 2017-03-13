using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Comzept.Genesis.NetRix.AspDotNetDesigner;
using Comzept.Genesis.NetRix.ComInterop;
using Comzept.Genesis.NetRix.WebEditing.Behaviors;
using Comzept.Genesis.NetRix.WebEditing.Elements;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.AspDotNetDesigner;

/// <summary>
/// Summary description for Class1
/// </summary>
public abstract class BaseBehavior : Interop.IHTMLPainter, Interop.IElementBehavior, Interop.IElementBehaviorFactory
{
    // Methods
    static BaseBehavior()
    {
        
    }


    public BaseBehavior()
    {
        this.myEvents = new EventSink(this);
        this.xc151a99213d76447 = -1;
        this._xe8e866e122b29057 = HtmlZOrder.AboveContent;
        this._x3d109d947cd56c1e = HtmlPainter.Transparent;
    }



    protected virtual void BehaviorNotify(int x1b313ede48d7e656, IntPtr xb007630f2bd4bf83)
    {
    }


    public virtual void Draw()
    {
    }

    void Interop.IHTMLPainter.GetPainterInfo(Interop.HTML_PAINTER_INFO x9f34ca2cf22c3ff1)
    {
        x9f34ca2cf22c3ff1.lFlags = (int)this._x3d109d947cd56c1e;
        x9f34ca2cf22c3ff1.lZOrder = (int)this._xe8e866e122b29057;
        x9f34ca2cf22c3ff1.iidDrawObject = Guid.Empty;
        x9f34ca2cf22c3ff1.rcBounds = new Interop.RECT(this.x4adad1da64c75e29.Left, this.x4adad1da64c75e29.Top, this.x4adad1da64c75e29.Right, this.x4adad1da64c75e29.Bottom);
    }



    void Interop.IElementBehavior.Init(Interop.IElementBehaviorSite site)
    {
        Dictionary<string, int> dictionary1;
        string text2;
        string text3;
        string text4;
        int num1;
        this.behaviorSite = site;
        if (4 == 0)
        {
            goto Label_01AE;
        }
        this.attachedElement = (Interop.IHTMLElement2)site.GetElement();
        this.paintSite = (Interop.IHTMLPaintSite)this.behaviorSite;
        IConnectionPointContainer container1 = (IConnectionPointContainer)this.attachedElement;
        Guid guid1 = Guid.Empty;
        string text1 = container1.GetType().Name;
        if (text1.EndsWith("Class"))
        {
            text2 = container1.GetType().Name.Substring(0, text1.Length - 5) + "Events2";
            if (text1 == "HTMLInputElementClass")
            {
                goto Label_03D4;
            }
            if ((((uint)num1) + ((uint)num1)) <= uint.MaxValue)
            {
                goto Label_022E;
            }
            if ((((uint)num1) <= uint.MaxValue) && ((((uint)num1) | uint.MaxValue) == 0))
            {
                goto Label_012B;
            }
            goto Label_024A;
        }
        return;
    Label_00D9:
        if ((0 == 0) && ((((uint)num1) & 0) == 0))
        {
            goto Label_0107;
        }
    Label_00DC:
        //if (<PrivateImplementationDetails>{FDFC6380-7665-4920-BE96-D8D7777662F6}.$$method0x6000139-1.TryGetValue(text4, out num1))
        {
            switch (num1)
            {
                case 0:
                    text2 = "HTMLInputTextElementEvents2";
                    if (4 != 0)
                    {
                        if ((((uint)num1) - ((uint)num1)) >= 0)
                        {
                        }
                        goto Label_0107;
                    }
                    if (0 == 0)
                    {
                        goto Label_02C9;
                    }
                    if (-2147483648 != 0)
                    {
                        goto Label_03D4;
                    }
                    goto Label_0360;

                case 1:
                    text2 = "HTMLOptionButtonElementEvents2";
                    goto Label_0107;

                case 2:
                    text2 = "HTMLButtonElementEvents2";
                    goto Label_0107;

                case 3:
                    goto Label_02C9;

                case 4:
                    text2 = "HTMLElementEvents2";
                    goto Label_0107;

                case 5:
                    throw new NotImplementedException("dunno correct class");

                case 6:
                    text2 = "HTMLInputTextElementEvents2";
                    goto Label_0107;
            }
        }
    Label_0107:
        text3 = "mshtml." + text2 + ", Microsoft.mshtml, Version=7.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
        if (((((uint)num1) + ((uint)num1)) <= uint.MaxValue) && ((((uint)num1) + ((uint)num1)) < 0))
        {
            goto Label_016C;
        }
        Type type1 = Type.GetType(text3);
        try
        {
            guid1 = type1.GUID;
            container1.FindConnectionPoint(ref guid1, out this.x09a9fe7720941e5b);
            this.x09a9fe7720941e5b.Advise(this.myEvents, out this.xc151a99213d76447);
            this.myEvents.ElementEvent += new ElementEventHandler(this.eventHandler);
        }
        catch
        {
            MessageBox.Show("Could not attach events for " + ((Interop.IHTMLElement)this.attachedElement).outerHTML);
        }
        return;
    Label_011E:
        if (text1 == "HTMLDivElementClass")
        {
            goto Label_016C;
        }
    Label_012B:
        if (text1 != "HTMLSpanElementClass")
        {
            goto Label_0107;
        }
        text2 = "HTMLElementEvents2";
        goto Label_00D9;
    Label_016C:
        text2 = "HTMLElementEvents2";
        goto Label_0107;
    Label_0177:
        if (text1 == "HTMLAnchorElementClass")
        {
            text2 = "HTMLAnchorEvents2";
            if (((uint)num1) >= 0)
            {
                goto Label_0107;
            }
            if ((((uint)num1) - ((uint)num1)) < 0)
            {
                goto Label_00D9;
            }
            if ((((uint)num1) | 1) != 0)
            {
                goto Label_01AE;
            }
            goto Label_022E;
        }
        if ((((uint)num1) + ((uint)num1)) > uint.MaxValue)
        {
            if (0 != 0)
            {
                goto Label_0177;
            }
            goto Label_011E;
        }
        if (((uint)num1) <= uint.MaxValue)
        {
            goto Label_011E;
        }
        goto Label_016C;
    Label_01AE:
        if ((((uint)num1) | 15) == 0)
        {
            goto Label_022E;
        }
        if (0xff != 0)
        {
            goto Label_0177;
        }
    Label_01CD:
        if ((((uint)num1) & 0) != 0)
        {
            goto Label_01AE;
        }
        goto Label_0177;
    Label_022E:
        if (text1 != "HTMLGenericElementClass")
        {
            if (-1 != 0)
            {
                goto Label_01CD;
            }
            goto Label_0107;
        }
    Label_024A:
        text2 = "HTMLElementEvents2";
        goto Label_0107;
    Label_02C9:
        text2 = "HTMLElementEvents2";
        goto Label_0107;
    Label_0360:
        dictionary1 = new Dictionary<string, int>(7);
        dictionary1.Add("text", 0);
        dictionary1.Add("option", 1);
        dictionary1.Add("button", 2);
        dictionary1.Add("submit", 3);
        dictionary1.Add("radio", 4);
        dictionary1.Add("image", 5);
        dictionary1.Add("hidden", 6);
        //<PrivateImplementationDetails>{FDFC6380-7665-4920-BE96-D8D7777662F6}.$$method0x6000139-1 = dictionary1;
        goto Label_00DC;
    Label_03D4:
        if ((text4 = ((HTMLInputElementClass)container1).type) == null)
        {
            goto Label_0107;
        }
        //if (<PrivateImplementationDetails>{FDFC6380-7665-4920-BE96-D8D7777662F6}.$$method0x6000139-1 == null)
        {
            goto Label_0360;
        }
        goto Label_00DC;
    }
    private void x4f599b2645a54e3c()
    {
    }
    private void x6d1219467c215ed1(int xc41e708957b76392, int xa76190a02e0e81de) { }

    void Interop.IHTMLPainter.Draw(int left, int top, int right, int bottom, int leftmargin, int topmargin, int rightmargin, int bottommargin, int xad10596756c169b7, IntPtr pvDraw, IntPtr x6f7f479b3406fc02)
    {
        this.pvDrawObject = pvDraw;
        this.leftBounds = left;
        do
        {
            this.topBounds = top;
            this.bottomBounds = bottom;
            this.rightBounds = right;
        }
        while (((uint)x6f7f479b3406fc02) > uint.MaxValue);
        this.Draw();
    }



    private void eventHandler(object sender, Interop.IHTMLEventObj eventObj)
    {
        if ((eventObj.type != "resizeend") || (0 != 0))
        {
            if (eventObj.type != "resizestart")
            {
                while (eventObj.type == "mousedown")
                {
                    eventObj.returnValue = true;
                    return;
                }
            }
            else
            {
                this._isResizing = true;
            }
        }
        else
        {
            this._isResizing = false;
        }
    }



    Interop.IElementBehavior Interop.IElementBehaviorFactory.FindBehavior(string xf97dfec4f6339176, string xe88e5ca6e9a3f0d2, Interop.IElementBehaviorSite xef78be42e0560c9f)
    {
        return this;
    }


    void Interop.IHTMLPainter.HitTestPoint(int xf664a75b506e4d4d, int x8a4a243b90d08b78, out bool xf6e447d6ef2acaa2, out bool x66bdb5beea470b60)
    {

    }



    void Interop.IElementBehavior.Notify(int id, IntPtr pVar)
    {
        this.BehaviorNotify(id, pVar);
    }




    // Properties
    public virtual Rectangle BorderMargin
    {
        get
        {
            return this.x4adad1da64c75e29;
        }
        set
        {
            this.x4adad1da64c75e29 = value;
        }
    }


    public virtual Pen BorderPenStyle
    {
        get
        {
            return this.x7501133900981dda;
        }
        set
        {
            this.x7501133900981dda = value;
        }
    }


    public virtual HtmlPainter HtmlPaintFlag
    {
        get
        {
            return this._x3d109d947cd56c1e;
        }
        set
        {
            this._x3d109d947cd56c1e = value;
        }
    }


    public virtual HtmlZOrder HtmlZOrderFlag
    {
        get
        {
            return this._xe8e866e122b29057;
        }
        set
        {
            this._xe8e866e122b29057 = value;
        }
    }


    public abstract string Name { get; }

    // Fields
    protected bool _isResizing;
    private HtmlPainter _x3d109d947cd56c1e;
    internal Interop.IHTMLPaintSite paintSite;
    private HtmlZOrder _xe8e866e122b29057;
    internal Interop.IElementBehaviorSite behaviorSite;
    protected Interop.IHTMLElement2 attachedElement;
    protected const int BEHAVIOREVENT_CONTENTREADY = 0;
    protected int bottomBounds;
    protected int leftBounds;
    internal EventSink myEvents;
    protected IntPtr pvDrawObject;
    protected int rightBounds;
    protected int topBounds;
    protected internal static readonly string url;
    private IConnectionPoint x09a9fe7720941e5b;
    private Rectangle x4adad1da64c75e29;
    private Pen x7501133900981dda;
    private int xc151a99213d76447;
}