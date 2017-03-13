using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.Documents;
using GuruComponents.Netrix.WebEditing.Styles;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using System.ComponentModel.Design;
using GuruComponents.Netrix.Designer;
using System.Web.UI;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.XmlDesigner
{


    /// <summary>
    /// The base class for all elements.
    /// </summary>
    /// <remarks>
    /// This class defines the basic propertygrid behavior and all helper methods to
    /// deal with attributes. This class is not intended to be used directly from users code. This class
    /// implements IElement and ICloneable. The latter is used to built shallow copy clones of elements.
    /// </remarks>
    /// <seealso cref="IElement"/>
    [ToolboxItem(false)]
    public class ViewElement : IElement, IDisposable //, ICloneable, ICustomTypeDescriptor
    {

        private Interop.IHTMLElement element;
        private IHtmlEditor htmlEditor;        // all elements belong to this control
        private IExtendedProperties extendedProperties;
        private IElementBehavior elementBehavior;
        private CssEffectiveStyle effectiveStyle = null;
        private IElementStyle runtimeStyle = null;
        private IElementStyle currentStyle = null;
        private ViewElementEventSink _eventSink;

        /// <summary>
        /// View element instnce ctor.
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="editor"></param>
        public ViewElement(Interop.IHTMLElement peer, IHtmlEditor editor)
        {
            element = peer;
            htmlEditor = editor;
            Connect();
        }

        /// <summary>
        /// Gives access to extended properties which do not have any relation to attributes.
        /// </summary>
        [Browsable(false)]
        public IExtendedProperties ExtendedProperties
        {
            get
            {
                if (extendedProperties == null)
                {
                    extendedProperties = new ExtendedProperties(element);
                }
                return extendedProperties;
            }
        }

        /// <summary>
        /// Access to element behavior manager.
        /// </summary>
        [Browsable(false)]
        public IElementBehavior ElementBehaviors
        {
            get
            {
                if (elementBehavior == null)
                {
                    elementBehavior = new ElementBehavior(this);
                }
                return elementBehavior;
            }
        }

        # region External Events

        /// <summary>
        /// Fired is element loses capture.
        /// </summary>
        public event GuruComponents.Netrix.Events.DocumentEventHandler LoseCapture;

        /// <summary>
        /// Fired if the user clicks on the element in design mode.
        /// </summary>
        public event DocumentEventHandler Click;

        /// <summary>
        /// Fired if the user double clicks on the element in design mode.
        /// </summary>
        public event DocumentEventHandler DblClick;

        /// <summary>
        /// Fired if the user starts dragging the in design mode.
        /// </summary>
        public event DocumentEventHandler DragStart;

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        public event DocumentEventHandler Focus;

        /// <summary>
        /// Fired if the element is receiving a drop event.
        /// </summary>
        public event DocumentEventHandler Drop;

        /// <summary>
        /// Fired if the elemens looses the focus.
        /// </summary>
        public event DocumentEventHandler Blur;

        /// <summary>
        /// Fired during drag drop.
        /// </summary>
        public event DocumentEventHandler DragOver;

        /// <summary>
        /// Fired if the mouse enters the element during drag drop.
        /// </summary>
        public event DocumentEventHandler DragEnter;

        /// <summary>
        /// Fired if the mouse leaves the element during drag drop.
        /// </summary>
        public event DocumentEventHandler DragLeave;

        /// <summary>
        /// Fired if the user hits a key down on the element in design mode.
        /// </summary>
        public event DocumentEventHandler KeyDown;

        /// <summary>
        /// Fired if the user pressed a key element in design mode.
        /// </summary>
        public event DocumentEventHandler KeyPress;

        /// <summary>
        /// Fired if the user hits and releases a key on the element in design mode.
        /// </summary>
        public event DocumentEventHandler KeyUp;
        
        /// <summary>
        /// Fired if the user clicks a mouse button on the element in design mode.
        /// </summary>
        public event DocumentEventHandler MouseDown;
        
        /// <summary>
        /// Sets or removes an event handler function that fires when the user begins to change the dimensions of the object.
        /// </summary>
        /// <remarks>
        /// This event handler will not suppress the embedded resizing behavior.
        /// </remarks>
        public event DocumentEventHandler ResizeStart;

        /// <summary>
        /// Sets or removes an event handler function that fires when the user has finished changing the dimensions of the object.
        /// </summary>
        /// <remarks>
        /// This event handler will not suppress the embedded resizing behavior.
        /// </remarks>
        public event DocumentEventHandler ResizeEnd;
        
        /// <summary>
        /// Sets or removes an event handler function that fires when the user moves the mouse pointer into the object.
        /// </summary>
        /// <remarks>
        /// Unlike the OnMouseOver event, the MouseEnter event does not bubble. In other words, the MouseEnter 
        /// event does not fire when the user moves the mouse pointer over elements contained by the object, 
        /// whereas <see cref="MouseOver">MouseOver</see> does fire. 
        /// </remarks>
        public event DocumentEventHandler MouseEnter;

        /// <summary>
        /// Sets or retrieves a pointer to the event handler function that fires, when the user moves the mouse pointer outside 
        /// the boundaries of the object.</summary>
        /// <remarks>Use in correspondence to MouseEnter.</remarks>
        public event DocumentEventHandler MouseLeave;

        /// <summary>
        /// Fired if the user moves the mouse over the element area in design mode.
        /// </summary>
        public event DocumentEventHandler MouseMove;

        /// <summary>
        /// Fired if the user mouse has left the element area with the mouse in design mode.
        /// </summary>        
        public event DocumentEventHandler MouseOut;

        /// <summary>
        /// Fired if the user has entered the element area with the mouse in design mode.
        /// </summary>
        public event DocumentEventHandler MouseOver;
        
        /// <summary>
        /// Fired if the user releases the mouse button over the element area in design mode.
        /// </summary>
        public event DocumentEventHandler MouseUp;

        /// <summary>
        /// Fired if the user starts selecting the element area in design mode.
        /// </summary>
        public event DocumentEventHandler SelectStart;

        /// <summary>
        /// Fired when the renderer has finished the layout.
        /// </summary>
        public event DocumentEventHandler LayoutComplete;

        /// <summary>
        /// Fired on document level after load complete.
        /// </summary>
        public event DocumentEventHandler Load;

        /// <summary>
        /// Fired when the mouse wheel moves.
        /// </summary>
        public event DocumentEventHandler MouseWheel;

        /// <summary>
        /// Fired when a move operation ends.
        /// </summary>
        public event DocumentEventHandler MoveEnd;

        /// <summary>
        /// Fired when a move operation starts.
        /// </summary>        
        public event DocumentEventHandler MoveStart;

        /// <summary>
        /// Fired after the element is getting activated.
        /// </summary>
        public event DocumentEventHandler Activate;

        /// <summary>
        /// Fired before the element is about going activated.
        /// </summary>
        public event DocumentEventHandler BeforeActivate;

        /// <summary>
        /// Fired before a copy operation starts.
        /// </summary>
        public event DocumentEventHandler BeforeCopy;

        /// <summary>
        /// Fired before a cut operation starts.
        /// </summary>
        public event DocumentEventHandler BeforeCut;

        /// <summary>
        /// Fired before a paste operation starts.
        /// </summary>
        public event DocumentEventHandler BeforePaste;

        /// <summary>
        /// Fired is the user has right clicked the element.
        /// </summary>
        public event DocumentEventHandler ContextMenu;

        /// <summary>
        /// Fired after a copy operation has been finished.
        /// </summary>
        public event DocumentEventHandler Copy;

        /// <summary>
        /// Fired after a cut operation has been finished.
        /// </summary>
        public event DocumentEventHandler Cut;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler Deactivate;
        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler Drag;
        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler DragEnd;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler FocusIn;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler FocusOut;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler Change;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler Select;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler SelectionChange;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler BeforeDeactivate;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler ControlSelect;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler EditFocus;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler Move;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler Paste;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler PropertyChange;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler Resize;

        /// <summary>
        /// 
        /// </summary>
        public event DocumentEventHandler Scroll;

        event DocumentEventHandler IElement.Abort { add { } remove { } }
        event DocumentEventHandler IElement.Stop { add { } remove { } }
        event DocumentEventHandler IElement.Error { add { } remove { } }
        event DocumentEventHandler IElement.Paged { add { } remove { } }
        event DocumentEventHandler IElement.FilterChange { add { } remove { } }

        # endregion

        # region Events

        internal void InvokeLoseCapture(Interop.IHTMLEventObj e)
        {
            if (LoseCapture != null)
            {
                LoseCapture(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeClick(Interop.IHTMLEventObj e)
        {
            if (Click != null)
            {
                Click(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeDblClick(Interop.IHTMLEventObj e)
        {
            if (DblClick != null)
            {
                DblClick(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeDragStart(Interop.IHTMLEventObj e)
        {
            if (DragStart != null)
            {
                DragStart(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeFocus(Interop.IHTMLEventObj e)
        {
            if (Focus != null)
            {
                Focus(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeDrop(Interop.IHTMLEventObj e)
        {
            if (Drop != null)
            {
                Drop(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeBlur(Interop.IHTMLEventObj e)
        {
            if (Blur != null)
            {
                Blur(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeDragOver(Interop.IHTMLEventObj e)
        {
            if (DragOver != null)
            {
                DragOver(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeDragEnter(Interop.IHTMLEventObj e)
        {
            if (DragEnter != null)
            {
                DragEnter(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeDragLeave(Interop.IHTMLEventObj e)
        {
            if (DragLeave != null)
            {
                DragLeave(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeKeyDown(Interop.IHTMLEventObj e)
        {
            if (KeyDown != null)
            {
                KeyDown(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeKeyPress(Interop.IHTMLEventObj e)
        {
            if (KeyPress != null)
            {
                KeyPress(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeKeyUp(Interop.IHTMLEventObj e)
        {
            if (KeyUp != null)
            {
                KeyUp(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeMouseDown(Interop.IHTMLEventObj e)
        {
            if (MouseDown != null)
            {
                MouseDown(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeResizeStart(Interop.IHTMLEventObj e)
        {
            if (ResizeStart != null)
            {
                ResizeStart(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeResizeEnd(Interop.IHTMLEventObj e)
        {
            if (ResizeEnd != null)
            {
                ResizeEnd(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeMouseEnter(Interop.IHTMLEventObj e)
        {
            if (MouseEnter != null)
            {
                MouseEnter(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeMouseLeave(Interop.IHTMLEventObj e)
        {
            if (MouseLeave != null)
            {
                MouseLeave(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeMouseMove(Interop.IHTMLEventObj e)
        {
            if (MouseMove != null)
            {
                MouseMove(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeMouseOut(Interop.IHTMLEventObj e)
        {
            if (MouseOut != null)
            {
                MouseOut(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeMouseOver(Interop.IHTMLEventObj e)
        {
            if (MouseOver != null)
            {
                MouseOver(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeMouseUp(Interop.IHTMLEventObj e)
        {
            if (MouseUp != null)
            {
                MouseUp(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeSelectStart(Interop.IHTMLEventObj e)
        {
            if (SelectStart != null)
            {
                SelectStart(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeLayoutComplete(Interop.IHTMLEventObj e)
        {
            if (LayoutComplete != null)
            {
                LayoutComplete(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeLoad(Interop.IHTMLEventObj e)
        {
            if (Load != null)
            {
                Load(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeMouseWheel(Interop.IHTMLEventObj e)
        {
            if (MouseWheel != null)
            {
                MouseWheel(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeMoveEnd(Interop.IHTMLEventObj e)
        {
            if (MoveEnd != null)
            {
                MoveEnd(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeMoveStart(Interop.IHTMLEventObj e)
        {
            if (MoveStart != null)
            {
                MoveStart(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeActivate(Interop.IHTMLEventObj e)
        {
            if (Activate != null)
            {
                Activate(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeBeforeActivate(Interop.IHTMLEventObj e)
        {
            if (BeforeActivate != null)
            {
                BeforeActivate(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeBeforeCopy(Interop.IHTMLEventObj e)
        {
            if (BeforeCopy != null)
            {
                BeforeCopy(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeBeforeCut(Interop.IHTMLEventObj e)
        {
            if (BeforeCut != null)
            {
                BeforeCut(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeBeforePaste(Interop.IHTMLEventObj e)
        {
            if (BeforePaste != null)
            {
                BeforePaste(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeContextMenu(Interop.IHTMLEventObj e)
        {
            if (ContextMenu != null)
            {
                ContextMenu(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeCopy(Interop.IHTMLEventObj e)
        {
            if (Copy != null)
            {
                Copy(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeCut(Interop.IHTMLEventObj e)
        {
            if (Cut != null)
            {
                Cut(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeDeactivate(Interop.IHTMLEventObj e)
        {
            if (Deactivate != null)
            {
                Deactivate(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeDrag(Interop.IHTMLEventObj e)
        {
            if (Drag != null)
            {
                Drag(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeDragEnd(Interop.IHTMLEventObj e)
        {
            if (DragEnd != null)
            {
                DragEnd(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeFocusIn(Interop.IHTMLEventObj e)
        {
            if (FocusIn != null)
            {
                FocusIn(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeFocusOut(Interop.IHTMLEventObj e)
        {
            if (FocusOut != null)
            {
                FocusOut(this, new DocumentEventArgs(e, this));
            }
        }
        //internal void InvokeFilterChange(Interop.IHTMLEventObj e)
        //{
        //    if (FilterChange != null)
        //    {
        //        FilterChange(this, new DocumentEventArgs(e, this));
        //    }
        //}
        //internal void InvokeAbort(Interop.IHTMLEventObj e)
        //{
        //    if (Abort != null)
        //    {
        //        Abort(this, new DocumentEventArgs(e, this));
        //    }
        //}
        internal void InvokeChange(Interop.IHTMLEventObj e)
        {
            if (Change != null)
            {
                Change(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeSelect(Interop.IHTMLEventObj e)
        {
            if (Select != null)
            {
                Select(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeSelectionChange(Interop.IHTMLEventObj e)
        {
            if (SelectionChange != null)
            {
                SelectionChange(this, new DocumentEventArgs(e, this));
            }
        }
        //internal void InvokeStop(Interop.IHTMLEventObj e)
        //{
        //    if (Stop != null)
        //    {
        //        Stop(this, new DocumentEventArgs(e, this));
        //    }
        //}
        internal void InvokeBeforeDeactivate(Interop.IHTMLEventObj e)
        {
            if (BeforeDeactivate != null)
            {
                BeforeDeactivate(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeControlSelect(Interop.IHTMLEventObj e)
        {
            if (ControlSelect != null)
            {
                ControlSelect(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeEditFocus(Interop.IHTMLEventObj e)
        {
            if (EditFocus != null)
            {
                EditFocus(this, new DocumentEventArgs(e, this));
            }
        }
        //internal void InvokeError(Interop.IHTMLEventObj e)
        //{
        //    if (Error != null)
        //    {
        //        Error(this, new DocumentEventArgs(e, this));
        //    }
        //}
        internal void InvokeMove(Interop.IHTMLEventObj e)
        {
            if (Move != null)
            {
                Move(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokePaste(Interop.IHTMLEventObj e)
        {
            if (Paste != null)
            {
                Paste(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokePropertyChange(Interop.IHTMLEventObj e)
        {
            if (PropertyChange != null)
            {
                PropertyChange(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeResize(Interop.IHTMLEventObj e)
        {
            if (Resize != null)
            {
                Resize(this, new DocumentEventArgs(e, this));
            }
        }
        internal void InvokeScroll(Interop.IHTMLEventObj e)
        {
            if (Scroll != null)
            {
                Scroll(this, new DocumentEventArgs(e, this));
            }
        }
        //internal void InvokePage(Interop.IHTMLEventObj e)
        //{
        //    if (Paged != null)
        //    {
        //        Paged(this, new DocumentEventArgs(e, this));
        //    }
        //}


        # endregion

        # region Access to internal used properties, COM interfaces, base classes

        /// <summary>
        /// Gives access to the HtmlEditor control and all subclasses
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(BindableSupport.No)]
        public IHtmlEditor HtmlEditor
        {
            get
            {                
                return htmlEditor;
            }
            set
            {
                htmlEditor = value;
            }
        }

        # endregion

        # region Public access to base properties

		# region Extended Properties

        /// <summary>
        /// Returns <c>true</c> if the element is selectable.
        /// </summary>
        /// <remarks>
        /// This is a short cut to the selection class.
        /// <seealso cref="GuruComponents.Netrix.ISelection">ISelection</seealso>
        /// </remarks>
        /// <returns>Returns <c>true</c> if the element is selectable.</returns>
        public bool IsSelectable()
        {
            return htmlEditor.Selection.IsSelectableElement(this);
        }

        private string uniqueName;
        /// <summary>
        /// Unique name internally used by DesignerHost.
        /// </summary>
        public string UniqueName
        {
            get
            {
                if (uniqueName == null)
                {
                    if (GetAttribute("id") != null)
                    {
                        uniqueName = GetStringAttribute("id");
                    }
                    else
                    {
                        throw new Exception("No unique id provided");
                    }
                }
                return uniqueName;
            }
        }


        # endregion Extended Properties

        /// <summary>
        /// Returns the absolute coordinates of the element in pixel.
        /// </summary>
        /// <remarks>
        /// This method works even for non absolute positioned elements. Some elements, which have no rectangle
        /// dimensions, may fail returning any useful values.
        /// </remarks>
        /// <returns>A rectangle which describes the dimensions of the client area of the element.</returns>
        public Rectangle GetAbsoluteArea()
        {
            int x = element.GetOffsetLeft();
            int y = element.GetOffsetTop();
            int height= element.GetOffsetHeight();
            int width = element.GetOffsetWidth();
            Interop.IHTMLElement parent = element.GetParentElement();
            while (parent != null)
            {
                if (parent.GetTagName().Equals("TR")) 
                {
                    parent = parent.GetParentElement();
                    continue;
                }
                x += parent.GetOffsetLeft();
                y += parent.GetOffsetTop();                
                parent = parent.GetParentElement();
            } 
            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Replaces the text adjacent to the element.
        /// </summary>
        /// <param name="where">Specifies where to locate the replacement text.</param>
        /// <param name="text">The text that replace the current one.</param>
        public void ReplaceAdjacentText(ElementAdjacency where, string text)
        {
            if (htmlEditor.ThreadSafety)
            {
                InvokeReplaceAdjacentText(where, text);
            }
            else
            {
                ((Interop.IHTMLElement2)element).ReplaceAdjacentText(Enum.GetName(typeof(ElementAdjacency), where), text);
            }
        }

        private delegate void ReplaceAdjacentTextDelegate(ElementAdjacency where, string text);

        private void InvokeReplaceAdjacentText(ElementAdjacency where, string text)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                ReplaceAdjacentTextDelegate d = new ReplaceAdjacentTextDelegate(InvokeReplaceAdjacentText);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, where, text);
            }
            else
            {
                ((Interop.IHTMLElement2) element).ReplaceAdjacentText(Enum.GetName(typeof(ElementAdjacency), where), text);
            }
        }

        /// <summary>
        /// Gets the text adjacent to the element.
        /// </summary>
        /// <param name="where">Specifies where to locate the replacement text.</param>
        public string GetAdjacentText(ElementAdjacency where)
        {
            return ((Interop.IHTMLElement2) element).GetAdjacentText(Enum.GetName(typeof(ElementAdjacency), where));
        }

        /// <summary>
        /// Removes mouse capture from the object in the current document.
        /// </summary>
        public void ReleaseCapture()
        {
            ((Interop.IHTMLElement2) element).ReleaseCapture();
        }

        /// <summary>
        /// Causes the element to lose focus and fires the <see cref="Blur"/> event.
        /// </summary>
        public void BlurFocus()
        {
            ((Interop.IHTMLElement2) element).Blur();
        }


		/// <summary>
		/// Gets or sets explicitly the design mode for this element.
		/// </summary>
		/// <value>Setting to true will set the attribute "contentEditable", which is visible in the documents DOM.
		/// Returning the value false means that the mode is not explicitly set but may be inherited. Setting a
		/// parent to ContentEditable will not change the value for the children, but in fact, they are now editable. 
		/// </value>
		[Browsable(false)]
		public bool ContentEditable
		{
			set
			{
				if (element.GetTagName().Equals("IFRAME"))
				{					
					((Interop.IHTMLDocument2) element.GetDocument()).SetDesignMode(value ? "On" : "Off");
				} 
				else 
				{
                    ((Interop.IHTMLElement3)this.element).contentEditable = value ? "true" : "false";
				}
				
			}
			get
			{
				if (element.GetTagName().Equals("IFRAME"))
				{
					return ((Interop.IHTMLDocument2) element.GetDocument()).GetDesignMode().Equals("On");
				} 
				else 
				{
                    return ((Interop.IHTMLElement3)this.element).contentEditable.Equals("true") || ((Interop.IHTMLElement3)this.element).contentEditable.Equals("inherit");
				}
			}
		}

        /// <summary>
        /// The element with this property set to TRUE will be selectable only as a unit.
        /// </summary>
        /// <remarks>
        /// This property is only available 
        /// </remarks>
        [Browsable(false)]
        public bool AtomicSelection
        {
            get
            {
                string sel = GetStringAttribute("ATOMICSELECTION");
                return (sel.ToLower().Equals("true"));
            } 
            set
            {
                SetStringAttribute("ATOMICSELECTION", value ? "true" : "false");
            }
        }

        /// <summary>
        /// Makes the element unselectable, so the user cannot activate it for resizing or moving.
        /// </summary>
        /// <remarks>
        /// The property is ignored if the element is already an unselectable element. Only block elements
        /// like DIV, TABLE, and IMG can be selectable.
        /// </remarks>
        [Browsable(false)]
        public bool Unselectable 
        { 
            get
            {
                string sel = GetStringAttribute("UNSELECTABLE");
                return (sel.ToLower().Equals("on"));
            } 
            set
            {
                SetStringAttribute("UNSELECTABLE", value ? "on" : "off");
            }
        }

        # region Inner/Outer Access

        private delegate void SetInnerOuterDelegate(string text);

        private void InvokeSetInnerHtml(string text)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetInnerOuterDelegate d = new SetInnerOuterDelegate(InvokeSetInnerHtml);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, text);                
            }
            else
            {
                element.SetInnerHTML(text);
            }
        }

        private void InvokeSetInnerText(string text)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetInnerOuterDelegate d = new SetInnerOuterDelegate(InvokeSetInnerText);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, text);
            }
            else
            {
                element.SetInnerText(text);
            }
        }

        private void InvokeSetOuterHtml(string text)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetInnerOuterDelegate d = new SetInnerOuterDelegate(InvokeSetOuterHtml);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, text);
            }
            else
            {
                element.SetOuterHTML(text);
            }
        }

        /// <summary>
        /// Gets or sets inner html of the element.
        /// </summary>
        /// <remarks>
        /// The inner html is the complete content between the opening and the closing tag.
        /// If the element is not a container element the property will return <see cref="System.String.Empty">String.Empty</see>.
        /// Any HTML is returned "as is", with the braces and tag names.
        /// </remarks>
        [Browsable(true)]
        public string InnerHtml
        {
            get
            {
                string str;

                try
                {
                    str = element.GetInnerHTML();
                }
                catch
                {
                    str = String.Empty;
                }
                return str;
            }

            set
            {
                try
                {
                    if (htmlEditor.ThreadSafety)
                    {
                        InvokeSetInnerHtml(value);
                    }
                    else
                    {
                        element.SetInnerHTML(value);
                    }
                }
                catch
                {                    
                }
            }
        }

        /// <summary>
        /// Gets or sets the inner text of the element.
        /// </summary>
        /// <remarks>
        /// The inner text is the complete content between the opening and the closing tag, with any HTML tags
        /// stripped out.
        /// If the element is not a container element the property will return <see cref="System.String.Empty">String.Empty</see>.
        /// Any HTML is returned "as is", with the braces and tag names.
        /// </remarks>
        [Browsable(false)]
        public string InnerText
        {
            get
            {
                string str;

                try
                {
                    str = element.GetInnerText();
                }
                catch
                {
                    str = String.Empty;
                }
                return str;
            }

            set
            {
                try
                {
                    if (htmlEditor.ThreadSafety)
                    {
                        InvokeSetInnerText(value);
                    }
                    else
                    {
                        element.SetInnerText(value);
                    }
                }
                catch
                {                    
                }
            }
        }

        /// <summary>
        /// Gets or sets outer html of the element.
        /// </summary>
        /// <remarks>
        /// The outer html is the complete content between the opening and the closing tag and it includes the
        /// tags themselfes.
        /// If the element is not a container element the property will return <see cref="System.String.Empty">String.Empty</see>.
        /// Any HTML is returned "as is", with the braces and tag names.
        /// </remarks>
        [Browsable(false)]
        public string OuterHtml
        {
            get
            {
                string str;

                try
                {
                    str = element.GetOuterHTML();
                }
                catch
                {
                    str = String.Empty;
                }
                return str;
            }

            set
            {
                try
                {
                    if (htmlEditor.ThreadSafety)
                    {
                        InvokeSetOuterHtml(value);
                    }
                    else
                    {
                        element.SetOuterHTML(value);
                    }
                }
                catch
                {
                }
            }
        }

        # endregion Inner/Outer Access
        
        /// <summary>
        /// Gives direct access to the element object from datasource or collection based lists.
        /// </summary>
        /// <remarks>
        /// Used for lists which needs a property to access collection members for displaying purposes. To
        /// prevent such lists from deleting the object we send only a shallow copy clone. This
        /// allows the caller to change all properties but deleting does not modify the DOM.
        /// </remarks>
        [Browsable(false)]
        IElement IElement.TagElement
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// The name of the element.
        /// </summary>
        /// <remarks>
        /// If the loaded document supports XML and does contain namespaces, the property does not
        /// return the assigned alias. See <see cref="Alias"/> and <see cref="ElementName"/> instead.
        /// </remarks>
        [Browsable(false)]
        public string TagName
        {
            get
            {
                string str;

                try
                {
                    str = element.GetTagName();
                }
                catch
                {
                    str = String.Empty;
                }
                return str;
            }
        }

        /// <summary>
        /// Returns the alias of the element's namespace.
        /// </summary>
        /// <remarks>
        /// HTML elements does have the legacy alias HTML, which is never ever required to be written
        /// in documents explicitly. This feature does not require XML plugins for proper support.
        /// </remarks>
        [Browsable(false), DesignOnly(true)]
        public string Alias
        {
            get
            {
                return ((Interop.IHTMLElement2)element).GetScopeName().Replace("HTML", "");
            }
        }

        /// <summary>
        /// Complete name of element including the alias.
        /// </summary>
        /// <remarks>
        /// If the loaded document supports XML and does contain namespaces, the property does 
        /// return the assigned alias and name in usual form "alias:name". 
        /// See also <see cref="Alias"/> and <see cref="TagName"/> for other naming information properties.
        /// </remarks>
        [Browsable(false), DesignOnly(true)]
        public string ElementName
        {
            get
            {
                return String.Format("{0}:{1}", Alias, TagName);
            }
        }

        /// <summary>
        /// Builds the string representation of the element class.
        /// </summary>
        /// <remarks>
        /// The method does not recognize container tags and therefore
        /// it renders all tags as &lt;tagname&gt; whether or not a closing tag exists.
        /// </remarks>
        /// <returns></returns>
        public override string ToString()
        {
            if (element != null)
            {
                try
                {
                    string str = String.Concat("<", element.GetTagName(), ">");
                    return str;
                }
                catch
                {
                    return String.Empty;
                }
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets the current STYLE definition.
        /// </summary>
        /// <remarks>
        /// This property shows the effective style if this element as a cascade of the global
        /// and inline styles defined elsewhere. Readonly.
        /// <para>
        /// The property returns <c>null</c> (<c>Nothing</c> in VB.NET) if the effective style can not be retrieved.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Element Style")]
        [Description("The effective style as a cascaded combination of global, embedded and inline styles. Readonly.")]        
        [System.ComponentModel.DisplayName("Effective Style")]
        public IEffectiveStyle EffectiveStyle
        {
            get
            {
                if (effectiveStyle == null)
                {
                    Interop.IHTMLCurrentStyle cs = ((Interop.IHTMLElement2)this.GetBaseElement()).GetCurrentStyle() as Interop.IHTMLCurrentStyle;
                    if (cs != null)
                    {
                        effectiveStyle = new CssEffectiveStyle(cs);
                    }
                }
                return effectiveStyle;
            }
        }
        
        /// <summary>
        /// The runtime style provide access to additonal appearance information at runtime. Does not persist.
        /// </summary>
        /// <remarks>
        /// This property allows access to styles not being persistent within the document. They affect only at runtime
        /// and can change the current appearance of an object. One can use this to add specific effects during user
        /// operation of to customize elements in particular situations.
        /// <seealso cref="EffectiveStyle"/>
        /// <seealso cref="SetStyleAttribute"/>
        /// <seealso cref="RemoveStyleAttribute"/>
        /// <seealso cref="GetStyleAttribute"/>
        /// </remarks>
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Element Style")]
        [Description("The runtime style provide access to additonal appearance information at runtime. Does not persist.")]        
        [System.ComponentModel.DisplayName("Runtime Style")]
        public IElementStyle RuntimeStyle
        {
            get
            {                
                if (runtimeStyle == null) 
                {   
                    Interop.IHTMLStyle cs = ((Interop.IHTMLElement2)this.GetBaseElement()).GetRuntimeStyle() as Interop.IHTMLStyle;
                    if (cs != null)
                    {
                        runtimeStyle = new ElementStyle(cs);
                    }
                }
                return runtimeStyle;
            }
        }

        /// <summary>
        /// Access to the style attribute in an object form.
        /// </summary>
        /// <seealso cref="EffectiveStyle"/>
        /// <seealso cref="RuntimeStyle"/>
        /// <seealso cref="SetStyleAttribute"/>
        /// <seealso cref="RemoveStyleAttribute"/>
        /// <seealso cref="GetStyleAttribute"/>
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Element Style")]
        [Description("Access to the style attribute in an object form.")]        
        [TE.DisplayName("Runtime Style")]
        public IElementStyle CurrentStyle
        {
            get
            {            
                if (currentStyle == null) 
                {   
                    Interop.IHTMLStyle cs = ((Interop.IHTMLElement)this.GetBaseElement()).GetStyle() as Interop.IHTMLStyle;
                    if (cs != null)
                    {
                        currentStyle = new ElementStyle(cs);
                    }
                }
                return currentStyle;
            }
        }

        /// <summary>
        /// Returns the COM instance of the element object.
        /// </summary>
        /// <returns></returns>
        public Interop.IHTMLElement GetBaseElement()
        {
            return element;        
        }

        # endregion

        # region Element creation, related data and linked information (children, parent, styles, ...)

		/// <summary>
		/// Inserts an element at the specified location.
		/// </summary>
		/// <param name="method">Specifies where to insert the HTML element.</param>
		/// <param name="element">Element to be inserted adjacent to the object.</param>
		public void InsertAdjacentElement(InsertWhere method, IElement element)
		{
			((Interop.IHTMLElement2)GetBaseElement()).InsertAdjacentElement(method.ToString(), element.GetBaseElement());
		}

		/// <summary>
		/// Inserts the given HTML text into the element at the location.
		/// </summary>
		/// <param name="method"></param>
		/// <param name="html"></param>
		public void InsertAdjacentHtml(InsertWhere method, string html)
		{
			GetBaseElement().InsertAdjacentHTML(method.ToString(), html);
		}

		/// <summary>
		/// Inserts the given text into the element at the specified location.
		/// </summary>
		/// <param name="method">Specifies where to insert the text.</param>
		/// <param name="text"></param>
		public void InsertAdjacentText(InsertWhere method, string text)
		{
			GetBaseElement().InsertAdjacentText(method.ToString(), text);
		}
        
        internal void Connect()
        {
            _eventSink = new ViewElementEventSink(this);
            _eventSink.Connect();
        }

        internal void DisConnect()
        {
            if (_eventSink != null)
            {
                _eventSink.Disconnect();
                _eventSink = null;
            }
        }

        /// <summary>
        /// Finalizer destroys the event link.
        /// </summary>
        ~ViewElement()
        {
            DisConnect();
        }

        /// <overloads>This method has two overloads.</overloads>
        /// <summary>
        /// Returns the child at the give index. </summary>
        /// <remarks>If a element has 4 children they are indexed zero based.
        /// </remarks>
        /// <param name="index">The Number of child in the children collection.</param>
        /// <returns>Returns the base class of child element or <c>null</c>, if the index was not found.</returns>
        public System.Web.UI.Control GetChild(int index)
        {
            try
            {
                Interop.IHTMLElement el = (Interop.IHTMLElement)((Interop.IHTMLElementCollection)element.GetChildren()).Item(null, index);
                return htmlEditor.GenericElementFactory.CreateElement(el);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// The child of the element or null if it does not have children.
        /// </summary>
        /// <remarks>
        /// This method will return the next child with the given tag name in the collection of children.
        /// </remarks>
        /// <param name="name">Tag name of the child we are searching for.</param>
        /// <returns>Returns the base class of child element or <c>null</c>, if the name was not found.</returns>
        public System.Web.UI.Control GetChild(string name)
        {
            try
            {
                Interop.IHTMLElement el = (Interop.IHTMLElement)((Interop.IHTMLElementCollection)element.GetChildren()).Item(name, null);
                return htmlEditor.GenericElementFactory.CreateElement(el);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the collection of children of the element.
        /// </summary>
        /// <remarks>
        /// The returned collection will always beeing created, but it could be empty if there are
        /// no children. The collection will contain all children elements that follow the current element,
        /// even their children. To get only the direct descendants use <see cref="GuruComponents.Netrix.WebEditing.Elements.ElementDom.GetChildNodes(bool)">ElementDom.GetChildNodes</see> instead.
        /// </remarks>
        /// <returns>Collection of elements of type IElement.</returns>
        public ElementCollection GetChildren()
        {
            Interop.IHTMLElementCollection ec = element.GetChildren() as Interop.IHTMLElementCollection;
            ElementCollection newEc = new ElementCollection();
            if (ec != null)
            {
                for (int i = 0; i < ec.GetLength(); i++)
                {
                    Interop.IHTMLElement el = (Interop.IHTMLElement) ec.Item(i, i);                   
                    newEc.Add(htmlEditor.GenericElementFactory.CreateElement(el));
                }
            }
            return newEc;
        }

        private IElementDom elementDom;

        /// <summary>
        /// Returns the DOM access to the element.
        /// </summary>
        /// <returns>The DOM representation of the element.</returns>
        [Browsable(false)]
        public IElementDom ElementDom
        {
            get
            {
                if (elementDom == null)
                {
                    elementDom = new ElementDom((Interop.IHTMLDOMNode) element, this.HtmlEditor) as IElementDom;
                }
                return elementDom;
            }
        }
        
        /// <summary>
        /// The parent of the current element or null if no parent.
        /// </summary>
        /// <returns>Returns the base class of child element or <c>null</c>, if there was no parent element.</returns>
        public System.Web.UI.Control GetParent()
        {
            Interop.IHTMLElement parent = element.GetParentElement();
            if (parent != null)
            {
                return htmlEditor.GenericElementFactory.CreateElement(parent);
            } 
            else 
            {
                return null;
            }
        }
      
        /// <summary>
        /// Gets true if the element can positioned freely on the surface by using the mouse.
        /// </summary>
        [Browsable(false)]
        public bool IsAbsolutePositioned
        {
            get
            {
                return ((this.GetStyleAttribute("position").ToLower() == "absolute") ? true : false);
            }
        }

        /// <summary>
        /// Gets <c>true</c> if the element is in text editing mode.
        /// </summary>
        [Browsable(false)]
        public bool IsTextEdit
        {
            get
            {
                return element.GetIsTextEdit();
            }
        }


        # endregion

        # region Get/Set Styles

        /// <summary>
        /// This method return the complete CSS inline definition, which the style attribute contains.
        /// </summary>
        /// <remarks>
        /// For easy access to specific styles it is recommended to use the 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement.GetStyleAttribute">GetStyleAttribute</see> and 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement.SetStyleAttribute">SetStyleAttribute</see>
        /// methods. This and both alternative methods will check the content and cannot assign values not
        /// processed by the Internet Explorer Engine. The final behavior may vary depending on the currently
        /// installed IE engine.
        /// </remarks>
        /// <returns>Returns the complete style text (all rules) as one string.</returns>
        public string GetStyle()
        {
            Interop.IHTMLStyle style = (Interop.IHTMLStyle) GetBaseElement().GetStyle();
            return style.GetCssText();
        }

        /// <summary>
        /// Set the current style by overwriting the complete content of the style attribute.
        /// </summary>
        /// <param name="CssText">The style text; should be CSS compliant.</param>
        public void SetStyle(string CssText)
        {
            if (htmlEditor.ThreadSafety)
            {
                InvokeSetStyle(CssText);
            }
            else
            {
                Interop.IHTMLStyle style = (Interop.IHTMLStyle)GetBaseElement().GetStyle();
                style.SetCssText(CssText);
            }
        }

        private delegate void SetStyleDelegate(string css);
        private delegate void SetStyleAttributeDelegate(string styleName, string styleText);

        private void InvokeSetStyle(string CssText)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetStyleDelegate d = new SetStyleDelegate(InvokeSetStyle);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, CssText);
            }
            else
            {
                element.GetStyle().SetCssText(CssText);
            }
        }

        private void InvokeSetStyleAttribute(string styleName, string styleText)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetStyleAttributeDelegate d = new SetStyleAttributeDelegate(InvokeSetStyleAttribute);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, styleName, styleText);
            }
            else
            {
                try
                {
                    element.GetStyle().SetAttribute(styleName.Replace("-", String.Empty), styleText, 0);
                }
                catch
                {
                    element.GetStyle().SetAttribute(styleName, styleText, 0);
                }
            }
        }

        /// <summary>
        /// Gets a specific part of an inline style.
        /// </summary>
        /// <param name="styleName">The style attribute to retrieve</param>
        /// <returns>The string representation of the style. Returns <see cref="String.Empty"/> if the 
        /// style does not exists.</returns>
        public string GetStyleAttribute(string styleName)
        {    
            Interop.IHTMLStyle style = (Interop.IHTMLStyle) GetBaseElement().GetStyle();
            object o = style.GetAttribute(styleName.Replace("-", String.Empty), 0);
            if (o == null)
            {
                return String.Empty;
            }
            else 
            {
                string styleText = o.ToString();
                return styleText;
            }
        }

        /// <summary>
        /// Sets a specific part of an inline style.
        /// </summary>
        /// <remarks>
        /// Setting to <seealso cref="System.String.Empty">String.Empty</seealso> does remove
        /// the style name. For a more intuitive way use <see cref="GuruComponents.Netrix.WebEditing.Elements.Element.RemoveStyleAttribute">RemoveStyleAttribute</see> instead.
        /// Setting impossible rule texts will either ignore the command or set unexpected values.
        /// </remarks>
        /// <param name="styleName">The name of the style rule beeing set, e.g. "font-family".</param>
        /// <param name="styleText">The rule content, like "Verdana,Arial" for the "font-family" rule.</param>
        public virtual void SetStyleAttribute(string styleName, string styleText)
        {
            if (htmlEditor.ThreadSafety)
            {
                InvokeSetStyleAttribute(styleName, styleText);
            }
            else
            {
                try
                {
                    element.GetStyle().SetAttribute(styleName.Replace("-", String.Empty), styleText, 0);
                }
                catch
                {
                    element.GetStyle().SetAttribute(styleName, styleText, 0);
                }
            }
        }

        /// <summary>
        /// Removes an style attribute and his content from the inline style string.
        /// </summary>
        /// <param name="styleName">Name of style to be removed</param>
        public virtual void RemoveStyleAttribute(string styleName)
        {
            if (styleName.IndexOf("-") != -1)
            {
                SetStyleAttribute(styleName, string.Empty);
            } 
            else 
            {
                Interop.IHTMLStyle style = GetBaseElement().GetStyle();
                style.RemoveAttribute(styleName, 0);
            }
        }

        #region GetAttr

        /// <summary>
        /// Get attribute without checking specific type.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public virtual object GetAttribute(string attribute)
        {
            try
            {
                if (htmlEditor.ThreadSafety)
                {
                    return InvokeGetAttribute(attribute);
                }
                else
                {
                    return GetNativeAttribute(attribute);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        private object InvokeGetAttribute(string attr)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                GetAttributeDelegate d = new GetAttributeDelegate(InvokeGetAttribute);
                return ((System.Windows.Forms.Control)htmlEditor).Invoke(d, new object[] { attr });
            }
            else
            {
                return GetNativeAttribute(attr);
            }
        }

        private delegate object GetAttributeDelegate(string attr);

        /// <summary>
        /// Universal access to any attribute.
        /// </summary>
        /// <remarks>
        /// The type returned may vary depended on the internal type.
        /// </remarks>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>The object which is the value of the attribute.</returns>
        private object GetNativeAttribute(string attribute)
        {
            object local2;
            attribute = (attribute.Equals("http-equiv")) ? "httpequiv" : attribute;
            try
            {
                object[] locals = new object[1];
                locals[0] = null;
                element.GetAttribute(attribute, 0, locals);
                object local1 = locals[0];
                if (local1 is DBNull)
                {
                    local1 = null;
                }
                local2 = local1;
            }
            catch
            {
                local2 = null;
            }
            return local2;
        }

        #endregion GetAttr

        # region SpecializedGetAttr

        /// <summary>
        /// Get boolean attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        protected internal bool GetBooleanAttribute(string attribute)
        {
            object local = GetAttribute(attribute);
            if (local == null)
            {
                return false;
            }
            if (local is Boolean)
            {
                return (bool)local;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get color from attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        protected internal Color GetColorAttribute(string attribute)
        {
            string str = GetStringAttribute(attribute);
            if (str.Length == 0)
            {
                return Color.Empty;
            }
            else
            {
                return ColorTranslator.FromHtml(str);
            }
        }

        /// <summary>
        /// Get enum from attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected internal Enum GetEnumAttribute(string attribute, Enum defaultValue)
        {
            Enum @enum;

            Type type = defaultValue.GetType();
            object local = GetAttribute(attribute);
            if (local == null)
            {
                return defaultValue;
            }
            string str = local as String;
            if (str == null || str.Length == 0)
            {
                return defaultValue;
            }
            try
            {
                @enum = (Enum)Enum.Parse(type, str, true);
            }
            catch
            {
                @enum = defaultValue;
            }
            return @enum;
        }

        /// <summary>
        /// Get int from attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected internal int GetIntegerAttribute(string attribute, int defaultValue)
        {
            object local = GetAttribute(attribute);
            if (local == null)
            {
                return defaultValue;
            }
            if (local is Int32)
            {
                return (int)local;
            }
            if (local is Int16)
            {
                return (short)local;
            }
            if (local is String)
            {
                string str = (String)local;
                if (str.Length != 0 && Char.IsDigit(str[0]))
                {
                    try
                    {
                        int i = Int32.Parse((String)local);
                        return i;
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Get string from attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        protected internal string GetStringAttribute(string attribute)
        {
            return GetStringAttribute(attribute, String.Empty);
        }

        /// <summary>
        /// Get string from attribute and returns default param if attribute is not present.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected internal string GetStringAttribute(string attribute, string defaultValue)
        {
            object local = GetAttribute(attribute);
            if (local == null)
            {
                return defaultValue;
            }
            if (local is String)
            {
                return (String)local;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get unit from attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        protected internal Unit GetUnitAttribute(string attribute)
        {
            Unit unit;

            object local = GetAttribute(attribute);
            if (local == null)
            {
                return Unit.Empty;
            }
            if (local is Int32)
            {
                return new Unit((int)local);
            }
            try
            {
                unit = new Unit((String)local, CultureInfo.InvariantCulture);
            }
            catch
            {
                unit = Unit.Empty;
            }
            return unit;
        }

        # endregion SpecializedGetAttr

        #region RemoveAttr

        /// <summary>
        /// Remove the give attribute from element.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is thread safe if <see cref="GuruComponents.Netrix.HtmlEditor.ThreadSafety">ThreadSafety</see>
        /// is turned on. 
        /// </para>
        /// </remarks>
        /// <seealso cref="GuruComponents.Netrix.HtmlEditor.ThreadSafety">ThreadSafety</seealso>
        /// <param name="attribute">The name of the attribute which is about to be removed. Case insensitive.</param>
        public virtual void RemoveAttribute(string attribute)
        {
            try
            {
                if (htmlEditor.ThreadSafety)
                {
                    InvokeRemoveAttribute(attribute);
                }
                else
                {
                    if (GetAttribute(attribute) != null)
                    {
                        element.RemoveAttribute(attribute, 0);
                    }
                }
            }
            catch
            {
            }
        }

        private delegate void RemoveAttributeDelegate(string attr);

        private void InvokeRemoveAttribute(string attr)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                RemoveAttributeDelegate d = new RemoveAttributeDelegate(InvokeRemoveAttribute);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, new object[] { attr });
            }
            else
            {
                if (GetAttribute(attr) != null)
				{
					element.RemoveAttribute(attr, 0);
				}
            }
        }

        #endregion RemoveAttr

        #region SetAttribute

        /// <summary>
        /// Sets an attribute to a specific value.
        /// </summary>
        /// <remarks>
        /// The command may does nothing if the value does not correspond with the attribute. E.g. it
        /// is almost impossible to write a pixel value if the attribute expects a font name.
        /// <para>
        /// This property is thread safe if <see cref="GuruComponents.Netrix.HtmlEditor.ThreadSafety">ThreadSafety</see>
        /// is turned on. This feature was added in Version 1.6 (Nov 2006) and is available in both, Standard and Advanced
        /// edition.
        /// </para>
        /// </remarks>
        /// <seealso cref="GuruComponents.Netrix.HtmlEditor.ThreadSafety">ThreadSafety</seealso>
        /// <param name="attribute">The name of the attribute.</param>
        /// <param name="value">The value being written.</param>
        public virtual void SetAttribute(string attribute, object value)
        {
            try
            {
                if (htmlEditor.ThreadSafety)
                {
                    RemoveAttribute(attribute);
                    InvokeSetAttribute(attribute, value);
                }
                else
                {
                    RemoveAttribute(attribute);
                    element.SetAttribute(attribute, value, 0);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        private delegate void SetAttributeDelegate(string attr, object value);

        private void InvokeSetAttribute(string attr, object value)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetAttributeDelegate d = new SetAttributeDelegate(InvokeSetAttribute);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, new object[] { attr, value });
            }
            else
            {
                element.SetAttribute(attr, value, 0);
            }
        }

        #endregion SetAttribute

        # region SpecializedSetAttr

        /// <summary>
        /// Set bool value to attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        protected internal void SetBooleanAttribute(string attribute, bool value)
        {
            if (value)
            {
                SetAttribute(attribute, 1);
            } 
            else 
            {
                RemoveAttribute(attribute);
                SetAttribute(attribute, String.Empty);
            }
        }

        /// <summary>
        /// Set color value to attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        protected internal void SetColorAttribute(string attribute, Color value)
        {
            if (value.IsEmpty)
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, ColorTranslator.ToHtml(value));
        }

        /// <summary>
        /// Set enum value to attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        protected internal void SetEnumAttribute(string attribute, Enum value, Enum defaultValue)
        {
            if (value.Equals(defaultValue))
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, value.ToString());
        }

        /// <summary>
        /// Set int value to attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        protected internal void SetIntegerAttribute(string attribute, int value, int defaultValue)
        {
            if (value == defaultValue)
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, value);
        }

        /// <summary>
        /// Set string value to attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        protected internal void SetStringAttribute(string attribute, string value)
        {
            SetStringAttribute(attribute, value, String.Empty);
        }

        /// <summary>
        /// Set string value to attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        protected internal void SetStringAttribute(string attribute, string value, string defaultValue)
        {
            if (value == null || value.Equals(defaultValue))
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, value);
        }

        /// <summary>
        /// Set unit value to attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        protected internal void SetUnitAttribute(string attribute, Unit value)
        {
            if (value.IsEmpty)
            {
                RemoveAttribute(attribute);
                return;
            }
            UnitType unitType = value.Type;
            if (unitType != UnitType.Pixel && unitType != UnitType.Percentage)
            {
                throw new ArgumentException("Only pixel and percent values are allowed here.");
            }
            SetAttribute(attribute, value.ToString(CultureInfo.InvariantCulture));
        }
        
        # endregion SpecializedGetAttr

        # endregion


        #region IDisposable Members
        private bool isDisposed = false;

        /// <summary>
        /// Shows whether the element is being disposed already.
        /// </summary>
        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        /// <summary>
        /// Disconnect events and free memory.
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            _eventSink.Disconnect();
            element = null;
        }

        #endregion

        public CssStyleCollection Style
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}