using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// This class provides event arguments for DHTML events.
    /// </summary>
    /// <remarks>
    /// These events are fired when an .NET event handler is attached to a DHTML event during design time.
    /// </remarks>
    public class DocumentEventArgs : CancelEventArgs
    {
        private Point clientXY;
        private Point screenXY;
        private MouseButtons button;
        private IElement sourceElement = null;
        private bool ctrlKey, altKey, shftKey;
        private Keys keyCode;
        private string type;
        
        private Interop.IHTMLEventObj referenceObject;

        /// <summary>
        /// Internally used ctor.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sourceElement"></param>
        public DocumentEventArgs(Interop.IHTMLEventObj e, IElement sourceElement)
        {            
			//System.Diagnostics.Debug.Assert(e != null, "NULL Event E");
			//System.Diagnostics.Debug.Assert(sourceElement != null, "NULL Event SRC");
            referenceObject = e;			
            this.sourceElement = sourceElement;
			if (e != null)
			{
				clientXY = new Point(e.clientX, e.clientY);
				screenXY = new Point(e.screenX, e.screenY);
				switch (e.button)
				{
					default:
					case 0:
						button = Control.MouseButtons;
						break;
					case 1:
						button = MouseButtons.Left;
						break;
					case 2:
						button = MouseButtons.Right;
						break;
					case 3:
						button = MouseButtons.Left | MouseButtons.Right;
						break;
					case 4:
						button = MouseButtons.Middle;
						break;
					case 5:
						button = MouseButtons.Left | MouseButtons.Middle;
						break;
					case 6:
						button = MouseButtons.Right | MouseButtons.Middle;
						break;
					case 7:
						button = MouseButtons.Left | MouseButtons.Middle | MouseButtons.Right;
						break;
				}
				ctrlKey = e.ctrlKey;
				shftKey = e.shiftKey;
				altKey = e.altKey;
				keyCode = ((Keys) e.keyCode) | ((altKey) ? Keys.Alt : Keys.None) | ((ctrlKey) ? Keys.Control : Keys.None) | ((shftKey) ? Keys.Shift : Keys.None);
				type = e.type.ToLower();
			}
        }

        /// <summary>
        /// Set the event bubble chain cancel.
        /// </summary>
        /// <param name="val"></param>
        public void SetCancelBubble(bool val)
        {
            referenceObject.cancelBubble = val;
        }

        /// <summary>
        /// Set or overwrite the return value.
        /// </summary>
        /// <param name="val"></param>
        public void SetReturnValue(bool val)
        {
            referenceObject.returnValue  = val;
        }

        # region Public Properties

		/// <summary>
		/// The property which was changed.
		/// </summary>
		/// <remarks>
		/// This property applies only when OnPropertyChange event has been fired.
		/// </remarks>
		public string PropertyName
		{
			get
			{
				return ((Interop.IHTMLEventObj2)referenceObject).propertyName;
			}
		}

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public new bool Cancel
        {
            get
            {
                return base.Cancel;
            }
            set
            {
                base.Cancel = value;
                if (referenceObject != null)
                {
                    referenceObject.returnValue = !base.Cancel;
                    referenceObject.cancelBubble = base.Cancel;
                }
            }
        }

        /// <summary>
        /// Mousebutton was clicked during event. 
        /// </summary>
        public MouseButtons MouseButton
        {
            get
            {
                return this.button;
            }
        }

        /// <summary>
        /// The element causes the event.
        /// </summary>
        public IElement SrcElement
        {
            get
            {
                return this.sourceElement;
            }
        }

        /// <summary>
        /// The position of mouse pointer in client area.
        /// </summary>
        /// <remarks>
        /// Retrieves the coordinates of the mouse pointer's position relative to the client area of the window, excluding window decorations and scroll bars.
        /// </remarks>
        public Point ClientXY
        {
            get
            {
                return this.clientXY;
            }
        }

        /// <summary>
        /// The position of mouse pointer in screen coordinates.
        /// </summary>
        /// <remarks>
        /// Retrieves the coordinates of the mouse pointer's position relative to the user's screen.
        /// </remarks>
        public Point ScreenXY
        {
            get
            {
                return this.screenXY;
            }
        }

        /// <summary>
        /// The state of the shift key during mouse events.
        /// </summary>
        public bool ShiftKey
        {
            get
            {
                return this.shftKey;
            }
        }
        /// <summary>
        /// The state of the alt key during mouse events.
        /// </summary>
        public bool AltKey
        {
            get
            {
                return this.altKey;
            }
        }

		/// <summary>
		/// Retrieves a value that indicates the state of the left ALT key.
		/// </summary>
		public bool LeftAltKey
		{
			get
			{
				return ((Interop.IHTMLEventObj3)this.referenceObject).altLeft;
			}
		}

		/// <summary>
		/// Retrieves a value that indicates the state of the left SHIFT key.
		/// </summary>
		public bool LeftShftKey
		{
			get
			{
				return ((Interop.IHTMLEventObj3)this.referenceObject).shiftLeft;
			}
		}

		/// <summary>
		/// Retrieves the distance and direction the wheel button has rolled.
		/// </summary>
		/// <remarks>
		/// This property indicates the distance that the wheel has rotated, expressed in multiples of 120. A positive value indicates 
		/// that the wheel has rotated away from the user. A negative value indicates that the wheel has rotated toward the user.
		/// </remarks>
		public int WheelButton
		{
			get
			{
				return ((Interop.IHTMLEventObj4)this.referenceObject).wheelDelta;
			}
		}

        /// <summary>
        /// The state of the control key during mouse events.
        /// </summary>
        public bool ControlKey
        {
            get
            {
                return this.ctrlKey;
            }
        }

		/// <summary>
		/// The state of the left CTRL key.
		/// </summary>
		public bool LeftCtrlKey
		{
			get
			{
				return ((Interop.IHTMLEventObj3)this.referenceObject).ctrlLeft;
			}
		}


        /// <summary>
        /// The keycode if a key causes the event.
        /// </summary>
        public Keys KeyCode
        {
            get
            {
                return this.keyCode;
            }
        }

        /// <summary>
        /// The event type fired the event.
        /// </summary>
        public DocumentEventType EventType
        {
            get
            {
				try
				{
					return (DocumentEventType) Enum.Parse(typeof(DocumentEventType), type, true);
				}
				catch
				{
					return DocumentEventType.Unknown;
				}
            }
        }

		/// <summary>
		/// Retrieves a data object that contains information about dragged or copied data.
		/// </summary>
		/// <remarks>
		/// This data object can be used to change or retrieve the data being involved in the current
		/// drag or copy operation which causes the event this object is attached to. 
		/// </remarks>
		public DataTransfer ClipboardData
		{
			get
			{
				return new DataTransfer(((Interop.IHTMLEventObj2) referenceObject).dataTransfer);
			}
		}

        # endregion

		# region Subclass

		/// <summary>
		/// This class provides access to predefined clipboard formats for use in data transfer operations.
		/// </summary>
		public class DataTransfer : Interop.IHTMLDataTransfer
		{

			private Interop.IHTMLDataTransfer native;

			internal DataTransfer(Interop.IHTMLDataTransfer native)
			{
				this.native = native;
			}

			#region IHTMLDataTransfer Members

			/// <summary>
			/// Assigns data in a specified format to the dataTransfer or clipboardData object.
			/// </summary>
			/// <param name="format"></param>
			/// <param name="data"></param>
			/// <returns></returns>
			public Boolean setData(String format, Object data)
			{
				return native.setData(format, data);
			}

			/// <summary>
			/// Retrieves the data in the specified format from the clipboard through the dataTransfer or clipboardData objects.
			/// </summary>
			/// <param name="format"></param>
			/// <returns></returns>
			public Object getData(String format)
			{
				return native.getData(format);
			}

			/// <summary>
			/// Removes one or more data formats from the clipboard through dataTransfer or clipboardData object.
			/// </summary>
			/// <param name="format"></param>
			/// <returns></returns>
			public Boolean clearData(String format)
			{
				return native.clearData(format);
			}

			/// <summary>
			/// Sets or retrieves the type of drag-and-drop operation and the type of cursor to display.
			/// </summary>
			public String dropEffect
			{
				get
				{
					return native.dropEffect;
				}
				set
				{
					native.dropEffect = value;
				}
			}

			/// <summary>
			/// Sets or retrieves, on the source element, which data transfer operations are allowed for the object.
			/// </summary>
			public String effectAllowed
			{
				get
				{
					return native.effectAllowed;
				}
				set
				{
					native.effectAllowed = value;
				}
			}

			#endregion

		}


		# endregion

    }
}