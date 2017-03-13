using System;
using GuruComponents.Netrix.WebEditing;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// Declares the event args for the <see cref="GuruComponents.Netrix.IHtmlEditor.ReadyStateChanged"/> event.
    /// </summary>
    /// <remarks>
    /// Use this event and the state "Interactive" to attach handlers of subclasses, like HtmlWindow.
    /// </remarks>
    public class ReadyStateChangedEventArgs : EventArgs 
    {
        private string m_strReadyState;
        private ReadyState m_State;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="p_strReadyState"></param>
        public ReadyStateChangedEventArgs(string p_strReadyState)
        {
            m_strReadyState = p_strReadyState;
            m_State = (GuruComponents.Netrix.WebEditing.ReadyState)Enum.Parse(typeof(GuruComponents.Netrix.WebEditing.ReadyState), p_strReadyState, true); 
        }

        /// <summary>
        /// Gets the raw state of the ready state changed event.
        /// </summary>
        /// <seealso cref="State"/>
        [Obsolete("Use State property instead")]
        public string ReadyState 
        {
            get 
            {
                return m_strReadyState;
            }
        }

        /// <summary>
        /// Definite state of loading procedure.
        /// </summary>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.ReadyStateChanged"/>
        public ReadyState State
        {
            get
            {
                return m_State;
            }
        }

    }
}
