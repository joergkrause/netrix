using System;
using System.Collections;
using System.ComponentModel;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
	/// <summary>
	/// Zusammenfassung für VgGradientColorArray.
	/// </summary>
	public class VgGradientColorArray : CollectionBase
	{

		Comzept.Genesis.NetRix.VgxDraw.IVgGradientColorArray native;
	
		public VgGradientColorArray(Comzept.Genesis.NetRix.VgxDraw.IVgGradientColorArray native)
		{
			this.native = native;
        }

		protected override void OnClear()
		{
			for (int i = 0; i < Count; i++)
			{
				Comzept.Genesis.NetRix.VgxDraw.IVgGradientColorArrayItem c = this[i];
				c.remove();
			}
		}

        protected override void OnInsert(int index, object value)
        {
            base.OnInsert(index, value);
            native.addColor(Convert.ToDouble(value));
        }

        #region IVgGradientColorArray Member

		[Browsable(false)]
		public int Creator
        {
            get
            {
                return native.Creator;
            }
        }

		[Browsable(false)]
		public object ParentShape
        {
            get
            {
                return null;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.IVgGradientColorArrayItem this[int Index]
        {
            get
            {
                return native[Index];
            }
        }

        /// <summary>
        /// Application
        /// </summary>				
        [Browsable(false)]
		public object Application
        {
            get
            {
                return native.Application;
            }
        }

        public void RemoveColor(double percent)
        {
			native.removeColor(percent);
        }

        public string Value
        {
            get
            {
                return native.value;
            }
            set
            {
                native.value = value;
            }
        }		

        public new int Count
        {
            get
            {
                return native.length;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.IVgColor addColor(double percent)
        {
            return native.addColor(percent);
        }

        #endregion

		public override string ToString()
		{
			return "Gradient Array";
		}



    }
}

