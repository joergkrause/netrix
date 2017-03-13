using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace GuruComponents.CodeEditor.Library.Collections.Generic
{
	/// <summary>
	/// Incomplete class, not use.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class TreeCollection<T>
	{
		public delegate void TreeItemEnumDelegate(TreeItem<T> current);
		public delegate void TreeItemXmlWriterDelegate(TreeItem<T> current, XmlTextWriter writer);
		public delegate void TreeItemStringWriterDelegate(TreeItem<T> current, StringBuilder builder);
		public delegate void TreeItemXmlReaderDelegate(ref TreeItem<T> current, XmlNode currentNode);

		private KeyedCollection<TreeItem<T>> _globalCollection;
		private KeyedCollection<T> _globalValues;
		private LightCollection<string> _rootItems;

		internal KeyedCollection<TreeItem<T>> GlobalCollection
		{
			get
			{
				return _globalCollection;
			}
		}
		internal KeyedCollection<T> GlobalValues
		{
			get
			{
				return _globalValues;
			}
		}

		internal bool Global_AddItem(string id, TreeItem<T> item)
		{
			if (_globalCollection.Contains(id)) return false;

			_globalCollection.Add(id, item);
			_globalValues.Add(id, default(T));
			return true;
		}
		internal bool Global_RemoveItem(string id)
		{
			if (_globalCollection.Contains(id)) return false;

			_globalCollection.Remove(id);
			_globalValues.Remove(id);
			return true;
		}

		public TreeCollection():this(32)
		{
		}
		public TreeCollection(int initialSize)
		{
			_globalCollection = new KeyedCollection<TreeItem<T>>(initialSize);
			_globalValues = new KeyedCollection<T>(initialSize);
			_rootItems = new LightCollection<string>();
		}

		public TreeItemCollection<T> Items
		{
			get 
			{
				TreeCollection<T> _this = this;
				return new TreeItemCollection<T>(ref _this, "_root", _rootItems); 
			}
		}
		public KeyedCollection<T> AllItems
		{
			get 
			{
				throw new NotImplementedException();
			}
		}

		/*public void AddRootItem(T value)
		{
			string key = _globalCollection.CreateFreeKey();
			AddRootItem(key, value);
		}
		public void AddRootItem(string id)
		{
			AddRootItem(id, default(T));
		}
		public void AddRootItem(string id, T value)
		{
			if(_globalCollection.Contains(id))
				throw new Exception("key \""+id+"\" already exist");

			TreeCollection<T> _this = this;
			TreeItem<T> treeItem = new TreeItem<T>(id, "_root", ref _this);
			_globalCollection.Add(id, treeItem);
			_globalCollection[id].Value = value;

			_rootItems.Add(id);

		}
		public bool TryAddRootItem(string id, T value)
		{
			if(_globalCollection.Contains(id))
			{
				return false;
			}

			TreeCollection<T> _this = this;
			TreeItem<T> treeItem = new TreeItem<T>(id, "_root", ref _this);
			_globalCollection.Add(id, treeItem);
			_globalCollection[id].Value = value;

			_rootItems.Add(id);
			return true;
		}*/

		public void EnumerateSubitems(TreeItemEnumDelegate itemsEnum)
		{
			EnumerateSubitems(itemsEnum, null);
		}
		public void EnumerateSubitems(TreeItemEnumDelegate itemsEnum, TreeItem<T> startItem)
		{
			LightCollection<string> subitems;
			if (startItem == null)
			{
				subitems = _rootItems;
			}
			else
			{
				subitems = startItem.Subitems.ItemsId;
			}

			for (int i = 0; i < subitems.Count; i++)
			{
				if (itemsEnum != null)
				{
					TreeItem<T> current = _globalCollection[subitems[i]];
					itemsEnum(current);
					EnumerateSubitems(itemsEnum, current);
				}
			}
		}

		public TreeItemEnumerator<T> GetEnumerator()
		{
			return new TreeItemEnumerator<T>(ref _globalCollection, _rootItems);
		}

		public static TreeCollection<T> FromXml(string xml, TreeItemXmlReaderDelegate readerDelegate)
		{
			XmlDocument xdoc = new XmlDocument();
			xdoc.LoadXml(xml);
			return FromXml(xdoc, readerDelegate);
		}
		public static TreeCollection<T> FromXml(Stream stream, TreeItemXmlReaderDelegate readerDelegate)
		{
			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(stream);
			return FromXml(xdoc, readerDelegate);
		}
		public static TreeCollection<T> FromXml(XmlDocument xdoc, TreeItemXmlReaderDelegate readerDelegate)
		{
			XmlNodeList xlist = xdoc.SelectNodes("TreeCollection//TreeItem");
			TreeCollection<T> treeColl = new TreeCollection<T>(xlist.Count);

			xlist = xdoc.SelectNodes("TreeCollection/TreeItem");

			for (int i = 0; i < xlist.Count; i++)
			{
				XmlNode currentNode = xlist[i];

				TreeItem<T> newItem = treeColl.Items.Add(treeColl.GlobalCollection.CreateFreeKey(), default(T));

				readerDelegate(ref newItem, currentNode);

				if (currentNode.ChildNodes.Count > 0)
				{
					TreeItemCollection<T> currentColl = treeColl.Items;
					FromXmlNode(treeColl, ref currentColl, xlist[i], readerDelegate);
				}
			}
			return treeColl;
		}
		public static TreeCollection<T> FromXmlByReflection(string xml)
		{
			XmlDocument xdoc = new XmlDocument();
			xdoc.LoadXml(xml);
			return FromXmlByReflection(xdoc);
		}
		public static TreeCollection<T> FromXmlByReflection(Stream stream)
		{
			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(stream);
			return FromXmlByReflection(xdoc);
		}

		public TreeItem<T> Find(string id)
		{
			return _globalCollection[id];
		}
		public LightCollection<TreeItem<T>> Find(Predicate<TreeItem<T>> match)
		{
			LightCollection<TreeItem<T>> results = new LightCollection<TreeItem<T>>();
			for (int i = 0; i < _globalCollection.Count; i++)
			{
				if(match(_globalCollection[i]))
				{
					results.Add(_globalCollection[i]);
				}
			}
			return results;
		}

		public string ToXml(TreeItemXmlWriterDelegate treeItemWriterDelegate)
		{
			MemoryStream ms = new MemoryStream();
			XmlTextWriter xtw = new XmlTextWriter(ms,Encoding.UTF8);

			xtw.WriteStartDocument(true);
			xtw.WriteStartElement("TreeCollection");

			ToXml(treeItemWriterDelegate);

			xtw.WriteEndElement();
			xtw.WriteEndDocument();

			xtw.Flush();

			Stream stream = xtw.BaseStream;
			stream.Position = 0;

			StreamReader sr = new StreamReader(stream);
			string xml = sr.ReadToEnd();

			sr.Close();
			stream.Close();
			xtw.Close();

			return xml;
		}
		public string ToString(TreeItemStringWriterDelegate treeItemWriterDelegate)
		{
			StringBuilder sb=new StringBuilder();
			ToString(null, sb, treeItemWriterDelegate);
			return sb.ToString();
		}
		public string ToIndexXml()
		{
			TreeItemXmlWriterDelegate writerDelegate = new TreeItemXmlWriterDelegate(IndexXmlWriterDelegate);
			return ToXml(writerDelegate);
		}
		public string ToIndexString()
		{
			TreeItemStringWriterDelegate writerDelegate = new TreeItemStringWriterDelegate(IndexStringWriterDelegate);
			return ToString(writerDelegate);
		}
		public string ToXmlByReflection()
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			TreeItemStringWriterDelegate writerDelegate = new TreeItemStringWriterDelegate(StringWriterDelegate);
			StringBuilder sb = new StringBuilder();
			ToString(null,sb,writerDelegate);
			return sb.ToString();
		}

		private static void FromXmlNode(TreeCollection<T> treeCollection,ref TreeItemCollection<T> currentColl, XmlNode xnode, TreeItemXmlReaderDelegate readerDelegate)
		{
			XmlNodeList xlist = xnode.SelectNodes("./TreeItem");

			for (int i = 0; i < xlist.Count; i++)
			{
				XmlNode currentNode = xlist[i];

				TreeItem<T> newItem = currentColl.Add(treeCollection.GlobalCollection.CreateFreeKey(), default(T));

				readerDelegate(ref newItem, currentNode);

				if (currentNode.ChildNodes.Count > 0)
				{
					TreeItemCollection<T> itemColl = newItem.Subitems;
					FromXmlNode(treeCollection,ref itemColl, xlist[i], readerDelegate);
				}
			}
		}
		private static TreeCollection<T> FromXmlByReflection(XmlDocument xdoc)
		{
			throw new NotImplementedException();
		}

		private void StringWriterDelegate(TreeItem<T> current, StringBuilder sb)
		{
			object value = current.Value;
			sb.Append('\t', current.Level);
			if (value != null)
			{
				sb.AppendLine(current.Value.ToString());
			}
			else
			{
				sb.AppendLine("null");
			}
		}
		private void IndexXmlWriterDelegate(TreeItem<T> current, XmlTextWriter xtw)
		{
			xtw.WriteStartElement("TreeItem");
			xtw.WriteAttributeString("Id", current.Id);
			xtw.WriteEndElement(); //TreeItem
		}
		private void IndexStringWriterDelegate(TreeItem<T> current, StringBuilder sb)
		{
			sb.Append('\t', current.Level);
			sb.AppendLine("[" + current.Id + "]");
		}

		private void ToIndexXml(string currentId,XmlTextWriter xtw)
		{
			LightCollection<string> items;
			if (currentId == null)
			{
				items = _rootItems;
			}
			else
			{
				items = _globalCollection[currentId].Subitems.ItemsId;
			}
			for (int i = 0; i < items.Count; i++)
			{
				xtw.WriteStartElement("TreeItem");
				xtw.WriteAttributeString("Id", items[i]);
				ToIndexXml(items[i], xtw);
				xtw.WriteEndElement(); //TreeItem
			}
		}
		private void ToXml(string currentId, XmlTextWriter xtw, TreeItemXmlWriterDelegate treeItemWriterDelegate)
		{
			LightCollection<string> items;
			if (currentId == null)
			{
				items = _rootItems;
			}
			else
			{
				items = _globalCollection[currentId].Subitems.ItemsId;
			}
			for (int i = 0; i < items.Count; i++)
			{
				treeItemWriterDelegate(_globalCollection[items[i]], xtw);
				ToXml(items[i], xtw, treeItemWriterDelegate);
			}
		}
		private void ToString(string currentId, StringBuilder sb, TreeItemStringWriterDelegate treeItemWriterDelegate)
		{
			LightCollection<string> items;
			if (currentId == null)
			{
				items = _rootItems;
			}
			else
			{
				items = _globalCollection[currentId].Subitems.ItemsId;
			}
			for (int i = 0; i < items.Count; i++)
			{
				treeItemWriterDelegate(_globalCollection[items[i]], sb);
				ToString(items[i], sb, treeItemWriterDelegate);
			}
		}

	}
}
