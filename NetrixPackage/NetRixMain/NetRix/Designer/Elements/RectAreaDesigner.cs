using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using System.Drawing;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.WebEditing.UndoRedo;

namespace GuruComponents.Netrix.Designer
{
    ///// <summary>
    ///// Implements basic behavior for elements that have a layout.
    ///// </summary>
    //internal class RectAreaDesigner : ElementDesigner
    //{

    //    Interop.IHTMLElement body = null; 
    //    Interop.IHTMLElement2 body2 = null;
    //    Interop.IHTMLStyle es;
    //    # region Move vars   
    //    private GuruComponents.Netrix.WebEditing.UndoRedo.UndoStack batch;
    //    private bool inMove = false;
    //    private int mousX = 0, mousY = 0;
    //    private int deltX = 0, deltY = 0; // delta of mouse at the beginning of move (pointer to topleft of element)
    //    private Size grid = new Size(8, 8);
    //    private bool addPosition = false;
    //    private Interop.IHTMLElement moveElement = null;
    //    private Interop.IHTMLWindow2 window = null;
    //    # endregion Move vars

    //    public override bool OnPreHandleEvent(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
    //    {
    //        bool returnCode = false;
    //        if (body == null)
    //        {
    //            body = ((Interop.IHTMLDocument2)((Interop.IHTMLElement)Element.GetBaseElement()).GetDocument()).GetBody();
    //            body2 = (Interop.IHTMLElement2) body;
    //        }
    //        IHtmlEditor htmlEditor = (IHtmlEditor) sender;
    //        Interop.IHTMLElement el = Element.GetBaseElement();
    //        if (el != null && window == null)
    //        {
    //            window = ((Interop.IHTMLDocument2)el.GetDocument()).GetParentWindow();                
    //        }
    //        int realX = e.ClientXY.X + body2.GetScrollLeft();
    //        int realY = e.ClientXY.Y + body2.GetScrollTop();
    //        # region Move
    //        switch (e.EventType)
    //        {
    //            case DhtmlEventType.MouseUp:
    //                if (inMove)
    //                {
    //                    inMove = false;
    //                    mousX = realX;
    //                    mousY = realY;
    //                    moveElement = null;
    //                    addPosition = false;
    //                    if (batch != null)
    //                    {
    //                        batch.Close();
    //                    }
    //                }
    //                break;
    //            case DhtmlEventType.MouseDown:
    //                if (!inMove && e.MouseButton == System.Windows.Forms.MouseButtons.Left) // left button, no move
    //                {       // check on elements, not body, and client area of element (==null)
    //                    if (!el.GetTagName().Equals("BODY") && ((Interop.IHTMLElement2)el).ComponentFromPoint(e.ClientXY.X, e.ClientXY.Y) == null)
    //                    {
    //                        object[] pvars = new object[1] { null };
    //                        el.GetAttribute("UNSELECTABLE", 0, pvars);
    //                        if (pvars[0] != null && pvars[0].ToString().ToLower().Equals("on"))
    //                        {
    //                            break;
    //                        }
    //                        el.GetAttribute("ATOMICSELECTION", 0, pvars);
    //                        if (pvars[0] != null && pvars[0].ToString().ToLower().Equals("true"))
    //                        {
    //                            // force atomic selection
    //                            Interop.IHTMLTextContainer container = body as Interop.IHTMLTextContainer;
    //                            object controlRange = container.createControlRange();
    //                            Interop.IHTMLControlRange2 htmlControlRange2 = controlRange as Interop.IHTMLControlRange2;
    //                            if (htmlControlRange2 != null)
    //                            {
    //                                string position = ((Interop.IHTMLElement2)el).GetCurrentStyle().position;
    //                                if ((position != null) && (String.Compare(position, "absolute", true) == 0))
    //                                {
    //                                    htmlEditor.Selection.ClearSelection();
    //                                    htmlControlRange2.addElement(el);
    //                                    ((Interop.IHTMLControlRange)controlRange).select();
    //                                    returnCode = true;
    //                                }
    //                            }
    //                        }
    //                        moveElement = el;
    //                        es = moveElement.GetStyle();
    //                        addPosition = true;
    //                        if (es.GetLeft() == null || es.GetTop() == null)
    //                        {
    //                            es.SetLeft(moveElement.GetOffsetLeft());
    //                            es.SetTop(moveElement.GetOffsetTop());
    //                        }
    //                        mousX = realX;
    //                        mousY = realY;
    //                        deltX = realX - (int)Unit.Parse(es.GetLeft().ToString()).Value;
    //                        deltY = realY - (int)Unit.Parse(es.GetTop().ToString()).Value;
    //                        grid = htmlEditor.Grid.GridSize;
    //                        inMove = true;
    //                        batch = new UndoStack("Move", htmlEditor, BatchedUndoType.Undo);
    //                    }
    //                }
    //                break;
    //            case DhtmlEventType.MouseMove:
    //                if (inMove && e.MouseButton == System.Windows.Forms.MouseButtons.Left)
    //                {
    //                    es = moveElement.GetStyle();
    //                    if (addPosition)
    //                    {
    //                        es.SetAttribute("position", "absolute", 0);
    //                        addPosition = false;
    //                    }
    //                    int snapX = realX - deltX;
    //                    int snapY = realY - deltY;
    //                    if (htmlEditor.Grid.SnapEnabled)
    //                    {
    //                        snapX = (int)Math.Ceiling((double)snapX / grid.Width) * grid.Width;
    //                        snapY = (int)Math.Ceiling((double)snapY / grid.Height) * grid.Height;
    //                    }
    //                    es.SetLeft(snapX);
    //                    es.SetTop(snapY);
    //                    if (body2.GetClientHeight() != body2.GetScrollHeight())
    //                    {
    //                        if (
    //                            ((moveElement.GetOffsetTop() + ((Interop.IHTMLElement2)moveElement).GetClientHeight() >= body2.GetClientHeight()) && (realY - mousY) > 0)
    //                            ||
    //                            ((moveElement.GetOffsetTop() <= body2.GetScrollTop() && (realY - mousY) < 0))
    //                            )
    //                        {
    //                            window.scrollTo(0, realY - mousY);
    //                        }
    //                    }
    //                    if (body2.GetClientWidth() != body2.GetScrollWidth())
    //                    {
    //                        if (
    //                            ((moveElement.GetOffsetLeft() + ((Interop.IHTMLElement2)moveElement).GetClientWidth() >= body2.GetClientWidth()) && (realX - mousX) > 0)
    //                            ||
    //                            ((moveElement.GetOffsetLeft() <= body2.GetScrollLeft() && (realX - mousX) < 0))
    //                            )
    //                        {
    //                            window.scrollTo(realX - mousX, 0);
    //                        }
    //                    }
    //                    mousX = realX;
    //                    mousY = realY;
    //                    //Interop.IHTMLDocument4 doc = (Interop.IHTMLDocument4)moveElement.GetDocument();
    //                    //Interop.IHTMLEventObj eventObject = doc.createEventObject(e.EventObj);
    //                    //((Interop.IHTMLElement3)moveElement).fireEvent("onmove", eventObject);
    //                    return true;
    //                }
    //                break;
    //        }
    //        # endregion Move

    //        return returnCode;
    //    }

    //}
}