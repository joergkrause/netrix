using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.Designer
{
	/// <summary>
	/// This class controls the access to events if shown in the PropertyGrid.
	/// </summary>
    /// <remarks>
    /// For simplified access use <see cref="GuruComponents.Netrix.Events.EventBinding">EventBinding</see> instead.
    /// This class is available through the <see cref="GuruComponents.Netrix.HtmlEditor.EventBinding">EventBinding</see>
    /// property, implemented by the base class.
    /// <para>
    /// 
    /// </para>
    /// </remarks>
	public class EventBindingService : IEventBindingService
	{

        private HtmlEditor editor;

        /// <summary>
        /// Creates a new instance of the EventBindingService.
        /// </summary>
        /// <remarks>
        /// This ctor supports the designer's infrastructure and should called externally.
        /// </remarks>
        /// <param name="editor">The Editor instance this class is related to.</param>
		public EventBindingService(IHtmlEditor editor)
		{
			this.editor = (HtmlEditor) editor;
        }

        private EventDisplay eventDisplay = EventDisplay.EventVisible;
        
        /// <summary>
        /// Controls which events has to be displayed.
        /// </summary>
        public EventDisplay EventDisplay
        {
            get { return eventDisplay; }
            set { EventDisplay = value; }
        }

        #region IEventBindingService Member

        public PropertyDescriptor GetEventProperty(EventDescriptor e)
        {
            return new EventPropertyDescriptor(e, this);
        }

        public ICollection GetCompatibleMethods(EventDescriptor e)
        {
            if (GetCompatibleMethodsRequest != null)
            {                
                GetCompatibleMethodsRequestEventArgs args = new GetCompatibleMethodsRequestEventArgs(e, null);
                GetCompatibleMethodsRequest(this, args);
                return args.CompatibleMethods;
            }
            return null;
        }

        public PropertyDescriptorCollection GetEventProperties(EventDescriptorCollection events)
        {
            ArrayList pArray = new ArrayList();
            // filter out internal events
            for (int i = 0; i < events.Count; i++)
            {
                switch (this.EventDisplay)
                {
                    case EventDisplay.All:
                        pArray.Add(GetEventProperty(events[i]));
                        break;
                    case EventDisplay.Both:
                        if (events[i].Attributes[typeof(EventVisibleAttribute)] != null
                            ||
                            events[i].Attributes[typeof(ScriptingVisibleAttribute)] != null)
                        {
                            pArray.Add(GetEventProperty(events[i]));
                        }
                        break;
                    case EventDisplay.EventVisible:
                        if (events[i].Attributes[typeof(EventVisibleAttribute)] != null)
                        {
                            pArray.Add(GetEventProperty(events[i]));
                        }
                        break;
                    case EventDisplay.Scripting:
                        if (events[i].Attributes[typeof(ScriptingVisibleAttribute)] != null)
                        {
                            pArray.Add(GetEventProperty(events[i]));
                        }
                        break;
                }
            }
            PropertyDescriptor[] props = new PropertyDescriptor[pArray.Count];
            pArray.CopyTo(props);
            return new PropertyDescriptorCollection(props);
        }

        public bool ShowCode(IComponent component, EventDescriptor e)
        {
            if (ShowCodeRequest != null)
            {
                return ShowCodeRequest(editor, new EventBindingEventArgs(e, component));
            }
            return false;
        }

        bool IEventBindingService.ShowCode(int lineNumber)
        {
            if (ShowCodeRequest != null)
            {                
                return ShowCodeRequest(editor, new EventBindingEventArgs(null, null));
            }
            return false;
        }

        bool IEventBindingService.ShowCode()
        {
            if (ShowCodeRequest != null)
            {                
                return ShowCodeRequest(editor, new EventBindingEventArgs(null, null));
            }
            return false;
        }

        public EventDescriptor GetEvent(PropertyDescriptor property)
        {
            EventPropertyDescriptor ep = property as EventPropertyDescriptor;
            if (ep == null)
            {
                return null;
            }
            else
            {
                ep.ResetValueRequest += new EventHandler(ep_ResetValueRequest);
                ep.SetValueRequest += new EventHandler(ep_SetValueRequest);
                ep.GetValueRequest += new EventHandler(ep_GetValueRequest);
            }
            return ep.Event;
        }

        public string CreateUniqueMethodName(IComponent component, EventDescriptor e)
        {
            if (CreateUniqueNameRequest != null)
            {
                CreateUniqueNameRequestEventArgs args = new CreateUniqueNameRequestEventArgs(e, component);
                CreateUniqueNameRequest(this.editor, args);
                return args.UniqueName;
            }    
            return null;
        }

        #endregion

        # region Events

        /// <summary>
        /// Fired when the host UI requests showing the code of the event handler.
        /// </summary>
        /// <remarks>
        /// In the PropertyGrid then a double click into the event handlers name text field or hitting the enter key
        /// invokes this event.
        /// </remarks>
        public event ShowCodeRequestHandler ShowCodeRequest;
        /// <summary>
        /// Fired when the host requests a unique name for the event handler of the current event.
        /// </summary>
        /// <remarks>
        /// In the PropertyGrid then a double click into the event handlers name text field or hitting the enter key
        /// invokes this event. In case the field was empty this event could be used to get the right handler name.
        /// </remarks>
        public event CreateUniqueNameRequestHandler CreateUniqueNameRequest;
        /// <summary>
        /// Fired when the host needs a list of comaptible methods.
        /// </summary>
        /// <remarks>
        /// This is event is fired if the PropertyGrid is used and the user opens the drop down list for the list
        /// of available event handlers.
        /// </remarks>
        public event GetCompatibleMethodsRequestHandler GetCompatibleMethodsRequest;
        /// <summary>
        /// Fired if the host needs to set the value of a specific event handler, e.g. the currently specified name.
        /// </summary>
        public event EventValueHandler EventSetValueRequest;
        /// <summary>
        /// Fired if the host needs to get the value of a specific event handler, e.g. the currently specified name.
        /// </summary>
        public event EventValueHandler EventGetValueRequest;
        /// <summary>
        /// Fired if the host needs the remove value of a specific event handler.
        /// </summary>
        public event EventValueHandler EventResetValueRequest;

        # endregion

        private TypeConverter eventTypeConverter;

        public TypeConverter EventTypeConverter
        {
            get
            {
                if (eventTypeConverter == null)
                {
                    eventTypeConverter = new EventTypeConverter();
                }
                return eventTypeConverter;
            }
        }

        private void ep_ResetValueRequest(object sender, EventArgs e)
        {
            if (EventResetValueRequest != null)
            {
                IComponent comp = null;
                EventResetValueRequest(this.editor, new EventBindingEventArgs(sender as EventDescriptor, comp));
            }
        }

        private void ep_SetValueRequest(object sender, EventArgs e)
        {
            if (EventSetValueRequest != null)
            {
                IComponent comp = null;
                EventSetValueRequest(this.editor, new EventBindingEventArgs(sender as EventDescriptor, comp));
            }
        }

        private void ep_GetValueRequest(object sender, EventArgs e)
        {
            if (EventGetValueRequest != null)
            {
                IComponent comp = null;
                EventGetValueRequest(this.editor, new EventBindingEventArgs(sender as EventDescriptor, comp));
            }            
        }
    }
}
