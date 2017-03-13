namespace GuruComponents.Netrix.WebEditing.Behaviors
{
	/// <summary>
	/// Notifies the behavior about the progress of parsing the document and the element to which the behavior is attached.
	/// </summary>
	public enum BehaviorNotifyType
	{
		/// <summary>
		/// Received when the end tag of the element to which this DHTML behavior is attached is parsed.
		/// </summary>
		ContentReady,
		/// <summary>
		/// Received when the content of the behavior is being saved.
		/// </summary>
		ContentSave,
		/// <summary>
		/// Received when the entire document to which this DHTML behavior is attached is parsed.
		/// </summary>
		DocumentReady,
		/// <summary>
		/// Received when a behavior is added or removed from a document.
		/// </summary>
		DocumentContextChange
	}
}
