using System;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// This class allows access to additional positioning information.
	/// </summary>
	/// <remarks>
	/// This class enhances the basic BodyElement class. It was introduced to avoid
	/// the ability of non-HTML attributes for propertygrid access. The ScrollXX properties allow the
	/// detection of the current scroll position to synchronize external controls like ruler or bars.
	/// <para>
	/// The positions are in pixel and relative to the control window.
	/// </para>
	/// </remarks>
	public class BodyElementExt : BodyElement
	{

        internal BodyElementExt(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }

		private Interop.IHTMLElement2 el2
		{
            get
            {
               return (Interop.IHTMLElement2) ((Element) this).GetBaseElement();
            }
		}

        public int ScrollLeft
        {
            get
            {
                return el2.GetScrollLeft();
            }
        }

        public int ScrollTop
        {
            get
            {
                return el2.GetScrollTop();
            }
        }

        public int ScrollHeight
        {
            get
            {
                return el2.GetScrollHeight();
            }
        }


        public int ScrollWidth
        {
            get
            {
                return el2.GetScrollWidth();
            }
        }

        public int ClientHeight
        {
            get
            {
                return el2.GetClientHeight();
            }
        }
        
        public int ClientWidth
        {
            get
            {
                return el2.GetClientWidth();
            }
        }
	}
}
