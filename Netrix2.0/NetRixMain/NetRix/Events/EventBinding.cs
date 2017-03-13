using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel.Design;
using GuruComponents.Netrix.Designer;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// This class provides simplified access to the event binding management.
    /// </summary>
    /// <remarks>
    /// If one is using the PropertyGrid to display properties and events, especially to create some
    /// kind of event driven editor, this class allows a finer grained control of the EventBinding behavior.
    /// 
    /// <seealso cref="EventManager"/>
    /// <seealso cref="GuruComponents.Netrix.Designer.EventBindingService">EventBindingService</seealso>
    /// </remarks>
    public sealed class EventBinding : GuruComponents.Netrix.Events.IEventBinding
    {
        private static Hashtable instance;
        private GuruComponents.Netrix.Designer.EventBindingService bindings;
        private IHtmlEditor editor;

        private EventBinding(IHtmlEditor editor)
        { 
            this.editor = editor;
            this.bindings = editor.ServiceProvider.GetService(typeof(IEventBindingService)) as GuruComponents.Netrix.Designer.EventBindingService;
            this.bindings.ShowCodeRequest += new ShowCodeRequestHandler(bindings_ShowCodeRequest);
            this.bindings.GetCompatibleMethodsRequest += new GetCompatibleMethodsRequestHandler(bindings_GetCompatibleMethodsRequest);
            this.bindings.EventSetValueRequest += new EventValueHandler(bindings_EventSetValueRequest);
            this.bindings.EventResetValueRequest += new EventValueHandler(bindings_EventResetValueRequest);
            this.bindings.EventGetValueRequest += new EventValueHandler(bindings_EventGetValueRequest);
            this.bindings.CreateUniqueNameRequest += new CreateUniqueNameRequestHandler(bindings_CreateUniqueNameRequest);
        }

        /// <summary>
        /// Controls which events has to be displayed.
        /// </summary>
        /// <remarks>
        /// This property refers to the list of events available in the PropertyGrid once the events tab is shown.
        /// The list is build once the component is assigned to the PropertyGrid. The current value of this property
        /// controls the list, but subsequent changes of the property will not refresh the list.
        /// </remarks>
        public EventDisplay EventDisplay
        {
            get { return bindings.EventDisplay; }
            set { bindings.EventDisplay = value; }
        }

        void bindings_CreateUniqueNameRequest(object sender, CreateUniqueNameRequestEventArgs e)
        {
            InvokeCreateUniqueNameRequest(e);
        }

        void bindings_EventGetValueRequest(object sender, EventBindingEventArgs e)
        {
            InvokeEventGetValueRequest(e);
        }

        void bindings_EventResetValueRequest(object sender, EventBindingEventArgs e)
        {
            InvokeEventResetValueRequest(e);
        }

        void bindings_EventSetValueRequest(object sender, EventBindingEventArgs e)
        {
            InvokeEventSetValueRequest(e);
        }

        void bindings_GetCompatibleMethodsRequest(object sender, GetCompatibleMethodsRequestEventArgs e)
        {
            InvokeGetCompatibleMethodsRequest(e);
        }

        bool bindings_ShowCodeRequest(object sender, EventBindingEventArgs e)
        {
            return InvokeShowCodeRequest(e);
        }

        /// <summary>
        /// The user has double clicked the event name field in PropertyGrid and the editor is supposed to
        /// open the editor view and navigate to the handler code.
        /// </summary>
        public event ShowCodeRequestHandler ShowCodeRequest;
        /// <summary>
        /// The user has double clicked the event name and before showing the code a unique name for the
        /// new event handler is required. If a name exists, this event did not fire again.
        /// </summary>
        public event CreatedEventHandler CreateUniqueNameRequest;
        /// <summary>
        /// The user has clicked on the dropdown icon in PropertyGrid and expects to see a list of
        /// compatible methods (e.g. methods with the same signature).
        /// </summary>
        public event GetCompatibleMethodsRequestHandler GetCompatibleMethodsRequest;
        /// <summary>
        /// The PropertyGrid is up to set a new value, because the user has entered a new string. The editor
        /// is supposed to change the corresponding code.
        /// </summary>
        public event EventValueHandler EventSetValueRequest;
        /// <summary>
        /// The PropertyGrid whishes to show the current values and the host is supposed to show the
        /// corresponding value.
        /// </summary>
        public event EventValueHandler EventGetValueRequest;
        /// <summary>
        /// The value is getting reset and the handler informs the host to reset corresponding code accordingly.
        /// </summary>
        public event EventValueHandler EventResetValueRequest;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        internal static IEventBinding GetInstance(IHtmlEditor editor)
        {
            if (instance == null)
            {
                instance = new Hashtable();
            }
            if (instance[editor] == null)
            {
                instance[editor] = new EventBinding(editor);
            }
            return instance[editor] as EventBinding;
        }

        private bool InvokeShowCodeRequest(EventBindingEventArgs e)
        {
            if (ShowCodeRequest != null)
            {
                return ShowCodeRequest(editor, e);
            }
            return false;
        }
        private void InvokeCreateUniqueNameRequest(CreateUniqueNameRequestEventArgs e)
        { 
            if (CreateUniqueNameRequest != null)
            {
                CreateUniqueNameRequest(editor, e);
            }
        }
        private void InvokeGetCompatibleMethodsRequest(GetCompatibleMethodsRequestEventArgs e)
        { 
            if (GetCompatibleMethodsRequest != null)
            {
                GetCompatibleMethodsRequest(editor, e);
            }
        }
        private void InvokeEventSetValueRequest(EventBindingEventArgs e)
        { 
            if (EventSetValueRequest != null)
            {
                EventSetValueRequest(editor, e);
            }
        }
        private void InvokeEventGetValueRequest(EventBindingEventArgs e)
        { 
            if (EventGetValueRequest != null)
            {
                EventGetValueRequest(editor, e);
            }
        }
        private void InvokeEventResetValueRequest(EventBindingEventArgs e)
        { 
            if (EventResetValueRequest != null)
            {
                EventResetValueRequest(editor, e);
            }
        }


    }
}
