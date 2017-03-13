using System;
using System.Drawing;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.Events;
using System.Web.UI;
using System.Runtime.InteropServices;

namespace GuruComponents.Netrix.WebEditing.Documents
{
    /// <summary>
    /// HtmlWindow allows basic access to window and document related features outside the designer scope.
    /// </summary>
    /// <remarks>
    /// The class is being used to access scripting sections, execute JavaScript and open embedded 
    /// scripting dialogs.
    /// </remarks>
    public class HtmlWindow : IHtmlWindow, IDisposable
    {
        private Interop.IHTMLWindow2 window;
        private Interop.IHTMLDocument2 doc;
        private IHtmlEditor editor;

        internal HtmlWindow(Interop.IHTMLWindow2 window, IHtmlEditor editor)
        {
            this.editor = editor;
            try
            {
                this.window = window.parent;
                doc = window.document;
            }
            catch
            {
            }
        }

        private Interop.IHTMLWindow3 window3
        {
            get
            {
                return ((Interop.IHTMLWindow3)this.window);
            }
        }

        private Interop.IHTMLWindow4 window4
        {
            get
            {
                return ((Interop.IHTMLWindow4)this.window);
            }
        }

        # region document

        //        public void GetXX()
        //        {
        //           
        //            string s = doc.GetSecurity();
        //            object o = doc.GetScript();
        //
        //            System.Diagnostics.Debug.WriteLine("GetXX");
        //        }
        //
        //        /// <overloads/>
        //        /// <summary>
        //        /// Writes one or more HTML expressions to the document.
        //        /// </summary>
        //        /// <remarks>
        //        /// This methods writes the text at the current caret position or at the beginning
        //        /// of the document. Use <see cref="GuruComponents.Netrix.WebEditing.HighLighting.TextSelector">TextSelector</see> to move the caret (insertion point)
        //        /// to a specific location.
        //        /// </remarks>
        //        /// <param name="text">Specifies the text and HTML tags to write.</param>
        //        public void Write(string text)
        //        {
        //            Write(text, false);                
        //        }
        //
        //        /// <summary>
        //        /// Writes one or more HTML expressions to the document.
        //        /// </summary>
        //        /// <remarks>
        //        /// When writing carriage return it might have no visible effect, because carriage returns has
        //        /// meaning in HTML rendering. However, the CR is preserved in the underlying document.
        //        /// </remarks>
        //        /// <param name="text">Specifies the text and HTML tags to write.</param>
        //        /// <param name="withNewLine">Writes one or more HTML expressions, followed by a carriage return.</param>
        //        public void Write(string text, bool withNewLine)
        //        {
        //            if (withNewLine)
        //            {
        //                doc.Writeln(text);
        //            } 
        //            else 
        //            {
        //                doc.Write(text);
        //            }
        //        }

        /// <summary>
        /// Returns the element for the specified x and y coordinates.
        /// </summary>
        /// <remarks>
        /// Coordinates are supplied in client coordinates. The top left corner of the client area is (0,0). 
        /// For this method to exhibit expected behavior, the element located at position (x, y) must 
        /// support and respond to mouse events.
        /// <para>
        /// The parameter accepts values directly exposed by Mouse event handlers, such as MouseMove.
        /// There is no need to convert to Client as well as Screen coordinates or whatever.
        /// </para>
        /// <para>
        /// If element was not found or not recognized, the method returns the Body element. However, it
        /// returns <c>null</c> (<c>Nothing</c> in Visual Basic) if the element was unknow to the list of registered
        /// elements. This happens, for instance, for VML elements, which indeed render in MSHTML, but 
        /// are not supported by NetRix v1. NetRix v2 and newer which have extensible element support
        /// will return the native element object, then.
        /// </para>
        /// </remarks>
        /// <param name="point">X- and Y-offset, in pixels.</param>
        /// <returns>Element for the specified x and y coordinates. Returns <c>null</c> (<c>Nothing</c> in Visual Basic) if the element was unknow.</returns>
        public Control GetElementFromPoint(Point point)
        {
            Interop.IHTMLElement el = doc.ElementFromPoint(point.X, point.Y);
            if (el != null)
            {
                return editor.GenericElementFactory.CreateElement(el);
            }
            else
            {
                return editor.GetBodyElement() as Control;
            }
        }

        /// <summary>
        /// Returns the element which is directly under the caret.
        /// </summary>
        /// <remarks>
        /// In case of invisible caret this method may return unpredictable results.
        /// <seealso cref="GetElementFromPoint"/>
        /// </remarks>
        /// <returns>Returns IElement object from caret.</returns>
        public Control GetElementFromCaret()
        {
            return GetElementFromPoint(GetCaretLocation());
        }

        /// <summary>
        /// Return the current location of the caret within the editor.
        /// </summary>
        /// <returns>Return the position in client coordinates.</returns>
        public Point GetCaretLocation()
        {
            Interop.IDisplayServices ids = editor.GetActiveDocument(false) as Interop.IDisplayServices;
            if (ids != null)
            {
                Interop.IHTMLCaret caret;
                ids.GetCaret(out caret);
                if (caret != null)
                {
                    Interop.POINT caretPoint = new Interop.POINT();
                    caret.GetLocation(ref caretPoint, false);
                    Interop.IDisplayPointer dp;
                    ids.CreateDisplayPointer(out dp);
                    caret.MoveDisplayPointerToCaret(dp);
                    Interop.IHTMLElement eel;
                    dp.GetFlowElement(out eel);
                    // if possible try to transform rectangular elements into their parent scope
                    if (eel != null)
                    {
                        ids.TransformPoint(ref caretPoint, Interop.COORD_SYSTEM.COORD_SYSTEM_CONTENT, Interop.COORD_SYSTEM.COORD_SYSTEM_GLOBAL, eel);
                    }
                    return new Point(caretPoint.x, caretPoint.y);
                }
            }
            return Point.Empty;
        }

        /// <summary>
        /// Retrieves the element that has the focus when the document has focus.
        /// </summary>
        /// <returns>Element of type <see cref="IElement"/> interface that is the element that has the focus.</returns>
        public IElement GetActiveElement()
        {
            Interop.IHTMLElement el = doc.GetActiveElement();
            return editor.GenericElementFactory.CreateElement(el) as IElement;
        }

        # endregion

        # region Window

        /// <summary>
        /// Fires a specified script event on the given object.
        /// </summary>
        /// <remarks>
        /// In difference to the <c>fireEvent</c> object the underlying IHTMLElement class provides it is always 
        /// necessary to provide a valid <see cref="IElement"/> object. The method call will fail if the element is
        /// not reletad to the current window or <c>null</c> (<c>Nothing</c> in Visual Basic).
        /// <para>
        /// If the event being fired cannot be cancelled, the method always returns <c>true</c>. 
        /// </para>
        /// <para>
        /// This method is available at runtime only. At design time it returns always true, but the event is not
        /// getting executed. To switch to runtime just set <see cref="GuruComponents.Netrix.HtmlEditor.DesignModeEnabled">DesignModeEnabled</see> to <c>false</c>.
        /// Remember to wait for <see cref="GuruComponents.Netrix.HtmlEditor.OnReadyStateComplete">OnReadyStateComplete</see>
        /// event before executing script calls after changing the document mode.
        /// </para>
        /// <para>
        /// The script code can be either JavaScript/JScript or VBScript.
        /// </para>
        /// <para>
        /// Regardless of their values specified in the event object, the values of the four event properties—cancelBubble, returnValue, srcElement, and type—are automatically initialized to the values shown in the following table:
        /// <list type="table">
        /// <listheader><item>Event object property</item><item>Value</item></listheader>
        /// <item>
        ///     <term>cancelBubble</term><term>false</term>
        /// </item>
        /// <item>
        ///     <term>returnValue</term><term>true</term>
        /// </item>
        /// <item>
        ///     <term>srcElement</term><term>element on which the event is fired</term>
        /// </item>
        /// <item>
        ///     <term>type</term><term>name of the event that is fired</term>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// IElement element = htmlEditor1.GetElementById("button1");
        /// bool success = FireScriptEvent("OnClick", element);
        /// </code>
        /// </example>
        /// <param name="eventName">String that specifies the name of the event to fire.</param>
        /// <param name="element">Object that specifies the event object from which to obtain event object properties.</param>
        /// <returns>Returns <c>true</c> if the event fired successfully.</returns>
        public bool FireScriptEvent(string eventName, IElement element)
        {
            Interop.IHTMLElement el = ((Element)element).GetBaseElement();
            try
            {
                Interop.IHTMLDocument4 doc4 = (Interop.IHTMLDocument4)window.document;
                // 
                Interop.IHTMLEventObj eventObject = doc4.createEventObject(null);
                bool result = ((Interop.IHTMLElement3)el).fireEvent(eventName, ref eventObject);

                return result;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the absolute position of the screen this windows has occupied.
        /// </summary>
        /// <remarks>
        /// Retrieves the coordinates of the upper corner of the browser's client area, relative to the upper corner of the screen.
        /// </remarks>
        public Point ScreenPosition
        {
            get
            {
                return new Point(window3.screenLeft, window3.screenTop);
            }
        }

        /// <overloads/>
        /// <summary>
        /// Executes the specified script in the provided language.
        /// </summary>
        /// <remarks>
        /// Script executed through the IWindow.execScript method can access all global variables available to the calling script. This can be useful when you want the functionality of another scripting language that would not otherwise be available in JScript, such as the Microsoft Visual Basic Scripting Edition (VBScript) MsgBox function.
        /// <para>
        /// This method is available at runtime as well as at design time.
        /// </para>
        /// </remarks>
        /// <param name="scriptCode">Value that specifies the code to be executed.</param>
        public void ExecScript(string scriptCode)
        {
            ExecScript(scriptCode, "JScript");
        }

        private object objectForScripting;

        /// <summary>
        /// Define an object that is being used as call back from JScript using the 'external' property.
        /// </summary>
        /// <remarks>
        /// First, decorate a class with callback methods with attributes as shown below. Then assign
        /// in ReadyStateComplete event handler. Load JScript code and execute. Control must be in 
        /// Non design mode
        /// <code>
        /// [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        /// [System.Runtime.InteropServices.ComVisibleAttribute(true)]
        /// public class JCall
        /// {
        ///     public void MyTest()
        ///     {
        ///        MessageBox.Show("Test called", "Testbox");
        ///     }
        /// }
        /// private void htmlEditor1_ReadyStateComplete(object sender, EventArgs e)
        /// {
        ///     htmlEditor1.Window.ScriptError += new GuruComponents.Netrix.Events.ShowErrorHandler(Window_ScriptError);
        ///     htmlEditor1.Window.ScriptMessage += new GuruComponents.Netrix.Events.ShowMessageHandler(Window_ScriptMessage);
        ///     htmlEditor1.Window.ObjectForScripting = new JCall();
        /// {
        /// // This is in InitializeComponent():
        /// htmlEditor1.DesignModeEnabled = false;
        /// // This is an action invokes by menu call ==> Click on Button in HTML calls C# method MyTest in class JCall
        /// htmlEditor1.LoadHtml("&lt;button onclick='window.external.MyTest()' /&gt;");
        /// </code>
        /// </remarks>
        public object ObjectForScripting
        {
            get
            {
                return this.objectForScripting;
            }
            set
            {
                if ((value != null) && !Marshal.IsTypeVisibleFromCom(value.GetType()))
                {
                    throw new ArgumentException();
                }
                this.objectForScripting = value;
            }
        }

        public object CallScript(string scriptName)
        {
            return CallScript(scriptName, null);
        }

        public object CallScript(string scriptName, object[] args)
        {
            object obj2 = null;
            Interop.DISPPARAMS pDispParams = new Interop.DISPPARAMS();
            pDispParams.rgvarg = IntPtr.Zero;
            try
            {
                Interop.IDispatch script = this.doc.GetScript() as Interop.IDispatch;
                if (script == null)
                {
                    return obj2;
                }
                Guid empty = Guid.Empty;
                string[] rgszNames = new string[] { scriptName };
                int rgDispId = -1;
                if (!Interop.Succeeded(script.GetIDsOfNames(ref empty, ref rgszNames, 1U, Win32.GetThreadLCID(), rgDispId)) || (rgDispId == -1))
                {
                    return obj2;
                }
                if (args != null)
                {
                    Array.Reverse(args);
                }
                pDispParams.rgvarg = (args == null) ? IntPtr.Zero : Interop.ArrayToVARIANTVector(args);
                pDispParams.cArgs = (args == null) ? 0 : args.Length;
                pDispParams.rgdispidNamedArgs = IntPtr.Zero;
                pDispParams.cNamedArgs = 0;
                object[] pVarResult = new object[1];
                int[] n = new int[] { 0 };
                //if  == 0
                script.Invoke(rgDispId, ref empty, Win32.GetThreadLCID(), 1, pDispParams, pVarResult, new Interop.EXCEPINFO(), n);
                {
                    obj2 = pVarResult[0];
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (pDispParams.rgvarg != IntPtr.Zero)
                {
                    Interop.FreeVARIANTVector(pDispParams.rgvarg, args.Length);
                }
            }
            return obj2;
        }

        /// <summary>
        /// Executes the specified script in the provided language.
        /// </summary>
        /// <remarks>
        /// Script executed through the IWindow.execScript method can access all global variables available to the calling script. This can be useful when you want the functionality of another scripting language that would not otherwise be available in JScript, such as the Microsoft Visual Basic Scripting Edition (VBScript) MsgBox function.
        /// <para>
        /// This method is available at runtime as well as at design time.
        /// </para>
        /// </remarks>
        /// <param name="scriptCode">Value that specifies the code to be executed.</param>
        /// <param name="language">Value that specifies the language in which the code is executed. The language defaults to Microsoft JScript.</param>
        /// <returns>Returns result of function execution.</returns>
        public object ExecScript(string scriptCode, string language)
        {
            object o;
            try
            {
                o = window.execScript(scriptCode, language);
                return o;
            }
            catch
            { // In case of Script error 4717cc40-bcb9-11d0-9336-00a0c90dcaa9 is getting called (SID GetCaller)
            }
            return null;
        }

        /// <summary>
        /// Displays a dialog box that prompts the user with a message and an input field.
        /// </summary>
        /// <param name="message">Value that specifies the message to display in the dialog box.</param>
        /// <param name="definition">Value that specifies the default value of the input field. By default, this parameter is set to "undefined".</param>
        /// <returns>Returns the value typed in by the user.</returns>
        public string ShowPrompt(string message, string definition)
        {
            return window.prompt(message, definition).ToString();
        }

        /// <summary>
        /// Displays a dialog box containing an application-defined message. 
        /// </summary>
        /// <remarks>You cannot change the title bar of the Alert dialog box.</remarks>
        /// <param name="message">Value that specifies the message to display in the dialog box.</param>
        public void ShowAlert(string message)
        {
            window.alert(message);
        }

        /// <summary>
        /// Displays a confirmation dialog box that contains an optional message as well as OK and Cancel buttons.
        /// </summary>
        /// <param name="message">Value that specifies the message to display in the confirmation dialog box. If no value is provided, the dialog box does not contain a message.</param>
        /// <returns>True, if the user clicked the OK button. False, if the user clicked Cancel button.</returns>
        public bool ShowConfirm(string message)
        {
            return window.confirm(message);
        }

        /// <summary>
        /// Causes the window to scroll relative to the current scrolled position by the specified x- and y-pixel offset.
        /// </summary>
        /// <param name="x">Value that specifies the horizontal scroll offset, in pixels. Positive values scroll the window right, and negative values scroll it left.</param>
        /// <param name="y">Value that specifies the vertical scroll offset, in pixels. Positive values scroll the window down, and negative values scroll it up.</param>
        public void ScrollBy(int x, int y)
        {
            window.scrollBy(x, y);
        }

        /// <summary>
        /// Causes the window to scroll to the specified x- and y-offset at the upper-left corner of the window. 
        /// </summary>
        /// <param name="x">Value that specifies the horizontal scroll offset, in pixels.</param>
        /// <param name="y">Value that specifies the vertical scroll offset, in pixels.</param>
        public void ScrollTo(int x, int y)
        {
            window.scrollTo(x, y);
        }

        /// <overloads/>
        /// <summary>
        /// Creates a modeless or modal dialog box that displays the specified HTML document.
        /// </summary>
        /// <remarks>See other overloads for additional information.</remarks>
        /// <param name="fileName"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        public void ShowWebPageDialog(string fileName, bool modal, int height, int width, int top, int left)
        {
            ShowWebPageDialog(fileName, modal, height, width, top, left, true, false, false, false, true, true, false, false);
        }

        /// <summary>
        /// Creates a modeless or modal dialog box that displays the specified HTML document.
        /// </summary>
        /// <remarks>
        /// The unit used for the properties height, width, top and left is pixels. The property center will be ignoring
        /// the values for top and left to position the window in the screens center. 
        /// The minimum height is 100 pixels, any smaller value will be ignored and set to at least 100. Despite that,
        /// the user can resize to smaller values if resizing is allowed.
        /// <para>
        /// This method fails, if the base document has no document loaded or the loaded document is from a different
        /// domain. That means, that the caller must assure that, if the base fileName property points to a local file,
        /// the base document must have a local document loaded. 
        /// </para>
        /// <para>
        /// If the dialog is non-modal, it stays still always on top of the host application. The method can be called multiple
        /// times to produce multiple windows, which exists independently from each other.
        /// </para>
        /// <para>
        /// If the document contains links with targets other than _self, a new Internet Explorer window will open to follow
        /// the link. This window does not have any conjunction with the host application and can therefore not being controlled
        /// from NetRix. To avoid this, carefully set the target="_self" attribute for all links. Remember, that the
        /// dialog opened cannot be controlled from NetRix and therefore does not provide any editing capabilities
        /// nor fires any events.
        /// </para>
        /// <para>
        /// The title of the dialog box is always the content of the title tag and the string "-- Web Page Dialog". This
        /// behavior cannot be changed. The dialog has no select, drag 'n drop and other mouse related features. 
        /// </para>
        /// </remarks>
        /// <param name="fileName">Specifies the URL of the document to load and display.</param>
        /// <param name="modal">If false, creates a modeless dialog box that displays the specified HTML document. If true, creates a modal dialog box that displays the specified HTML document.</param>
        /// <param name="height">Sets the height of the dialog window (see Remarks for default unit of measure).</param>
        /// <param name="width">Sets the width of the dialog window (see Remarks for default unit of measure).</param>
        /// <param name="top">Sets the top position of the dialog window relative to the upper-left corner of the desktop.</param>
        /// <param name="left">Sets the left position of the dialog window relative to the upper-left corner of the desktop.</param>
        /// <param name="center">Specifies whether to center the dialog window within the desktop. The default is true.</param>
        /// <param name="hide">Specifies whether the dialog window is hidden when printing or using print preview.</param>
        /// <param name="edgeSunken">Specifies the edge style of the dialog window. The value false is raised.</param>
        /// <param name="helpIcon">Specifies whether the dialog window displays the context-sensitive Help icon.</param>
        /// <param name="resizable">Specifies whether the dialog window has fixed dimensions.</param>
        /// <param name="scrollBar">Specifies whether the dialog window displays scrollbars.</param>
        /// <param name="statusBar">Specifies whether the dialog window displays a status bar.</param>
        /// <param name="unAdorned">Specifies whether the dialog window displays the border window chrome.</param>
        public void ShowWebPageDialog(string fileName, bool modal, int height, int width, int top, int left, bool center, bool hide, bool edgeSunken, bool helpIcon, bool resizable, bool scrollBar, bool statusBar, bool unAdorned)
        {
            string options = String.Format("dialogHeight:{0}px;dialogLeft:{3}px;dialogTop:{2}px;dialogWidth:{1}px;center:{4};dialogHide:{5};edge:{6};help:{7};resizable:{8};scroll:{9};status:{10};unadorned:{11};color:red",
                height, width, top, left,
                center ? "yes" : "no",
                hide ? "yes" : "no",
                edgeSunken ? "yes" : "no",
                helpIcon ? "yes" : "no",
                resizable ? "yes" : "no",
                scrollBar ? "yes" : "no",
                statusBar ? "yes" : "no",
                unAdorned ? "yes" : "no");
            try
            {
                object a = null;
                object o = options;
                if (modal)
                    window.showModalDialog(fileName, a, o);
                else
                    window3.showModelessDialog(fileName, null, options);
            }
            catch
            {
            }
        }

        # endregion

        # region events

        protected void OnLoad(DocumentEventArgs e) { Load(this, e); }
        internal void InvokeLoad(Interop.IHTMLEventObj e) { if (Load != null) { OnLoad(new DocumentEventArgs(e, null)); } }

        protected void OnUnload(DocumentEventArgs e) { Unload(this, e); }
        internal void InvokeUnload(Interop.IHTMLEventObj e) { if (Unload != null) { OnUnload(new DocumentEventArgs(e, null)); } }

        protected void OnHelp(DocumentEventArgs e) { Help(this, e); }
        internal void InvokeHelp(Interop.IHTMLEventObj e) { if (Help != null) { OnHelp(new DocumentEventArgs(e, null)); } }

        protected void OnFocus(DocumentEventArgs e) { Focus(this, e); }
        internal void InvokeFocus(Interop.IHTMLEventObj e) { if (Focus != null) { OnFocus(new DocumentEventArgs(e, null)); } }

        protected void OnBlur(DocumentEventArgs e) { Blur(this, e); }
        internal void InvokeBlur(Interop.IHTMLEventObj e) { if (Blur != null) { OnBlur(new DocumentEventArgs(e, null)); } }

        protected void OnResize(DocumentEventArgs e) { Resize(this, e); }
        internal void InvokeResize(Interop.IHTMLEventObj e) { if (Resize != null) { OnResize(new DocumentEventArgs(e, null)); } }

        protected void OnScroll(DocumentEventArgs e) { Scroll(this, e); }
        internal void InvokeScroll(Interop.IHTMLEventObj e) { if (Scroll != null) { OnScroll(new DocumentEventArgs(e, null)); } }

        protected void OnBeforeUnload(DocumentEventArgs e) { BeforeUnload(this, e); }
        internal void InvokeBeforeUnload(Interop.IHTMLEventObj e) { if (BeforeUnload != null) { OnBeforeUnload(new DocumentEventArgs(e, null)); } }

        protected void OnBeforePrint(DocumentEventArgs e) { BeforePrint(this, e); }
        internal void InvokeBeforePrint(Interop.IHTMLEventObj e) { if (BeforePrint != null) { OnBeforePrint(new DocumentEventArgs(e, null)); } }

        protected void OnAfterPrint(DocumentEventArgs e) { AfterPrint(this, e); }
        internal void InvokeAfterPrint(Interop.IHTMLEventObj e) { if (AfterPrint != null) { OnAfterPrint(new DocumentEventArgs(e, null)); } }

        # endregion

        #region IHtmlWindow Members

        /// <summary>
        /// Fires immediately after the browser loads the object. 
        /// </summary>
        public event DocumentEventHandler Load;

        /// <summary>
        /// Fires immediately before the object is unloaded. 
        /// </summary>
        public event DocumentEventHandler Unload;

        /// <summary>
        /// Fires when the user presses the F1 key while the browser is the active window.
        /// </summary>
        /// <remarks>
        /// If the internal shortcut processing is suppressed the event will not fire.
        /// </remarks>
        public event DocumentEventHandler Help;

        /// <summary>
        /// Fires when the object receives focus. 
        /// </summary>
        public event DocumentEventHandler Focus;

        /// <summary>
        /// Fires when the object loses the input focus.
        /// </summary>
        public event DocumentEventHandler Blur;

        //void Interop.IHTMLWindowEvents.onerror(string description, string url, int line)
        //{
        //    System.Diagnostics.Debug.WriteLine("WIN EVENT");
        //}

        /// <summary>
        /// Fires when the size of the window is about to change. 
        /// </summary>
        public event DocumentEventHandler Resize;

        /// <summary>
        /// Fires when the user repositions the scroll box in the scroll bar on the object. 
        /// </summary>
        public event DocumentEventHandler Scroll;

        /// <summary>
        /// Fires prior to a page being unloaded. 
        /// </summary>
        public event DocumentEventHandler BeforeUnload;

        /// <summary>
        /// Fires on the object before its associated document prints or previews for printing. 
        /// </summary>
        public event DocumentEventHandler BeforePrint;

        /// <summary>
        /// Fires on the object immediately after its associated document prints or previews for printing. 
        /// </summary>
        public event DocumentEventHandler AfterPrint;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            window = null;
            doc = null;
        }

        #endregion

        # region Extended JavaScript Support (V16)

        private bool internalScriptMessages;

        /// <summary>
        /// Suppresses or allows JavaScript messages, including error messages.
        /// </summary>
        /// <remarks>
        /// The purpose of this feature is to control the messages invoked by javascript
        /// at runtime (non-design mode). One can also hook up into the <see cref="ScriptMessage"/>
        /// event to get the internal message data and create it's very own message window instead.
        /// </remarks>
        /// <seealso cref="ScriptMessage"/>
        /// <seealso cref="ScriptError"/>
        public bool InternalScriptMessages
        {
            get { return internalScriptMessages; }
            set { internalScriptMessages = value; }
        }

        # region Events

        /// <summary>
        /// Fired if scripting invokes a message. 
        /// </summary>
        /// <remarks>
        /// Internal messages can be supressed by setting <see cref="InternalScriptMessages"/> to <c>false</c>.
        /// </remarks>
        public event ShowMessageHandler ScriptMessage;
        /// <summary>
        /// Fired if scripting block invoke an error message. Cancellable.
        /// </summary>
        public event ShowErrorHandler ScriptError;
        /// <summary>
        /// Get fired if an external script is being called by Script engine.
        /// </summary>
        /// <remarks>
        /// See <see cref="ObjectForScripting"/> for a property where one can set the class with external methods.
        /// </remarks>
        public event ScriptExternalHandler ScriptExternal;

        internal protected void OnScriptMessage(ShowMessageEventArgs e)
        {
            if (ScriptMessage != null)
            {
                ScriptMessage(this, e);
            }
        }

        internal protected void OnScriptError(ShowErrorEventArgs e)
        {
            if (ScriptError != null)
            {
                ScriptError(this, e);
            }
        }

        internal protected void OnScriptExternal(ScriptExternalEventArgs e)
        {
            if (ScriptExternal != null)
            {
                ScriptExternal(this, e);
            }
        }

        # endregion

        # endregion

    }
}