using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
    /// <summary>
    /// StylesheetCollectionEditor edits the collection of &lt;link&gt; tags in the head
    /// section of the HTML document.
    /// </summary>    
    /// <remarks>
    /// This editor is used to support the PropertyGrid.
    /// You should not use this class directly from user code as far as you don't write
    /// your own PropertyGrid support.
    /// </remarks>
    public class LinkCollectionEditor : CollectionEditor
    {

        /// <summary>
        /// Instantiates the collection editor the first time.
        /// </summary>
        /// <remarks>
        /// This method supports the NetRix infrastructure and should not beeing used from user code.
        /// </remarks>
        /// <param name="type"></param>
        public LinkCollectionEditor(Type type) : base(type)
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
            form.Text = "<LINK> Editor";
            Type t = form.GetType();
            // Get the private variables via Reflection            
            FieldInfo fieldInfo;
			fieldInfo = t.GetField("listbox", BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
			{
				ListBox lb = (ListBox) fieldInfo.GetValue(form);
				if (lb != null)
				{
					lb.Width += 45;
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
            fieldInfo = t.GetField("helpButton", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Button helpButton = (Button) fieldInfo.GetValue(form);
                helpButton.Visible = false;
            }    
			fieldInfo = t.GetField("removeButton", BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
			{
				Button removeButton = (Button) fieldInfo.GetValue(form);
				removeButton.Left += 45;
				removeButton.Text = ResourceManager.GetString("StylesheetCollectionEditor.removeButton.Text");
                removeButton.FlatStyle = FlatStyle.System;
			}    
			fieldInfo = t.GetField("addButton", BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
			{
				Button addButton = (Button) fieldInfo.GetValue(form);
				addButton.Text = ResourceManager.GetString("StylesheetCollectionEditor.addButton.Text");
                addButton.FlatStyle = FlatStyle.System;
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
            memberLabel.Text = ResourceManager.GetString("StylesheetCollectionEditor.memberLabel.Text");
            memberLabel.Location = new Point(10, 8);
            memberLabel.Size = new Size(200, 15);
            form.Controls.Add(memberLabel);
            memberLabel.BringToFront();
            return form;
        } 

        /// <summary>
        /// User can remove tags.
        /// </summary>
        /// <param name="value">The current value. Not beeing used here.</param>
        /// <returns>Returns always <c>true</c> to allow instance removing.</returns>
        protected override bool CanRemoveInstance(object value)
        {
            return true;
        }

        /// <summary>
        /// User cannot select multiple tags at the same time.
        /// </summary>
        /// <returns></returns>
        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

    }
}