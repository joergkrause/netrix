using System;

namespace GuruComponents.Netrix.TableDesigner
{
	/// <summary>
	/// This class sorts the ArrayList of highlighted cells first rowwise and second columnwise.
	/// </summary>
	/// <remarks>
	/// If the cell code is r,c the order is 0,1; 0,2; 0,3; 1,1; 1,2; 1,3.
	/// </remarks>
	internal class HighLightCellComparer : System.Collections.IComparer
	{

        #region IComparer Member

        public int Compare(object x, object y)
        {
            BaseHighLightCell c1 = (BaseHighLightCell) x;
            BaseHighLightCell c2 = (BaseHighLightCell) y;
            if (c1.Row < c2.Row)
            {
                return -1;
            }
            if (c1.Row > c2.Row)
            {
                return 1;
            }
            if (c1.Col < c2.Col)
            {
                return -1;
            }
            if (c1.Col > c2.Col)
            {
                return 1;
            }
            return 0;
        }

        #endregion
    }
}
