using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using GuruComponents.Netrix.WebEditing.Elements;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using GuruComponents.Netrix.Events;
using System.Web.UI.Design;

namespace GuruComponents.Netrix.Designer
{

    /// <summary>
    /// Default designer support class. 
    /// </summary>
    /// <remarks>
    /// Primarily, this class supports the elements infrastructur behavior. User can replace this class
    /// by adding the <see cref="DesignerAttribute"/> to element derived from <see cref="Element"/> as well other inheritable classes,
    /// which derive from <see cref="Element"/>. 
    /// <seealso cref="IElement"/>
    /// <seealso cref="Element"/>
    /// </remarks>
    public class ElementDesigner : ComponentDesigner
    {
        IElement component;

        /// <summary>
        /// The element which is currently assigned to this designer.
        /// </summary>
        public IElement Element
        {
            get { return component; }
            set { component = value; }
        }

        /// <summary>
        /// Just override to avoid null ref exception in case of UI calls (like PropertyGrid).
        /// </summary>
        [Obsolete()]
        public override void OnSetComponentDefaults()
        {
            // just override to avoid null ref exception in case of UI calls (like PropertyGrid)
        }

        /// <summary>
        /// Called by internal designer host to initialize the designer.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the default event attribute is being used, but the exposed event did not exists.</exception>
        /// <param name="component"></param>
        public override void Initialize(System.ComponentModel.IComponent component)
        {
            this.component = (IElement)component;
            object[] attr = this.component.GetType().GetCustomAttributes(typeof(DefaultEventAttribute), true);
            if (attr != null && attr.Length >= 1)
            {
                DefaultEventAttribute dea = (DefaultEventAttribute)attr[0];
                EventInfo ei = this.component.GetType().GetEvent(dea.Name);
                if (ei != null)
                {
                    ei.AddEventHandler(component, new GuruComponents.Netrix.Events.DocumentEventHandler(component_Action));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("The default event defined by the element's class does not exist in that element. Event: " + dea.Name);
                }
            }
            //this.component.DblClick -= new GuruComponents.Netrix.Events.DocumentEventHandler(component_DblClick);
            //this.component.DblClick += new GuruComponents.Netrix.Events.DocumentEventHandler(component_DblClick);
        }

        void component_Action(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            DoDefaultAction();
        }

        /// <summary>
        /// The default action for the component.
        /// </summary>
        /// <remarks>
        /// The default event on an element causes this method to be called. Override this method to change the default
        /// behavior. In an application which behaves like Visual Studio a double click might open the code editor
        /// and add a event handler for the default event of the element. 
        /// <para>
        /// The abstract base class, <see cref="GuruComponents.Netrix.WebEditing.Elements.Element">Element</see> defines the 
        /// attribute <see cref="DefaultEventAttribute"/> with the value "Click". This makes the click event the default
        /// event for any element. If one writes a new element class, which derives from <see cref="GuruComponents.Netrix.WebEditing.Elements.Element">Element</see>
        /// or which implements <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement">IElement</see>, the attribute could
        /// be overwritten with each other handler name.
        /// </para>
        /// <para>
        /// To fully implement this behavior one must:
        /// <list type="bullet">
        ///     <item>1. Add the DefaultEventAttribute to the component's class.</item>
        ///     <item>2. Override the DoDefaultAction method to receive the event at design time.</item>
        /// </list>
        /// At design time, the user may issue the event by some action. Then the event manager receives the event and calles
        /// the attached handler, which in turn calles DoDefaultAction. 
        /// </para>
        /// <para>
        /// <b>Important Note to implementors:</b> Some events, like MouseMove, PropertyChange, or Resize are called very 
        /// frequently and using these event in the scenario described above could slow the performance drastically. It's strongly
        /// recommended to use only events which have slow performance impact (so called single event actions). This is the case
        /// for events like Click, ResizeStart, ResizeEnd, MoveStart, MoveEnd, DblClick, MouseUp, MouseDown. It could be critical
        /// for key events and several mixed events, like those for drag 'n drop, for editing (cut/copy/paste) and selection.
        /// </para>
        /// </remarks>
        public override void DoDefaultAction()
        {
            base.DoDefaultAction();
        }

        /// <summary>
        /// Allows adding properties to the properties which show up in the PropertyGrid.
        /// </summary>
        /// <remarks>
        /// Allows a designer to add to the set of properties that it exposes through
        ///  a <see cref="System.ComponentModel.TypeDescriptor"/>.
        /// </remarks>
        /// <param name="properties">List of properties defined by the component.</param>
        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            Hashtable ht = new Hashtable(properties);
            foreach (DictionaryEntry de in properties)
            {
                PropertyDescriptor pd = (PropertyDescriptor)de.Value;
                if (pd.Name == "EnableViewState")
                {
                    ht.Remove(pd);
                }
            }
            base.PreFilterProperties(properties);
        }

        /// <summary>
        /// Allows change or modify properties to the properties which show up in the PropertyGrid.
        /// </summary>
        /// <remarks>
        /// Allows a designer to change or modify to the set of properties that it exposes through
        ///  a <see cref="System.ComponentModel.TypeDescriptor"/>.
        /// </remarks>
        /// <param name="properties">List of properties defined by the component.</param>
        protected override void PostFilterProperties(IDictionary properties)
        {
            Hashtable ht = new Hashtable(properties);
            foreach (DictionaryEntry de in properties)
            {
                PropertyDescriptor pd = (PropertyDescriptor)de.Value;
                if (pd.Name == "Expression"
                    ||
                    pd.Name == "EnableViewState"
                    ||
                    pd.Name == "ID")
                {
                    ht.Remove(pd);
                }
            }
            base.PostFilterProperties(ht);
        }

        /// <summary>
        /// Gets the parent element in the element's hierarchy.
        /// </summary>
        protected override IComponent ParentComponent
        {
            get
            {
                return component.GetParent() as IComponent;
            }
        }

        /// <summary>
        /// Gets the children of the element in the element's hierarchy.
        /// </summary>
        public override ICollection AssociatedComponents
        {
            get
            {
                return component.ElementDom.GetChildNodes();
            }
        }

        /// <summary>
        /// This property provides commands.
        /// </summary>
        /// <remarks>
        /// Command apperar in PropertyGrid's command area or a IMenuCommandService driven context menu, if the
        /// host makes use of the designer host. Inheritors should override this property and either add commands
        /// or replace the existing command.
        /// </remarks>
        /// <example>
        /// The following example shows how to create new commands:
        /// <code>
        /// <![CDATA[
        /// DesignerVerbCollection verbs = new DesignerVerbCollection();
        /// verbs.Add(new DesignerVerb("This Name appears in the menu", OnAction));
        /// ]]>
        /// </code>
        /// <c>OnAction</c> is a default event handler (<see cref="System.EventHandler"/>). To access the component which
        /// later issues the command one can get the <see cref="Element"/>.
        /// </example>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection verbs = new DesignerVerbCollection();
                return verbs;
            }
        }

        /// <summary>
        /// Called from event chain before any internal processing.
        /// </summary>
        /// <remarks>
        /// Inheritors may implement here private behavior to change the default event handling.
        /// Returning <c>True</c> will cause the internal chain to stop event processing.
        /// </remarks>
        /// <param name="sender">The EditHost which received and re-fired the event.</param>
        /// <param name="e">Information about the event.</param>
        /// <returns>Return <c>true</c> to inform the caller that any default handling has to be suppressed. Default is <c>false</c>.</returns>
        public virtual bool OnPreHandleEvent(object sender, DocumentEventArgs e)
        {
            return false;
        }

        /// <summary>
        /// Called from event chain after default internal processing.
        /// </summary>
        /// <remarks>
        /// Inheritors may implement here private behavior to change the default event handling.
        /// Returning <c>True</c> will cause the internal chain to stop event processing.
        /// </remarks>
        /// <param name="sender">The EditHost which received and re-fired the event.</param>
        /// <param name="e">Information about the event.</param>
        /// <returns>Return <c>true</c> to inform the caller that any default handling has to be suppressed. Default is <c>false</c>.</returns>
        public virtual bool OnPostHandleEvent(object sender, DocumentEventArgs e)
        {
            return false;
        }

        /// <summary>
        /// Called from event chain before any internal and external processing.
        /// </summary>
        /// <remarks>
        /// This is to inform the designer that any internal processing is done. The purpose is to check the results of any
        /// previous action or refresh the state of an object after finishing the event chain.
        /// </remarks>
        /// <param name="sender">The EditHost which received and re-fired the event.</param>
        /// <param name="e">Information about the event.</param>
        public virtual void OnPostEditorNotify(object sender, DocumentEventArgs e)
        {

        }

    }
}

