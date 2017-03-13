using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.PlugIns;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.TableDesigner
{
	/// <summary>
	/// The TableDesigner Extender Provider.
	/// </summary>
    /// <remarks>
    /// The TableDesigner plug-in provides advanced designer features in interactive as well as programmatic mode.
    /// To activate the designer add the component from toolbox to your form and view the properties in the
    /// Property Grid. 
    /// <para>
    /// In case you want or need set the properties directly, without the VS.NET designer, the following code shows the basic way:
    /// <code>
    /// this.tableDesigner1 = new GuruComponents.Netrix.TableDesigner.TableDesigner(this.components);
    /// TableDesignerProperties tp = new TableDesignerProperties();
    /// tp.Active = true;
    /// tp.ProcessTABKey = true;
    /// tp.SliderActivated = false;
    /// tp.SliderAddMode = false;
    /// tp.TableBackground = Color.Empty;
    /// tp.StaticBehavior = true;
    /// tp.WithCellSelection = false;
    /// tp.TableBorder = new TableDesignerProperties.TableBorderBehavior(Color.Gray, 1, System.Drawing.Drawing2D.DashStyle.Solid);
    /// tp.CellBorder = new TableDesignerProperties.CellBorderBehavior(Color.Silver, 1, System.Drawing.Drawing2D.DashStyle.Dot);
    /// this.tableDesigner1.SetTableDesigner(htmlEditor, tp);
    /// </code>
    /// To get more information about the properties, watch the <see cref="TableDesignerProperties"/> class. Instead of using the
    /// property bag at runtime you can also call the several Set-Methods available in this class to set the values. All settings
    /// have immediate effect on the current table and effect other tables once the screen refresh requires a redraw.
    /// </para>
    /// The table designer has two basic features:
    /// <list type="bullet">
    ///     <item>Editing tables interactively, by user using the mouse or key.</item>
    ///     <item>Editing tables programmatically, by sending commands to the TableDesigner plugin.</item>
    /// </list>
    /// In both usage scenarios it's recommended to add a few basic features to your editor project:
    /// <example>
    /// Activating the editor and making tables visible by using build-in functionality:
    /// <code>
    /// this.htmlEditor.InvokeCommand(this.tableDesigner1.Commands.Activate);
    /// this.htmlEditor.InvokeCommand(this.tableDesigner1.Commands.AddBehaviors);
    /// this.htmlEditor.BordersVisible = false;
    /// this.tableDesigner1.TableBecomesInactive -= new GuruComponents.Netrix.TableDesigner.TableEventBecomesInactiveHandler(this.tableDesigner1_TableBecomesInactive);
    /// this.tableDesigner1.TableBecomesActive -= new GuruComponents.Netrix.TableDesigner.TableEventBecomesActiveHandler(this.tableDesigner1_TableBecomesActive);
    /// this.tableDesigner1.TableBecomesInactive += new GuruComponents.Netrix.TableDesigner.TableEventBecomesInactiveHandler(this.tableDesigner1_TableBecomesInactive);
    /// this.tableDesigner1.TableBecomesActive += new GuruComponents.Netrix.TableDesigner.TableEventBecomesActiveHandler(this.tableDesigner1_TableBecomesActive);
    /// </code>
    /// The commands invokes first activate the designer and the build-in border behavior makes the tables visible, even with no borders.
    /// Then, the internal (default) table border feature of the NetRix control is switched off. Otherwise both borders will overlap,
    /// which is usually not very helpful.
    /// Finally, two events being attached to interact with the table designer. The handler usually activate/deactive tools, menu items
    /// or dialogs used to invoke table related commands. If the user clicks a menu item or tool, you need to invoke a command to 
    /// perform the required action against a table:
    /// <code>
    /// this.htmlEditor.InvokeCommand(this.tableDesigner1.Commands.InsertTableRowAfter);
    /// </code>
    /// Invoking commands is done via the NetRix editor, which holds the current table. That way we support multiple editors on a
    /// form with one instance of the table designer only. The commands collection contains available commands. These are being send
    /// using the editor's InvokeCommand method. Available commands are described <see cref="TableCommands">here</see>.
    /// </example>
    /// Note: More information about user driven designer features are available in the manual.
    /// </remarks>
    /// <seealso cref="TableDesignerProperties"/>
    /// <seealso cref="TableCommands"/>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(GuruComponents.Netrix.TableDesigner.TableDesigner), "Resources.TableDesigner.ico")]
	[ProvideProperty("TableDesigner", "GuruComponents.Netrix.IHtmlEditor")]
	public class TableDesigner : Component, System.ComponentModel.IExtenderProvider, GuruComponents.Netrix.PlugIns.IPlugIn
    {
        
        private Hashtable properties;
		private Hashtable behaviors;        

		/// <summary>
        /// Default constructor supports design time behavior.
		/// </summary>
		public TableDesigner()
        {
            properties = new Hashtable();
			behaviors = new Hashtable();
		}

        /// <summary>
        /// This constructor supports design time behavior.
        /// </summary>
        /// <param name="parent"></param>
		public TableDesigner(IContainer parent) : this()
		{
			properties = new Hashtable();
			parent.Add(this);
		}

		private TableDesignerProperties EnsurePropertiesExists(IHtmlEditor key)
		{
			TableDesignerProperties p = (TableDesignerProperties) properties[key];
			if (p == null)
			{
				p = new TableDesignerProperties();
				properties[key] = p;
			}
			return p;
		}

		private TableEditDesigner EnsureBehaviorExists(IHtmlEditor key)
		{
			TableEditDesigner b = (TableEditDesigner) behaviors[key];
			if (b == null)
			{
				b = new TableEditDesigner(key as IHtmlEditor, EnsurePropertiesExists(key), this);                
				behaviors[key] = b;
			}
			return b;
		}

		# region Events

        /// <summary>
        /// Called if the context menu is up to show.
        /// </summary>
        /// <remarks>Fires the <see cref="ShowContextMenu"/> event, if not overriden in derived classes.</remarks>
        /// <seealso cref="ShowContextMenuEventArgs"/>
        /// <param name="clientX">X coordinate of mouse.</param>
        /// <param name="clientY">Y coodinate of mouse.</param>
        /// <param name="t">Element causing the context menu to show up.</param>
        /// <param name="key">Reference to HTML editor causes the event within the table designer.</param>
        protected internal void OnShowContextMenu(int clientX, int clientY, Interop.IHTMLElement t, IHtmlEditor key)
        {
            if (ShowContextMenu != null)
            {
                ShowContextMenuEventArgs cme = new ShowContextMenuEventArgs(
                    new Point(clientX, clientY),
                    false,
                    (int)MenuIdentifier.Table,
                    key.GenericElementFactory.CreateElement(t));
                ShowContextMenu(this, cme);
            }
        }

        /// <summary>
        /// Called if a table becomes active.
        /// </summary>
        /// <param name="e"></param>
		protected internal void OnTableBecomesActive(TableEventArgs e)
		{
			if (TableBecomesActive != null)
			{
				TableBecomesActive(e);
			}
		}

        /// <summary>
        /// Called if a table becomes inactive. 
        /// </summary>
        /// <param name="e"></param>
		protected internal void OnTableBecomesInactive(TableEventArgs e)
		{
			if (TableBecomesInactive != null)
			{
				TableBecomesInactive(e);
			}
		}
    
        /// <summary>
        /// Called after cell seletions.
        /// </summary>
        /// <param name="e"></param>
		protected internal void OnTableCellSelection(TableCellSelectionEventArgs e)
		{
			if (TableCellSelection != null)
			{
				TableCellSelection(e);
			}
		}

        /// <summary>
        /// Called after cell selection is being removed.
        /// </summary>
        /// <param name="e"></param>
		protected internal void OnTableCellSelectionRemoved(TableCellSelectionEventArgs e)
		{
			if (TableCellSelectionRemoved != null)
			{
				TableCellSelectionRemoved(e);
			}
		}

		/// <summary>
		/// Will fire if the user has made a cell selection. 
		/// </summary>
		/// <remarks>
		/// This informs the handler that operations based 
		/// on selections are now possible.
		/// </remarks>
		[Category("NetRix Events"), Description("Will fire if the user has made a cell selection.")]
		public event TableCellSelectionEventHandler TableCellSelection;

		/// <summary>
		/// Will fire if the selection was removed from the table and the cellstack is cleared.
		/// </summary>
		[Category("NetRix Events"), Description("Will fire if the selection was removed from the table and the cellstack is cleared.")]
		public event TableCellSelectionEventHandler TableCellSelectionRemoved;

		/// <summary>
		/// Fired if a table becomes active in table designer mode.
		/// </summary>
		/// <remarks>
		/// This happens by key operations inside the table only. 
		/// The event is fired on time after entering the table with caret. See <see cref="TableBecomesInactive"/> for a related event.
		/// </remarks>
		[Category("NetRix Events"), Description("Fired if a table becomes active in table designer mode.")]
		public event TableEventBecomesActiveHandler TableBecomesActive;

		/// <summary>
		/// Fired if the currently active table becomes inactive.
		/// </summary>
		/// <remarks>
		/// The occurence of this events means that the caret is placed outside the table. If the caret
		/// left the current table and enters another one the event is still fired.
		/// </remarks>
		[Category("NetRix Events"), Description("Fired if the currently active table becomes inactive.")]
		public event TableEventBecomesInactiveHandler TableBecomesInactive;

                /// <summary>
        /// Fired if the user tries to show up the context menu above a table.
        /// </summary>
        [Category("NetRix Events"), Description("Fired if the user tries to show up the context menu above a table.")]
		public event ShowContextMenuEventHandler ShowContextMenu;


		# endregion

		# region ExtenderProvider Code

        /// <summary>
        /// Designer method, returns the properties of the attached editor.
        /// </summary>
        /// <param name="htmlEditor">The HtmlEditor to what the table designer belongs to.</param>
        /// <returns>The property object.</returns>
		[ExtenderProvidedProperty(), Category("NetRix Component"), Description("HelpLine Properties")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public TableDesignerProperties GetTableDesigner(IHtmlEditor htmlEditor)
		{
			return this.EnsurePropertiesExists(htmlEditor);
		}
        /// <summary>
        /// Designer method, sets the properties for given editor.
        /// </summary>
        /// <param name="htmlEditor">The HtmlEditor to what the table designer belongs to.</param>
        /// <param name="Properties">The Property object.</param>
        /// <returns>The property object.</returns>		
        public void SetTableDesigner(IHtmlEditor htmlEditor, TableDesignerProperties Properties)
		{
			// assutre that only first call sets alls values
			if (properties[htmlEditor] == null)
			{
				EnsurePropertiesExists(htmlEditor).Active = Properties.Active;
				EnsurePropertiesExists(htmlEditor).FastResizeMode = Properties.FastResizeMode;
				EnsurePropertiesExists(htmlEditor).ProcessTABKey = Properties.ProcessTABKey;
				EnsurePropertiesExists(htmlEditor).SliderActivated = Properties.SliderActivated;
				EnsurePropertiesExists(htmlEditor).SliderAddMode = Properties.SliderAddMode;
				EnsurePropertiesExists(htmlEditor).SliderLine = Properties.SliderLine;
				EnsurePropertiesExists(htmlEditor).TableBackground = Properties.TableBackground;
				EnsurePropertiesExists(htmlEditor).WithCellSelection = Properties.WithCellSelection;
                EnsurePropertiesExists(htmlEditor).AdvancedParameters = Properties.AdvancedParameters;
				EnsurePropertiesExists(htmlEditor).TableBorder = Properties.TableBorder;
				EnsurePropertiesExists(htmlEditor).CellBorder = Properties.CellBorder;
				EnsurePropertiesExists(htmlEditor).StaticBehavior = Properties.StaticBehavior;

				EnsurePropertiesExists(htmlEditor).CellSelectionBorderColor = Properties.CellSelectionBorderColor;
				EnsurePropertiesExists(htmlEditor).CellSelectionColor = Properties.CellSelectionColor;
                       				

				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.AddBehaviors));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.RemoveBehaviors));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.Activate));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.DeActivate));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.AddCaption));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.RemoveCaption));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.DeleteTableRow));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.DeleteTableColumn));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.InsertTableRow));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.InsertTableRowAfter));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.InsertTableColumn));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.InsertTableColumnAfter));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.MergeLeft));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.MergeRight));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.MergeCells));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.MergeCellsPreserveContent));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.MergeUp));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.MergeDown));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.InsertCell));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.DeleteCell));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.SplitHorizontal));
				htmlEditor.AddCommand(new CommandWrapper(new EventHandler(TableOperation), Commands.SplitVertical));
			} 
			else 
			{
				EnsurePropertiesExists(htmlEditor).Active = Properties.Active;
				EnsurePropertiesExists(htmlEditor).FastResizeMode = Properties.FastResizeMode;
				EnsurePropertiesExists(htmlEditor).ProcessTABKey = Properties.ProcessTABKey;
				EnsurePropertiesExists(htmlEditor).SliderActivated = Properties.SliderActivated;
				EnsurePropertiesExists(htmlEditor).SliderAddMode = Properties.SliderAddMode;
				EnsurePropertiesExists(htmlEditor).SliderLine = Properties.SliderLine;
				EnsurePropertiesExists(htmlEditor).TableBackground = Properties.TableBackground;
				EnsurePropertiesExists(htmlEditor).WithCellSelection = Properties.WithCellSelection;
            
				EnsurePropertiesExists(htmlEditor).TableBorder = Properties.TableBorder;
				EnsurePropertiesExists(htmlEditor).CellBorder = Properties.CellBorder;
				EnsurePropertiesExists(htmlEditor).StaticBehavior = Properties.StaticBehavior;

				EnsurePropertiesExists(htmlEditor).CellSelectionBorderColor = Properties.CellSelectionBorderColor;
				EnsurePropertiesExists(htmlEditor).CellSelectionColor = Properties.CellSelectionColor;
			}
            // Register
            htmlEditor.RegisterPlugIn(this);
            // Set behaviors is requested
            htmlEditor.AddEditDesigner(EnsureBehaviorExists(htmlEditor) as Interop.IHTMLEditDesigner);
        }        

		# region Direct Property Settings During Runtime
        /// <summary>
        /// Activates of deactivates the table designer.
        /// </summary>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="active">The mode to select (<c>true</c> = activated, <c>false</c> = deactivated).</param>
		public void SetActive(IHtmlEditor htmlEditor, bool active)
		{
			EnsurePropertiesExists(htmlEditor).Active = active;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's fast resize mode.
        /// </summary>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="mode">The mode to select (<c>true</c> = activated, <c>false</c> = deactivated).</param>
        public void SetFastResizeMode(IHtmlEditor htmlEditor, bool mode)
		{
			EnsurePropertiesExists(htmlEditor).FastResizeMode = mode;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's tab key mode.
        /// </summary>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="recognizeTAB">The mode to select (<c>true</c> = activated, <c>false</c> = deactivated).</param>
        public void SetProcessTABKey(IHtmlEditor htmlEditor, bool recognizeTAB)
		{
			EnsurePropertiesExists(htmlEditor).ProcessTABKey = recognizeTAB;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's slider mode.
        /// </summary>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="active">The mode to select (<c>true</c> = activated, <c>false</c> = deactivated).</param>
        public void SetSliderActivated(IHtmlEditor htmlEditor, bool active)
		{
			EnsurePropertiesExists(htmlEditor).SliderActivated = active;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's add mode.
        /// </summary>
        /// <remarks>
        /// The add mode determines how the resize of the right most column and the right outer border.
        /// Usually the table is not being resized if the last column width is being changed. If the add mode
        /// is activated, the right most column will grow and the table width is growing accordingly.
        /// </remarks>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="addMode">The mode to select (<c>true</c> = activated, <c>false</c> = deactivated).</param>
        public void SetAddMode(IHtmlEditor htmlEditor, bool addMode)
		{
			EnsurePropertiesExists(htmlEditor).SliderAddMode = addMode;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's advanced mode.
        /// </summary>
        /// <remarks>
        /// The advanced mode shows additional properties concerning the current cell properties as well as 
        /// rulers and other information about table dimensions. This option requires higher performance (we
        /// recommend at least 2 GHz).
        /// </remarks>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="advModeOn">The mode to select (<c>true</c> = activated, <c>false</c> = deactivated).</param>
        public void SetAdvancedMode(IHtmlEditor htmlEditor, bool advModeOn)
        {
            EnsurePropertiesExists(htmlEditor).AdvancedParameters = advModeOn;
            EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
        }
        /// <summary>
        /// Activates of deactivates the table designer's slider line properties.
        /// </summary>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="sliderLine">The mode to select (<c>true</c> = activated, <c>false</c> = deactivated).</param>
        public void SetSliderLine(IHtmlEditor htmlEditor, GuruComponents.Netrix.TableDesigner.TableDesignerProperties.SliderLineProperty sliderLine)
		{
			EnsurePropertiesExists(htmlEditor).SliderLine = sliderLine;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's background properties.
        /// </summary>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="backGround">The mode to select (<c>true</c> = activated, <c>false</c> = deactivated).</param>
        public void SetTableBackGround(IHtmlEditor htmlEditor, Color backGround)
		{
			EnsurePropertiesExists(htmlEditor).TableBackground = backGround;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's cell selection properties.
        /// </summary>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="withSelection">The mode to select (<c>true</c> = activated, <c>false</c> = deactivated).</param>
        public void SetWithCellSelection(IHtmlEditor htmlEditor, bool withSelection)
		{
			EnsurePropertiesExists(htmlEditor).WithCellSelection = withSelection;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's border properties.
        /// </summary>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="tableBorder">The mode to select (<c>true</c> = activated, <c>false</c> = deactivated).</param>
        public void SetTableBorder(IHtmlEditor htmlEditor, GuruComponents.Netrix.TableDesigner.TableDesignerProperties.TableBorderBehavior tableBorder)
		{
			EnsurePropertiesExists(htmlEditor).TableBorder = tableBorder;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's cell properties.
        /// </summary>
        /// <remarks>
        /// This belongs to cell in non selected mode.
        /// </remarks>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="cellBorder">The .</param>
        public void SetCellBorder(IHtmlEditor htmlEditor, GuruComponents.Netrix.TableDesigner.TableDesignerProperties.CellBorderBehavior cellBorder)
		{
			EnsurePropertiesExists(htmlEditor).CellBorder = cellBorder;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's static mode.
        /// </summary>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="staticBehavior">The mode to select (<c>true</c> = activated, <c>false</c> = deactivated).</param>
        public void SetStaticBehavior(IHtmlEditor htmlEditor, bool staticBehavior)
		{
			EnsurePropertiesExists(htmlEditor).StaticBehavior = staticBehavior;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's cell selection border color.
        /// </summary>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="cellBorder">The cell border color for selected mode.</param>
        public void SetCellSelectionBorderColor(IHtmlEditor htmlEditor, Color cellBorder)
		{
			EnsurePropertiesExists(htmlEditor).CellSelectionBorderColor = cellBorder;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}
        /// <summary>
        /// Activates of deactivates the table designer's cell selection color.
        /// </summary>
        /// <param name="htmlEditor">The editor this designer instance belongs too.</param>
        /// <param name="cellSelection">The cell color in selected mode.</param>
        public void SetCellSelectionColor(IHtmlEditor htmlEditor, Color cellSelection)
		{
			EnsurePropertiesExists(htmlEditor).CellSelectionColor = cellSelection;
			EnsureBehaviorExists(htmlEditor).SetProperties(EnsurePropertiesExists(htmlEditor));
		}

		# endregion

		private TableCommands commands;

        /// <summary>
        /// Gives access to the commands the table designer accepts via the Invoke call.
        /// </summary>
		[Browsable(false)]
		public TableCommands Commands
		{
			get
			{
				if (commands == null)
				{
					commands = new TableCommands();
				}
				return commands;
			}
		}

		/// <summary>
		/// Sets the current table and cell explicitly.
		/// </summary>
		/// <remarks>
		/// The host can use this method to force setting the current table and cell to allow
		/// any table formatting operation issued by the table designer working with that table,
		/// instead of the user selected one.
		/// <para>
		/// Setting the cell is necessary, because many commands use the current cell as a starting
		/// point for there operation.
		/// </para>
		/// <para>
		/// Calling this method will not fire the <see cref="TableBecomesActive"/> event. Additionally,
		/// the designer and the formatter stay no longer in sync regarding the used table.
		/// </para>
		/// </remarks>
		/// <param name="editor">The editor instance, from which the table element comes.</param>
		/// <param name="table">The table, which has to be set.</param>
		/// <param name="cell">The cell within the table.</param>
		public void SetFormatter(IHtmlEditor editor, TableElement table, TableCellElement cell)
		{
			EnsureBehaviorExists(editor).TableFormatter.TableInfo = null;
			((TableFormatter)EnsureBehaviorExists(editor).TableFormatter).CurrentTable = (Interop.IHTMLTable) table.GetBaseElement();
			((TableFormatter)EnsureBehaviorExists(editor).TableFormatter).CurrentCell = cell;
		}

		/// <summary>
		/// Detects whether the editor has a current table.
		/// </summary>
		/// <remarks>
		/// This information is useful to update related menu items or toolbars.
		/// </remarks>
		/// <param name="editor">The editor this operation refers to.</param>
		/// <returns>Returns <c>true</c>, if the table designer has a current table.</returns>
		public bool IsInTable(IHtmlEditor editor)
		{
			return (EnsureBehaviorExists(editor).CurrentTable != null) ? true : false;
		}

		/// <summary>
		/// Put the formatter back into automatic detection mode.
		/// </summary>
		/// <remarks>
        /// Any caller of the <see cref="TableFormatter.CurrentTable"/> or <see cref="TableFormatter.CurrentCell"/> is supposed to call this method to
		/// put the formatter back into detection mode. This means, that the current table follows the
		/// mouse and as soon as a mouse move appears on top of a table the current table changes.
		/// </remarks>
		/// <param name="editor"></param>
		public void ResetFormatter(IHtmlEditor editor)
		{
            EnsureBehaviorExists(editor).TableFormatter.TableInfo = null; 
		}

        /// <summary>
        /// Gets direct access to the formatter.
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        public ITableFormatter GetFormatter(IHtmlEditor editor)
        {
            return EnsureBehaviorExists(editor).TableFormatter;
        }

		private void TableOperation(object sender, EventArgs e)
		{
			CommandWrapper cw = (CommandWrapper) sender;
			if (cw.CommandID.Guid.Equals(Commands.CommandGroup))
			{
				switch ((TableCommand)cw.ID)
				{
					case TableCommand.Activate:
						EnsureBehaviorExists(cw.TargetEditor).Activate(true);
						break;
					case TableCommand.DeActivate:
						EnsureBehaviorExists(cw.TargetEditor).Activate(false);
						break;
					case TableCommand.AddBehaviors:
						EnsureBehaviorExists(cw.TargetEditor).AddBehaviors();
						break;
					case TableCommand.RemoveBehaviors:
						EnsureBehaviorExists(cw.TargetEditor).RemoveBehaviors();
						break;
					case TableCommand.AddCaption:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.AddCaption();
						break;
					case TableCommand.RemoveCaption:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.RemoveCaption();
						break;
					case TableCommand.DeleteTableRow:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.DeleteTableRow();
						break;
					case TableCommand.DeleteTableColumn:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.DeleteTableColumn();
						break;
					case TableCommand.InsertTableRow:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.InsertTableRow(false);
						break;
					case TableCommand.InsertTableRowAfter:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.InsertTableRow(true);
						break;
					case TableCommand.InsertTableColumn:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.InsertTableColumn(false);
						break;
					case TableCommand.InsertTableColumnAfter:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.InsertTableColumn(true);
						break;
					case TableCommand.MergeLeft:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.MergeLeft();
						break;
					case TableCommand.MergeRight:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.MergeRight();
						break;
					case TableCommand.MergeCells:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.MergeCells();
						break;
					case TableCommand.MergeCellsPreserveContent:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.MergeCells(true);
						break;
					case TableCommand.MergeUp:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.MergeUp();
						break;
					case TableCommand.MergeDown:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.MergeDown();
						break;
					case TableCommand.InsertCell:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.InsertCell();
						break;
					case TableCommand.DeleteCell:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.DeleteCell();
						break;
					case TableCommand.SplitHorizontal:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.SplitHorizontal();
						break;
					case TableCommand.SplitVertical:
						EnsureBehaviorExists(cw.TargetEditor).TableFormatter.SplitVertical();
						break;
				}                
			}            
		}

        /// <summary>
        /// Called by the main control to inform the plug-in that the content is now ready.
        /// </summary>
        /// <remarks>THIS METHOD MUST NOT BE CALLED FROM USER CODE.</remarks>
        /// <param name="editor"></param>
        public void NotifyReadyStateCompleted(IHtmlEditor editor)
        {            
			if (EnsureBehaviorExists(editor).IsActivated && EnsurePropertiesExists(editor).StaticBehavior)
			{
				EnsureBehaviorExists(editor).AddBehaviors();
			}
            if (NotifySubReadyStateCompleted != null)
            {
                NotifySubReadyStateCompleted(editor, EventArgs.Empty);
            }
		}

        internal event EventHandler NotifySubReadyStateCompleted;

        /// <summary>
        /// Version of Plug-in Assembly.
        /// </summary>
		[Browsable(true), ReadOnly(true), Description("Version of Plug-in Assembly.")]
		public string Version
		{
			get
			{
				return this.GetType().Assembly.GetName().Version.ToString();
			}
		}

        /// <summary>
        /// Indicated whether the designer should serialize the property object.
        /// </summary>
        /// <param name="htmlEditor">Reference to editor the designer belongs to.</param>
        /// <returns>True in any case (constant value).</returns>
		public bool ShouldSerializeTableDesigner(IHtmlEditor htmlEditor)
		{
			return true;
		}

		# endregion

		#region IExtenderProvider Member

        /// <summary>
        /// Indicates that the plugin can extend the NetRix HTML editor component.
        /// </summary>
        /// <param name="extendee"></param>
        /// <returns></returns>
		public bool CanExtend(object extendee)
		{
			if (extendee is IHtmlEditor)
			{
				return true;
			} 
			else 
			{
				return false;
			}
		}

		#endregion

        /// <summary>
        /// Designer support.
        /// </summary>
        /// <returns>String visible in PropertyGrid.</returns>
		public override string ToString()
		{
			return "Click plus sign for details";
		}

        /// <summary>
        /// Name of the Plugin, constant "TableDesigner".
        /// </summary>
		#region IPlugIn Member
		
        [Browsable(true)]
		public string Name
		{
			get
			{
				return "TableDesigner";
			}
		}
        /// <summary>
        /// Informs that this is an ExtenderProvider. Always true.
        /// </summary>
		[Browsable(false)]
		public bool IsExtenderProvider
		{
			get
			{
				return true;
			}
		}
        /// <summary>
        /// Type of component.
        /// </summary>
		[Browsable(false)]
        public Type Type
        {
            get
            {
                return this.GetType();
            }
        }

        /// <summary>
        /// List of element types, which the extender plugin extends.
        /// </summary>
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.PlugIns.IPlugIn.GetElementExtenders"/> for background information.
        /// </remarks>
        public List<CommandExtender> GetElementExtenders(IElement component)
        {
            List<CommandExtender> extenderVerbs = new List<CommandExtender>();
            switch (component.GetType().Name)
            {
                case "TableElement":
                    extenderVerbs.AddRange(GetTblCommands());
                    break;
                case "TableCellElement":
                    extenderVerbs.AddRange(GetCellCommands());
                    break;
            }
            return extenderVerbs;
        }

        private IEnumerable<CommandExtender> GetCellCommands()
        {
            List<CommandExtender> lc = new List<CommandExtender>();
            CommandExtender cdel = new CommandExtender(Commands.DeleteCell);
            cdel.Text = "Remove Cell";
            cdel.Description = "Removes the cell from table";
            lc.Add(cdel);
            CommandExtender cadd = new CommandExtender(Commands.InsertCell);
            cadd.Text = "Add Cell";
            cadd.Description = "Add the cell from table";
            lc.Add(cadd);
            return lc;
        }

        private IEnumerable<CommandExtender> GetTblCommands()
        {
            List<CommandExtender> lc = new List<CommandExtender>();
            CommandExtender c1 = new CommandExtender(Commands.InsertTableRow);
            c1.Text = "Insert Row Before";
            c1.Description = "Insert Row Before";
            lc.Add(c1);
            CommandExtender c2 = new CommandExtender(Commands.InsertTableRowAfter);
            c2.Text = "Insert Row After";
            c2.Description = "Insert Row After";
            lc.Add(c2);
            CommandExtender c3 = new CommandExtender(Commands.InsertTableColumn);
            c3.Text = "Insert Column Before";
            c3.Description = "Insert Column Before";
            lc.Add(c3);
            CommandExtender c4 = new CommandExtender(Commands.InsertTableColumnAfter);
            c4.Text = "Insert Column After";
            c4.Description = "Insert Column After";
            lc.Add(c4);
            return lc;
        }

        /// <summary>
        /// Returns element specific features this plug-in supports.
        /// </summary>
        public Feature Features
        {
            get { return Feature.EditDesigner; }
        }

        /// <summary>
        /// Returns supported namespaces.
        /// </summary>
        /// <returns>Returns null, the table designer does not support namespaces.</returns>
        IDictionary GuruComponents.Netrix.PlugIns.IPlugIn.GetSupportedNamespaces(IHtmlEditor editor)
        {
            return null;
        }

        /// <summary>
        /// Creates an element.
        /// </summary>
        /// <remarks>
        /// Throws always a <see cref="NotSupportedException"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">The table designer cannot create elements that way.</exception>
        /// <param name="tagName"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        System.Web.UI.Control IPlugIn.CreateElement(string tagName, IHtmlEditor editor)
        {
            throw new NotSupportedException("The method or operation is not available. Use commands instead");
        }

        #endregion
    }
}
