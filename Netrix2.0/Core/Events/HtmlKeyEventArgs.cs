using System;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Collections.Generic;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// This enumeration defines some types of key events.
    /// </summary>
    public enum KeyEventType
    {
        /// <summary>
        /// The type was not recognized.
        /// </summary>
        Unknown   = 0,
        /// <summary>
        /// The key was pressed (a complete phase, key down, processing, key up).
        /// </summary>
        KeyPress  = 1,
        /// <summary>
        /// The key goes up (NOT USED).
        /// </summary>
        KeyUp     = 2,
        /// <summary>
        /// The key goes down.
        /// </summary>
        KeyDown   = 3
    }

    /// <summary>
    /// This class defines the event arguments used for handling of key events.
    /// </summary>
    public class HtmlKeyEventArgs : EventArgs
    {
        private KeyEventType eventType = KeyEventType.Unknown;
        private IElement element;
        private Keys key;
        private bool handled;
        private static List<string> keys;

        static HtmlKeyEventArgs()
        {
            keys = new List<string> (Enum.GetNames(typeof(Keys)));
        }


        /// <summary>
        /// If set to true the caller informs the callee that the keystroke was handled and no further internal 
        /// processing is required. (Default: false).
        /// </summary>
        public bool Handled
        {
            get { return handled; }
            set { handled = value; }
        }

        /// <summary>
        /// Retrieves the element which has the current scope.
        /// </summary>
        /// <remarks>
        /// This property may return <c>null</c> (<c>Nothing</c> in Visual.Basic) if the element was not recognized.
        /// The element may be BODY if the caret cannot be placed inside a container (e.g. IMG cannot contain the
        /// caret and therefore the caret remains in the parent element, even if the last key stroke has made a 
        /// valid selection of the IMG element).
        /// </remarks>
        public IElement ElementUnderPointer
        {
            get
            {
                return element;
            }
        }

        /// <summary>
        /// Get the KeyEventType, which can be "press" (after key up) or "down".
        /// </summary>
        public KeyEventType EventType
        {
            get
            {
                return eventType;
            }
            set
            {
                eventType = value;
            }
        }

        /// <summary>
        /// The key the user has pressed.
        /// </summary>
        /// <remarks>
        /// Correct recognition of keys for extended characters depends on keyboard layout
        /// and language version. 
        /// </remarks>
        public Keys Key
        {
            get
            {  
                return key;
            }
        }

        /// <summary>
        /// Return the last typed ASCII character.
        /// </summary>
        /// <remarks>
        /// This property wil not recognize special chars, culture dependent chars and 
        /// ASCII characters beyond 128. It will return a char anyway, but its usable for
        /// lower standard ASCII only.
        /// </remarks>
        public char AsciiCharacter
        {
            get
            {                
                if ((int) key < 128)
                    return Win32.GetAsciiCharacter((int)key);
                else
                    return (char)(int)key;
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="el"></param>
        /// <param name="keycode"></param>
        /// <param name="htmlEditor"></param>
        public HtmlKeyEventArgs(Interop.IHTMLElement el, Keys keycode, IHtmlEditor htmlEditor)
        {
            handled = false;
            try
            {
                if (el != null)
                {
                    SetKey(keycode);
                    if (htmlEditor.GetCurrentScopeElement() != null)
                    {
                        element = htmlEditor.GetCurrentScopeElement();
                        htmlEditor.Document.SetActiveElement(htmlEditor.GetCurrentScopeElement());
                    }
                }
            }
            catch
            {
            }
        }

        private static Interop.IHTMLElement GetElement(Interop.IHTMLEventObj e, IHtmlEditor htmlEditor)
        {
            if (htmlEditor.GetCurrentScopeElement() == null)
            {
                return e.srcElement;
            } 
            else 
            {
                return htmlEditor.GetCurrentScopeElement().GetBaseElement();
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="eventObject"></param>
        /// <param name="htmlEditor"></param>
        public HtmlKeyEventArgs(Interop.IHTMLEventObj eventObject, IHtmlEditor htmlEditor)
        {   
            try
            {
                if (eventObject.srcElement != null)
                {
					switch (eventObject.type)
					{
                        case "keypress":
                            SetKey(eventObject, true);
                            if (GetElement(eventObject, htmlEditor) != null)
                            {

                                element = htmlEditor.GenericElementFactory.CreateElement(GetElement(eventObject, htmlEditor)) as IElement;
                                if (element != null)
                                {
                                    htmlEditor.Document.SetActiveElement(element);
                                }
                            }
                            eventType = KeyEventType.KeyPress;
                            break;
                        case "keydown":
                            SetKey(eventObject, false);
                            if (GetElement(eventObject, htmlEditor) != null)
                            {
                                element = htmlEditor.GenericElementFactory.CreateElement(GetElement(eventObject, htmlEditor)) as IElement;
                                if (element != null)
                                {
                                    htmlEditor.Document.SetActiveElement(element);
                                }
                            }
                            eventType = KeyEventType.KeyDown;
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        private void SetKey(Keys keycode)
        {            
            key = keycode;
        }

        private void SetKey(Interop.IHTMLEventObj e, bool pressed)
        {
            key = MapKeyLayout(e);
            if (key != Keys.ShiftKey && key != Keys.ControlKey)
            {
                key |= e.ctrlKey ? Keys.Control : Keys.None; // 
                key |= e.shiftKey ? Keys.Shift : Keys.None; // 
                key |= e.altKey ? Keys.Alt : Keys.None; // 
            }
        }

        private Keys MapKeyLayout(Interop.IHTMLEventObj e)
        {
            int hkl = Win32.GetKeyboardLayout(0);
            char m = (char)Win32.MapVirtualKeyEx(((e.keyCode >= 97 && e.keyCode <= 122 && e.type.IndexOf("pressed") != -1) ? e.keyCode - 32 : e.keyCode), 2, hkl);
            if (keys.Contains(m.ToString()))
            {
                Keys k = (Keys)Enum.Parse(typeof(Keys), m.ToString(), true);
                return k;
            }
            else
            {
                switch (m)
                {
                    case '.':
                        return Keys.OemPeriod;
                    case '+':
                        return Keys.Oemplus;
                    case '-':
                        return Keys.OemMinus;
                    case '\\':
                        return Keys.OemBackslash;
                    case ',':
                        return Keys.Oemcomma;
                    case '?':
                        return Keys.OemQuestion;
                    case '"':
                        return Keys.OemQuotes;
                    case '~':
                        return Keys.Oemtilde;
                    case ';':
                        return Keys.OemSemicolon;
                    case '[':
                        return Keys.OemOpenBrackets;
                    case ']':
                        return Keys.OemCloseBrackets;
                }
            }
            return Keys.NoName;
        }

    }

}
