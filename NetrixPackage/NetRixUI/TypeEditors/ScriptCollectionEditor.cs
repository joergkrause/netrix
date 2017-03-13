using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
	/// <summary>
	/// ScriptCollectionEditor lets the user edit script blocks outside the editor using
	/// the PropertyGrid.
	/// </summary>
	/// <remarks>
    /// This editor is used to support the PropertyGrid.
    /// You should not use this class directly from user code as far as you don't write
    /// your own PropertyGrid support.
    /// <para>
	/// This is a <seealso cref="System.ComponentModel.Design.CollectionEditor">
	/// System.ComponentModel.Design.CollectionEditor</seealso> derived class changing some of the
	/// base features via reflection.</para>
	/// </remarks>
	public class ScriptCollectionEditor : CollectionEditor
	{
        /// <summary>
        /// Instantiates the collection editor the first time.
        /// </summary>
        /// <remarks>
        /// This method supports the NetRix infrastructure and should not beeing used from user code.
        /// </remarks>
        /// <param name="type"></param>
        public ScriptCollectionEditor(Type type) : base(type)
        {
        }

        protected override object CreateInstance(Type itemType)
        {
            if (itemType.BaseType.Name.EndsWith("Element"))
            {
                // need to pull the editor to override the default ctor behavior
                IDesignerHost host = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
                IHtmlEditor editor = null;
                if (host != null)
                {
                    editor = host.GetType().GetField("editor", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(host) as IHtmlEditor;
                }
                return Activator.CreateInstance(itemType, editor);
            }
            else
            {
                return base.CreateInstance(itemType);
            }
        }

        /// <summary>
        /// Takes the base collection form and apply some changes.
        /// </summary>
        /// <remarks>
        /// This method uses reflection to change the internal collection editor dialog on-thy-fly.
        /// </remarks>
        /// <returns>The changed form is beeing returned completely.</returns>
        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm form = base.CreateCollectionForm();
            form.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            form.Text = "<SCRIPT> Editor";
            Type t = form.GetType();
            // Get the private variables via Reflection and change whatever we need to change...
            FieldInfo fieldInfo;
			fieldInfo = t.GetField("listbox", BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
			{
				ListBox lb = (ListBox) fieldInfo.GetValue(form);
				if (lb != null)
				{
					lb.Width += 45;
					lb.Height += 35;
				}
			}
			fieldInfo = t.GetField("propertyBrowser", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                PropertyGrid propertyGrid = (PropertyGrid) fieldInfo.GetValue(form);
                if (propertyGrid != null)
                {
                    propertyGrid.ToolbarVisible = false;
                    propertyGrid.HelpVisible = true;
                }
            }
            fieldInfo = t.GetField("upButton", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Button upButton = (Button) fieldInfo.GetValue(form);
                upButton.Visible = false;
            }    
            fieldInfo = t.GetField("downButton", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Button downButton = (Button) fieldInfo.GetValue(form);
                downButton.Visible = false;
            }    
            fieldInfo = t.GetField("addButton", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Button addButton = (Button) fieldInfo.GetValue(form);
                addButton.Visible = false;
            }    
            fieldInfo = t.GetField("removeButton", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Button removeButton = (Button) fieldInfo.GetValue(form);
                removeButton.Visible = false;
            }    
            fieldInfo = t.GetField("helpButton", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Button helpButton = (Button) fieldInfo.GetValue(form);
                helpButton.Visible = false;
            }    
			fieldInfo = t.GetField("okButton", BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
			{
				Button okButton = (Button) fieldInfo.GetValue(form);
				okButton.Left += okButton.Width + 3;
                okButton.FlatStyle = FlatStyle.System;
			}    
			fieldInfo = t.GetField("cancelButton", BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
			{
				Button cancelButton = (Button) fieldInfo.GetValue(form);
				cancelButton.Left += cancelButton.Width + 3;
                cancelButton.FlatStyle = FlatStyle.System;
			}    
			Label memberLabel = new Label();
            memberLabel.Text = ResourceManager.GetString("ScriptCollectionEditor.memberLabel.Text"); 
            memberLabel.Location = new Point(10, 8);
            memberLabel.Size = new Size(200, 15);
            form.Controls.Add(memberLabel);
            memberLabel.BringToFront();
            return form;
        } 

        /// <summary>
        /// Prohibits the removing of instances.
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to avoid the user from removing script blocks using the collection editor.
        /// This is necessary because the user cannot recognize the real position of the script fragment within the
        /// HTML page using this editor. Removing and adding can cause scripting errors because JavaScript and VBScript
        /// fragments depends on their position of the page.
        /// <para>
        /// It is recommended to replace the script editing features with a more sophisticated solution based on the DOM
        /// instead of using this editor in a real life environment. The purpose of this editor is to have an fast and simple
        /// access to the script fragments on the page without any programming tasks required.
        /// </para>
        /// </remarks>
        /// <param name="value">The current value. Not beeing used here.</param>
        /// <returns>Returns always <c>false</c> to prohibit instance removing.</returns>
        protected override bool CanRemoveInstance(object value)
        {
            return false;
        }

        /// <summary>
        /// Prohibits the selection of multiple instances.
        /// </summary>
        /// <remarks>
        /// It is not allowed to edit multiple instances of the script editor at the same time.
        /// </remarks>
        /// <returns></returns>
        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

	}
}