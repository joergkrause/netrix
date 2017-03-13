

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GuruComponents.CodeEditor.Forms
{
	/// <summary>
	/// The ThumbControl is only intended to be used as a splitter thumb for the splitview in the SyntaxBox.
	/// </summary>
	[ToolboxItem(false)]
	public class ThumbControl : Control
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		/// <summary>
		/// Default constructor for the Thumbcontrol
		/// </summary>
		public ThumbControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ThumbControl
			// 
			this.Name = "ThumbControl";
			this.Size = new System.Drawing.Size(24, 24);
			this.BackColor = SystemColors.Control;

		}

		#endregion

		/// <summary>
		/// Draws a 2px Raised Border for the ThumbControl
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			ControlPaint.DrawBorder3D(e.Graphics, 0, 0, this.Width, this.Height, Border3DStyle.Raised);
		}
	}
}