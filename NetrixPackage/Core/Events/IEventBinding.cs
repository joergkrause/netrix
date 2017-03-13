using GuruComponents.Netrix.Designer;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// Access to event binding events.
    /// </summary>
    /// <remarks>
    /// The event's function mentioned in the description is not implemented in the component, rather the implementer
    /// is supposed to do this task according to the requested features. For instance, the <see cref="ShowCodeRequest"/>
    /// event is being fired when the user double clicks the event's field in the PropertyGrid. The expected task is
    /// to open a text editor, and either create or search and show the appropriate event handler. However, implementing
    /// the creation or search code is up to the application developer. 
    /// </remarks>
    public interface IEventBinding
    {
        /// <summary>
        /// Creates a unique name for an event-handler method for the specified component and event.
        /// </summary>
        event CreatedEventHandler CreateUniqueNameRequest;
        /// <summary>
        /// Event binding service requests a value.
        /// </summary>
        event EventValueHandler EventGetValueRequest;
        /// <summary>
        /// Event binding service resets a value.
        /// </summary>
        event EventValueHandler EventResetValueRequest;
        /// <summary>
        /// Event binding service sets a value.
        /// </summary>
        event EventValueHandler EventSetValueRequest;
        /// <summary>
        /// Gets a collection of event-handler methods that have a method signature compatible with the specified event.
        /// </summary>
        event GetCompatibleMethodsRequestHandler GetCompatibleMethodsRequest;
        /// <summary>
        /// Displays the user code for the designer.
        /// </summary>
        event ShowCodeRequestHandler ShowCodeRequest;

        /// <summary>
        /// TODO: Add comments
        /// </summary>
        EventDisplay EventDisplay
        {
            get;
            set;
        }
    }
}