using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.WebEditing.Documents;
using GuruComponents.Netrix.WebEditing.Styles;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;
using System.ComponentModel.Design.Serialization;
using System.Collections.Generic;

namespace GuruComponents.Netrix.WebEditing.Elements
{

    //public class MyComponentEditor : ComponentEditor
    //{
    //    public override bool EditComponent(ITypeDescriptorContext context, object component)
    //    {
    //        System.Windows.Forms.MessageBox.Show("");
    //        return true;
    //    }
    //}

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
    [PropertyTabAttribute(typeof(EventsTab), PropertyTabScope.Component)]
    [DefaultEvent("Click")]
    //[Editor("GuruComponents.Netrix.WebEditing.Elements.MyComponentEditor", typeof(ComponentEditor))] 
    [DesignerAttribute(typeof(ElementDesigner))]
    [Serializable]
    public abstract class Element : WebControl, IElement, ICloneable, ICustomTypeDescriptor
    {

        private Interop.IHTMLElement element;
        private IHtmlEditor htmlEditor;        // all elements belong to this control
        private IExtendedProperties extendedProperties;
        private IElementBehavior elementBehavior;
        private static readonly Regex fileRegex = new Regex(@"file:/+", RegexOptions.IgnoreCase);

        public new System.Web.UI.CssStyleCollection Style
        {
            get
            {
                System.Web.UI.CssStyleCollection css = base.Style;
                css.Value = this.GetStyle();
                return css;
            }
            set
            {
                this.SetStyle(value.Value);
            }
        }

        public override string UniqueID
        {
            get
            {
                string ms_id;
                ((Interop.IHTMLUniqueName) element).uniqueID(out ms_id);
                return ms_id;
            }
        }

        /// <summary>
        /// Gets or sets the element's ID attribute.
        /// </summary>
        public override string ID
        {
            get
            {
                return GetAttribute("id") as string;
            }
            set
            {
                SetAttribute("id", value);
            }
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
        /// Access to the binary behavior manager at element level.
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

        private static ArrayList customColors;

        # region External Events

        [EventVisible(), Category("Netrix Component"), Description("OnClick event is fired when the user clicks the element.")]
        public event DocumentEventHandler LoseCapture;
        protected internal void OnLoseCapture()
        {
            if (LoseCapture != null)
            {
                LoseCapture(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
            }
        }

        /// <summary>
        /// Fired if the user clicks on the element in design mode.
        /// </summary>
        [EventVisible(), Category("Mouse"), Description("OnClick event is fired when the user clicks the element.")]
        [ReadOnly(false)]
        public event DocumentEventHandler Click;
        
        protected internal void OnClick()
        {
            if (Click != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                    Click(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the user double clicks on the element in design mode.
        /// </summary>
        [EventVisible(), Category("Mouse"), Description("OnDbClick event is fired when the user double clicks the element.")]
        public event DocumentEventHandler DblClick;
        protected internal void OnDblClick()
        {
            if (DblClick != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                DblClick(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the user starts dragging the in design mode.
        /// </summary>
        [EventVisible(), Category("Drag Drop"), Description(" Fired if the user starts dragging the in design mode.")]
        public event DocumentEventHandler DragStart;
        protected internal void OnDragStart()
        {
            if (DragStart != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                DragStart(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        [EventVisible(), Category("Selection"), Description("Fired if the elemens receives the focus.")]
        public new event DocumentEventHandler Focus;
        protected internal void OnFocus()
        {
            if (Focus != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Focus(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the element is receiving a drop event.
        /// </summary>
        [EventVisible(), Category("Drag Drop"), Description("Fired if the element is receiving a drop event.")]
        public event DocumentEventHandler Drop;
        protected internal void OnDrop()
        {
            if (Drop != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Drop(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the elemens looses the focus.
        /// </summary>
        [EventVisible(), Category("Selection"), Description("Fired if the elemens looses the focus.")]
        public event DocumentEventHandler Blur;
        protected internal void OnBlur()
        {
            if (Blur != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Blur(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired during drag drop.
        /// </summary>
        [EventVisible(), Category("Drag Drop"), Description("Fired during drag drop.")]
        public event DocumentEventHandler DragOver;
        protected internal void OnDragOver()
        {
            if (DragOver != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                DragOver(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the mouse enters the element during drag drop.
        /// </summary>
        [EventVisible(), Category("Drag Drop"), Description("Fired if the mouse enters the element during drag drop.")]
        public event DocumentEventHandler DragEnter;
        protected internal void OnDragEnter()
        {
            if (DragEnter != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                DragEnter(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the mouse leaves the element during drag drop.
        /// </summary>
        [EventVisible(), Category("Drag Drop"), Description("Fired if the mouse leaves the element during drag drop.")]
        public event DocumentEventHandler DragLeave;
        protected internal void OnDragLeave()
        {
            if (DragLeave != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                DragLeave(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the user hits a key down on the element in design mode.
        /// </summary>
        [EventVisible(), Category("Key"), Description("Fired if the user hits a key down on the element in design mode.")]
        public event DocumentEventHandler KeyDown;
        protected internal void OnKeyDown()
        {
            if (KeyDown != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                KeyDown(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the user pressed a key in design mode.
        /// </summary>
        [EventVisible(), Category("Key"), Description("Fired if the user pressed a key in design mode.")]
        public event DocumentEventHandler KeyPress;
        protected internal void OnKeyPress()
        {
            if (KeyPress != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                KeyPress(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the user hits and releases a key on the element in design mode.
        /// </summary>
        [EventVisible(), Category("Key"), Description("Fired if the user hits and releases a key on the element in design mode.")]
        public event DocumentEventHandler KeyUp;
        protected internal void OnKeyUp()
        {
            if (KeyUp != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                KeyUp(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }
        /// <summary>
        /// Fired if the user clicks a mouse button on the element in design mode.
        /// </summary>
        [EventVisible(), Category("Mouse"), Description("")]
        public event DocumentEventHandler MouseDown;
        protected internal void OnMouseDown()
        {
            if (MouseDown != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                MouseDown(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }		/// <summary>
        /// Sets or removes an event handler function that fires when the user begins to change the dimensions of the object.
        /// </summary>
        /// <remarks>
        /// This event handler will not suppress the embedded resizing behavior.
        /// </remarks>
        [EventVisible(), Category("Resize"), Description("")]
        public event DocumentEventHandler ResizeStart;
        protected internal void OnResizeStart()
        {
            if (ResizeStart != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                ResizeStart(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Sets or removes an event handler function that fires when the user has finished changing the dimensions of the object.
        /// </summary>
        /// <remarks>
        /// This event handler will not suppress the embedded resizing behavior.
        /// </remarks>
        [EventVisible(), Category("Resize"), Description("Fires when the user has finished changing the dimensions of the object.")]
        public event DocumentEventHandler ResizeEnd;
        protected internal void OnResizeEnd()
        {
            if (ResizeEnd != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                ResizeEnd(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }
        /// <summary>
        /// Sets or removes an event handler function that fires when the user moves the mouse pointer into the object.
        /// </summary>
        /// <remarks>
        /// Unlike the OnMouseOver event, the MouseEnter event does not bubble. In other words, the MouseEnter 
        /// event does not fire when the user moves the mouse pointer over elements contained by the object, 
        /// whereas <see cref="MouseOver">MouseOver</see> does fire. 
        /// </remarks>
        [EventVisible(), Category("Mouse"), Description("")]
        public event DocumentEventHandler MouseEnter;
        protected internal void OnMouseEnter()
        {
            if (MouseEnter != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                MouseEnter(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Sets or retrieves a pointer to the event handler function that fires, when the user moves the mouse pointer outside 
        /// the boundaries of the object.</summary>
        /// <remarks>Use in correspondence to MouseEnter.</remarks>
        [EventVisible(), Category("Mouse"), Description("Sets or retrieves a pointer to the event handler function that fires, when the user moves the mouse pointer outside the boundaries of the object.")]
        public event DocumentEventHandler MouseLeave;
        protected internal void OnMouseLeave()
        {
            if (MouseLeave != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                MouseLeave(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the user moves the mouse over the element area in design mode.
        /// </summary>
        [EventVisible(), Category("Mouse"), Description("")]
        public event DocumentEventHandler MouseMove;
        protected internal void OnMouseMove()
        {
            if (MouseMove != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                MouseMove(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the user mouse has left the element area with the mouse in design mode.
        /// </summary>
        /// <example>
        /// To use this event to show the content of a link the mouse pointer is over, use this code:
        /// <code>
        /// ArrayList anchors = this.htmlEditor2.DocumentProperties.GetElementCollection("A") as ArrayList;
        /// if (anchors != null)
        /// {
        ///    foreach (AnchorElement a in anchors)
        ///    {
        ///        a.MouseOver -= new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOver);
        ///        a.MouseOver += new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOver);
        ///        a.MouseOut -= new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOut);
        ///        a.MouseOut += new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOut);
        ///    }
        /// }</code>
        /// Place this code in the <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> event.
        /// The handler are now able to do something with the element.
        /// <code>
        ///private void a_MouseOut(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///  AnchorElement a = e.SrcElement as AnchorElement;
        ///  if (a != null)
        ///  {
        ///     this.statusBarPanelLinks.Text = "";
        ///  }
        ///}</code>
        /// <code>
        ///private void a_MouseOver(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///   AnchorElement a = e.SrcElement as AnchorElement;
        ///   if (a != null)
        ///   {
        ///      this.statusBarPanelLinks.Text = a.href;
        ///   }
        ///}</code>
        /// </example>
        [EventVisible(), Category("Mouse"), Description("Fired if the user mouse has left the element area with the mouse in design mode.")]
        public event DocumentEventHandler MouseOut;
        protected internal void OnMouseOut()
        {
            if (MouseOut != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                MouseOut(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the user has entered the element area with the mouse in design mode.
        /// </summary>
        /// <example>
        /// To use this event to show the content of a link the mouse pointer is over, use this code:
        /// <code>
        /// ArrayList anchors = this.htmlEditor2.DocumentProperties.GetElementCollection("A") as ArrayList;
        /// if (anchors != null)
        /// {
        ///    foreach (AnchorElement a in anchors)
        ///    {
        ///        a.MouseOver -= new GuruComponents.Netrix.Events.DocumentEventHandler(a_MouseOver);
        ///        a.MouseOver += new GuruComponents.Netrix.Events.DocumentEventHandler(a_MouseOver);
        ///        a.MouseOut -= new GuruComponents.Netrix.Events.DocumentEventHandler(a_MouseOut);
        ///        a.MouseOut += new GuruComponents.Netrix.Events.DocumentEventHandler(a_MouseOut);
        ///    }
        /// }</code>
        /// Place this code in the <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> event.
        /// The handler are now able to do something with the element.
        /// <code>
        ///private void a_MouseOut(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///  AnchorElement a = e.SrcElement as AnchorElement;
        ///  if (a != null)
        ///  {
        ///     this.statusBarPanelLinks.Text = "";
        ///  }
        ///}</code>
        /// <code>
        ///private void a_MouseOver(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///   AnchorElement a = e.SrcElement as AnchorElement;
        ///   if (a != null)
        ///   {
        ///      this.statusBarPanelLinks.Text = a.href;
        ///   }
        ///}</code>
        /// </example>
        [EventVisible(), Category("Mouse"), Description("")]
        public event DocumentEventHandler MouseOver;
        protected internal void OnMouseOver()
        {
            if (MouseOver != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                MouseOver(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the user releases the mouse button over the element area in design mode.
        /// </summary>
        [EventVisible(), Category("Mouse"), Description("Fired if the user releases the mouse button over the element area in design mode.")]
        public event DocumentEventHandler MouseUp;
        protected internal void OnMouseUp()
        {
            if (MouseUp != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                MouseUp(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired if the user starts selecting the element area in design mode.
        /// </summary>
        [EventVisible(), Category("Editing"), Description("Fired if the user starts selecting the element area in design mode.")]
        public event DocumentEventHandler SelectStart;

        protected internal void OnSelectStart()
        {
            if (SelectStart != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                SelectStart(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired when the renderer has finished the layout.
        /// </summary>
        [EventVisible(), Category("Netrix Component"), Description("Fired when the renderer has finished the layout.")]
        public event DocumentEventHandler LayoutComplete;

        protected internal void OnLayoutComplete()
        {
            if (LayoutComplete != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                LayoutComplete(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired on document level after load complete.
        /// </summary>
        [EventVisible(), Category("Netrix Component"), Description("Fired on document level after load complete.")]
        public new event DocumentEventHandler Load;

        protected internal void OnLoad()
        {
            if (Load != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Load(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired when the mouse wheel moves.
        /// </summary>
        [EventVisible(), Category("Mouse"), Description("Fired when the mouse wheel moves.")]
        public event DocumentEventHandler MouseWheel;

        protected internal void OnMouseWheel()
        {
            if (MouseWheel != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                MouseWheel(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }


        /// <summary>
        /// Fired when a move operation ends.
        /// </summary>
        [EventVisible(), Category("Move"), Description("Fired when a move operation ends.")]
        public event DocumentEventHandler MoveEnd;

        protected internal void OnMoveEnd()
        {
            if (MoveEnd != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                MoveEnd(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired when a move operation starts.
        /// </summary>        
        [EventVisible(), Category("Move"), Description("Fired when a move operation starts.")]
        public event DocumentEventHandler MoveStart;

        protected internal void OnMoveStart()
        {
            if (MoveStart != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                MoveStart(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired after the element is getting activated.
        /// </summary>
        [EventVisible(), Category("Selection"), Description("Fired after the element is getting activated.")]
        public event DocumentEventHandler Activate;

        protected internal void OnActivate()
        {
            if (Activate != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Activate(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired before the element is about going activated.
        /// </summary>
        [EventVisible(), Category("Selection"), Description("Fired before the element is about going activated.")]
        public event DocumentEventHandler BeforeActivate;

        protected internal void OnBeforeActivate()
        {
            if (BeforeActivate != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                BeforeActivate(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired before a copy operation starts.
        /// </summary>
        [EventVisible(), Category("Editing"), Description("Fired before a copy operation starts.")]
        public event DocumentEventHandler BeforeCopy;

        protected internal void OnBeforeCopy()
        {
            if (BeforeCopy != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                BeforeCopy(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired before a cut operation starts.
        /// </summary>
        [EventVisible(), Category("Editing"), Description("Fired before a cut operation starts.")]
        public event DocumentEventHandler BeforeCut;

        protected internal void OnBeforeCut()
        {
            if (BeforeCut != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                BeforeCut(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired before a paste operation starts.
        /// </summary>
        [EventVisible(), Category("Editing"), Description("Fired before a paste operation starts.")]
        public event DocumentEventHandler BeforePaste;

        protected internal void OnBeforePaste()
        {
            if (BeforePaste != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                BeforePaste(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired is the user has right clicked the element.
        /// </summary>
        [EventVisible(), Category("Editing"), Description("Fired is the user has right clicked the element.")]
        public event DocumentEventHandler ContextMenu;

        protected internal void OnContextMenu()
        {
            if (ContextMenu != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                ContextMenu(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired after a copy operation has been finished.
        /// </summary>
        [EventVisible(), Category("Editing"), Description("Fired after a copy operation has been finished.")]
        public event DocumentEventHandler Copy;

        protected internal void OnCopy()
        {
            if (Copy != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Copy(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        /// <summary>
        /// Fired after a cut operation has been finished.
        /// </summary>
        [EventVisible(), Category("Editing"), Description("Fired after a cut operation has been finished.")]
        public event DocumentEventHandler Cut;

        protected internal void OnCut()
        {
            if (Cut != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Cut(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Selection"), Description("")]
        public event DocumentEventHandler Deactivate;

        protected internal void OnDeactivate()
        {
            if (Deactivate != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Deactivate(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Drag Drop"), Description("")]
        public event DocumentEventHandler Drag;

        protected internal void OnDrag()
        {
            if (Drag != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Drag(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Drag Drop"), Description("")]
        public event DocumentEventHandler DragEnd;

        protected internal void OnDragEnd()
        {
            if (DragEnd != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                DragEnd(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Selection"), Description("")]
        public event DocumentEventHandler FocusIn;

        protected internal void OnFocusIn()
        {
            if (FocusIn != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                FocusIn(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Selection"), Description("")]
        public event DocumentEventHandler FocusOut;

        protected internal void OnFocusOut()
        {
            if (FocusOut != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                FocusOut(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Style"), Description("")]
        public event DocumentEventHandler FilterChange;

        protected internal void OnFilterChange()
        {
            if (FilterChange != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                FilterChange(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Netrix Component"), Description("")]
        public event DocumentEventHandler Abort;

        protected internal void OnAbort()
        {
            if (Abort != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Abort(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }


        [EventVisible(), Category("Behavior"), Description("")]
        public event DocumentEventHandler Change;

        protected internal void OnChange()
        {
            if (Change != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Change(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Netrix Component"), Description("")]
        public event DocumentEventHandler Select;

        protected internal void OnSelect()
        {
            if (Select != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Select(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Netrix Component"), Description("")]
        public event DocumentEventHandler SelectionChange;

        protected internal void OnSelectionChange()
        {
            if (SelectionChange != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                SelectionChange(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Netrix Component"), Description("")]
        public event DocumentEventHandler Stop;

        protected internal void OnStop()
        {
            if (Stop != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Stop(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Selection"), Description("")]
        public event DocumentEventHandler BeforeDeactivate;

        protected internal void OnBeforeDeactivate()
        {
            if (BeforeDeactivate != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                BeforeDeactivate(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Selection"), Description("")]
        public event DocumentEventHandler ControlSelect;

        protected internal void OnControlSelect()
        {
            if (ControlSelect != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                    ControlSelect(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Selection"), Description("")]
        public event DocumentEventHandler EditFocus;

        protected internal void OnEditFocus()
        {
            if (EditFocus != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                EditFocus(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Netrix Component"), Description("")]
        public event DocumentEventHandler Error;

        protected internal void OnError()
        {
            if (Error != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Error(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Move Object"), Description("")]
        public event DocumentEventHandler Move;

        protected internal void OnMove()
        {
            if (Move != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Move(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Editing"), Description("")]
        public event DocumentEventHandler Paste;

        protected internal void OnPaste()
        {
            if (Paste != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Paste(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Netrix Component"), Description("")]
        public event DocumentEventHandler PropertyChange;

        protected internal void OnPropertyChange()
        {
            if (PropertyChange != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                PropertyChange(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Resize"), Description("")]
        public event DocumentEventHandler Resize;

        protected internal void OnResize()
        {
            if (Resize != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Resize(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Netrix Component"), Description("")]
        public event DocumentEventHandler Scroll;

        protected internal void OnScroll()
        {
            if (Scroll != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                Scroll(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        [EventVisible(), Category("Netrix Component"), Description("")]
        public event DocumentEventHandler Paged;

        protected internal void OnPage()
        {
            if (Paged != null)
            {
                if (_eventSink.EventSource.GetEventObject())
                {
                    Paged(this, new DocumentEventArgs(_eventSink.NativeEventObject, this));
                }
            }
        }

        # endregion

        # region Events

        public void InvokeLoseCapture(Interop.IHTMLEventObj e)
        {
            if (LoseCapture != null)
            {
                LoseCapture(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeClick(Interop.IHTMLEventObj e)
        {
            if (Click != null)
            {
                Click(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeDblClick(Interop.IHTMLEventObj e)
        {
            if (DblClick != null)
            {
                DblClick(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeDragStart(Interop.IHTMLEventObj e)
        {
            if (DragStart != null)
            {
                DragStart(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeFocus(Interop.IHTMLEventObj e)
        {
            if (Focus != null)
            {
                Focus(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeDrop(Interop.IHTMLEventObj e)
        {
            if (Drop != null)
            {
                Drop(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeBlur(Interop.IHTMLEventObj e)
        {
            if (Blur != null)
            {
                Blur(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeDragOver(Interop.IHTMLEventObj e)
        {
            if (DragOver != null)
            {
                DragOver(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeDragEnter(Interop.IHTMLEventObj e)
        {
            if (DragEnter != null)
            {
                DragEnter(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeDragLeave(Interop.IHTMLEventObj e)
        {
            if (DragLeave != null)
            {
                DragLeave(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeKeyDown(Interop.IHTMLEventObj e)
        {
            if (KeyDown != null)
            {
                KeyDown(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeKeyPress(Interop.IHTMLEventObj e)
        {
            if (KeyPress != null)
            {
                KeyPress(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeKeyUp(Interop.IHTMLEventObj e)
        {
            if (KeyUp != null)
            {
                KeyUp(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeMouseDown(Interop.IHTMLEventObj e)
        {
            if (MouseDown != null)
            {
                MouseDown(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeResizeStart(Interop.IHTMLEventObj e)
        {
            if (ResizeStart != null)
            {
                ResizeStart(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeResizeEnd(Interop.IHTMLEventObj e)
        {
            if (ResizeEnd != null)
            {
                ResizeEnd(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeMouseEnter(Interop.IHTMLEventObj e)
        {
            if (MouseEnter != null)
            {
                MouseEnter(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeMouseLeave(Interop.IHTMLEventObj e)
        {
            if (MouseLeave != null)
            {
                MouseLeave(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeMouseMove(Interop.IHTMLEventObj e)
        {
            if (MouseMove != null)
            {
                MouseMove(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeMouseOut(Interop.IHTMLEventObj e)
        {
            if (MouseOut != null)
            {
                MouseOut(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeMouseOver(Interop.IHTMLEventObj e)
        {
            if (MouseOver != null)
            {
                MouseOver(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeMouseUp(Interop.IHTMLEventObj e)
        {
            if (MouseUp != null)
            {
                MouseUp(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeSelectStart(Interop.IHTMLEventObj e)
        {
            if (SelectStart != null)
            {
                SelectStart(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeLayoutComplete(Interop.IHTMLEventObj e)
        {
            if (LayoutComplete != null)
            {
                LayoutComplete(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeLoad(Interop.IHTMLEventObj e)
        {
            if (Load != null)
            {
                Load(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeMouseWheel(Interop.IHTMLEventObj e)
        {
            if (MouseWheel != null)
            {
                MouseWheel(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeMoveEnd(Interop.IHTMLEventObj e)
        {
            if (MoveEnd != null)
            {
                MoveEnd(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeMoveStart(Interop.IHTMLEventObj e)
        {
            if (MoveStart != null)
            {
                MoveStart(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeActivate(Interop.IHTMLEventObj e)
        {
            if (Activate != null)
            {
                Activate(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeBeforeActivate(Interop.IHTMLEventObj e)
        {
            if (BeforeActivate != null)
            {
                BeforeActivate(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeBeforeCopy(Interop.IHTMLEventObj e)
        {
            if (BeforeCopy != null)
            {
                BeforeCopy(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeBeforeCut(Interop.IHTMLEventObj e)
        {
            if (BeforeCut != null)
            {
                BeforeCut(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeBeforePaste(Interop.IHTMLEventObj e)
        {
            if (BeforePaste != null)
            {
                BeforePaste(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeContextMenu(Interop.IHTMLEventObj e)
        {
            if (ContextMenu != null)
            {
                ContextMenu(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeCopy(Interop.IHTMLEventObj e)
        {
            if (Copy != null)
            {
                Copy(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeCut(Interop.IHTMLEventObj e)
        {
            if (Cut != null)
            {
                Cut(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeDeactivate(Interop.IHTMLEventObj e)
        {
            if (Deactivate != null)
            {
                Deactivate(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeDrag(Interop.IHTMLEventObj e)
        {
            if (Drag != null)
            {
                Drag(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeDragEnd(Interop.IHTMLEventObj e)
        {
            if (DragEnd != null)
            {
                DragEnd(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeFocusIn(Interop.IHTMLEventObj e)
        {
            if (FocusIn != null)
            {
                FocusIn(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeFocusOut(Interop.IHTMLEventObj e)
        {
            if (FocusOut != null)
            {
                FocusOut(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeFilterChange(Interop.IHTMLEventObj e)
        {
            if (FilterChange != null)
            {
                FilterChange(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeAbort(Interop.IHTMLEventObj e)
        {
            if (Abort != null)
            {
                Abort(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeChange(Interop.IHTMLEventObj e)
        {
            if (Change != null)
            {
                Change(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeSelect(Interop.IHTMLEventObj e)
        {
            if (Select != null)
            {
                Select(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeSelectionChange(Interop.IHTMLEventObj e)
        {
            if (SelectionChange != null)
            {
                SelectionChange(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeStop(Interop.IHTMLEventObj e)
        {
            if (Stop != null)
            {
                Stop(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeBeforeDeactivate(Interop.IHTMLEventObj e)
        {
            if (BeforeDeactivate != null)
            {
                BeforeDeactivate(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeControlSelect(Interop.IHTMLEventObj e)
        {
            if (ControlSelect != null)
            {
                ControlSelect(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeEditFocus(Interop.IHTMLEventObj e)
        {
            if (EditFocus != null)
            {
                EditFocus(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeError(Interop.IHTMLEventObj e)
        {
            if (Error != null)
            {
                Error(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeMove(Interop.IHTMLEventObj e)
        {
            if (Move != null)
            {
                Move(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokePaste(Interop.IHTMLEventObj e)
        {
            if (Paste != null)
            {
                Paste(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokePropertyChange(Interop.IHTMLEventObj e)
        {
            if (PropertyChange != null)
            {
                PropertyChange(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeResize(Interop.IHTMLEventObj e)
        {
            if (Resize != null)
            {
                Resize(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokeScroll(Interop.IHTMLEventObj e)
        {
            if (Scroll != null)
            {
                Scroll(this, new DocumentEventArgs(e, this));
            }
        }
        public void InvokePage(Interop.IHTMLEventObj e)
        {
            if (Paged != null)
            {
                Paged(this, new DocumentEventArgs(e, this));
            }
        }


        # endregion

        # region Access to internal used properties, COM interfaces, base classes

		/// <summary>
		/// Returns the displaypointer as a helper for other internal methods. It is up to the calling
		/// routine to determine 
		/// </summary>
		/// <param name="Adjacency"></param>
		/// <returns></returns>
		internal Interop.IDisplayPointer GetElementsPointer(ElementAdjacency Adjacency)
		{
		    Interop.IMarkupServices ms = (Interop.IMarkupServices) HtmlEditor.GetActiveDocument(false);
		    Interop.IDisplayServices ds = (Interop.IDisplayServices) HtmlEditor.GetActiveDocument(false);
		    Interop.IDisplayPointer dpStart, dpEnd;
			ds.CreateDisplayPointer(out dpStart);
			ds.CreateDisplayPointer(out dpEnd);
		    Interop.IMarkupPointer mpStart, mpEnd;
			ms.CreateMarkupPointer(out mpStart);
			ms.CreateMarkupPointer(out mpEnd);
			if (Adjacency == ElementAdjacency.AfterBegin || Adjacency == ElementAdjacency.AfterEnd)
			{
				mpStart.MoveAdjacentToElement(this.GetBaseElement(), (Adjacency == ElementAdjacency.AfterEnd) ? Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterEnd : Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterBegin);
				dpStart.MoveToMarkupPointer(mpStart, null);
				return dpStart;
			}
			if (Adjacency == ElementAdjacency.BeforeBegin || Adjacency == ElementAdjacency.BeforeEnd)
			{
				mpEnd.MoveAdjacentToElement(this.GetBaseElement(),  (Adjacency == ElementAdjacency.AfterEnd) ? Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeEnd : Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeBegin);
				dpEnd.MoveToMarkupPointer(mpEnd, null);
				return dpEnd;
			}
			return null;
		}

        /// <summary>
        /// Sets or gets a static (shared in VB) field that stores the customized
        /// colors of each color picker control in the PropertyGrid.
        /// </summary>
        [Browsable(false)]
        internal protected static ArrayList CustomColors
        {
            set
            {
                customColors = value;
            }
            get
            {
                if (customColors == null)
                {
                    customColors = new ArrayList();
                }
                return customColors;
            }
        }

        /// <summary>
        /// Access to SetMousePointer method. Used to create behaviors for elements.
        /// </summary>
        /// <param name="Hide">false to show original mouse, true to hide and present elements pointer image</param>
        internal void SetMousePointer(bool Hide)
        {
            this.HtmlEditor.SetMousePointer(Hide);
        }

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

        /// <summary>
        /// Returns a pointer to the document containing this element.
        /// </summary>
        [Browsable(false)]
        internal Interop.IHTMLDocument2 Document
        {
            get
            {
                return HtmlEditor.GetActiveDocument(false);
            }
        }

		/// <summary>
		/// Creates a new element. For internal use only.
		/// </summary>
		/// <param name="tag">Tagname without quotes and brackets</param>
		/// <returns></returns>
		internal Interop.IHTMLElement CreateElement(string tag)
		{
            if (tag.IndexOf(" ") != -1)
            {
                string[] taggy = tag.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Interop.IHTMLElement el = HtmlEditor.GetActiveDocument(false).CreateElement(taggy[0]);
                foreach (string t in taggy)
                {
                    string[] attr = t.Split('=');
                    if (attr.Length == 2)
                    {
                        attr[1] = (attr[1].StartsWith(@"""") ? attr[1].Substring(1) : attr[1]);
                        attr[1] = (attr[1].EndsWith(@"""") ? attr[1].Substring(0, attr[1].Length-1) : attr[1]);
                        el.SetAttribute(attr[0], attr[1], 0);
                    }
                }
                return el;
            }
            else
            {
                return HtmlEditor.GetActiveDocument(false).CreateElement(tag);
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
                    if (Site != null)
                    {
                        uniqueName = Site.Name;
                    }
                    else
                    {
                        //// Build a unique name
                        IDesignerHost host = (IDesignerHost)htmlEditor.ServiceProvider.GetService(typeof(IDesignerHost));
                        INameCreationService nc = (INameCreationService)htmlEditor.ServiceProvider.GetService(typeof(INameCreationService));
                        //string baseName = TagName;
                        //int num = 0;
                        //bool found;
                        //do
                        //{
                        //    num++;
                        //    found = false;
                        //    foreach (IComponent c in host.Container.Components)
                        //    {
                        //        if (c.Site != null && c.Site.Name.Equals(baseName + num))
                        //        {
                        //            found = true;
                        //            break;
                        //        }
                        //    }
                        //} while (found);
                        uniqueName = nc.CreateName(host.Container, GetType());
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
            if (((Control)htmlEditor).InvokeRequired)
            {
                ReplaceAdjacentTextDelegate d = new ReplaceAdjacentTextDelegate(InvokeReplaceAdjacentText);
                ((Control)htmlEditor).Invoke(d, where, text);
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
		public virtual bool ContentEditable
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
            if (((Control)htmlEditor).InvokeRequired)
            {
                SetInnerOuterDelegate d = new SetInnerOuterDelegate(InvokeSetInnerHtml);
                ((Control)htmlEditor).Invoke(d, text);                
            }
            else
            {
                element.SetInnerHTML(text);
            }
        }

        private void InvokeSetInnerText(string text)
        {
            if (((Control)htmlEditor).InvokeRequired)
            {
                SetInnerOuterDelegate d = new SetInnerOuterDelegate(InvokeSetInnerText);
                ((Control)htmlEditor).Invoke(d, text);
            }
            else
            {
                element.SetInnerText(text);
            }
        }

        private void InvokeSetOuterHtml(string text)
        {
            if (((Control)htmlEditor).InvokeRequired)
            {
                SetInnerOuterDelegate d = new SetInnerOuterDelegate(InvokeSetOuterHtml);
                ((Control)htmlEditor).Invoke(d, text);
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
        [Browsable(false)]
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
        public virtual IElement TagElement
        {
            get
            {
                return (IElement) this.Clone();
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
        public new string TagName
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
                if (TagName.IndexOf(":") != -1)
                {
                    return TagName;
                }
                else
                {
                    return String.Format("{0}:{1}", Alias, TagName);
                }
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
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Element Style")]
        [Description("The effective style as a cascaded combination of global, embedded and inline styles. Readonly.")]        
        [System.ComponentModel.DisplayName("Effective Style")]
        public IEffectiveStyle EffectiveStyle
        {
            get
            {
                //if (effectiveStyle == null)
                {
                    Interop.IHTMLCurrentStyle cs = ((Interop.IHTMLElement2)this.GetBaseElement()).GetCurrentStyle();
                    if (cs != null)
                    {
                        effectiveStyle = new CssEffectiveStyle(cs);
                    }
                }
                return effectiveStyle;
            }
        }

        private CssEffectiveStyle effectiveStyle = null;
        private IElementStyle runtimeStyle = null;
        private IElementStyle currentStyle = null;
        
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
        [Browsable(false)]
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
                    Interop.IHTMLStyle cs = ((Interop.IHTMLElement2)this.GetBaseElement()).GetRuntimeStyle();
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
        [DisplayNameAttribute("Current Style")]
        public IElementStyle CurrentStyle
        {
            get
            {            
                if (currentStyle == null) 
                {   
                    Interop.IHTMLStyle cs = this.GetBaseElement().GetStyle();
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

        /// <summary>
        /// Sets the peer element. Not for public use.
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="editor"></param>
        internal protected Element(Interop.IHTMLElement peer, IHtmlEditor editor)
        {
            htmlEditor = editor;
            element = peer;
            Connect();
        }

        public Element() : base()
        {

        }

        public Element(string tagName, IHtmlEditor editor)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor", "Wrong component usage: The Ctor call must have a valid reference to the editor this element belongs to. For internal calls, like one from PropertyGrid, you may 'Site' the design time environment with the Site of the HtmlEditor control. This allows the Designer to call the appropriate services which contain information about the editor.");
            }
            htmlEditor = editor;            
            element = this.CreateElement(tagName);
            Connect();
        }

        private EventSink _eventSink;
        
        internal void Connect()
        {
            _eventSink = new EventSink(this);
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

        ~Element()
        {
            //DisConnect();
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
                Interop.IHTMLElementCollection ec = (Interop.IHTMLElementCollection)element.GetChildren();
                for (int i = 0; i < ec.GetLength(); i++)
                {
                    Interop.IHTMLElement el = (Interop.IHTMLElement)ec.Item(i, i);
                    if (el.GetTagName().ToLower().Equals(name.ToLower()))
                    {
                        return htmlEditor.GenericElementFactory.CreateElement(el);
                    }
                }
                return null;
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
        /// even their children. To get only the direct descendants use <see cref="GuruComponents.Netrix.WebEditing.Elements.ElementDom.GetChildNodes()">ElementDom.GetChildNodes</see> instead.
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
                    elementDom = new ElementDom((Interop.IHTMLDOMNode) element, this.HtmlEditor);
                }
                return elementDom;
            }
        }
        
        /// <summary>
        /// The parent of the current element or null if no parent.
        /// </summary>
        /// <remarks>
        /// Instead of the access in <see cref="ElementDom"/> this property returns the object as base class type
        /// <see cref="System.Web.UI.Control"/>, and therefore accepts types defined in plug-ins, too. <see cref="ElementDom"/>
        /// is a specialized DOM access method implemented especially for <see cref="IElement"/> derived types.
        /// Plug-ins may implement there own version or allow access to the private element DOM elsewhere.
        /// </remarks>
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
        /// Wraps the <see cref="GetParent"/> method. This is the natural (HTML) parent.
        /// </summary>
        /// <remarks>
        /// This overload was added in version 2.0 (2010) second build.
        /// </remarks>
        public override System.Web.UI.Control Parent
        {
            get
            {
                return GetParent();
            }
        }

        /// <summary>
        /// This method takes a full (absolute) URL and returns it as relative URL.
        /// </summary>
        /// <remarks>
        /// This method is internally used to deal with relative paths even if the control
        /// need absolute paths for images and other sources. The base URL which is used to
        /// recognize the relative path is found in the <see cref="GuruComponents.Netrix.HtmlEditor.Url"/>
        /// property.
        /// <para>
        /// If that path set to c:\inetpub\wwwroot\path\ and the absolute path of an image is
        /// c:\inetpub\wwwroot\path\image\xxx.jpg the method will return "image\xxx.jpg". The
        /// PropertyGrid uses this to display only relative parts.
        /// </para>
        /// <para>
        /// Additionally a possibly attached file:// moniker will be removed. Hashmarks (#) at the end of the string
        /// will not harm the resolvation. The method removes them before resolvation and add them at the end.
        /// </para>
        /// </remarks>
        /// <param name="absoluteUrl">The absolute URL of a source.</param>
        /// <returns>The relative part accordingly to the base URL.</returns>
        protected string GetRelativeUrl(string absoluteUrl)
        {
            if (absoluteUrl == null || absoluteUrl.Length == 0)
            {
                return String.Empty;
            }
            string[] hashvalue = absoluteUrl.Split(new char[] {'#'}, 2);
            string str1 = absoluteUrl;
            string hash = String.Empty;
            if (hashvalue.Length == 2)
            {                
                str1 = hashvalue[0];
                hash = String.Concat("#", hashvalue[1]);
            } 
            if (HtmlEditor != null)
            {
                string str2 = (HtmlEditor.Url == null || HtmlEditor.Url.StartsWith("about:")) ? ((HtmlEditor.TempFile.Equals(String.Empty) ? String.Empty : Path.GetDirectoryName(HtmlEditor.TempFile))) : HtmlEditor.Url;
                if (str2 != null && str2.Length > 0)
                {
                    try
                    {
                        Uri uri1 = new Uri(str2);
                        Uri uri2 = new Uri(str1);
                        if (uri1.MakeRelativeUri(uri2).IsAbsoluteUri)
                        {
                            str1 = uri1.MakeRelativeUri(uri2).AbsolutePath;
                        }
						str1 = HttpUtility.UrlDecode(str1);
                    }
                    catch
                    {
                    }
                }
            }
            str1 = fileRegex.Replace(str1, String.Empty, 1);
            if (str1.StartsWith("about:") && !str1.EndsWith(":blank"))
            {
                str1 = str1.Replace("about:", "");
            }
            return String.Concat(str1, hash);
        }

        private static void AddRuleFromCollection(StyleType selectorType, Interop.IHTMLStyleSheetRulesCollection rules, ref List<string> selector)
        {
            for (int i = 0; i < rules.GetLength(); i++)
            {
                Interop.IHTMLStyleSheetRule rule = rules.Item(i);
                if (rule.GetSelectorText().Length == 0) continue;
                // first char determines type (. = class, # = id, @ = pseudo)
                if (selectorType == StyleType.Class && rule.GetSelectorText()[0].Equals('.'))
                {
                    selector.Add(rule.GetSelectorText().Substring(1));
                    continue;
                }
                if (selectorType == StyleType.Id && rule.GetSelectorText()[0].Equals('#'))
                {
                    selector.Add(rule.GetSelectorText().Substring(1));
                    continue;
                }
                if (selectorType == StyleType.Pseudo && rule.GetSelectorText()[0].Equals('@'))
                {
                    selector.Add(rule.GetSelectorText().Substring(1));
                    continue;
                }
            }
        }

        /// <summary>
        /// This method return all style selectors linkes with the current document this element is in.
        /// </summary>
        /// <remarks>
        /// The type will by recognized by the sign typically used for selectors, like ".class", "#id", and "@rule".
        /// </remarks>
        /// <param name="selectorType">String with the type</param>
        /// <returns>Array of objects</returns>
        [Drop()]
        public string[] GetDocumentStyleSelectors(StyleType selectorType)
        {
            List<string> selector = new List<string>();
            Interop.IHTMLStyleSheetRulesCollection rules;
            Interop.IHTMLStyleSheet ssh;
            // search linked styles
            LinkElementCollection stylesL = this.HtmlEditor.DocumentStructure.LinkedStylesheets as LinkElementCollection;
            if (stylesL != null && stylesL.Count > 0)
            {
                for (int s = 0; s < stylesL.Count; s++)
                {
                    LinkElement link = stylesL[s];
                    if (link != null)
                    {
                        Interop.IHTMLLinkElement le;
                        le = (Interop.IHTMLLinkElement) link.GetBaseElement();
                        ssh = le.styleSheet;
                        rules = ssh.GetRules();
                        if (rules.GetLength() > 0)
                        {
                            AddRuleFromCollection(selectorType, rules, ref selector);
                        } 
                    }
                }
            }
            // search embedded styles
            StyleElementCollection stylesS = this.HtmlEditor.DocumentStructure.EmbeddedStylesheets as StyleElementCollection;
            if (stylesS != null && stylesS.Count > 0)
            {
                for (int s = 0; s < stylesS.Count; s++)
                {
                    StyleElement style = stylesS[s];
                    if (style != null)
                    {
                        Interop.IHTMLStyleElement se;
                        se = (Interop.IHTMLStyleElement) style.GetBaseElement();
                        ssh = se.styleSheet;
                        rules = ssh.GetRules();
                        if (rules.GetLength() > 0)
                        {
                            AddRuleFromCollection(selectorType, rules, ref selector);
                        } 
                    }
                }
            }
            if (selector.Count > 0)
            {
                return selector.ToArray();
            } 
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// Gets true if the element can positioned freely on the surface by using DHTML.
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
        public virtual string GetStyle()
        {
            Interop.IHTMLStyle style = GetBaseElement().GetStyle();
            return style.GetCssText();
        }

        /// <summary>
        /// Set the current style by overwriting the complete content of the style attribute.
        /// </summary>
        /// <param name="CssText">The style text; should be CSS compliant.</param>
        public virtual void SetStyle(string CssText)
        {
            if (htmlEditor.ThreadSafety)
            {
                InvokeSetStyle(CssText);
            }
            else
            {
                Interop.IHTMLStyle style = GetBaseElement().GetStyle();
                style.SetCssText(CssText);
            }
        }

        private delegate void SetStyleDelegate(string css);
        private delegate void SetStyleAttributeDelegate(string styleName, string styleText);

        private void InvokeSetStyle(string CssText)
        {
            if (((Control)htmlEditor).InvokeRequired)
            {
                SetStyleDelegate d = new SetStyleDelegate(InvokeSetStyle);
                ((Control)htmlEditor).Invoke(d, CssText);
            }
            else
            {
                element.GetStyle().SetCssText(CssText);
            }
        }

        private void InvokeSetStyleAttribute(string styleName, string styleText)
        {
            if (String.IsNullOrEmpty(styleText)) return;
            if (((Control)htmlEditor).InvokeRequired)
            {
                SetStyleAttributeDelegate d = new SetStyleAttributeDelegate(InvokeSetStyleAttribute);
                ((Control)htmlEditor).Invoke(d, styleName, styleText);
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
            Interop.IHTMLStyle style = GetBaseElement().GetStyle();
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
            try
            {
                Interop.IHTMLStyle style = GetBaseElement().GetStyle();
                if (styleName.IndexOf("-") != -1)
                {
                    styleName = styleName.Replace("-", "");
                }
                style.RemoveAttribute(styleName, 0);
            }
            catch
            {
            }
        }

        #region GetAttr

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
            catch (Exception)
            {
                return null;
            }
        }

        private object InvokeGetAttribute(string attr)
        {
            if (((Control)htmlEditor).InvokeRequired)
            {
                GetAttributeDelegate d = new GetAttributeDelegate(InvokeGetAttribute);
                return ((Control)htmlEditor).Invoke(d, new object[] { attr });
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
            if (!htmlEditor.DesignModeEnabled) return null;
            object local2;
            attribute = (attribute.Equals("http-equiv")) ? "httpequiv" : attribute;
            try
            {
                // special treatment
                switch (attribute.ToLower())
                {
                    case "lang":
                        return element.GetLang();
                    case "class":
                        return element.GetClassName();
                    case "id":
                        return element.GetId();
                    default:
                        object[] locals = new object[1];
                        locals[0] = null;
                        element.GetAttribute(attribute, 0, locals);
                        object local1 = locals[0];
                        if (local1 is DBNull)
                        {
                            local1 = null;
                        }
                        local2 = local1;
                        break;
                }

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
        /// Returns an attribute's value which is supposedly boolean.
        /// </summary>
        /// <param name="attribute">Name of attribute.</param>
        /// <returns>Boolean representation of value.</returns>
        protected internal virtual bool GetBooleanAttribute(string attribute)
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
        /// Returns an attribute's value which is supposedly a color.
        /// </summary>
        /// <param name="attribute">Name of attribute.</param>
        /// <returns>Color representation of value.</returns>
        protected internal virtual Color GetColorAttribute(string attribute)
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
        /// Returns an attribute's value which is supposedly enum.
        /// </summary>
        /// <param name="attribute">Name of attribute.</param>
        /// <returns>Enum representation of value.</returns>
        protected internal virtual Enum GetEnumAttribute(string attribute, Enum defaultValue)
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
        /// Returns an attribute's value which is supposedly int.
        /// </summary>
        /// <param name="attribute">Name of attribute.</param>
        /// <returns>Int representation of value.</returns>
        protected internal virtual int GetIntegerAttribute(string attribute, int defaultValue)
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
        /// Returns an attribute's value which is supposedly string.
        /// </summary>
        /// <param name="attribute">Name of attribute.</param>
        /// <returns>String representation of value.</returns>
        protected internal virtual string GetStringAttribute(string attribute)
        {
            return GetStringAttribute(attribute, String.Empty);
        }

        /// <summary>
        /// Returns an attribute's value which is supposedly string.
        /// </summary>
        /// <param name="attribute">Name of attribute.</param>
        /// <returns>String representation of value.</returns>
        protected internal virtual string GetStringAttribute(string attribute, string defaultValue)
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
        /// Returns an attribute's value which is supposedly a Unit.
        /// </summary>
        /// <param name="attribute">Name of attribute.</param>
        /// <returns>Unit representation of value.</returns>
        protected internal virtual Unit GetUnitAttribute(string attribute)
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
                        switch (attribute.ToLower())
                        {
                            case "class":
                                element.RemoveAttribute("className", 0);
                                break;
                            default:
                                element.RemoveAttribute(attribute, 0);
                                break;
                        }
                        
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
            if (((Control)htmlEditor).InvokeRequired)
            {
                RemoveAttributeDelegate d = new RemoveAttributeDelegate(InvokeRemoveAttribute);
                ((Control)htmlEditor).Invoke(d, new object[] { attr });
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
        /// is turned on. 
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
                    switch (attribute.ToLower())
                    {
                        case "class":
                            if (value != null)
                            {
                                if (!String.IsNullOrEmpty(value.ToString()))
                                {
                                    element.SetClassName(value.ToString());
                                }
                            }
                            break;
                        case "id":
                            if (value != null)
                            {
                                if (!String.IsNullOrEmpty(value.ToString()))
                                {
                                    element.SetId(value.ToString());
                                }
                            }
                            break;
                        default:
                            element.SetAttribute(attribute, value, 0);
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private delegate void SetAttributeDelegate(string attr, object value);

        private void InvokeSetAttribute(string attr, object value)
        {
            if (((Control)htmlEditor).InvokeRequired)
            {
                SetAttributeDelegate d = new SetAttributeDelegate(InvokeSetAttribute);
                ((Control)htmlEditor).Invoke(d, new object[] { attr, value });
            }
            else
            {
                switch (attr.ToLower())
                {
                    case "class":
                        if (value != null)
                        {
                            if (!String.IsNullOrEmpty(value.ToString()))
                            {
                                element.SetClassName(value.ToString());
                            }
                        }
                        break;
                    case "id":
                        if (value != null)
                        {
                            if (!String.IsNullOrEmpty(value.ToString()))
                            {
                                element.SetId(value.ToString());
                            }
                        }
                        break;
                    default:
                        element.SetAttribute(attr, value, 0);
                        break;
                }

            }
        }

        #endregion SetAttribute

        # region SpecializedSetAttr

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

        protected internal void SetColorAttribute(string attribute, Color value)
        {
            if (value.IsEmpty)
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, ColorTranslator.ToHtml(value));
        }

        protected internal void SetEnumAttribute(string attribute, Enum value, Enum defaultValue)
        {
            if (value.Equals(defaultValue))
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, value.ToString());
        }

        protected internal void SetIntegerAttribute(string attribute, int value, int defaultValue)
        {
            if (value == defaultValue)
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, value);
        }

        protected internal void SetStringAttribute(string attribute, string value)
        {
            SetStringAttribute(attribute, value, String.Empty);
        }

        protected internal void SetStringAttribute(string attribute, string value, string defaultValue)
        {
            if (value == null || value.Equals(defaultValue))
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, value);
        }

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

        #region ICloneable Member

        /// <summary>
        /// We need this to prevent callers (like comboBox) using elements directly from deleting the
        /// original object. This makes a shallow copy, which let the internal references untouched.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        #region ICustomTypeDescriptor Member

        PropertyDescriptorCollection pdc = null;
        EventDescriptorCollection edc = null;

        private PropertyDescriptorCollection GetFilteredProperties(Attribute[] filter)
        {
            Type t = GetType();
            PropertyDescriptorCollection currentPdc;
            if (pdc == null)
            {
                PropertyDescriptorCollection baseProps;
                try
                {
                    baseProps = TypeDescriptor.GetProperties(t, filter);
                }
                catch
                {
                    baseProps = TypeDescriptor.GetProperties(t, filter);
                }
                // notice we use the type here so we don't recurse 
                List<PropertyDescriptor> pdcs = new List<PropertyDescriptor>();
                for (int i = 0; i < baseProps.Count; i++)
                {
                    PropertyDescriptor pd = new CustomPropertyDescriptor(baseProps[i], filter);
                    if (pd.ComponentType == typeof(WebControl) || pd.ComponentType == typeof(Control)) continue;
                    PropertyFilterEventArgs eargs = new PropertyFilterEventArgs(this, pd);
                    ((HtmlEditor)HtmlEditor).OnPropertyFilterRequest(eargs);
                    if (eargs.Cancel == false)
                    {
                        pdcs.Add(pd);
                    }
                }
                if (pdcs.Count > 0)
                {                    
                    pdc = new PropertyDescriptorCollection(pdcs.ToArray());
                }
            }
            currentPdc = pdc;
            PropertyDisplayEventArgs pea = new PropertyDisplayEventArgs(this, currentPdc);
            ((HtmlEditor)HtmlEditor).OnPropertyDisplayRequest(pea);
            // force reload on each request
            if (pea.DescriptorCollection != null && !pea.DescriptorCollection.Equals(currentPdc))
            {
                currentPdc = pdc = pea.DescriptorCollection;
            }
            if (pea.ResetPropertyFilterList)
            {
                pdc = null;
            }
            return currentPdc;
        }

        private EventDescriptorCollection GetFilteredEvents(Attribute[] filter)
        {
            Type t = GetType();
            EventDescriptorCollection currentEdc;
            if (edc == null)
            {
                EventDescriptorCollection baseProps;
                baseProps = TypeDescriptor.GetEvents(t, filter);
                // notice we use the type here so we don't recurse 
                List<EventDescriptor> pdcs = new List<EventDescriptor>();
                for (int i = 0; i < baseProps.Count; i++)
                {
                    EventDescriptor pd = baseProps[i];
                    if (pd.ComponentType == typeof(WebControl) || pd.ComponentType == typeof(Control)) continue;
                    EventFilterEventArgs eargs = new EventFilterEventArgs(this, pd);
                    ((HtmlEditor)HtmlEditor).OnEventFilterRequest(eargs);
                    if (eargs.Cancel == false)
                    {
                        pdcs.Add(pd);
                    }
                }
                if (pdcs.Count > 0)
                {
                    edc = new EventDescriptorCollection(pdcs.ToArray());
                }
            }
            currentEdc = edc;
            EventDisplayEventArgs pea = new EventDisplayEventArgs(this, currentEdc);
            ((HtmlEditor)HtmlEditor).OnEventDisplayRequest(pea);
            // force reload on each request
            if (pea.DescriptorCollection != null && !pea.DescriptorCollection.Equals(currentEdc))
            {
                currentEdc = edc = pea.DescriptorCollection;
            }
            if (pea.ResetEventFilterList)
            {
                edc = null;
            }
            return currentEdc;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return GetFilteredProperties(null);
        } 

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] filter) 
        {
            return GetFilteredProperties(filter);
        } 
 
        System.ComponentModel.AttributeCollection ICustomTypeDescriptor.GetAttributes() 
        { 
            // TODO: Get element extender attributes here
            return TypeDescriptor.GetAttributes(this, true); 
        } 
 
        string ICustomTypeDescriptor.GetClassName() 
        { 
            return TypeDescriptor.GetClassName(this, true); 
        } 
 
        string ICustomTypeDescriptor.GetComponentName() 
        { 
            return TypeDescriptor.GetComponentName(this, true); 
        } 
 
        TypeConverter ICustomTypeDescriptor.GetConverter() 
        { 
            return TypeDescriptor.GetConverter(this, true); 
        } 
 
        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() 
        { 
            return TypeDescriptor.GetDefaultEvent(this, true); 
        } 
 
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] filter) 
        {
            return GetFilteredEvents(filter); 
        } 
 
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents() 
        { 
            return null; 
        } 
 
        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() 
        { 
            return TypeDescriptor.GetDefaultProperty(this, true); 
        } 
 
        object ICustomTypeDescriptor.GetEditor(Type editorBaseType) 
        { 
            return TypeDescriptor.GetEditor(this, editorBaseType, true); 
        } 
 
        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) 
        { 
            return this; 
        } 
 
        #endregion

        #region IComponent Member

        public new event EventHandler Disposed;

        ///// <summary>
        ///// Access to Site, if the component is sited.
        ///// </summary>
        ///// <remarks>
        ///// The designer will normally site any component upon creation to have access
        ///// to common services from any point.
        ///// </remarks>
        //[Browsable(false)]
        //public ISite Site
        //{
        //    get
        //    {
        //        return base.Site;
        //    }
        //    set
        //    {
        //        base.Site = value;
        //    }
        //}

        #endregion

        #region IDisposable Member

        /// <summary>
        /// Shows that the element has been disposed.
        /// </summary>
        /// <remarks>
        /// After loading a document, changing <see cref="GuruComponents.Netrix.HtmlEditor.InnerHtml">InnerHtml</see> of the document
        /// or clearing the designer collections each element object is disposed. To avoid accessing you could check
        /// this property and retrieve a new instance calling the ElementFactory.
        /// </remarks>
        [Browsable(false)]
        public new bool IsDisposed = false;

        /// <summary>
        /// Internally called by element factory to detach event handlers and dispose the object.
        /// </summary>
        public new void Dispose()
        {
            IsDisposed = true;
            if (_eventSink != null)
            {
                _eventSink.Disconnect();
            }            
            extendedProperties = null;
            elementBehavior = null;
            if (Disposed != null)
            {
                Disposed(this, EventArgs.Empty);
            }
        }

        #endregion

    }
}