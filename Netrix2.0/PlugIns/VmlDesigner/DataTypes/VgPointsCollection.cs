using System;
using System.ComponentModel;

using GuruComponents.Netrix.ComInterop;
using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.VmlDesigner.Elements;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
    /// <summary>
    /// Zusammenfassung für VgPointsCollection.
    /// </summary>
    public class VgPointsCollection : System.Collections.CollectionBase, IVgPoints
    {

        private Interop.IHTMLElement baseShape;
        private IVgPoints internalPoints;

        internal VgPointsCollection(Interop.IHTMLElement baseShape, IVgPoints internalPoints) : base()
        {
            this.baseShape = baseShape;
            this.internalPoints = internalPoints;
            base.InnerList.Clear();
            // copy original list into internal base list
            for (int i = 0; i < internalPoints.length; i++)
            {
                this.Add(new VgVector2D(internalPoints[i]));
            }
        }

        /// <summary>
        /// Adds a style element to the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o"></param>
        public void Add(VgVector2D vector) 
        {
            if (vector == null) return;
            base.List.Add(vector);
        }

        /// <summary>
        /// Checks if the element is part of the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Contains(VgVector2D vector) 
        {
            if (vector == null) return false;
            return List.Contains(vector);
        }

        /// <summary>
        /// Removes the element from the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o"></param>
        public void Remove(VgVector2D vector) 
        {
            if (vector == null) return;
            base.List.Remove(vector);
        }

        public void RemoveAll()
        {
            base.InnerList.Clear();
        }

        /// <summary>
        /// Gets or sets a style element in the collection using an index. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        public VgVector2D this[int index] 
        {
            get 
            {
                return (VgVector2D)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        /// <summary>
        /// Fired if the collection editor inserts a new element. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        public event CollectionInsertHandler Insert;
        /// <summary>
        /// Fired if the colection editor starts a new sequence. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        public event CollectionClearHandler ClearCollection;

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsert (index, value);
            // prevent from fail during first collection fill process, because the handler is temporary removed
            if (Insert != null)
            {
                Insert(index, value);
            }
        }

        protected override void OnClear()
        {
            base.OnClear ();
            // prevent from fail during first collection fill process, because the handler is temporary removed
            if (ClearCollection != null)
            {
                ClearCollection();
            }
        }

        #region IVgPoints Member

        [Browsable(false)]
        public int Creator
        {
            get
            {         
                return internalPoints.Creator; 
            }
        }

        [Browsable(false)]
        public object parentShape
        {
            get
            {
                return baseShape;
            }
        }

        [Browsable(false)]
        Comzept.Genesis.NetRix.VgxDraw.IVgVector2D Comzept.Genesis.NetRix.VgxDraw.IVgPoints.this[int Index]
        {
            get
            {
                return internalPoints[Index];
            }
        }

        [Browsable(false)]
        public object Application
        {
            get
            {
                return internalPoints.Application;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.IVgVector2D add(uint insertionIndex)
        {
            return internalPoints.add(insertionIndex);
        }

        [Browsable(false)]
        public string @value
        {
            get
            {
                return internalPoints.@value;
            }
            set
            {
                internalPoints.@value = value;
            }
        }

        [Browsable(false)]
        public int length
        {
            get
            {
                return internalPoints.length;
            }
        }

        #endregion
    }
}