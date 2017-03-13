using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.PlugIns;
using System.Collections.Generic;

namespace GuruComponents.Netrix.Designer
{
	/// <summary>
	/// Provides methods to manage the global designer verbs and menu commands available in design mode, and to show some types of shortcut menus.
	/// </summary>
    /// <remarks>
    /// Provides support for commands and verbs. Verbs are opposite to properties and does not have any value one can set but instead provide method invoking. Though, the methods being invoked cannot have parameters. An good example of usage is the TableDesigner plugin. It exposes some commands like "AddRow", "DeleteColumn", or "RemoveCell". These commands are related to the current object and context and does not need any parameters.
    /// The way commands appear to the user depends on the host's user interface. The PropertyGrid will show them as links the ould click on. Implementers often add the commands to the context menu or extend parts of the main menu dynamically. This way the menu can show only valuable commands available for the current selection.
    /// The interface calls also the ContextMenu (and <see cref="GuruComponents.Netrix.HtmlEditor.ContextMenuStrip">ContextMenuStrip</see>) event. It does not provide any information about the menu itself. The latter is part of the user interface and not of NetRix. However, it provides the element which causes the menu being shown and from the element's Site property we have access to the services and the IMenuCommandService allows us to get the appropriate verbs.
    /// Adding verbs is done on several ways:
    /// <list type="bullet">
    /// <item>Using default verbs, implemented by NetRix. These verbs are available for all elements.</item>
    /// <item>Add verbs globally by calling the interface's AddCommand method. These verbs are available for al elements.</item>
    /// <item>Add verbs from plugin. The plugin can decide to add verbs based on current element or globally.</item>
    /// <item>Add verbs using a designer. The element's designer (if one exists) can provide individual verbs for each object instance.</item>
    /// </list>
    /// As you can see, adding verbs can be very flexible. Commands can appear permanently and globally, per element type, or per element instance.
    /// Verbs have specific properties to manage the appearance. The appearance itself depends on the hosts capabilities. The PropertyGrid for instance does not support the Checked property, whereas a context menu could support this and show a check sign accordingly. The NetRix implementation tries to apply these properties:
    /// <list type="bullet">
    /// <item><c>Checked</c> *</item>
    /// <item><c>Enabled</c></item>
    /// <item><c>Visible</c></item>
    /// <item><c>Supported</c> *</item>
    /// </list>
    /// * = Not supported by PropertyGrid
    /// <para>
    /// The properties does control the UI only, setting a the Enabled property to false does not prevent an object from calling the Invoke method and invoking the command. It's up to the host to prevent unwanted access to commands.
    /// </para>
    /// </remarks>
    /// <seealso cref="Verbs">Verbs (property)</seealso>
    /// <seealso cref="DesignerHost">DesignerHost (class)</seealso>
	public class CommandService : System.ComponentModel.Design.IMenuCommandService
	{

        private ArrayList commands;
        private HtmlEditor editor;
        private DesignerVerbCollection verbs = null;

        /// <summary>
        /// Creates an instance of the MenuCommandService.
        /// </summary>
        /// <param name="editor">Reference to the editor this service belongs to.</param>
		public CommandService(IHtmlEditor editor)
		{
            this.editor = (HtmlEditor) editor;
            this.commands = new ArrayList();
        }

        #region IMenuCommandService Member

        /// <summary>
        /// Adds the specified standard menu command to the menu.
        /// </summary>
        /// <param name="command">The System.ComponentModel.Design.MenuCommand to add.</param>
        public void AddCommand(System.ComponentModel.Design.MenuCommand command)
        {
            commands.Add(command);
        }

        /// <summary>
        /// Removes the specified designer verb from the collection of global designer verbs.
        /// </summary>
        /// <param name="verb">The System.ComponentModel.Design.DesignerVerb to remove.</param>
        public void RemoveVerb(System.ComponentModel.Design.DesignerVerb verb)
        {
            Verbs.Remove(verb);
        }

        /// <summary>
        /// Removes the specified standard menu command from the menu.
        /// </summary>
        /// <param name="command">The System.ComponentModel.Design.MenuCommand to remove.</param>
        public void RemoveCommand(System.ComponentModel.Design.MenuCommand command)
        {
            commands.Remove(command);
        }

        /// <summary>
        /// Searches for the specified command ID and returns the menu command associated
        /// with it.
        /// </summary>
        /// <param name="commandID">The System.ComponentModel.Design.CommandID to search for.</param>
        /// <returns>The System.ComponentModel.Design.MenuCommand associated with the command
        /// ID, or <c>null</c> if no command is found.</returns>
        public System.ComponentModel.Design.MenuCommand FindCommand(System.ComponentModel.Design.CommandID commandID)
        {
            foreach (MenuCommand command in commands)
            {
                if (command.CommandID == commandID)
                {
                    return command;
                }
            }
            return null;
        }

        /// <summary>
        /// Invokes a menu or designer verb command matching the specified command ID.
        /// </summary>
        /// <remarks>
        /// This is widely used by plug-ins to add commands to the base control.
        /// </remarks>
        /// <seealso cref="System.ComponentModel.Design.CommandID"/>
        /// <param name="commandID">The System.ComponentModel.Design.CommandID of the command to search for and execute.</param>
        /// <returns><c>True</c> if the command was found and invoked successfully; otherwise, <c>false</c>.</returns>
        public bool GlobalInvoke(System.ComponentModel.Design.CommandID commandID)
        {
            foreach (MenuCommand command in commands)
            {
                if (command.CommandID.Equals(commandID))
                {
                    command.Invoke();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Shows the specified shortcut menu at the specified location.
        /// </summary>
        /// <remarks>
        /// Calling this method invokes the <see cref="OnShowContextMenu"/> event.
        /// </remarks>
        /// <seealso cref="System.ComponentModel.Design.CommandID"/>
        /// <param name="menuID">The System.ComponentModel.Design.CommandID for the shortcut menu to show.</param>
        /// <param name="x">The x-coordinate at which to display the menu, in screen coordinates.</param>
        /// <param name="y">The y-coordinate at which to display the menu, in screen coordinates.</param>
        public void ShowContextMenu(System.ComponentModel.Design.CommandID menuID, int x, int y)
        {
            if (OnShowContextMenu != null)
            {
                System.Web.UI.Control element = null;
                element = this.editor.GenericElementFactory.CreateElement(GetPrimaryElement());
                OnShowContextMenu(this, new ShowContextMenuEventArgs(new System.Drawing.Point(x, y), false, 0, element));
            }
        }

        /// <summary>
        /// Adds the specified designer verb to the set of global designer verbs.
        /// </summary>
        /// <param name="verb">The System.ComponentModel.Design.DesignerVerb to add.</param>
        public void AddVerb(System.ComponentModel.Design.DesignerVerb verb)
        {
            if (verbs == null) verbs = new DesignerVerbCollection();
            verbs.Add(verb);
        }

        /// <summary>
        /// Default designer verb collection. 
        /// </summary>
        /// <remarks>
        /// By default only one verb is added to invoke the element delete command.
        /// </remarks>
        public System.ComponentModel.Design.DesignerVerbCollection Verbs
        {
            get
            {
                //if (verbs == null || verbs.Count == 0)

                IComponent component = this.GetPrimaryComponent();
                IDesignerHost host = (IDesignerHost)editor.ServiceProvider.GetService(typeof(IDesignerHost));
                DesignerVerbCollection localverbs = new DesignerVerbCollection();
                if (host != null && component != null)
                {
                    IDesigner d = host.GetDesigner(component);
                    // designer verbs
                    if (d != null && d.Verbs.Count > 0)
                    {
                        localverbs.AddRange(d.Verbs);
                    }                    
                    if (component is IElement)
                    {
                        // default
                        //if (editor.Selection.IsSelectableElement(component as IElement))
                        //{
                        //    localverbs.Add(new DesignerVerb("Delete " + (component as IElement).UniqueName, Delete_Element));
                        //}
                        if (((HtmlEditor)editor).RegisteredPlugIns != null)
                        {
                            //add verbs provided by plugins
                            foreach (IPlugIn plugin in ((HtmlEditor)editor).RegisteredPlugIns)
                            {
                                List<CommandExtender> ceList = plugin.GetElementExtenders(component as IElement);
                                if (ceList == null) continue;
                                foreach (CommandExtender ce in ceList)
                                {
                                    if (FindCommand(ce.CommandID) == null)
                                    {
                                        throw new NotSupportedException("The command + " + ce.CommandID + " is not supported");
                                    }
                                    DesignerVerb v = new DesignerVerb(ce.Text, new EventHandler(InvokePlugInVerb), ce.CommandID);
                                    v.Checked = ce.Checked;
                                    v.Visible = ce.Visible;
                                    v.Enabled = ce.Enabled;
                                    v.Supported = ce.Supported;
                                    localverbs.Add(v);
                                }
                            }
                        }
                    }
                }
                // global verbs
                if (verbs != null && verbs.Count > 0)
                {
                    localverbs.AddRange(verbs);
                }
                return localverbs;
            }
        }

        private void InvokePlugInVerb(object sender, EventArgs e)
        {
            GlobalInvoke(((MenuCommand) sender).CommandID);
        }

        private void Delete_Element(object sender, EventArgs e)
        {
            if (sender is IElement)
            {
                ((IElement) sender).ElementDom.RemoveElement(true);
            }
        }

        #endregion

        /// <summary>
        /// Fired when the command service recognizes a right click and is up to show the context menu.
        /// </summary>
        public event GuruComponents.Netrix.Events.ShowContextMenuEventHandler OnShowContextMenu;

        private Interop.IHTMLElement GetPrimaryElement()
        {
            IComponent component = GetPrimaryComponent();
            if (component is IElement)
            {
                return ((IElement)component).GetBaseElement() as Interop.IHTMLElement;
            }
            else
            {
                return null;
            }
        }

        private IComponent GetPrimaryComponent()
        {
            ISelectionService s = (ISelectionService) editor.ServiceProvider.GetService(typeof(ISelectionService));
            IComponent component = (IComponent) s.PrimarySelection;
            return component;
        }



    }
}
