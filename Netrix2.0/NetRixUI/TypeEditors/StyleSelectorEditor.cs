using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
    /// <summary>
    /// StyleSelectorEditor edits the collection of selectors of an embedded style element.
    /// </summary>    
    /// <remarks>
    /// This editor is used to support the PropertyGrid.
    /// You should not use this class directly from user code as far as you don't write
    /// your own PropertyGrid support.
    /// <para>
    /// This editor supports style rules only. It is used from the 
    /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.StyleCollectionEditor">StyleCollectionEditor</see>.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.UserInterface.TypeEditors.StyleCollectionEditor">StyleCollectionEditor</seealso>
    /// </remarks>
    public class StyleSelectorEditor : CollectionEditor
    {

        /// <summary>
        /// Instantiates the collection editor the first time.
        /// </summary>
        /// <remarks>
        /// This method supports the NetRix infrastructure and should not beeing used from user code.
        /// </remarks>
        /// <param name="type"></param>
        public StyleSelectorEditor(Type type) : base(type)
        {
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
            form.Text = "{ ... } CSS-Editor";
            Type t = form.GetType();
            // Get the private variables via Reflection            
            FieldInfo fieldInfo;
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