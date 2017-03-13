using System;
using System.Collections;
using System.Xml;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner.Edx
{
	/// <summary>
	/// Summary description for Transaction.
	/// </summary>
	public class Transaction
	{

		public Stack aRedo;
		public Stack aUndo;
		public ArrayList aNotify;

		/// <summary>
		/// Constructor for a transaction record, take a node to notify on or null if not known yet.
		/// </summary>
		/// <param name="n"></param>
		public Transaction(XmlNode n )
		{
			this.aNotify = new ArrayList();
			if( n != null )
			{
				this.aNotify[0] = Util.GetSigFromNode( n );
			}
			this.aRedo = new Stack();
			this.aUndo = new Stack();
		}
	}
}
