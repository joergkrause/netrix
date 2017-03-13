using System;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.HighLighting
{
	/// <summary>
	/// HighLightSegment holds a highlighting object and provides a remove method to remove the decoration
	/// at a later time.
	/// </summary>
	public interface IHighLightSegment
	{

       	/// <summary>
		/// This method removes the highlight decoration from the segment.
		/// </summary>
		void RemoveSegment();

	}
}