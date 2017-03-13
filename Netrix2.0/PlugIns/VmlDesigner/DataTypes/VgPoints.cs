using System;
using System.ComponentModel;
using Comzept.Genesis.NetRix.VgxDraw;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
	/// <summary>
	/// VgPoints represents a collection of native points.
	/// </summary>
	public class VgPoints : Comzept.Genesis.NetRix.VgxDraw.IVgPoints
	{

        IVgPoints nativePoints;

		public VgPoints(){
        }

        public VgPoints(IVgPoints nativePoints){
            this.nativePoints = nativePoints ;
        }

        /// <summary>
        /// Used internally to set the attribute back. Later, when the native wrapper classes are ready, we 
        /// will remove this and let the wrapper classes handle the internal stuff.
        /// </summary>
        internal IVgPoints NativePoints {
            get {
                return this.nativePoints;
            }
        }

        #region IVgPoints Member

        int _IVgDispObj.Creator
        {
            get
            {
                return (nativePoints != null) ? nativePoints.Creator : -1;
            }
        }

        int IVgPoints.Creator
        {
            get
            {
                return (nativePoints != null) ? nativePoints.Creator : -1;
            }
        }

		[Browsable(false)]
        object IVgPoints.parentShape
        {
            get
            {
                return (nativePoints != null) ? nativePoints.parentShape : null;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.IVgVector2D this[int Index]
        {
            get { return nativePoints[Index]; }
        }

        /// <summary>
        /// Application
        /// </summary>				
        [Browsable(false)]
        public object Application
        {
            get
            {
                return (nativePoints != null) ? nativePoints.Application : null;
            }
        }

        Comzept.Genesis.NetRix.VgxDraw.IVgVector2D Comzept.Genesis.NetRix.VgxDraw.IVgPoints.add(uint insertionIndex)
        {
            return nativePoints.add(insertionIndex);
        }

        /// <summary>
        /// Add a point at the specified index.
        /// </summary>
        /// <param name="insertionIndex"></param>
        /// <returns></returns>
        public GuruComponents.Netrix.VmlDesigner.DataTypes.VgVector2D Add(uint insertionIndex)
        {
           return new GuruComponents.Netrix.VmlDesigner.DataTypes.VgVector2D(((Comzept.Genesis.NetRix.VgxDraw.IVgPoints) this).add(insertionIndex));
        }

        /// <summary>
        /// Native value of points collection.
        /// </summary>
        [Browsable(false)]
        public string value {
            get { return nativePoints.@value; }
            set { nativePoints.@value = value; }
        }

        /// <summary>
        /// Number of elements in the points collection.
        /// </summary>
        public int length{
            get { return nativePoints.length; }
        }

        #endregion

        public override string ToString() {
            return String.Format("VgPoints [{0}]", nativePoints.@value);
        }
    }
}
