using System;
using GuruComponents.Netrix.WebEditing.Elements;
#pragma warning disable 1591
namespace GuruComponents.Netrix.WebEditing.HighLighting
{
        
    /// <summary>
    /// Returns some useful information about the underlying pointer.
    /// </summary>
    public struct PointerInformation
    {

        /// <summary>
        /// Checks the position of the pointer.
        /// </summary>
        public bool IsPositioned;
        /// <summary>
        /// Checks to see whether first pointer's position is to the right of second pointer's position.
        /// </summary>
        public bool FirstIsRightOfSecond;

        public bool FirstIsRightOfOrEqualToSecond;

        public bool FirstIsLeftOfSecond;

        public bool FirstIsLefttOfOrEqualToSecond;

        /// <summary>
        /// The first and the second pointer have the same location.
        /// </summary>
        public bool PointersAreEqual;

        /// <summary>
        /// The cling attribute determines whether a markup pointer will move or be deleted with the markup around it when the markup is moved or deleted.
        /// </summary>
        public bool FirstPointerCling;

        /// <summary>
        /// The cling attribute determines whether a markup pointer will move or be deleted with the markup around it when the markup is moved or deleted.
        /// </summary>
        public bool SecondPointerCling;

        public PointerGravity FirstPointerGravity;

        public PointerGravity SecondPointerGravity;

        public ContextType FirstPointerLeftContext;
        public ContextType FirstPointerRightContext;
        public ContextType SecondPointerLeftContext;
        public ContextType SecondPointerRightContext;

        /// <summary>
        /// An IElement interface for the element, if any, that is coming into scope, is exiting scope, 
        /// or is a no-scope element (such as a br element), as specified by ContextType.
        /// </summary>
        /// <remarks>
        /// Applies to the first pointer and its left scope.
        /// See <see cref="ContextType"/> for more information about the scope behavior.
        /// </remarks>
        public IElement FirstPointerLeftElementScope;
        /// <summary>
        /// An IElement interface for the element, if any, that is coming into scope, is exiting scope, 
        /// or is a no-scope element (such as a br element), as specified by ContextType.
        /// </summary>
        /// <remarks>
        /// Applies to the first pointer and its right scope.
        /// See <see cref="ContextType"/> for more information about the scope behavior.
        /// </remarks>
        public IElement FirstPointerRightElementScope;
        /// <summary>
        /// An IElement interface for the element, if any, that is coming into scope, is exiting scope, 
        /// or is a no-scope element (such as a br element), as specified by ContextType.
        /// </summary>
        /// <remarks>
        /// Applies to the second pointer and its left scope.
        /// See <see cref="ContextType"/> for more information about the scope behavior.
        /// </remarks>
        public IElement SecondPointerLeftElementScope;
        /// <summary>
        /// An IElement interface for the element, if any, that is coming into scope, is exiting scope, 
        /// or is a no-scope element (such as a br element), as specified by ContextType.
        /// </summary>
        /// <remarks>
        /// Applies to the second pointer and its right scope.
        /// See <see cref="ContextType"/> for more information about the scope behavior.
        /// </remarks>
        public IElement SecondPointerRightElementScope;

        /// <summary>
        /// Property that specifies the number of characters to retrieve.
        /// </summary>
        /// <remarks>
        /// Property that specifies the number of characters to retrieve to <see cref="FirstPointerLeftRetrievedChars"/>, 
        /// if <see cref="FirstPointerLeftContext"/> is <see cref="ContextType.Text"/>, and receives the actual 
        /// number of characters the method was able to retrieve. It can be set to -1, indicating that 
        /// the method should retrieve an arbitrary amount of text, up to the next no-scope element or element 
        /// scope transition.
        /// </remarks>
        public int FirstPointerLeftRetrievedLength;
        /// <summary>
        /// Property that specifies the number of characters to retrieve.
        /// </summary>
        /// <remarks>
        /// Property that specifies the number of characters to retrieve to <see cref="FirstPointerRightRetrievedChars"/>, 
        /// if <see cref="FirstPointerRightContext"/> is <see cref="ContextType.Text"/>, and receives the actual 
        /// number of characters the method was able to retrieve. It can be set to -1, indicating that 
        /// the method should retrieve an arbitrary amount of text, up to the next no-scope element or element 
        /// scope transition.
        /// </remarks>
        public int FirstPointerRightRetrievedLength;
        /// <summary>
        /// Property that specifies the number of characters to retrieve.
        /// </summary>
        /// <remarks>
        /// Property that specifies the number of characters to retrieve to <see cref="SecondPointerLeftRetrievedChars"/>, 
        /// if <see cref="SecondPointerLeftContext"/> is <see cref="ContextType.Text"/>, and receives the actual 
        /// number of characters the method was able to retrieve. It can be set to -1, indicating that 
        /// the method should retrieve an arbitrary amount of text, up to the next no-scope element or element 
        /// scope transition.
        /// </remarks>
        public int SecondPointerLeftRetrievedLength;
        /// <summary>
        /// Property that specifies the number of characters to retrieve.
        /// </summary>
        /// <remarks>
        /// Property that specifies the number of characters to retrieve to <see cref="SecondPointerRightRetrievedChars"/>, 
        /// if <see cref="SecondPointerRightContext"/> is <see cref="ContextType.Text"/>, and receives the actual 
        /// number of characters the method was able to retrieve. It can be set to -1, indicating that 
        /// the method should retrieve an arbitrary amount of text, up to the next no-scope element or element 
        /// scope transition.
        /// </remarks>
        public int SecondPointerRightRetrievedLength;

        /// <summary>
        /// String that receives the text specified, by <see cref="FirstPointerLeftRetrievedLength"/>.
        /// </summary>
        public string FirstPointerLeftRetrievedChars;
        /// <summary>
        /// String that receives the text specified, by <see cref="FirstPointerRightRetrievedLength"/>.
        /// </summary>
        public string FirstPointerRightRetrievedChars;
        /// <summary>
        /// String that receives the text specified, by <see cref="SecondPointerLeftRetrievedLength"/>.
        /// </summary>
        public string SecondPointerLeftRetrievedChars;
        /// <summary>
        /// String that receives the text specified, by <see cref="SecondPointerRightRetrievedLength"/>.
        /// </summary>
        public string SecondPointerRightRetrievedChars;

    }

}
