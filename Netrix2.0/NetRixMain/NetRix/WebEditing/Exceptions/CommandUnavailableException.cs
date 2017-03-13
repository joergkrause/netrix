using System;

namespace GuruComponents.Netrix.WebEditing.Exceptions
{
    /// <summary>
    /// Exception thrown if a command was not successfull operate within the 
    /// control. If the reason was a wrong or missing argument or parameter
    /// a <see cref="System.ArgumentException">ArgumentException</see> is thrown instead.
    /// </summary>
    public class CommandUnavailableException : Exception
    {
        private int _command;

        public CommandUnavailableException(int command) : base()
        {
            _command = command;
        }
        public CommandUnavailableException(int command, string message) : base(message)
        {
            _command = command;
        }
        public CommandUnavailableException(int command, string message, Exception inner) : base(message, inner)
        {
            _command = command;
        }

        /// <summary>
        /// The number of the command sent causes the exception.
        /// </summary>
        public int Command
        {
            get
            {
                return _command;
            }
        }

    }
}
