using System;  
using System.ComponentModel;  

using GuruComponents.Netrix.ComInterop;
using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This top-level element is used to group shapes.
	/// </summary>
	/// <remarks>
    /// This top-level element is used to group shapes (including other groups) so that they can 
    /// be positioned and transformed as a single unit.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class GroupElement : VmlBaseElement
	{

        /// <summary>
        /// Parent in a nested hierarchy.
        /// </summary>
        [Browsable(true), TypeConverter(typeof(ExpandableObjectConverter))]
        public ShapeElement ParentShape
		{
			get
			{
				 IVgShape shape = ((IVgGroupShapes) base.GetBaseElement()).parentShape as IVgShape;
                 if (shape != null)
                 {
                     return new ShapeElement(shape as Interop.IHTMLElement, base.HtmlEditor);
                 }
                 else
                 {
                     return null;
                 }
			}
		}

        /// <summary>
        /// Specified item in the array of shapes.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ShapeElement this[int index]
		{
			get
			{
				 IVgShape shape = ((IVgGroupShapes) base.GetBaseElement())[index] as IVgShape;
                 if (shape != null)
                 {
                     return new ShapeElement(shape as Interop.IHTMLElement, base.HtmlEditor);
                 }
                 else
                 {
                     return null;
                 }
			}
		}


        /// <summary>
        /// The number of values in the list, after parsing the string in <see cref="Value"/>.
        /// </summary>
		[DefaultValue(0)]
        public int Length
		{
			get
			{                
				return ((IVgGroupShapes) base.GetBaseElement()).length;
			}
		}

		public GroupElement(IHtmlEditor editor) : base("v:group", editor)
		{
		}

        internal GroupElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
