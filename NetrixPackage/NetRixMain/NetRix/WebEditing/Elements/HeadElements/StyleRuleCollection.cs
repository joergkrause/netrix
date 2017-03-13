using System;
using System.Collections;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Styles;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This class holds a collection of style rules.
    /// </summary>
    /// <remarks>
    /// The purpose of this class is to support the NetRix infrastructure and the NetRix UI. You should
    /// not use this class directly in user code.
    /// </remarks>
    public class StyleRuleCollection : System.Collections.CollectionBase
    {

        internal StyleRuleCollection(Interop.IHTMLElement s) : base()
        {
            Interop.IHTMLStyleElement style = (Interop.IHTMLStyleElement) s;
            Interop.IHTMLStyleSheet ssheet = style.styleSheet;
			if (ssheet != null)
			{
				Interop.IHTMLStyleSheetRulesCollection rules = ssheet.GetRules();
				if (rules != null)
				{
					for (int i = 0; i < rules.GetLength(); i++)
					{
						Add(new StyleRule((Interop.IHTMLStyleSheetRule) rules.Item(i)));
					}
				}
			}
        }

        internal StyleRuleCollection(IStyleRule[] rules)
            : base()
        {
            for (int i = 0; i < rules.Length; i++)
            {
                Add((StyleRule)rules[i]);
            }
        }
        
        public void Add(StyleRule o) 
        {
            if (o == null) return;
            base.InnerList.Add(o);
        }

        public bool Contains(StyleRule o) 
        {
            if (o == null) return false;
            return InnerList.Contains(o);
        }

        public void Remove(StyleRule o) 
        {
            if (o == null) return;
            base.InnerList.Remove(o);
        }

        public StyleRule this[int index] 
        {
            get 
            {
                return base.InnerList[index] as StyleRule;
            }
            set
            {
                base.InnerList[index] = value;
            }
        }

        /// <summary>
        /// Fired if the collection editor inserts a new element.
        /// </summary>
        public event CollectionInsertHandler OnInsertHandler;
        /// <summary>
        /// Fired if the collection editor starts a new sequence.
        /// </summary>
        public event CollectionClearHandler OnClearHandler;

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete (index, value);
            // prevent from fail during first collection fill process, because the handler is temporary removed
            if (OnInsertHandler != null)                                                             
            {
                OnInsertHandler(index, value);
            }
        }

        protected override void OnClear()
        {
            base.OnClear ();
            // prevent from fail during first collection fill process, because the handler is temporary removed
            if (OnClearHandler != null)
            {
                OnClearHandler();
            }
        }
    }
}