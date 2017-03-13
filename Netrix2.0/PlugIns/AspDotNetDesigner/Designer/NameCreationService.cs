using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Collections.Generic;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.AspDotNetDesigner
{
	/// <summary>
	/// Creates new names for components that need a name.
	/// </summary>
	public class NameCreationService : INameCreationService 
	{

        private IHtmlEditor editor;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="editor"></param>
        public NameCreationService(IHtmlEditor editor)
        {
            this.editor = editor;
            //TODO: Assign NameCreation properties here, if any 
        }

        #region INameCreationService Member

        /// <summary>
        /// Create a new valid name for given type.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if a name is valid.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Validates a name
        /// </summary>
        /// <param name="name"></param>
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
