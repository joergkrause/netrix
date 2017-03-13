using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
    /// <summary>
    /// OptionCollectionEditor lets the user edit OPTION tags within a SELECT tag.
    /// </summary>
    /// <remarks>
    /// This editor is used to support the PropertyGrid.
    /// You should not use this class directly from user code as far as you don't write
    /// your own PropertyGrid support.
    /// <para>
    /// This editor supports OPTION and OPTGROUP tags.
    /// </para>
    /// </remarks>
    public class OptionCollectionEditor : CollectionEditor
    {
        Type tBase;
        IHtmlEditor editor;

        /// <summary>
        /// Instantiates the collection editor the first time.
        /// </summary>
        /// <remarks>
        /// This method supports the NetRix infrastructure and should not beeing used from user code.
        /// </remarks>
        /// <param name="type"></param>
        public OptionCollectionEditor(Type type) : base(type)
        {
            tBase = type;
        }

        /// <summary>
        /// This method defines the both option types for the dropdown button.
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to prepare the editor for handling to different instances.
        /// This is necessary to support the HTML 4.0 SELECT tag which can handle OPTION as well as OPTGROUP
        /// elements the same time.
        /// </remarks>
        /// <returns></returns>
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { 
                                  (Type) tBase.GetField("OptionElementType").GetValue("OptionElementType"),
                                  (Type) tBase.GetField("OptGroupElementType").GetValue("OptGroupElementType")
            };
        }

        protected override object CreateInstance(Type itemType)
        {
            if (itemType.BaseType.Name.EndsWith("Element"))
            {
                // need to pull the editor to override the default ctor behavior
                IDesignerHost host = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
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

        private static Stream stream = typeof(OptionCollectionEditor).Assembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.CollectionEditorAddImage.ico");


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
            form.Text = "<OPTION><OPTGROUP> Editor";
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
            fieldInfo = t.GetField("addDownButton", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Button addDownButton = (Button) fieldInfo.GetValue(form);
                addDownButton.BackColor = SystemColors.Control;
                addDownButton.ImageList = null;
                // we modify the image because the original one has a terrible gray background...
                addDownButton.Image = Image.FromStream(stream);
            }
            fieldInfo = t.GetField("addDownMenu", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)            
            {
                switch (fieldInfo.GetValue(form).GetType().Name)
                {
                    case "ContextMenu":
                        ContextMenu addMenu = (ContextMenu) fieldInfo.GetValue(form);
                        if (addMenu.MenuItems.Count == 2)
                        {
                            addMenu.MenuItems[0].Text = "<OPTION>";
                            addMenu.MenuItems[1].Text = "<OPTGROUP>";
                        }
                        break;
# if !DOTNET20
                    case "ContextMenuStrip":
                        ContextMenuStrip ctxMenu = (ContextMenuStrip) fieldInfo.GetValue(form);
                        if (ctxMenu.Items.Count == 2)
                        {
                            ctxMenu.Items[0].Text = "<OPTION>";
                            ctxMenu.Items[1].Text = "<OPTGROUP>";
                        }
                        break;
# endif                        
                }
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
        /// <returns>Returns always <c>false</c>.</returns>
        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

    }
}