using System;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.HighLighting
{
	/// <summary>
	/// HighLightSegment holds a highlighting object and provides a remove method to remove the decoration
	/// at a later time.
	/// </summary>
	public class HighLightSegment : IHighLightSegment
	{

        private Interop.IHighlightSegment ppISegment = null;
		private Interop.IHighlightRenderingServices render;
        private Interop.IHTMLTxtRange range;

		internal HighLightSegment(Interop.IHighlightRenderingServices render, Interop.IHTMLRenderStyle renderStyle, Interop.IDisplayPointer dpStart, Interop.IDisplayPointer dpEnd, Interop.IHTMLTxtRange range)
		{
			Interop.IHighlightSegment ppI;
			this.render = render;
            this.range = range;
			this.render.AddSegment(dpStart, dpEnd, renderStyle, out ppI);
            this.ppISegment = ppI;
		}

		internal void SetSegment(Interop.IHighlightSegment ppISegment)
		{
			this.ppISegment = ppISegment;
		}

		/// <summary>
		/// This method removes the highlight decoration from the segment.
		/// </summary>
		public void RemoveSegment()
		{
			if (ppISegment != null)
			{
				this.render.RemoveSegment(this.ppISegment);
				this.ppISegment = null;
			}
		}

        public override string ToString()
        {
            return range.GetText();
        }

        public string Html
        {
            get
            {
                return range.GetHtmlText();
            }
            set
            {
                range.PasteHTML(value);
            }

        }

        public void ScrollIntoView(bool startOfRange)
        {
            range.ScrollIntoView(startOfRange);
        }

        public void CollapseRange(bool startOfRange)
        {
            range.Collapse(startOfRange);
        }

        /// <summary>
        /// Expand the current range to that partial units are completely contained.
        /// </summary>
        /// <param name="ExpandTo"></param>
        /// <returns></returns>
        public bool ExpandRange(MoveUnit ExpandTo)
        {
            return range.Expand(Enum.GetName(typeof(MoveUnit), ExpandTo).ToLower());
        }

	}
}