using System;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;

namespace GuruComponents.Netrix
{

    /// <summary>
    /// Handles basic events for editing purposes.
    /// </summary>
    class EditPanel : Panel
    {

        private HtmlEditor editor;

        public EditPanel() : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        public void SetEditor(IHtmlEditor editor)
        {
            this.editor = (HtmlEditor)editor;
        }

        /// <summary>
        /// Checks out when we want to handle the TAB key internally.
        /// </summary>
        private bool ShouldHandleTAB
        {
            get
            {
                Interop.IHTMLElement scopeElement = editor.CurrentScopeElement;
                // LI TAB
                if (editor.HandleTAB && (scopeElement != null && scopeElement.GetTagName() == "LI"))
                {
                    return true;
                }
                if (editor.HandleTAB)
                {
                    return true;
                }
                return false;
            }
        }

        /// <internalonly/>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.Up | Keys.Shift:
                case Keys.Down | Keys.Shift:
                case Keys.Left | Keys.Shift:
                case Keys.Right | Keys.Shift:
                case Keys.Up | Keys.Control:
                case Keys.Down | Keys.Control:
                case Keys.Left | Keys.Control:
                case Keys.Right | Keys.Control:
                    return true;
            }
            // Optionally we process the TAB Key only, if the property is turned on
            if (keyData == Keys.Tab && ShouldHandleTAB)
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <internalonly/>
        protected override bool IsInputKey(Keys keyData)
        {
            if (editor.InternalShortcutKeys)
            {
                switch (keyData)
                {
                    case Keys.A | Keys.Control: //Select All
                    case Keys.C | Keys.Control: //Copy
                    case Keys.P | Keys.Control: //Print
                    case Keys.OemOpenBrackets | Keys.Control: //Outdent
                    case Keys.OemCloseBrackets | Keys.Control: //Indent
                    case Keys.B | Keys.Control: //Bold
                    case Keys.I | Keys.Control: //Italic
                    case Keys.U | Keys.Control: //Underline
                    case Keys.L | Keys.Control: //Left Justify
                    case Keys.J | Keys.Control: //Center Justify
                    case Keys.R | Keys.Control: //Right Justify
                    case Keys.V | Keys.Control: //Paste
                    case Keys.X | Keys.Control: //Cut
                    case Keys.Y | Keys.Control: //Redo
                    case Keys.Z | Keys.Control: //Undo
                        return true;
                }
            }
            // Optionally we process the TAB Key only, if the property is turned on
            if (keyData == Keys.Tab && ShouldHandleTAB)
            {
                return true;
            }
            return base.IsInputKey(keyData);
        }
        /// <internalonly/>
        public override bool PreProcessMessage(ref Message msg)
        {
            Keys k = (Keys)msg.WParam.ToInt32();
            bool handled = false;
            // Look for internal processed Shortcut key on Key down.
            if (msg.Msg == 256)
            {
                handled = DoShortCut(k);
            }
            // After internal processing add modifiers for simplified checks
            k |= ModifierKeys;
            // Special Support for Keys in Design Mode
            if (editor.DesignModeEnabled)
            {
                if (k == Keys.Tab && ShouldHandleTAB && msg.Msg == 256)
                {
                    base.PreProcessMessage(ref msg);
                    Interop.IHTMLElement scopeElement = editor.CurrentScopeElement;
                    switch (scopeElement.GetTagName())
                    {
                        case "LI":
                            editor.TextFormatting.Indent();
                            break;
                    }
                    return false;
                }
            }
            // Special Support for Keys in Browse Mode
            else
            {
                if (!handled) // && editor.HtmlEditorSite!= null)
                {
                    handled = editor.MshtmlSite.PreTranslateMessage(msg);
                }
            }
            if (!handled && msg.Msg == 256)
            {
                switch (k)
                {
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Up | Keys.Shift:
                    case Keys.Down | Keys.Shift:
                    case Keys.Left | Keys.Shift:
                    case Keys.Right | Keys.Shift:
                    case Keys.Up | Keys.Control:
                    case Keys.Down | Keys.Control:
                    case Keys.Left | Keys.Control:
                    case Keys.Right | Keys.Control:
                        break;
                    case Keys.Enter:
                        if (editor.TransposeEnterBehavior)
                        {
                            editor.CreateElementAtCaret("br");
                            handled = true;
                        }
                        break;
                    case Keys.Enter | Keys.Shift:
                        if (editor.TransposeEnterBehavior)
                        {
                            if (editor.BlockDefault == BlockDefaultType.P)
                                editor.Exec(Interop.IDM.PARAGRAPH);
                            else
                                editor.Exec(Interop.IDM.DIV);
                            handled = true;
                        }
                        break;
                    default:
                        bool baseHandler = false;
                        if ((ModifierKeys & Keys.Control) != 0)
                        {
                            baseHandler = true;
                        }
                        if ((ModifierKeys & Keys.Alt) != 0)
                        {
                            baseHandler = true;
                        }
                        if ((((ModifierKeys & Keys.Control) != 0) || ((ModifierKeys & Keys.Alt) != 0) && (ModifierKeys & Keys.Shift) != 0))
                        {
                            baseHandler = true;
                        }
                        if (k == Keys.Tab)
                        {
                            baseHandler = true;
                        }
                        if (((k & ~Keys.Shift) == Keys.Tab) && (k & Keys.Shift) == Keys.Shift)
                        {
                            baseHandler = true;
                        }
                        if ((k >= Keys.F1 && k <= Keys.F24) && !((msg.Msg & 2) == 2))
                        {
                            baseHandler = true;
                        }
                        if (baseHandler)
                        {
                            handled = base.PreProcessMessage(ref msg);
                        }
                        break;
                }
            }
            if (!handled && msg.Msg > 256 && (ModifierKeys & Keys.Alt) != 0)
            {
                handled = base.PreProcessMessage(ref msg);
            }
            return handled;
        }

        /// <summary>
        /// Executes the short cut keys that should be available and handles all of the cases of design mode versus not.
        /// </summary>
        /// <param name="Key">The key to process.</param>
        /// <returns>If the key was handled succesfully, return true, otherwise return false.</returns>
        private bool DoShortCut(Keys Key)
        {
            try
            {
                if ((ModifierKeys & Keys.Control) != 0
                     &&
                     (ModifierKeys & Keys.Shift) == 0
                     &&
                     (ModifierKeys & Keys.Alt) == 0
                     )
                {
                    //fire the BeforeShortcut event and cancel if necessary
                    BeforeShortcutEventArgs e = new BeforeShortcutEventArgs(Key);
                    editor.InvokeBeforeShortcut(e);
                    if (e.Cancel) return true;
                    bool Done = false;
                    // Do no processing if globally turned off
                    if (!editor.InternalShortcutKeys) return false;
                    // process keys internally
                    switch (Key)
                    {
                        case Keys.A: //Select All
                            editor.TextFormatting.SelectAll();
                            Done = true;
                            break;
                        case Keys.C: //Copy
                            editor.Copy();
                            Done = true;
                            break;
                        case Keys.P: //Print
                            editor.PrintImmediately();
                            Done = true;
                            break;
                    }
                    if (editor.DesignModeEnabled)
                    {
                        //Only do the keys that work in design mode.
                        switch (Key)
                        {
                            case Keys.OemOpenBrackets: //Outdent
                                editor.TextFormatting.UnIndent();
                                Done = true;
                                break;
                            case Keys.OemCloseBrackets: //Indent
                                editor.TextFormatting.Indent();
                                Done = true;
                                break;
                            case Keys.B: //Bold
                                editor.TextFormatting.ToggleBold();
                                Done = true; // true;
                                break;
                            case Keys.I: //Italic
                                editor.TextFormatting.ToggleItalics();
                                Done = true;
                                break;
                            case Keys.U: //Underline
                                editor.TextFormatting.ToggleUnderline();
                                Done = true;
                                break;
                            case Keys.L: //Left Justify
                                editor.TextFormatting.SetAlignment(Alignment.Left);
                                Done = true;
                                break;
                            case Keys.J: //Center Justify
                                editor.TextFormatting.SetAlignment(Alignment.Full);
                                Done = true;
                                break;
                            case Keys.R: //Right Justify
                                editor.TextFormatting.SetAlignment(Alignment.Right);
                                Done = true;
                                break;
                            case Keys.V: //Paste
                                editor.Paste();
                                Done = true;
                                break;
                            case Keys.X: //Cut
                                editor.Cut();
                                Done = true;
                                break;
                            case Keys.Y: //Redo
                                editor.Redo();
                                Done = true;
                                break;
                            case Keys.Z: //Undo
                                editor.Undo();
                                Done = true;
                                break;
                        }
                    }
                    return Done;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                editor.OnUpdateUI("key");
            }
        }

        /// <internalonly/>
        override protected void WndProc(ref Message m)
        {
            if (editor != null) // needed for design time support
            {
                switch ((Interop.WM)m.Msg)
                {
                    case Interop.WM.WM_SETFOCUS:
                        break;
                    case Interop.WM.WM_KILLFOCUS:
                        break;
                    case Interop.WM.WM_MOUSEACTIVATE:
                        if (editor.MshtmlSite != null && !this.DesignMode)
                        {
                            IntPtr fromHandle = Win32.GetFocus();
                            editor.DocumentHandle = editor.MshtmlSite.DocumentHandle;
                            if ((!editor.Focused) && (fromHandle != editor.PanelHandle) && (fromHandle != editor.DocumentHandle))
                            {
                                Win32.SendMessage(editor.PanelHandle, (int)Interop.WM.WM_SETFOCUS, fromHandle, IntPtr.Zero);
                            }
                        }
                        break;
                }
            }
            base.WndProc(ref m);
        }

    }
}

