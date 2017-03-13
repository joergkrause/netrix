using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using System;
using System.ComponentModel;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This interface supports the NetRix infrastructure und should not be used in user code.
    /// </summary>
	public interface IOptionElement
	{

        string label
		{
            get;
            set;
		}

        string text
        {
            get;
            set;
        }

		string @value
		{
			get;
			set;
		}

        bool selected
		{
			get;
			set;
		}

        bool disabled
		{
			get;
			set;
		}
	}
}
