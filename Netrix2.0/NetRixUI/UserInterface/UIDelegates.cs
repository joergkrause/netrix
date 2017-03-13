using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a src or href attribute where an URL or file
    /// can selected. 
    /// </summary>
    /// <remarks>
    /// If no callback exists the standard openFileDialog control will
    /// be used. The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UIUrlEditor(ITypeDescriptorContext context, ref System.String Url);
    
    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a attribute containing a unit (pixel or percentage value).
    /// </summary><remarks>
    /// If no callback exists the <see cref="GuruComponents.Netrix.UserInterface.UnitEditor">UnitEditor</see> control will be used.
    /// The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UIUnitEditor(ITypeDescriptorContext context, ref System.Web.UI.WebControls.Unit Unit);

    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a attribute containing a unit (pixel or percentage value).
    /// </summary><remarks>
    /// If no callback exists the <see cref="GuruComponents.Netrix.UserInterface.UnitEditor">UnitEditor</see> control will be used.
    /// The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UIPixelEditor(ITypeDescriptorContext context, ref System.Web.UI.WebControls.Unit Unit);    
    
    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a attribute containing an angle (in degrees between 0° and 360°).
    /// </summary><remarks>
    /// If no callback exists the <see cref="GuruComponents.Netrix.UserInterface.AngleEditor">AngleEditor</see> control will be used.
    /// The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UIAngleEditor(ITypeDescriptorContext context, ref decimal angle);


    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a attribute containing a color.    
    /// </summary>
    /// <remarks>
    /// If no callback exists the <see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl"/> control will be used.
    /// The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UIColorEditor(ITypeDescriptorContext context, ref System.Drawing.Color Color);

    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a attribute containing a font.
    /// </summary>
    /// <remarks>
    /// If no callback exists the <see cref="GuruComponents.Netrix.UserInterface.FontPicker.FontPickerUserControl">FontPickerUserControl</see> control will be used.
    /// The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UIFontEditor(ITypeDescriptorContext context, ref System.String Font);

    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a attribute containing a style definition.
    /// </summary>
    /// <remarks>
    /// If no callback exists the <see cref="GuruComponents.Netrix.UserInterface.StyleEditor.StyleControl">StyleUserControl</see> will be used.
    /// The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UIStyleEditor(ITypeDescriptorContext context, ref System.String Style);

    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a attribute containing a comment.
    /// </summary>
    /// <remarks>
    /// If no callback exists the <see cref="GuruComponents.Netrix.UserInterface.UnitEditor">UnitEditor</see> control will be used.
    /// The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UICommentEditor(ITypeDescriptorContext context, ref System.String Comment);

    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a attribute containing a script from a scripting section (JavaScript).
    /// </summary>
    /// <remarks>
    /// If no callback exists a simple text editor is used (based on the <see cref="TextBox"/> control).
    /// The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UIScriptEditor(ITypeDescriptorContext context, ref System.String Script);

    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a attribute containing an integer.
    /// </summary>
    /// <remarks>
    /// If no callback exists the the standard editor is used.
    /// The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UIIntEditor(ITypeDescriptorContext context, ref System.Int32 Integer);

    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a attribute containing a string.
    /// </summary>
    /// <remarks>
    /// If no callback exists the the standard editor is used.
    /// The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UIStringEditor(ITypeDescriptorContext context, ref System.String String);

    /// <summary>
    /// This delegate allows the host application to define a dialog which is called
    /// whenever the propertygrid edits a attribute containing a dropdown list, build from an enumeration.
    /// </summary>
    /// <remarks>
    /// The signature accepts a string (the old value in propertgrid item) and gets
    /// a string back as the new item.
    /// </remarks>
    public delegate DialogResult UIDropListEditor(ITypeDescriptorContext context, ref System.String EnumMember);
}
