using System;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
    /// <summary>
    /// 
    /// </summary>
    public class RowEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        public RowEventArgs(Row row)
        {
            this.Row = row;
        }

        /// <summary>
        /// 
        /// </summary>
        public Row Row = null;
    }
}