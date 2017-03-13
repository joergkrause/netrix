using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;
using System.Drawing;

namespace GuruComponents.Netrix.NetRix.WebEditing.HighLighting
{
    public class DisplayPointer 
    {

        private Interop.IDisplayPointer dp;
        private IHtmlEditor editor;

        public DisplayPointer(Interop.IDisplayPointer dp, IHtmlEditor editor)
        {
            this.editor = editor;
            this.dp = dp;
        }


        internal Interop.IDisplayPointer Native
        {
            get { return dp; }
        }

        #region IDisplayPointer Members

        public int MoveToPoint(Point point, Interop.COORD_SYSTEM eCoordSystem, Interop.IHTMLElement pElementContext, uint dwHitTestOptions)        
        {
            uint pdwHitTestResults;
            Interop.POINT ptPoint = new Interop.POINT();
            ptPoint.x = point.X;
            ptPoint.y = point.Y;
            dp.MoveToPoint(ptPoint, eCoordSystem, pElementContext, dwHitTestOptions, out pdwHitTestResults);
            return (int)pdwHitTestResults;
        }

        public void MoveUnit(Interop.DISPLAY_MOVEUNIT eMoveUnit, int lXPos)
        {
            dp.MoveUnit(eMoveUnit, lXPos);
        }

        public void PositionMarkupPointer(MarkupPointer markupPointer)
        {
            dp.PositionMarkupPointer(markupPointer.Native);
        }

        public void MoveToPointer(DisplayPointer dispPointer)
        {
            dp.MoveToPointer(dispPointer.Native);
        }

        public Interop.POINTER_GRAVITY PointerGravity
        {
            get
            {
                Interop.POINTER_GRAVITY peGravity;
                dp.GetPointerGravity(out peGravity);
                return peGravity;
            }
            set
            {
                dp.SetPointerGravity(value);
            }
        }

        public Interop.DISPLAY_GRAVITY DisplayGravity
        {
            get
            {
                Interop.DISPLAY_GRAVITY peGravity;
                dp.GetDisplayGravity(out peGravity);
                return peGravity;
            }
            set
            {
                dp.SetDisplayGravity(value);
            }
        }

        public bool IsPositioned()
        {
            int pfPositioned;
            dp.IsPositioned(out pfPositioned);
            return (pfPositioned == 1);
        }

        public void Unposition()
        {
            dp.Unposition();
        }

        public bool IsEqualTo(DisplayPointer dispPointer)
        {
            Interop.IDisplayPointer pDispPointer = dispPointer.Native;
            int pfIsEqual;
            dp.IsEqualTo(pDispPointer, out pfIsEqual);
            return (pfIsEqual == 1);
        }

        public bool IsLeftOf(DisplayPointer dispPointer)
        {
            Interop.IDisplayPointer pDispPointer = dispPointer.Native;
            int pfIsLeftOf;
            dp.IsLeftOf(pDispPointer, out pfIsLeftOf);
            return (pfIsLeftOf == 1);
        }

        public bool IsRightOf(DisplayPointer dispPointer)
        {
            Interop.IDisplayPointer pDispPointer = dispPointer.Native;
            int pfIsRightOf;
            dp.IsRightOf(pDispPointer, out pfIsRightOf);
            return (pfIsRightOf == 1);
        }

        public bool IsAtBOL()
        {
            int pfBOL;
            dp.IsAtBOL(out pfBOL);
            return (pfBOL == 1);
        }

        public void MoveToMarkupPointer(MarkupPointer pointer, DisplayPointer dispLineContext)
        {
            Interop.IMarkupPointer pPointer = pointer.Native;
            Interop.IDisplayPointer pDispLineContext = dispLineContext.Native;
            dp.MoveToMarkupPointer(pPointer, pDispLineContext);
        }

        public void ScrollIntoView()
        {
            dp.ScrollIntoView();
        }

        public LineInfo GetLineInfo()
        {
            Interop.ILineInfo ppLineInfo;
            dp.GetLineInfo(out ppLineInfo);
            return new LineInfo(ppLineInfo);
        }

        public Interop.IHTMLElement GetFlowElement()
        {
            Interop.IHTMLElement ppLayoutElement;
            dp.GetFlowElement(out ppLayoutElement);
            return ppLayoutElement;
        }

        public int QueryBreaks()
        {
            uint pdwBreaks;
            dp.QueryBreaks(out pdwBreaks);
            return (int)pdwBreaks;
        }

        #endregion
    }
}
