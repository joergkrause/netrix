using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using Comzept.Genesis.NetRix.VgxDraw;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;
using System.Runtime.InteropServices.ComTypes;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{    
	/// <summary>
	/// A base class for all VML shapes.
	/// </summary>
	/// <remarks>
	/// This class can not be used to instantiate VML elements directly. Its purpose is the definition of common
	/// attributes for predefined shapes.
	/// <para>
	/// Predefined shapes serve two purposes - they provide a more compact representation of a small number of very 
	/// frequently encountered drawing operations (particularly rectangles and circles) and they give an easy to use 
	/// form for people who hand-edit VML.
	/// </para>
	/// <para>
	/// Predefined shapes have the same properties as shape except that the type attribute is not permitted.  
	/// In some cases the definition of the shape precludes use of some of the standard shape properties.  
	/// These exceptions are given in the shape class descriptions.
	/// </para>
	/// </remarks>
	[ToolboxItem(false)]
    public abstract class BaseShapeElement : VmlBaseElement
	{
        private static readonly Regex fileRegex = new Regex(@"file:/+", RegexOptions.IgnoreCase);

		# region events

		/// <summary>
		/// onClick sets a script to run when the user clicks on the link.
		/// </summary>
		[DescriptionAttribute("")]
		[DefaultValueAttribute("")]
		[CategoryAttribute("JavaScript Events")]
		[EditorAttribute(
			 typeof(UITypeEditorString),
			 typeof(UITypeEditor))]
		[DisplayNameAttribute()]
		public string ScriptOnClick
		{
			get
			{
				return base.GetStringAttribute("onClick");
			}

			set
			{
				base.SetStringAttribute("onClick", value);
			}
		}

		/// <summary>
		/// onMouseOver is an event handlers that is triggered when the mouse moves over the link. 
		/// </summary>
		/// <remarks>
		/// The most common use for these event handler are rollover images, but you put any kind of program in them you want.
		/// </remarks>
		[CategoryAttribute("JavaScript Events")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
		[EditorAttribute(
			 typeof(UITypeEditorString),
			 typeof(UITypeEditor))]
		[DisplayName()]
		public string ScriptOnMouseOver
		{
			get
			{
				return base.GetStringAttribute("onMouseOver");
			}

			set
			{
				base.SetStringAttribute("onMouseOver", value);
			}
		}

		/// <summary>
		/// onMouseOut is an event handler that is triggered when the mouse moves out from the element again.
		/// </summary>
		/// <remarks>
		/// The most common use for these event handler are rollover images, but you put any kind of program in them you want.
		/// </remarks> 
		[DescriptionAttribute("")]
		[DefaultValueAttribute("")]
		[CategoryAttribute("JavaScript Events")]
		[EditorAttribute(
			 typeof(UITypeEditorString),
			 typeof(UITypeEditor))]
		[DisplayName()]
		public string ScriptOnMouseOut
		{
			get
			{
				return base.GetStringAttribute("onMouseOut");
			}

			set
			{
				base.SetStringAttribute("onMouseOut", value);
			}
		}


		# endregion

		# region Common Attributes

		/// <summary>
		/// Specifies the shadow for this shape.
		/// </summary>
		/// <remarks>
		/// See the <see cref="ShadowElement"/> for details.
		/// </remarks>
		[CategoryAttribute("Element Layout")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public virtual VgShadow Shadow
		{
			get
			{
				object o = GetAttribute("shadow");
				if (o == null) return null;
                IntPtr ptr = Marshal.GetIDispatchForObject(o);
				Interop.IDispatch idisp = (Interop.IDispatch) Marshal.GetObjectForIUnknown(ptr); 

				ITypeInfo ti;
				idisp.GetTypeInfo(0, 0, out ti);

				string name, doc, f;
				int h;
                ((ITypeInfo)ti).GetDocumentation(-1, out name, out doc, out h, out f);
                IVgShadow vgShadow = o as IVgShadow;
                if (vgShadow == null) return null;
				return new VgShadow(vgShadow);
			}
		}

        /// <summary>
        /// HEIGHT is the total width of the rectangle which surrounds the shape.
        /// </summary>
        [CategoryAttribute("Element Layout")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorUnit),
             typeof(UITypeEditor))]
        [DisplayName()]
        public virtual Unit Height
        {
            set
            {
                this.SetStyleAttribute ("height", value.ToString());
            } 
      
            get
            {
                return Unit.Parse(this.GetStyleAttribute("height"), VmlDesignerBehavior.DefaultCulture);
            } 
      
        }

        /// <summary>
        /// WIDTH is the total height of the rectangle which surrounds the shape.
        /// </summary>
        [CategoryAttribute("Element Layout")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorUnit),
             typeof(UITypeEditor))]
        [DisplayName()]
        public virtual Unit Width
        {
            set
            {
                this.SetStyleAttribute ("width", value.ToString());
            } 
      
            get
            {
                return Unit.Parse(this.GetStyleAttribute("width"), VmlDesignerBehavior.DefaultCulture);
            } 
      
        }

        /// <summary>
        /// LEFT is the left position relative to the container.
        /// </summary>
        [CategoryAttribute("Element Layout")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorUnit),
             typeof(UITypeEditor))]
        [DisplayName()]
        public virtual Unit Left
        {
            set
            {
                this.SetStyleAttribute ("left", value.ToString());
            } 
      
            get
            {
                return Unit.Parse(this.GetStyleAttribute("left"), VmlDesignerBehavior.DefaultCulture);
            } 
      
        }

        /// <summary>
        /// TOP is the top position relative to the container.
        /// </summary>
        [CategoryAttribute("Element Layout")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorUnit),
             typeof(UITypeEditor))]
        [DisplayName()]
        public virtual Unit Top
        {
            set
            {
                this.SetStyleAttribute ("top", value.ToString());
            } 
      
            get
            {
                return Unit.Parse(this.GetStyleAttribute("top"), VmlDesignerBehavior.DefaultCulture);
            } 
      
        }

        /// <summary>
        /// The CLASS attribute.
        /// </summary>
        /// <remarks>
        /// This attribute is used to assign style classes from a style sheet (CSS).
        /// </remarks>
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [CategoryAttribute("Element Style")]
        [TypeConverterAttribute(typeof(UITypeConverterDropSelection))]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string Class
        {
            get
            {
                return base.GetBaseElement().GetClassName();
            }

            set
            {
                base.GetBaseElement().SetClassName(value);
            }
        }

        /// <summary>
        /// The ID attribute.
        /// </summary>
        /// <remarks>
        /// This attribute defines an unique ID to the element. It can be used to assign styles from a style sheet (CSS) or
        /// to access the element using Scripting.
        /// </remarks>
        [MergablePropertyAttribute(false)]
        [DescriptionAttribute("")]
        [ParenthesizePropertyNameAttribute(true)]
        [CategoryAttribute("Element Style")]
        [DefaultValueAttribute("")]
        [TypeConverterAttribute(typeof(UITypeConverterDropSelection))]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string Id
        {
            get
            {
                return base.GetBaseElement().GetId();
            }
            set
            {
                base.GetBaseElement().SetId(value);
            }
        }

        /// <summary>
        /// Defines the STYLE attribute.
        /// </summary>
        /// <remarks>
        /// The STYLE attribute can contain inline style definitions. If the NetRix UI is used (not supported by Light Version) the integrated
        /// style editor of the UI assembly can be used to create the styles.
        /// </remarks>
        [CategoryAttribute("Element Style")]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [TypeConverterAttribute(typeof(string))]
        [EditorAttribute(
             typeof(UITypeEditorStyleStyle),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string Style
        {
            get
            {
//                Interop.IHTMLStyle s = base.GetBaseElement().GetStyle() as Interop.IHTMLStyle;
//                if (s == null)
//                    return System.String.Empty;
//                else
//                    return s.GetCssText();
                return base.GetStringAttribute("style");
            }

            set
            {
                base.SetStringAttribute("style", value);
//                base.GetBaseElement().GetStyle().SetCssText(value);
            }
        }

        /// <summary>
        /// W3C says that TITLE is "an advisory title for the linked resource".
        /// </summary>
        /// <remarks>
        /// The idea is that TITLE gives a description of the linked resource that is more informative than the URL.
        /// <para>
        /// Most browsers have ignored this useful attribute. One that doesn't ignore it is Opera. Opera displays the value of TITLE in a small box when the pointer is over a link. 
        /// The latest version of MSIE also shows a little box over the link, but otherwise ignores TITLE.
        /// </para>
        /// </remarks>
        [DescriptionAttribute("")]
        [CategoryAttribute("Standard Values")]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string Title
        {
            get
            {
                return base.GetStringAttribute("title");
            }

            set
            {
                base.SetStringAttribute("title", value);
            }
        }

        # endregion

        # region Helper Methods

        /// <summary>
        /// This method takes a full (absolute) URL and returns it as relative URL.
        /// </summary>
        /// <remarks>
        /// This method is internally used to deal with relative Paths even if the control
        /// need absolute Paths for images and other sources. The base URL which is used to
        /// recognize the relative Path is found in the <see cref="GuruComponents.Netrix.HtmlEditor.Url"/>
        /// property.
        /// <para>
        /// If that Path set to c:\inetpub\wwwroot\Path\ and the absolute Path of an image is
        /// c:\inetpub\wwwroot\Path\image\xxx.jpg the method will return "image\xxx.jpg". The
        /// PropertyGrid uses this to display only relative parts.
        /// </para>
        /// <para>
        /// Additionally a possibly attached file:// moniker will be removed. Hashmarks (#) at the end of the string
        /// will not harm the resolvation. The method removes them before resolvation and add them at the end.
        /// </para>
        /// </remarks>
        /// <param name="absoluteUrl">The absolute URL of a source.</param>
        /// <returns>The relative part accordingly to the base URL.</returns>
        protected string GetRelativeUrl(string absoluteUrl)
        {
            if (absoluteUrl == null || absoluteUrl.Length == 0)
            {
                return String.Empty;
            }
            string[] hashvalue = absoluteUrl.Split(new char[] {'#'}, 2);
            string str1 = absoluteUrl;
            string hash = String.Empty;
            if (hashvalue.Length == 2)
            {                
                str1 = hashvalue[0];
                hash = String.Concat("#", hashvalue[1]);
            } 
            if (HtmlEditor != null)
            {
                string str2 = (base.HtmlEditor.Url == null || HtmlEditor.Url.Equals("about:blank")) ? Path.GetDirectoryName(HtmlEditor.TempFile) : HtmlEditor.Url;
                if (str2 != null && str2.Length > 0)
                {
                    try
                    {
                        Uri uri1 = new Uri(str2);
                        Uri uri2 = new Uri(str1);
                        str1 = uri1.MakeRelativeUri(uri2).AbsolutePath;
                    }
                    catch
                    {
                    }
                }
            }
            str1 = fileRegex.Replace(str1, String.Empty);
            return String.Concat(str1, hash);
        }

        # endregion

        /// <summary>
        /// Create new instance of element.
        /// </summary>
        /// <param name="newTag">Tag name</param>
        /// <param name="editor">Editor reference</param>
        protected BaseShapeElement(string newTag, IHtmlEditor editor) : base(newTag, editor)
        {
        }

        /// <summary>
        /// Create new instance of element.
        /// </summary>
        /// <param name="peer">Peer object</param>
        /// <param name="editor">Editor reference</param>
        protected BaseShapeElement(Interop.IHTMLElement peer, IHtmlEditor editor)
            : base(peer, editor)
        {
        }
    }

}