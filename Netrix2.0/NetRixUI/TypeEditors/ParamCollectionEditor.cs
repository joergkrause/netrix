using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
    /// <summary>
    /// ParamCollectionEditor lets the user edit PARAM tags within a OBJECT or EMBED tag.
    /// </summary>
    /// <remarks>
    /// This editor is used to support the PropertyGrid.
    /// You should not use this class directly from user code as far as you don't write
    /// your own PropertyGrid support.
    /// <para>
    /// This editor supports OPTION tags only.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.UserInterface.TypeEditors.OptionCollectionEditor">OptionCollectionEditor</seealso>
    /// </remarks>
    public class ParamCollectionEditor : CollectionEditor
    {

        /// <summary>
        /// Instantiates the collection editor the first time.
        /// </summary>
        /// <remarks>
        /// This method supports the NetRix infrastructure and should not beeing used from user code.
        /// </remarks>
        /// <param name="type"></param>
        public ParamCollectionEditor(Type type) : base(type)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
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
            form.Text = "<PARAM> Editor";
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
            fieldInfo = t.GetField("upButton", BindingFlags.NonPublic | BindingFlags.Instance);
            // we add the events here to force the underlying collection beeing reordered after each up/down step to
            // allow the application to refresh the display of the element even on order steps
            if (fieldInfo != null)
            {
                Button upButton = (Button) fieldInfo.GetValue(form);
            }    
            fieldInfo = t.GetField("downButton", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Button downButton = (Button) fieldInfo.GetValue(form);
            }    
            fieldInfo = t.GetField("helpButton", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Button removeButton = (Button) fieldInfo.GetValue(form);
                removeButton.Visible = false;
            }    
			fieldInfo = t.GetField("addButton", BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
			{
				Button addButton = (Button) fieldInfo.GetValue(form);
				addButton.Text = ResourceManager.GetString("OptionCollectionEditor.addButton.Text");
			}    
            fieldInfo = t.GetField("removeButton", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Button removeButton = (Button) fieldInfo.GetValue(form);
                removeButton.Text = ResourceManager.GetString("OptionCollectionEditor.removeButton.Text");
            }    
			fieldInfo = t.GetField("okButton", BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
			{
				Button okButton = (Button) fieldInfo.GetValue(form);
				okButton.Left += okButton.Width + 3;
			}    
			fieldInfo = t.GetField("cancelButton", BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
			{
				Button cancelButton = (Button) fieldInfo.GetValue(form);
				cancelButton.Left += cancelButton.Width + 3;
			}    
			Label memberLabel = new Label();
            memberLabel.Text = ResourceManager.GetString("OptionCollectionEditor.memberLabel.Text");
            memberLabel.Location = new Point(10, 8);
            memberLabel.Size = new Size(200, 15);
            form.Controls.Add(memberLabel);
            memberLabel.BringToFront();
            return form;
        } 

        /// <summary>
        /// Allows the removing of instances.
        /// </summary>
        /// <remarks>
        /// Removing an instance will change the DOM immediataly. It cannot be guaranteed that the UNDO manager will
        /// recognize the change all the time.
        /// </remarks>
        /// <param name="value">The current value. Not beeing used here.</param>
        /// <returns>Returns always <c>true</c> to allow instance removing.</returns>
        protected override bool CanRemoveInstance(object value)
        {
            return true;
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