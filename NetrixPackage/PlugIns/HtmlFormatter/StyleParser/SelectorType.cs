namespace GuruComponents.Netrix.UserInterface.StyleParser
{
	/// <summary>
	/// The type of selector currently processed.
	/// </summary>
	public enum SelectorType
	{
        /// <summary>
        /// A Condition.
        /// </summary>
		CONDITIONAL_SELECTOR		= 0,
        /// <summary>
        /// Any other type.
        /// </summary>
		ANY_NODE_SELECTOR			= 1,
        /// <summary>
        /// The root node (*).
        /// </summary>
		ROOT_NODE_SELECTOR			= 2,
        /// <summary>
        /// A negative (exclude) selector.
        /// </summary>
		NEGATIVE_SELECTOR			= 3,
        /// <summary>
        /// An element node like B, P, or H1.
        /// </summary>
		ELEMENT_NODE_SELECTOR		= 4,
        /// <summary>
        /// A text node.
        /// </summary>
		TEXT_NODE_SELECTOR			= 5,
        /// <summary>
        /// A cdata node. Not recognized in versions less or equal 1.1.
        /// </summary>
		CDATA_SECTION_NODE_SELECTOR = 6,
        /// <summary>
        /// A attribute. Poorly supported by browsers.
        /// </summary>
		ATTRIBUTE_SELECTOR          = 7,
        /// <summary>
        /// A comment node. Not recognized in versions less or equal 1.1.
        /// </summary>
		COMMENT_NODE_SELECTOR		= 8,
        /// <summary>
        /// A pseudo element like @block.
        /// </summary>
		PSEUDO_ELEMENT_SELECTOR		= 9,
        /// <summary>
        /// Descendant selector, like A+B.
        /// </summary>
		DESCENDANT_SELECTOR			= 10,
        /// <summary>
        /// Adjacent selector, like A B.
        /// </summary>
		DIRECT_ADJACENT_SELECTOR	= 11,
        /// <summary>
        /// Child selector, like A>B.
        /// </summary>
		CHILD_SELECTOR				= 12,
        /// <summary>
        /// A class selector, like .class.
        /// </summary>
		CLASS_SELECTOR				= 13,
        /// <summary>
        /// An ID selector, like #id.
        /// </summary>
		ID_SELECTOR					= 14,
        /// <summary>
        /// A pseudo class selector, like A:hover.
        /// </summary>
		PSEUDO_CLASS_SELECTOR		= 15
	}
}
