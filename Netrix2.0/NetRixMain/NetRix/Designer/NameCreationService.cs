using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Collections.Generic;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Creates new names for components that need a name.
    /// </summary>
    public class NameCreationService : INameCreationService
    {

        private IHtmlEditor editor;

        public NameCreationService(IHtmlEditor editor)
        {
            this.editor = editor;
            //TODO: Assign NameCreation properties here, if any 
        }

        #region INameCreationService Member

        public string CreateName(IContainer container, Type dataType)
        {
            string str2;
            string str1 = dataType.Name;
            int i = 1;
            List<string> names = new List<string>();
            foreach (IComponent cmp in container.Components)
            {
                if (cmp is System.Web.UI.Control)
                {
                    string n = ((System.Web.UI.Control)cmp).ID;
                    if (!String.IsNullOrEmpty(n))
                    {
                        names.Add(n);
                    }
                    else
                    {
                        if (cmp is IElement)
                        {
                            object id = ((IElement)cmp).GetAttribute("id");
                            if (id == null)
                            {
                                names.Add(((IElement)cmp).UniqueName);
                            }
                            else
                            {
                                names.Add(id.ToString());
                            }
                        }
                    }
                }
            }
            while (names.Contains(str2 = String.Concat(str1, i)))
            {
                i++;
            }
            return str2;
        }

        public bool IsValidName(string name)
        {
            for (int i = name.Length - 1; i >= 1; i--)
            {
                char ch1 = name[i];
                if (!Char.IsLetterOrDigit(ch1) && ch1 != '_')
                {
                    return false;
                }
            }
            char ch2 = name[0];
            if (!Char.IsLetter(ch2) && ch2 != '_')
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ValidateName(string name)
        {
            if (!IsValidName(name))
            {
                throw new Exception(String.Concat("Not a valid name : ", name));
            }
            else
            {
                return;
            }
        }

        #endregion
    }
}
