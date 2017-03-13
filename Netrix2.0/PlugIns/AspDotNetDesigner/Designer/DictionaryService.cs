using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.Collections;

namespace GuruComponents.Netrix.Designer
{
    class DictionaryService : IDictionaryService
    {

        Hashtable privateKeys = new Hashtable();

        #region IDictionaryService Members

        public object GetKey(object value)
        {
            foreach (DictionaryEntry de in privateKeys)
            {
                if (de.Equals(de.Value))
                    return de.Key;
            }
            return null;
        }

        public object GetValue(object key)
        {
            return privateKeys[key];
        }

        public void SetValue(object key, object value)
        {
           privateKeys[key] = value;
        }

        #endregion
    }
}
