using System;
using System.Collections;
using System.Collections.Generic;

namespace GuruComponents.Netrix.Events
{
	/// <summary>
	/// Enables or disables event on element level types globally.
	/// </summary>
	/// <remarks>
	/// The purpose of this class is the global management of events. Disabling events globally
	/// in situations where the events are not is use can drastically improve the performance,
	/// because the internal event processing is being stopped at a very early stage.
	/// <para>
	/// By default all events are switched on and available.
	/// </para>
	/// </remarks>
	public class EventManager
	{

        private bool allEvents;
		private Dictionary<EventType, State> eventStore;

		/// <summary>
		/// Particular State of an Event.
		/// </summary>
		public class State
		{
			/// <summary>
            /// We pack the state into an object to save our enumerators.
			/// </summary>
			public bool EventState;
            /// <summary>
            /// State
            /// </summary>
            /// <param name="state"></param>
			public State(bool state)
			{
				EventState = state;
			}
		}

		/// <summary>
		/// private Ctor, set all events active.
		/// </summary>
		public EventManager()
		{			
			allEvents = true;
			Type t = typeof(EventType);
            eventStore = new Dictionary<EventType, State>();
			foreach (string evt in Enum.GetNames(t))
			{
				eventStore.Add((EventType)Enum.Parse(t, evt), new State(true));
			}
		}

		/// <summary>
		/// switches the event handling globally on or off.
		/// </summary>
        public bool AllEventsEnabled  
        { 
            get 
            {
                return allEvents;
            }
            set
            {
                allEvents = value;
				foreach (KeyValuePair<EventType, State> de in eventStore)
				{
					de.Value.EventState = value;
				}
            }
        }

		/// <summary>
		/// Enables or disables the specified event.
		/// </summary>
		/// <param name="type">Type of event.</param>
		/// <param name="enabled">State</param>
		public void SetEnabled(EventType type, bool enabled)
		{ 
			eventStore[type].EventState = enabled;
		}

		/// <summary>
		/// Retrieves the state of a specified event.
		/// </summary>
		/// <param name="type">Type of event.</param>
		/// <returns>Current State.</returns>
		public bool GetEnabled(EventType type)
		{
			return (eventStore[type] == null) ? true : ((State) eventStore[type]).EventState;
		}
        /// <summary>
        /// Set element events to specified state.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="enabled"></param>
		public void SetEnabled(EventGroup group, bool enabled)
		{ 
			switch (group)
			{
                # region Global Events from Base Control
                case EventGroup.Control:
                    break;
                case EventGroup.Element:
                    AllEventsEnabled = true;
                    break;
                case EventGroup.ControlMouse:
                    break;
                case EventGroup.ControlKey:
                    break;
                # endregion
					# region Local Events from Element
				case EventGroup.Mouse:
					SetEnabled(EventType.Click, enabled);
					SetEnabled(EventType.DblClick, enabled);
					SetEnabled(EventType.MouseDown, enabled);
					SetEnabled(EventType.MouseEnter, enabled);
					SetEnabled(EventType.MouseLeave, enabled);
					SetEnabled(EventType.MouseOut, enabled);
					SetEnabled(EventType.MouseUp, enabled);
					SetEnabled(EventType.MouseWheel, enabled);
					break;
				case EventGroup.Edit:
					SetEnabled(EventType.BeforeCut, enabled);
					SetEnabled(EventType.Cut, enabled);
					SetEnabled(EventType.BeforeCopy, enabled);
					SetEnabled(EventType.Copy, enabled);
					SetEnabled(EventType.BeforePaste, enabled);
					SetEnabled(EventType.Paste, enabled);
					break;
				case EventGroup.Resize:
					SetEnabled(EventType.Resize, enabled);
					SetEnabled(EventType.ResizeEnd, enabled);
					SetEnabled(EventType.ResizeStart, enabled);
					break;
				case EventGroup.Move:
					SetEnabled(EventType.Move, enabled);
					SetEnabled(EventType.MoveEnd, enabled);
					SetEnabled(EventType.MoveStart, enabled);
					break;
				case EventGroup.Drag:
					SetEnabled(EventType.Drag, enabled);
					SetEnabled(EventType.DragEnd, enabled);
					SetEnabled(EventType.DragEnter, enabled);
					SetEnabled(EventType.DragLeave, enabled);
					SetEnabled(EventType.DragStart, enabled);
					SetEnabled(EventType.DragOver, enabled);
					SetEnabled(EventType.Drop, enabled);
					break;
				case EventGroup.Select:
					SetEnabled(EventType.ControlSelect, enabled);
					SetEnabled(EventType.Select, enabled);
					SetEnabled(EventType.SelectStart, enabled);
					break;
				case EventGroup.Change:
					SetEnabled(EventType.Change, enabled);
					SetEnabled(EventType.SelectionChange, enabled);
					break;
				case EventGroup.Focus:
					SetEnabled(EventType.Focus, enabled);
					SetEnabled(EventType.Focusin, enabled);
					SetEnabled(EventType.Focusout, enabled);
					SetEnabled(EventType.Blur, enabled);					
					break;
				case EventGroup.Data:
					SetEnabled(EventType.DataAvailable, enabled);
					SetEnabled(EventType.DataSetChanged, enabled);
					SetEnabled(EventType.DatasetComplete, enabled);
					break;
				case EventGroup.All:
					AllEventsEnabled = true;
					break;
					# endregion
			}

		}
        
	}

	/// <summary>
	/// Switch events on or off for the specified group.
	/// </summary>
    [Flags]
    public enum EventGroup
    {
		/// <summary>
		/// All control event, fired by base control. Not implemented.
		/// </summary>
        Control         = 0x0001,
		/// <summary>
		/// All element events, fired on element level.
		/// </summary>
        Element         = 0x0002,
		/// <summary>
		/// Not implemented.
		/// </summary>
        ControlMouse    = 0x0004,
        /// <summary>
        /// All mouse events.
        /// </summary>
		Mouse			= 0x0008,
		/// <summary>
		/// Not implemented.
		/// </summary>
		ControlKey      = 0x0010,
        /// <summary>
        /// All Key events.
        /// </summary>
		Key				= 0x0020,
        /// <summary>
        /// Move events, such as Move, MoveStart, MoveEnd.
        /// </summary>
		Move            = 0x0040,
        /// <summary>
        /// All drag events, such as DragDrop, DragStart etc.
        /// </summary>
		Drag            = 0x0080,
        /// <summary>
        /// All select events.
        /// </summary>
		Select          = 0x0100,
        /// <summary>
        /// Events like Change, SelectionChange.
        /// </summary>
		Change          = 0x0200,
        /// <summary>
        /// Focus events, such as FocusIn, FocusOut, Blur.
        /// </summary>
		Focus           = 0x0400,  // incl. Blur
        /// <summary>
        /// Data binding related events, not currently used.
        /// </summary>
        Data            = 0x0800,
		/// <summary>
		/// Resize events, such as Resize, ResizeStart, ResizeEnd.
		/// </summary>
		Resize          = 0x1000,
		/// <summary>
		/// Edit events, such as Copy, BeforeCopy, Paste, BeforePaste, Cut, BeforeCut.
		/// </summary>
		Edit			= 0x2000,
        /// <summary>
        /// All events
        /// </summary>
        All             = 0xFFFF
    }

	/// <summary>
	/// Available events the event manager can turn on or off.
	/// </summary>
    public enum EventType
    {
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        None				= 0,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Help,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Click,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        DblClick,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        KeyPress,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        KeyDown,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        KeyUp,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        MouseOut,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        MouseOver,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        MouseMove,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        MouseDown,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        MouseUp,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        SelectStart,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        FilterChange,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        DragStart,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        BeforeUpdate,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        AfterUpdate,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        ErrorUpdate,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Rowexit,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Rowenter,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        DataSetChanged,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        DataAvailable,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        DatasetComplete,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        LoseCapture,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        PropertyChange,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Scroll,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Focus,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Blur,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Resize,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Drag,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        DragEnd,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        DragEnter,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        DragOver,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        DragLeave,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Drop,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        BeforeCut,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Cut,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        BeforeCopy,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Copy,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        BeforePaste,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Paste,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        ContextMenu,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        RowsDelete,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Rowsinserted,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Cellchange,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        ReadyStateChange,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        BeforeEditFocus,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        LayoutComplete,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Page,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        BeforeDeactivate,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        BeforeActivate,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Move,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        ControlSelect,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        MoveStart,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        MoveEnd,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        ResizeStart,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        ResizeEnd,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        MouseEnter,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        MouseLeave,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        MouseWheel,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Activate,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Deactivate,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Focusin,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Focusout,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Load,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Error,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Change,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Abort,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Select,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        SelectionChange,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Stop,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Reset,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        Submit,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        BeforeUnLoad,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        BeforeLoad,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        BeforePrint,
        /// <summary>
        /// Use this to set the event manager to a specific state for this event.
        /// </summary>
        AfterPrint,
    }

}
