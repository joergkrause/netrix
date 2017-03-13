using System;
using System.Collections;
using System.ComponentModel;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// Used to inform the host that the PropertyGrid wants to show code.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate bool ShowCodeRequestHandler (object sender, EventBindingEventArgs e);

    /// <summary>
    /// Used to inform that the PropertyGrid is up to set, get or reset an entry.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EventValueHandler (object sender, EventBindingEventArgs e);

    /// <summary>
    /// Used if the PropertyGrid requests a unique event handler name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CreateUniqueNameRequestHandler (object sender, CreateUniqueNameRequestEventArgs e);

    /// <summary>
    /// Used if the PropertyGrid shows a dropdown list with all compatible methods.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void GetCompatibleMethodsRequestHandler (object sender, GetCompatibleMethodsRequestEventArgs e);

    /// <summary>
    /// Contains information about events exposed by several binding methods.
    /// </summary>
    /// <remarks>
    /// Depending on the current event this arguments are used with some or all of the properties may return <c>null</c> (<c>Nothing</c> in 
    /// Visual Basic).
    /// </remarks>
    public class EventBindingEventArgs : EventArgs
    {
        private EventDescriptor desc;
        private IComponent component;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="component"></param>
        public EventBindingEventArgs(EventDescriptor desc, IComponent component)
        { 
            this.desc = desc;
            this.component = component;
        }
        /// <summary>
        /// Descriptor
        /// </summary>
        public EventDescriptor Descriptor
        {
            get { return desc; }
        }
        /// <summary>
        /// Component, usually of type IElement or Control.
        /// </summary>
        public IComponent Component
        {
            get { return component; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetCompatibleMethodsRequestEventArgs : EventBindingEventArgs
    {
        private ICollection compMethods;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="component"></param>
        public GetCompatibleMethodsRequestEventArgs(EventDescriptor desc, IComponent component)
            :
            base(desc, component)
        {
        }

        /// <summary>
        /// The host should set an array of string objects to show a list of compatible handler methods.
        /// </summary>
        public ICollection CompatibleMethods
        {
            get { return compMethods; }
            set { compMethods = value; }
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateUniqueNameRequestEventArgs : EventBindingEventArgs
    {
        string name;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="component"></param>
        public CreateUniqueNameRequestEventArgs(EventDescriptor desc, IComponent component)
            :
            base(desc, component)
        {
        }

        /// <summary>
        /// The new unique name the code editor has created as name for the requested event handler.
        /// </summary>
        public string UniqueName
        {
            get { return name; }
            set { name = value; }
        }


    }

}
