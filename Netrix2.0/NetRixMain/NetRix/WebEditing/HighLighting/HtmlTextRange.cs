using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;
using System.Drawing;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Web.UI;

namespace GuruComponents.Netrix.NetRix.WebEditing.HighLighting
{
    public class HtmlTextRange
    {

        private Interop.IHTMLTxtRange tr;
        private IHtmlEditor editor;

        internal HtmlTextRange(Interop.IHTMLTxtRange tr, IHtmlEditor editor)
        {
            this.editor = editor;
            this.tr = tr;
        }

        internal Interop.IHTMLTxtRange Native
        {
            get { return tr; }
        }

        #region IHTMLTxtRange Members

        public string Html
        {
            get
            {
                return tr.GetHtmlText();
            }
            set
            {
                tr.PasteHTML(value);
            }
        }

        public string Text
        {
            set
            {
                tr.SetText(value);
            }
            get
            {
                return tr.GetText();
            }
        }

        public Control ParentElement
        {
            get { return editor.GenericElementFactory.CreateElement(tr.ParentElement()); }
        }

        public HtmlTextRange Duplicate()
        {
            return new HtmlTextRange(tr.Duplicate(), editor);
        }

        public bool InRange(HtmlTextRange range)
        {
            return tr.InRange(range.Native);
        }

        public bool IsEqual(HtmlTextRange range)
        {
            return tr.IsEqual(range.Native);
        }

        public void ScrollIntoView(bool fStart)
        {
            tr.ScrollIntoView(fStart);
        }

        public void Collapse(bool Start)
        {
            tr.Collapse(Start);
        }

        public bool Expand(string Unit)
        {
            return tr.Expand(Unit);
        }

        public int Move(string Unit, int Count)
        {
            return tr.Move(Unit, Count);
        }

        public int MoveStart(string Unit, int Count)
        {
            return tr.MoveStart(Unit, Count);
        }

        public int MoveEnd(string Unit, int Count)
        {
            return tr.MoveEnd(Unit, Count);
        }

        public void Select()
        {
            tr.Select();
        }

        public void MoveToElementText(IElement element)
        {
            Interop.IHTMLElement pElement = element.GetBaseElement();
            tr.MoveToElementText(pElement);
        }

        public void SetEndPoint(string how, HtmlTextRange SourceRange)
        {
            tr.SetEndPoint(how, SourceRange.Native);
        }

        public int CompareEndPoints(string how, HtmlTextRange SourceRange)
        {
            return tr.CompareEndPoints(how, SourceRange.Native);
        }

        public bool FindText(string String, int Count, int Flags)
        {
            return tr.FindText(String, Count, Flags);
        }

        public void MoveToPoint(Point pt)
        {
            MoveToPoint(pt.X, pt.Y);
        }

        public void MoveToPoint(int x, int y)
        {
            tr.MoveToPoint(x, y);
        }

        public string GetBookmark()
        {
            return tr.GetBookmark();
        }

        public bool MoveToBookmark(string Bookmark)
        {
            return tr.MoveToBookmark(Bookmark);
        }

        public bool QueryCommandSupported(string cmdID)
        {
            return tr.QueryCommandSupported(cmdID);
        }

        public bool QueryCommandEnabled(string cmdID)
        {
            return tr.QueryCommandEnabled(cmdID);
        }

        public bool QueryCommandState(string cmdID)
        {
            return tr.QueryCommandState(cmdID);
        }

        public bool QueryCommandIndeterm(string cmdID)
        {
            return tr.QueryCommandIndeterm(cmdID);
        }

        public string QueryCommandText(string cmdID)
        {
            return tr.QueryCommandText(cmdID); ;
        }

        public object QueryCommandValue(string cmdID)
        {
            return tr.QueryCommandValue(cmdID); ;
        }

        public bool ExecCommand(string cmdID, object value)
        {
            return tr.ExecCommand(cmdID, false, value);
        }

        public bool ExecCommand(string cmdID, bool showUI, object value)
        {
            return tr.ExecCommand(cmdID, showUI, value);
        }

        #endregion
    }
}
