
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GuruComponents.CodeEditor.CodeEditor
{
	[ToolboxItem(false)]
	public class TabListBox : ListBox
	{
		/// <summary>
		/// For public use only.
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool IsInputKey(Keys keyData)
		{
			return true;
		}

		/// <summary>
		/// For public use only.
		/// </summary>
		/// <param name="charCode"></param>
		/// <returns></returns>
		protected override bool IsInputChar(char charCode)
		{
			return true;
		}
	}

	/// <summary>
	/// Summary description for ListItem.
	/// </summary>
	public class ListItem : IComparable
	{
		/// <summary>
		/// The text of a ListItem
		/// </summary>
		public string Text = "";

		/// <summary>
		/// The insert text of a ListItem
		/// </summary>
		public string InsertText = "";

		/// <summary>
		/// The type of the ListItem (the type is used as an index to choose what icon to display)
		/// </summary>
		public int Type = 0;

		/// <summary>
		/// The tooltip text that should be displayed when selecting a ListItem
		/// </summary>
		public string ToolTip = "";

		/// <summary>
		/// ListItem constructor , takes text and type as parameters
		/// </summary>
		/// <param name="text">The text that should be assigned to the ListItem</param>
		/// <param name="type">The type of the ListItem</param>
		public ListItem(string text, int type)
		{
			Text = text;
			Type = type;
			ToolTip = "";
		}

		/// <summary>
		/// ListItem constructor , takes text , type and tooltip text as parameters
		/// </summary>
		/// <param name="text">The text that should be assigned to the ListItem</param>
		/// <param name="type">The type of the ListItem</param>
		/// <param name="tooltip">The tooltip text that should be assigned to the ListItem</param>
		public ListItem(string text, int type, string tooltip)
		{
			Text = text;
			Type = type;
			ToolTip = tooltip;
		}

		/// <summary>
		/// ListItem constructor , takes text , type , tooltip text and insert text as parameters
		/// </summary>
		/// <param name="text">The text that should be assigned to the ListItem</param>
		/// <param name="type">The type of the ListItem</param>
		/// <param name="tooltip">The tooltip text that should be assigned to the ListItem</param>
		/// <param name="inserttext">The text that should be inserted into the text when this item is selected</param>
		public ListItem(string text, int type, string tooltip, string inserttext)
		{
			Text = text;
			Type = type;
			ToolTip = tooltip;
			InsertText = inserttext;
		}

		#region Implementation of IComparable

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			ListItem li = (ListItem) obj;
			return this.Text.CompareTo(li.Text);
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Text;
		}

	}
}