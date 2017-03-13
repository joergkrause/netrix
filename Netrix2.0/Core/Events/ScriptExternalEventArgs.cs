using System;
using System.Windows.Forms;
using System.Drawing;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This class defines data for the ShowError event in HtmlWindow class.
    /// </summary>
    /// <remarks>
    /// This class allows to cancel the navigation by setting the cancel property to <c>true</c>.
    /// </remarks>
    public class ScriptExternalEventArgs : EventArgs
    {

        /// <summary>
        /// Return codes for external Script method call.
        /// </summary>
        public enum ExternalErrorCode
        {
            /// <summary>
            /// Show Abort error message.
            /// </summary>
            E_ABORT = Interop.E_ABORT,
            /// <summary>
            /// Show Security error message.
            /// </summary>
            E_ACCESSDENIED = Interop.E_ACCESSDENIED,
            /// <summary>
            /// Show Failure error message.
            /// </summary>
            E_FAIL = Interop.E_FAIL,
            /// <summary>
            /// Show Handle not found error message.
            /// </summary>
            E_HANDLE = Interop.E_HANDLE,
            /// <summary>
            /// Show Argument error message.
            /// </summary>
            E_INVALIDARG = Interop.E_INVALIDARG,
            /// <summary>
            /// Show Pointer error message.
            /// </summary>
            E_POINTER = Interop.E_POINTER,
            /// <summary>
            /// Show error message.
            /// </summary>
            E_NOTIMPL = Interop.E_NOTIMPL,
            /// <summary>
            /// Show error message.
            /// </summary>
            E_NOINTERFACE = Interop.E_NOINTERFACE,
            /// <summary>
            /// Show error message.
            /// </summary>
            E_OUTOFMEMORY = Interop.E_OUTOFMEMORY,
            /// <summary>
            /// Show error message.
            /// </summary>
            E_UNEXPECTED = Interop.E_UNEXPECTED,
            /// <summary>
            /// Show error message.
            /// </summary>
            E_PENDING = Interop.E_PENDING,
            /// <summary>
            /// Show no error message.
            /// </summary>
            E_DEFAULTACTION = Interop.E_DEFAULTACTION,
            /// <summary>
            /// Show generic error message.
            /// </summary>
            S_FALSE = Interop.S_FALSE,
            /// <summary>
            /// Show no message at all.
            /// </summary>
            S_OK = Interop.S_OK
        }


        /// <summary>
        /// Ctor for event arguments.
        /// </summary>
        public ScriptExternalEventArgs()
        {
        }

        private ExternalErrorCode _externalerror;

        /// <summary>
        /// Controls how the script engine is handling the call to an external method. This might force an Script error.
        /// </summary>
        /// <remarks>
        /// E_NOTIMPL = fires native error window
        /// E_DEFAULTACTION = security exception
        /// E_FAIL = unspecified error
        /// E_ABORT = suppress an native window
        /// E_HANDLE = provide valid handle to invoke code
        /// E_UNEXPECTED = unexpected error
        /// E_POINTER = pointer expected
        /// E_NOINTERFACE = null or not object
        /// E_ACCESSDENIED = security error
        /// E_OUTOFMEMORY = out of mem error
        /// </remarks>
        public ExternalErrorCode ExternalError
        {
            get { return _externalerror; }
            set { _externalerror =value; }
        }



    }
        
}