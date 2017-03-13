using System;
          
namespace GuruComponents.Netrix.WebEditing.HighLighting
{
    /// <summary>
    /// An enumerated type that indicates whether a markup pointer will stay with the markup to its right or left when markup is inserted at the pointer's location.
    /// </summary>
    public enum PointerGravity
    {
        /// <summary>
        /// The pointer will stay with the markup to its left.
        /// </summary>
        Left = 0,
        /// <summary>
        /// The pointer will stay with the markup to its right.
        /// </summary>
        Right = 1
    }

}
