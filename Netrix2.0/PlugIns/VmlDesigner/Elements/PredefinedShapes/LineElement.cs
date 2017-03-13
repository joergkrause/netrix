using System;  
using System.ComponentModel;  
using System.Drawing;  

using GuruComponents.Netrix.ComInterop;
using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.VmlDesigner;
using GuruComponents.Netrix.VmlDesigner.DataTypes;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This element is used to specify a straight line.
	/// </summary>
    [ToolboxItem(false)]
    public class LineElement : PredefinedElement
    {

        /// <summary>
        /// Not used, returns always null.
        /// </summary>
        [Browsable(false)]
        public override VgFillFormat Fill
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
		[Browsable(false)]
		public override float Opacity
		{
			get
			{
				return 0F;
			}
			set
			{
				throw new NotImplementedException("Opacity is not defined for Line elements");
			}
		}

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public override Color FillColor
        {
            get
            {
                return this.StrokeColor;
            }
            set
            {
                this.StrokeColor = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public override Color ChromaKey
        {
            get
            {
                return Color.Empty;
            }
            set
            {
                throw new NotImplementedException("Chromakey is not defined for Line elements");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
                [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
             typeof(System.Drawing.Design.UITypeEditor))]
        public override System.Web.UI.WebControls.Unit Top
        {
            get
            {
                return base.Top;
            }
            set
            {
                base.Top = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
                [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
             typeof(System.Drawing.Design.UITypeEditor))]
        public override System.Web.UI.WebControls.Unit Left
        {
            get
            {
                return base.Left;
            }
            set
            {
                base.Left = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
                [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
             typeof(System.Drawing.Design.UITypeEditor))]
        public override System.Web.UI.WebControls.Unit Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
                [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
             typeof(System.Drawing.Design.UITypeEditor))]
        public override System.Web.UI.WebControls.Unit Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
            }
        }

        /// <summary>
        /// Gets or sets the start point of the element.
        /// </summary>
        /// <remarks>
        /// <seealso cref="To"/>
        /// </remarks>
        [Browsable(true), TypeConverter(typeof(ExpandableObjectConverter)), Category("Element Layout")]
        public virtual GuruComponents.Netrix.VmlDesigner.DataTypes.VgVector2D To
        {
            get
            {
                return new VgVector2D((IVgVector2D) GetAttribute("to"));
            }
            set
            {
                SetAttribute("to", value.NativeVector);
            }
        }

        /// <summary>
        /// Gets or sets the start point of the element.
        /// </summary>
        /// <remarks>
        /// <seealso cref="To"/>
        /// </remarks>
        [Browsable(true), TypeConverter(typeof(ExpandableObjectConverter)), Category("Element Layout")]
        public virtual GuruComponents.Netrix.VmlDesigner.DataTypes.VgVector2D From
        {
            get
            {
                return new VgVector2D((IVgVector2D) GetAttribute("from"));
            }
            set
            {
                SetAttribute("from", value.NativeVector);
            }
        }

        /// <summary>
        /// Create new instance of element.
        /// </summary>
        /// <param name="newTag">Tag name</param>
        /// <param name="editor">Editor reference</param>
        protected LineElement(string name, IHtmlEditor editor)
            : base(name, editor)
        {
        }

        /// <summary>
        /// Create new instance of element.
        /// </summary>
        /// <param name="editor">Editor reference</param>
        public LineElement(IHtmlEditor editor)
            : base("v:line", editor)
		{
		}

        internal LineElement(Interop.IHTMLElement peer, IHtmlEditor editor)
            : base(peer, editor)
        {
        }

	}
}
