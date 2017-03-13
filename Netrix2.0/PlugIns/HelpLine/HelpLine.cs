using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using GuruComponents.Netrix.HelpLine.Events;
using GuruComponents.Netrix.PlugIns;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.ComInterop;
using System.Collections.Generic;
using System.ComponentModel.Design;
          
namespace GuruComponents.Netrix.HelpLine
{
    /// <summary>
	/// Designer for HelpLine support.
	/// </summary>
	/// <remarks>
	/// Checks for mouse over cross, moves and draws the
	/// helpline. This designer may work if it is attached as behavior to body and as edit designer
	/// to mshtml site. The host application may switch on/off the behavior but never removes the designer. 
	/// <para>
	/// Additional features available for helplines:
	/// <list type="bullet">
	/// <item>
	/// <term>Snap helpline to grid (default: On)</term>
	/// <description>You can define a (invisible) grid which the helpline snaps into. The grids default distance is 16 pixels.</description>
	/// <term>Snap Elements to helpline (Default: On)</term>
	/// <description>If the control is in 2D (absolute) position mode the elements can be snapped to the line. The magnetic zone is 4 pixels.</description>
	/// <term>Change the Color and Width of the Pen (Default: Blue, width 1 pixel)</term>
	/// <description>You can use a <see cref="System.Drawing.Pen">Pen</see> object to change the style of the lines.</description>
	/// </item>
	/// </list>
	/// The helpline can be moved using the mouse either on the cross (changes x and y coordinates the same time) or on each
	/// line (moves only x or y, respectively). During the move with the cross the mouse pointer becomes a hand and the Ctrl-Key
	/// can be used to modify the behavior.
	/// </para>
	/// <para>
	/// <b>Usage instructions:</b>
	/// </para>
	/// <para>
	/// To use the helpline you must retrieve an instance of that class using the property
    /// <see cref="GuruComponents.Netrix.HelpLine.HelpLine">HelpLine</see>. The returned object can be changed
	/// in any way. After changing must use the command 
    /// <see cref="GuruComponents.Netrix.HelpLine.HelplineCommands.Activate">Activate</see> to make the lines visible.
    /// The behavior can changed at any time. The object returned from <see cref="GuruComponents.Netrix.HelpLine.HelpLine">HelpLine</see>
	/// is always the same (singleton).
	/// </para>
	/// </remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(GuruComponents.Netrix.HelpLine.HelpLine), "Resources.HelpLine.ico")]
    [ProvideProperty("HelpLine", typeof(GuruComponents.Netrix.IHtmlEditor))]
    public class HelpLine : Component, System.ComponentModel.IExtenderProvider, GuruComponents.Netrix.PlugIns.IPlugIn
    {

        private Hashtable properties;
        private Hashtable behaviors;

        /// <summary>
        /// Default Constructor supports design time behavior
        /// </summary>
        public HelpLine()
        {
            properties = new Hashtable();
            behaviors = new Hashtable();			
        }
        
        /// <summary>
        /// Ctor used from designer
        /// </summary>
        /// <param name="parent"></param>
        public HelpLine(IContainer parent) : this()
        {
            properties = new Hashtable();
            parent.Add(this);
        }

        private HelpLineProperties EnsurePropertiesExists(IHtmlEditor key)
        {
            HelpLineProperties p = (HelpLineProperties) properties[key];
            if (p == null)
            {
                p = new HelpLineProperties();
                properties[key] = p;
            }
            return p;
        }

        private HelpLineBehavior EnsureBehaviorExists(IHtmlEditor key)
        {
            HelpLineBehavior b = (HelpLineBehavior) behaviors[key];
            if (b == null)
            {
                b = new HelpLineBehavior(key as IHtmlEditor, EnsurePropertiesExists(key), this);                
                behaviors[key] = b;
            }
            return b;
        }

        # region +++++ Block: HelpLine 

        /// <summary>
        /// Fired if the mouse is released at the final position of the HelpLine.
        /// </summary>
        /// <remarks>
        /// Normally this event is used to update an display which informs the user about the final position of the helpline.
        /// </remarks>
        [Category("NetRix Events"), Description("Fired if the mouse is released at the final position of the HelpLine.")]
        public event HelpLineMoved HelpLineMoved;

        /// <summary>
        /// Fired during the helpline move at any mouse move step to update an display that shows the
        /// current position of the HelpLine.
        /// </summary>
        [Category("NetRix Events"), Description("Fired during the helpline move at any mouse move.")]
        public event HelpLineMoving HelpLineMoving;

        /// <summary>
        /// Method to fire the helpline moved event, if any handler is attached. This method
        /// is called from the HelpLine designer host class if the user releases the mouse (mouse up)
        /// and the HelpLine is fixed at the final position. The position is the EventArgs.
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <param name="xy"></param>
        internal void OnHelpLineMoved(IHtmlEditor htmlEditor, Point xy)
        {
            if (HelpLineMoved != null)
            {
                HelpLineMoved(htmlEditor, new HelplineMovedEventArgs(xy));
            }
        }
        /// <summary>
        /// Method to fire the helpline moving event, if any handler is attached. This method
        /// is called from the HelpLine designer host class if the user moves the mouse and HelpLine.
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <param name="xy"></param>
        internal void OnHelpLineMoving(IHtmlEditor htmlEditor, Point xy)
        {
            if (HelpLineMoving != null)
            {
                HelpLineMoving(htmlEditor, new HelplineMovedEventArgs(xy));
            }
        }

        /// <summary>
        /// Support the extender infrastructure.
        /// </summary>
        /// <remarks>Should not be called directly from user code.</remarks>
        /// <param name="htmlEditor"></param>
        /// <returns></returns>
        [ExtenderProvidedProperty(), Category("NetRix Component"), Description("HelpLine Properties")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public HelpLineProperties GetHelpLine(IHtmlEditor htmlEditor)
        {
            return this.EnsurePropertiesExists(htmlEditor);
        }

        /// <summary>
        /// Support the extender infrastructure.
        /// </summary>
        /// <remarks>Should not be called directly from user code.</remarks>
        /// <param name="htmlEditor"></param>
        /// <param name="Properties"></param>
        public void SetHelpLine(IHtmlEditor htmlEditor, HelpLineProperties Properties)
        {
            EnsurePropertiesExists(htmlEditor).SetBehaviorReference(EnsureBehaviorExists(htmlEditor));

	        EnsurePropertiesExists(htmlEditor).Active       = Properties.Active;
	        EnsurePropertiesExists(htmlEditor).LineColor    = Properties.LineColor;
            EnsurePropertiesExists(htmlEditor).LineWidth    = Properties.LineWidth;
	        EnsurePropertiesExists(htmlEditor).CrossEnabled = Properties.CrossEnabled;
	        EnsurePropertiesExists(htmlEditor).LineVisible  = Properties.LineVisible;
	        EnsurePropertiesExists(htmlEditor).LineXEnabled = Properties.LineXEnabled;
	        EnsurePropertiesExists(htmlEditor).LineYEnabled = Properties.LineYEnabled;
	        EnsurePropertiesExists(htmlEditor).SnapToGrid   = Properties.SnapToGrid;
            EnsurePropertiesExists(htmlEditor).SnapElements = Properties.SnapElements;
	        EnsurePropertiesExists(htmlEditor).SnapGrid     = Properties.SnapGrid;
	        EnsurePropertiesExists(htmlEditor).SnapOnResize = Properties.SnapOnResize;
	        EnsurePropertiesExists(htmlEditor).SnapZone     = Properties.SnapZone;
	        EnsurePropertiesExists(htmlEditor).X            = Properties.X;
	        EnsurePropertiesExists(htmlEditor).Y            = Properties.Y;                   
	        // Designer
	        htmlEditor.AddEditDesigner(EnsureBehaviorExists(htmlEditor) as Interop.IHTMLEditDesigner);
	        // activate behaviors when document is ready, otherwise it will fail
	        htmlEditor.BeforeSnapRect += new GuruComponents.Netrix.Events.BeforeSnapRectEventHandler(htmlEditor_BeforeSnapRect);
            // Done register
            htmlEditor.RegisterPlugIn(this);
        }

        private HelplineCommands commands;

        /// <summary>
        /// Returns the available commands.
        /// </summary>
        [Browsable(false)]
        public HelplineCommands Commands
        {
            get
            {
                if (commands == null)
                {
                    commands = new HelplineCommands();
                }
                return commands;
            }
        }

        private void HelplineOperation(object sender, EventArgs e)
        {
            CommandWrapper cw = (CommandWrapper) sender;
            if (cw.CommandID.Guid.Equals(Commands.CommandGroup))
            {
                switch ((HelplineCommand)cw.ID)
                {
                    case HelplineCommand.Activate:
                        EnsureBehaviorExists(cw.TargetEditor).LineVisible = true;                        
                        break;
                    case HelplineCommand.Deactivate:
                        EnsureBehaviorExists(cw.TargetEditor).LineVisible = false;
                        break;
                }                
            }
        }

        private void ActivateBehavior(IHtmlEditor htmlEditor)
        {
            if (htmlEditor == null) return;
            IElement body = htmlEditor.GetBodyElement();
            if (body != null)
            {
                body.ElementBehaviors.AddBehavior(EnsureBehaviorExists(htmlEditor));
            }
        }

        /// <summary>
        /// Removes the current behavior for the specified editor.
        /// </summary>
        /// <param name="htmlEditor">The editor the helpline is attached to.</param>
        public void RemoveBehavior(IHtmlEditor htmlEditor)
        {
            if (htmlEditor == null) return;
            IElement body = htmlEditor.GetBodyElement();
            if (body != null)
            {
                body.ElementBehaviors.RemoveBehavior(EnsureBehaviorExists(htmlEditor));
            }
        }

        /// <summary>
        /// Current assembly version
        /// </summary>
        [Browsable(true), ReadOnly(true)]
        public string Version
        {
            get
            {
                return this.GetType().Assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Ensures properties and returns <c>true</c>.
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <returns></returns>
        public bool ShouldSerializeHelpLine(IHtmlEditor htmlEditor)
        {
            HelpLineProperties p = EnsurePropertiesExists(htmlEditor);
            return true;
        }

        # endregion

        #region IExtenderProvider Member

        /// <summary>
        /// Controls the behavior of the extender.
        /// </summary>
        /// <param name="extendee"></param>
        /// <returns></returns>
        public bool CanExtend(object extendee)
        {
            if (extendee is IHtmlEditor)
            {
                return true;
            } 
            else 
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// By calling this method the plugin gets notified that the control is ready.
        /// </summary>
        /// <remarks>Should not be called from user code.</remarks>
        /// <param name="editor">Editor component reference.</param>
        public void NotifyReadyStateCompleted(IHtmlEditor editor)
        {
            if (editor.DesignModeEnabled)
            {
                ActivateBehavior(editor);
                // Commands
                editor.AddCommand(new CommandWrapper(new EventHandler(HelplineOperation), Commands.Activate));
                editor.AddCommand(new CommandWrapper(new EventHandler(HelplineOperation), Commands.Deactivate));
            }
            else
            {
                //RemoveBehavior(editor);
            }
        }

        /// <summary>
        /// Supports propertsgrid.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Click + for details";
        }

        #region IPlugIn Member

        /// <summary>
        /// Name of Plug-in.
        /// </summary>
        public string Name
        {
            get
            {
                return "HelpLine";
            }
        }

        /// <summary>
        /// Indicates whether this is an extender.
        /// </summary>
        [Browsable(false)]
        public bool IsExtenderProvider
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Type Type
        {
            get
            {
                return this.GetType();
            }
        }

        /// <summary>
        /// Editor features.
        /// </summary>
        [Browsable(false)]
        Feature IPlugIn.Features
        {
            get { return Feature.None; }
        }

        /// <summary>
        /// Returns supported namespaces.
        /// </summary>
        /// <remarks>This Plugin does not supprt namespaces and hence this method always returns <c>null</c>.
        /// </remarks>
        /// <param name="key">Editor component reference.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        [Browsable(false)]
        public IDictionary GetSupportedNamespaces(IHtmlEditor key)
        {
            return null;
        }

        System.Web.UI.Control IPlugIn.CreateElement(string tagName, IHtmlEditor editor)
        {
            throw new Exception("The method or operation is not available.");
        }

        /// <summary>
        /// List of element types, which the extender plugin extends.
        /// </summary>
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.PlugIns.CommandExtender">CommandExtender</see> for background information.
        /// <para>
        /// For this plugin the method always returns <c>null</c>.
        /// </para>
        /// </remarks>
        public List<CommandExtender> GetElementExtenders(IElement component)
        {
            return null;
        }

        #endregion

        private void htmlEditor_BeforeSnapRect(object sender, GuruComponents.Netrix.Events.BeforeSnapRectEventArgs e)
        {
            Rectangle r = e.Rectangle;
            Snap.SnapRectToHelpLine(ref r, e.SnapZone, EnsurePropertiesExists(((IHtmlEditor)sender)), e.ScrollPos);
            e.Rectangle = r;
        }

    }
}
