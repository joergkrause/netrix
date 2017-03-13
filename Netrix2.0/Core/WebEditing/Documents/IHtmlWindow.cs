using System.Drawing;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Web.UI;

namespace GuruComponents.Netrix.WebEditing.Documents
{
	/// <summary>
	/// Public access to the HtmlWindow class.
	/// </summary>
	public interface IHtmlWindow
	{

		#region IHTMLWindowEvents Members

		/// <summary>
		/// Fires immediately after the browser loads the object. 
		/// </summary>
		event DocumentEventHandler Load;

		/// <summary>
		/// Fires immediately before the object is unloaded. 
		/// </summary>
		event DocumentEventHandler Unload;

		/// <summary>
		/// Fires when the user presses the F1 key while the browser is the active window.
		/// </summary>
		/// <remarks>
		/// If the internal shortcut processing is suppressed the event will not fire.
		/// </remarks>
		event DocumentEventHandler Help;

		/// <summary>
		/// Fires when the object receives focus. 
		/// </summary>
		event DocumentEventHandler Focus;

		/// <summary>
		/// Fires when the object loses the input focus.
		/// </summary>
		event DocumentEventHandler Blur;

		//void Interop.IHTMLWindowEvents.onerror(string description, string url, int line)
		//{
		//    System.Diagnostics.Debug.WriteLine("WIN EVENT");
		//}

		/// <summary>
		/// Fires when the size of the window is about to change. 
		/// </summary>
		event DocumentEventHandler Resize;

		/// <summary>
		/// Fires when the user repositions the scroll box in the scroll bar on the object. 
		/// </summary>
		event DocumentEventHandler Scroll;

		/// <summary>
		/// Fires prior to a page being unloaded. 
		/// </summary>
		event DocumentEventHandler BeforeUnload;

		/// <summary>
		/// Fires on the object before its associated document prints or previews for printing. 
		/// </summary>
		event DocumentEventHandler BeforePrint;

		/// <summary>
		/// Fires on the object immediately after its associated document prints or previews for printing. 
		/// </summary>
		event DocumentEventHandler AfterPrint;

		#endregion

		/// <summary>
		/// Invoke Script Code within the page.
		/// </summary>
		/// <remarks>This method is experimental.</remarks>
		/// <param name="callCode">Script code called by method, a JScript method name for instance.</param>
		object CallScript(string callCode);

		/// <summary>
		/// Invoke Script Code within the page.
		/// </summary>
		/// <remarks>This method is experimental.</remarks>
		/// <param name="callCode">Script code called by method, a JScript method name for instance.</param>
		/// <param name="args">Arguments for method call. Provide as many items as method has parameters.</param>
		object CallScript(string callCode, object[] args);

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
		object ObjectForScripting { get; set; }

		/// <overloads/>
		/// <summary>
		/// Executes the specified script in the provided language.
		/// </summary>
		/// <remarks>
		/// Script executed through the IWindow.execScript method can access all global variables available to the calling script. This can be useful when you want the functionality of another scripting language that would not otherwise be available in JScript, such as the Microsoft Visual Basic Scripting Edition (VBScript) MsgBox function.
		/// </remarks>
		/// <param name="scriptCode">Value that specifies the code to be executed.</param>
		void ExecScript(string scriptCode);
		/// <summary>
		/// Executes the specified script in the provided language.
		/// </summary>
		/// <remarks>Script executed through the IWindow.execScript method can access all global variables available to the calling script. This can be useful when you want the functionality of another scripting language that would not otherwise be available in JScript, such as the Microsoft Visual Basic Scripting Edition (VBScript) MsgBox function.</remarks>
		/// <param name="scriptCode">Value that specifies the code to be executed.</param>
		/// <param name="language">Value that specifies the language in which the code is executed. The language defaults to Microsoft JScript.</param>
		object ExecScript(string scriptCode, string language);

		/// <summary>
		/// Displays a dialog box that prompts the user with a message and an input field.
		/// </summary>
		/// <param name="message">Value that specifies the message to display in the dialog box.</param>
		/// <param name="definition">Value that specifies the default value of the input field. By default, this parameter is set to "undefined".</param>
		/// <returns>Returns the value typed in by the user.</returns>
		string ShowPrompt(string message, string definition);

		/// <summary>
		/// Displays a dialog box containing an application-defined message. 
		/// </summary>
		/// <remarks>You cannot change the title bar of the Alert dialog box.</remarks>
		/// <param name="message">Value that specifies the message to display in the dialog box.</param>
		void ShowAlert(string message);

		/// <summary>
		/// Displays a confirmation dialog box that contains an optional message as well as OK and Cancel buttons.
		/// </summary>
		/// <param name="message">Value that specifies the message to display in the confirmation dialog box. If no value is provided, the dialog box does not contain a message.</param>
		/// <returns>True, if the user clicked the OK button. False, if the user clicked Cancel button.</returns>
		bool ShowConfirm(string message);

		/// <summary>
		/// Causes the window to scroll relative to the current scrolled position by the specified x- and y-pixel offset.
		/// </summary>
		/// <param name="x">Value that specifies the horizontal scroll offset, in pixels. Positive values scroll the window right, and negative values scroll it left.</param>
		/// <param name="y">Value that specifies the vertical scroll offset, in pixels. Positive values scroll the window down, and negative values scroll it up.</param>
		void ScrollBy(int x, int y);

		/// <summary>
		/// Causes the window to scroll to the specified x- and y-offset at the upper-left corner of the window. 
		/// </summary>
		/// <param name="x">Value that specifies the horizontal scroll offset, in pixels.</param>
		/// <param name="y">Value that specifies the vertical scroll offset, in pixels.</param>
		void ScrollTo(int x, int y);

		/// <overloads/>
		/// <summary>
		/// Creates a modeless or modal dialog box that displays the specified HTML document.
		/// </summary>
		/// <remarks>See other overloads for additional information.</remarks>
		/// <param name="fileName"></param>
		/// <param name="height"></param>
		/// <param name="modal"></param>
		/// <param name="width"></param>
		/// <param name="top"></param>
		/// <param name="left"></param>
		void ShowWebPageDialog(string fileName, bool modal, int height, int width, int top, int left);

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
		/// dialog opened cannot being controlled from NetRix and therefore does not provide any editing capabilities
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
		void ShowWebPageDialog(string fileName, bool modal, int height, int width, int top, int left, bool center, bool hide, bool edgeSunken, bool helpIcon, bool resizable, bool scrollBar, bool statusBar, bool unAdorned);

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
		Control GetElementFromPoint(Point point);

		/// <summary>
		/// Returns the element which is directly under the caret.
		/// </summary>
		/// <remarks>
		/// In case of invisible caret this method may return unpredictable results.
		/// <seealso cref="GetElementFromPoint"/>
		/// </remarks>
		/// <returns>Returns IElement object from caret.</returns>
		Control GetElementFromCaret();

		/// <summary>
		/// Return the current location of the caret within the editor.
		/// </summary>
		/// <returns>Return the position in client coordinates.</returns>
		Point GetCaretLocation();

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
		bool InternalScriptMessages
		{
			get;
			set;
		}

		/// <summary>
		/// Fired if scripting invokes a message. 
		/// </summary>
		/// <remarks>
		/// Internal messages can be supressed by setting <see cref="InternalScriptMessages"/> to <c>false</c>.
		/// </remarks>
		event ShowMessageHandler ScriptMessage;
		/// <summary>
		/// Fired if scripting block invoke an error message. Cancellable.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The usage requires an explicit handling of ready state conditions. This is unfortunately due to internal document handling.
		/// The error isn’t caught if the event is attached and the browse mode is activated because of the document’s 
		/// ready state being overwritten by setting the control to browse mode.
		/// </para>
		/// <para>
		/// We suggest using this workaround (assume that <c>editor</c> is the HtmlEditor object):
		/// <example>
		/// bool firstTime = true;
		///  
		/// void editor_ReadyStateComplete(object sender, EventArgs e)
		/// {
		///    if (firstTime)
		///    {
		///        string errFile = @"\scriptwitherror1.html";
		///        editor.LoadFile(errFile);
		///        firstTime = false;
		///    }
		/// }
		/// </example>
		/// </para>
		/// <para>
		/// This makes the onerror event firing properly and showing error conditions and allows suppressing the internal dialog.
		/// </para>
		/// </remarks>
		event ShowErrorHandler ScriptError;
	}
}
