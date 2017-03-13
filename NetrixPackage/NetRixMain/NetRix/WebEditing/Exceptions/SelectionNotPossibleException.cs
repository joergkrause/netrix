using System;

namespace GuruComponents.Netrix.WebEditing.Exceptions
{
    /// <summary>
    /// Exception thrown if a selection was not possbile. This happens when the host application
    /// tries to select the body or a table selection.
    /// a <see cref="System.ArgumentException">ArgumentException</see> is thrown instead.
    /// </summary>
    public class SelectionNotPossibleException : Exception
    {
        private string _tagName;

        public SelectionNotPossibleException(string tagName) : base()
        {
            _tagName = tagName;
        }
        public SelectionNotPossibleException(string tagName, string message) : base(message)
        {
            _tagName = tagName;
        }
        public SelectionNotPossibleException(string tagName, string message, Exception inner) : base(message, inner)
        {
            _tagName = tagName;
        }

        /// <summary>
        /// Name of Tag which was not selectable.
        /// </summary>
        public string TagName
        {
            get
            {
                return _tagName;
            }
        }

    }
}
