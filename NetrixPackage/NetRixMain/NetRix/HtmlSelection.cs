using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.UndoRedo;
using System.Web.UI;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel;

# pragma warning disable 0618

namespace GuruComponents.Netrix
{

    /// <summary>
    /// This class provides methods and properties for element selections and layer operations.
    /// </summary>
    /// <remarks>
    /// If the application provides absolute positioning various methods can be used to set z-order or align floating 
    /// elements to the left, right or top. The class contains also method to wrap elements around the current
    /// selection. It doesn't matter if this selection is primarily a text or control selection.
    /// </remarks>
    /// <example>
    /// This example shows how to get an instance of this class in C#:
    /// <code lang="C#">
    /// using GuruComponents.Netrix;
    /// 
    /// // run this code anywhere after ReadyStateComplete event was fired
    /// this.htmlEditor1 = new HtmlEditor();
    /// ISelection selection = this.htmlEditor1.Selection;
    /// 
    /// </code>
    /// This example shows how to get an instance of this class in VB.NET:
    /// <code>
    /// Imports GuruComponents.Netrix
    /// 
    /// // run this code anywhere after ReadyStateComplete event was fired
    /// Me.htmlEditor1 = New HtmlEditor()
    /// Dim selection As ISelection = this.htmlEditor1.Selection
    /// </code>    
    /// </example>
	public class HtmlSelection : ISelection, ISelectionService, IDisposable
	{

		private static readonly string DesignTimeLockAttribute = "Design_Time_Lock";

		private HtmlEditor htmlEditor;
		private HtmlSelectionType _type = HtmlSelectionType.Empty;
		private int _selectionLength;
		/// <summary>
		/// The text of the current selection, tags are stripped out
		/// </summary>
		private string _text;
		/// <summary>
		/// The current selection with tags
		/// </summary>
		private string _html;
		private object _mshtmlSelection;
		private List<Interop.IHTMLElement> _items;
        private List<Interop.IHTMLElement> _oldItems;
		private ElementCollection _elements;
		private bool _sameParentValid;
		private int _maxZIndex;
		private int _minZIndex;

		internal HtmlSelection(IHtmlEditor editor) 
		{
			htmlEditor = (HtmlEditor) editor;
			_maxZIndex = 99;
			_minZIndex = 100;
		}

		# region Alignment

		/// <summary>
		/// Indicates if the current selection of absolutely positionable elements can be aligned to 
		/// any of the borders. </summary>
		/// <remarks>
		/// This is possible if two or more elements are selected and all of them
		/// have the position:absolute style attribute.
		/// </remarks>
		public bool CanAlign 
		{
			get 
			{
				this.SynchronizeSelection();
				if (_items.Count < 2) 
				{
					return false;
				}
				if (_type == HtmlSelectionType.ElementSelection) 
				{
					foreach (Interop.IHTMLElement elem in _items) 
					{
						//First check if they are all absolutely positioned
						if (!IsElement2DPositioned(elem)) 
						{
							return false;
						}

						//Then check if none of them are locked
						if (IsElementLocked(elem)) 
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// This method returns true, if all selected elements have the same parent.
		/// </summary>
		/// <remarks>
		/// Under some circumstances it is recommended not to align floating elements if they are
		/// defined under different parent elements.
		/// </remarks>
		public bool HaveSameParent
		{
			get
			{
				this.SynchronizeSelection();
				return SameParent;
			}
		}

		/// <summary>
		/// Assign bottom alignment to current selection.
		/// </summary>
		public void AlignBottom()
		{
			SynchronizeSelection();
			if (SelectionType == HtmlSelectionType.ElementSelection)
			{
				int i = _items.Count;
			    Interop.IHTMLElement interop_IHTMLElement = (Interop.IHTMLElement)_items[i - 1];
			    Interop.IHTMLStyle interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
				int j = interop_IHTMLStyle.GetPixelTop() + interop_IHTMLElement.GetOffsetHeight();
                IUndoStack batchedUndoUnit = htmlEditor.InternalUndoStack("Align Bottom");
				try
				{
					for (int k = 0; k < i - 1; k++)
					{
						interop_IHTMLElement = (Interop.IHTMLElement)_items[k];
						interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
						interop_IHTMLStyle.SetTop((j - interop_IHTMLElement.GetOffsetHeight()));
					}
				}
				catch
				{
				}
				finally
				{
					batchedUndoUnit.Close();
				}
			}
		}

		/// <summary>
		/// Align selected elements horizontal centered.
		/// </summary>
		public void AlignHorizontalCenter()
		{
			SynchronizeSelection();
			if (SelectionType == HtmlSelectionType.ElementSelection)
			{
				int i = _items.Count;
			    Interop.IHTMLElement interop_IHTMLElement = (Interop.IHTMLElement)_items[i - 1];
			    Interop.IHTMLStyle interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
				int j = interop_IHTMLStyle.GetPixelLeft() + interop_IHTMLElement.GetOffsetWidth() / 2;
                IUndoStack batchedUndoUnit = htmlEditor.InternalUndoStack("Align Left");
				try
				{
					for (int k = 0; k < i - 1; k++)
					{
						interop_IHTMLElement = (Interop.IHTMLElement)_items[k];
						interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
						interop_IHTMLStyle.SetLeft(j - interop_IHTMLElement.GetOffsetWidth() / 2);
					}
				}
				catch
				{
				}
				finally
				{
					batchedUndoUnit.Close();
				}
			}
		}

		/// <summary>
		/// Align selection left.
		/// </summary>
		public void AlignLeft()
		{
			SynchronizeSelection();
			if (SelectionType == HtmlSelectionType.ElementSelection)
			{
				int i = _items.Count;
			    Interop.IHTMLElement interop_IHTMLElement = (Interop.IHTMLElement)_items[i - 1];
			    Interop.IHTMLStyle interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
				int j = interop_IHTMLStyle.GetPixelLeft();
				IUndoStack batchedUndoUnit = htmlEditor.InternalUndoStack("Align Left");
				try
				{
					for (int k = 0; k < i - 1; k++)
					{
						interop_IHTMLElement = (Interop.IHTMLElement)_items[k];
						interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
						interop_IHTMLStyle.SetLeft(j);
					}
				}
				catch
				{
				}
				finally
				{
					batchedUndoUnit.Close();
				}
			}
		}

		/// <summary>
		/// Align selection to the right.
		/// </summary>
		public void AlignRight()
		{
			SynchronizeSelection();
			if (SelectionType == HtmlSelectionType.ElementSelection)
			{
				int i = _items.Count;
			    Interop.IHTMLElement interop_IHTMLElement = (Interop.IHTMLElement)_items[i - 1];
			    Interop.IHTMLStyle interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
				int j = interop_IHTMLStyle.GetPixelLeft() + interop_IHTMLElement.GetOffsetWidth();
                IUndoStack batchedUndoUnit = htmlEditor.InternalUndoStack("Align Left");
				try
				{
					for (int k = 0; k < i - 1; k++)
					{
						interop_IHTMLElement = (Interop.IHTMLElement)_items[k];
						interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
						interop_IHTMLStyle.SetLeft((j - interop_IHTMLElement.GetOffsetWidth()));
					}
				}
				catch
				{
				}
				finally
				{
					batchedUndoUnit.Close();
				}
			}
		}

		/// <summary>
		/// Align elements vertically at top of containing container.
		/// </summary>
		public void AlignTop()
		{
			SynchronizeSelection();
			if (SelectionType == HtmlSelectionType.ElementSelection)
			{
				int i = _items.Count;
			    Interop.IHTMLElement interop_IHTMLElement = (Interop.IHTMLElement)_items[i - 1];
			    Interop.IHTMLStyle interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
				int j = interop_IHTMLStyle.GetPixelTop();
                IUndoStack batchedUndoUnit = htmlEditor.InternalUndoStack("Align Left");
				try
				{
					for (int k = 0; k < i - 1; k++)
					{
						interop_IHTMLElement = (Interop.IHTMLElement)_items[k];
						interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
						interop_IHTMLStyle.SetTop(j);
					}
				}
				catch
				{
				}
				finally
				{
					batchedUndoUnit.Close();
				}
			}
		}

		/// <summary>
		/// Align elements vertically centered if inside a block element.
		/// </summary>
		public void AlignVerticalCenter()
		{
			SynchronizeSelection();
			if (SelectionType == HtmlSelectionType.ElementSelection)
			{
				int i = _items.Count;
			    Interop.IHTMLElement interop_IHTMLElement = (Interop.IHTMLElement)_items[i - 1];
			    Interop.IHTMLStyle interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
				int j = interop_IHTMLStyle.GetPixelTop() + interop_IHTMLElement.GetOffsetHeight() / 2;
				IUndoStack batchedUndoUnit = htmlEditor.InternalUndoStack("Align Left");
				try
				{
					for (int k = 0; k < i - 1; k++)
					{
						interop_IHTMLElement = (Interop.IHTMLElement)_items[k];
						interop_IHTMLStyle = interop_IHTMLElement.GetStyle();
						interop_IHTMLStyle.SetTop(((j - interop_IHTMLElement.GetOffsetHeight() / 2)));
					}
				}
				catch
				{
				}
				finally
				{
					batchedUndoUnit.Close();
				}
			}
		}


		# endregion

		# region Design Time Lock

		/// <summary>
		/// Toggle the design time lock state of the selected items. 
		/// </summary>
		/// <remarks>
		/// Locked elements cannot be changed in design mode.
		/// </remarks>
		public void ToggleLock() 
		{			
            SynchronizeSelection();
			foreach (Interop.IHTMLElement elem in _items) 
			{
			    Interop.IHTMLStyle style = elem.GetStyle();
				if (IsElementLocked(elem)) 
				{
					//We need to remove attributes off the element and the style because of a bug in MSHTML
					elem.RemoveAttribute(DesignTimeLockAttribute,0);
					style.RemoveAttribute(DesignTimeLockAttribute,0);
				}
				else 
				{
					//We need to add attributes to the element and the style because of a bug in MSHTML
					elem.SetAttribute(DesignTimeLockAttribute,"true",0);
					style.SetAttribute(DesignTimeLockAttribute,"true",0);
				}
			}
		}

		/// <summary>
		/// Returns info about the design time lock state of the selection
		/// </summary>
		/// <returns></returns>
		public HtmlCommandInfo GetLockInfo() 
		{
			if (_type == HtmlSelectionType.ElementSelection) 
			{
				foreach (Interop.IHTMLElement elem in _items) 
				{
					//We only need to check that all elements are absolutely positioned
					if (!IsElement2DPositioned(elem)) 
					{
						return 0;
					}

					if (IsElementLocked(elem)) 
					{
						return HtmlCommandInfo.Checked | HtmlCommandInfo.Enabled;
					}
					return HtmlCommandInfo.Enabled;
				}
			}
			return 0;
		}

		/// <summary>
		/// Convenience method for checking if the specified element has a design time lock
		/// </summary>
		/// <param name="elem"></param>
		/// <returns></returns>
		private static bool IsElementLocked(Interop.IHTMLElement elem) 
		{
			object[] attribute = new object[1];
			elem.GetAttribute(DesignTimeLockAttribute, 0, attribute);
			if (attribute[0] == null) 
			{
			    Interop.IHTMLStyle style = elem.GetStyle();
				attribute[0] = style.GetAttribute(DesignTimeLockAttribute, 0);
			}
			if ((attribute[0] == null) || !(attribute[0] is string)) 
			{
				return false;
			}
			return true;
		}

		# endregion

		# region 2D Positioning

		/// <summary>
		/// Indicates if the current selection have it's z-index modified
		/// </summary>
		public bool CanChangeZIndex 
		{
			get 
			{
                SynchronizeSelection();
                if (_items == null || _items.Count == 0) 
				{
					return false;
				}
				if (_type == HtmlSelectionType.ElementSelection) 
				{
					foreach (Interop.IHTMLElement elem in _items) 
					{
						//First check if they are all absolutely positioned
						if (!IsElement2DPositioned(elem)) 
						{
							return false;
						}
					}
					//Then check if they all have the same parent
					if (!SameParent) 
					{
						return false;
					}
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Convenience method for checking if the specified element is absolutely positioned
		/// </summary>
		/// <param name="element">The element to check.</param>
		/// <returns>Returns <c>true</c> if "position:absolute" style is set.</returns>
		public bool IsElement2DPositioned(IElement element) 
		{
			return IsElement2DPositioned(element.GetBaseElement());
		}

		/// <summary>
		/// Convenience method for checking if the specified element is absolutely positioned
		/// </summary>
		/// <param name="elem"></param>
		/// <returns></returns>
		private bool IsElement2DPositioned(Interop.IHTMLElement elem) 
		{
		    Interop.IHTMLElement2 elem2 = (Interop.IHTMLElement2) elem;
		    Interop.IHTMLCurrentStyle style = elem2.GetCurrentStyle();
			string position = style.position;
			if ((position == null) || (String.Compare(position, "absolute", true) != 0)) 
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns info about the absolute positioning of the selection
		/// </summary>
		/// <returns></returns>
		public HtmlCommandInfo GetAbsolutePositionInfo() 
		{
			return htmlEditor.GetCommandInfo(Interop.IDM.ABSOLUTE_POSITION);
		}

		/// <summary>
		/// Toggle the absolute positioning state of the selected items.
		/// </summary>
		public void ToggleAbsolutePosition() 
		{
			htmlEditor.Exec(Interop.IDM.ABSOLUTE_POSITION, !((GetAbsolutePositionInfo() & HtmlCommandInfo.Checked) != 0));
			SynchronizeSelection();
			if (_type == HtmlSelectionType.ElementSelection) 
			{
				foreach (Interop.IHTMLElement elem in _items) 
				{
					elem.GetStyle().SetZIndex(++_maxZIndex);
				}
			}
		}

        /// <summary>
		/// Bring currently selected elements to front by changing the z.order in a layered document
		/// </summary>
		public void BringToFront()
        {
			SynchronizeSelection();
            if (SelectionType == HtmlSelectionType.ElementSelection)
            {
                if (_items.Count > 1)
                {
                    BringToFront(_items);
                }
                else
                {
                    if (_items.Count == 1)
                    {
                        BringToFront(Editor.GenericElementFactory.CreateElement(_items[0]) as IElement);
                    }
                }
            }
        }

        /// <summary>
		/// Bring the given element to front by changing the z.order in a layered document
		/// </summary>
        /// <param name="element">The element which has to bring to front.</param>
		public void BringToFront(IElement element)
        {
            SynchronizeSelection();
            ArrayList items = new ArrayList(new Interop.IHTMLElement[] { element.GetBaseElement() });
            BringToFront(items);
        }

		/// <summary>
		/// Bring element to front by changing the z.order in a layered document
		/// </summary>
        private void BringToFront(IList items)
        {
            if (items.Count > 1)
            {
                int currentZIndex = _maxZIndex;
                int selectedItems = items.Count;
                Interop.IHTMLStyle[] interop_IHTMLStyles = new Interop.IHTMLStyle[(uint)selectedItems];
                int[] nums = new int[(uint)selectedItems];
                for (int i = 0; i < selectedItems; i++)
                {
                    Interop.IHTMLElement interop_IHTMLElement1 = (Interop.IHTMLElement)items[i];
                    interop_IHTMLStyles[i] = interop_IHTMLElement1.GetStyle();
                    nums[i] = (int)interop_IHTMLStyles[i].GetZIndex();
                    if (nums[i] < currentZIndex)
                    {
                        currentZIndex = nums[i];
                    }
                }
                int i2 = _maxZIndex + 1 - currentZIndex;
                IUndoStack batchedUndoUnit = htmlEditor.InternalUndoStack("Bring To Front");
                try
                {
                    for (int j2 = 0; j2 < selectedItems; j2++)
                    {
                        int k2 = i2 + nums[j2];
                        if (nums[j2] == _minZIndex)
                        {
                            _minZIndex++;
                        }
                        interop_IHTMLStyles[j2].SetZIndex(k2);
                        if (k2 > _maxZIndex)
                        {
                            _maxZIndex = k2;
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    batchedUndoUnit.Close();
                }
            }
            else
            {
                Interop.IHTMLElement interop_IHTMLElement2 = (Interop.IHTMLElement)items[0];
                object local = interop_IHTMLElement2.GetStyle().GetZIndex();
                if (local != null && !(local is DBNull))
                {
                    if ((int)local == _maxZIndex)
                    {
                        return;
                    }
                    if ((int)local == _minZIndex)
                    {
                        _minZIndex++;
                    }
                    if ((int)local > _maxZIndex)
                    {
                        _maxZIndex = (int)local;
                    }
                }
                interop_IHTMLElement2.GetStyle().SetZIndex(_maxZIndex++);
            }
        }

        /// <summary>
		/// Sends all selected items to the back.
		/// </summary>
        public void SendToBack()
        {
            SynchronizeSelection();
            if (_type == HtmlSelectionType.ElementSelection)
            {
                SendToBack(_items);
            }
        }

        /// <summary>
		/// Sends given item to the back.
		/// </summary>
        public void SendToBack(IElement element)
        {
            ArrayList items = new ArrayList(new IElement[] { element });
            SendToBack(items);
        }

		/// <summary>
		/// Sends all selected items to the back
		/// </summary>
        private void SendToBack(IList items)
        {
            if (items.Count > 1)
            {
                //We have to move all items to the back, and maintain their ordering, so
                //Find the maximum ZIndex in the group
                int max = _minZIndex;
                int count = items.Count;
                Interop.IHTMLStyle[] styles = new Interop.IHTMLStyle[count];
                int[] zIndexes = new int[count];
                for (int i = 0; i < count; i++)
                {
                    Interop.IHTMLElement elem = (Interop.IHTMLElement)items[i];
                    styles[i] = elem.GetStyle();
                    zIndexes[i] = (int)styles[i].GetZIndex();
                    if (zIndexes[i] > max)
                    {
                        max = zIndexes[i];
                    }
                }
                //Calculate how far the first element has to be moved in order to be in the back
                int offset = max - (_minZIndex - 1);
                IUndoStack unit = htmlEditor.InternalUndoStack("Send To Back");
                try
                {
                    //Then send all items in the selection that far back
                    for (int i = 0; i < count; i++)
                    {
                        int newPos = zIndexes[i] - offset;
                        if (zIndexes[i] == _maxZIndex)
                        {
                            _maxZIndex--;
                        }
                        styles[i].SetZIndex(newPos);
                        if (newPos < _minZIndex)
                        {
                            _minZIndex = newPos;
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    unit.Close();
                }
            }
            else
            {
                Interop.IHTMLElement elem = (Interop.IHTMLElement)items[0];
                object zIndex = elem.GetStyle().GetZIndex();
                if ((zIndex != null) && !(zIndex is DBNull))
                {
                    if ((int)zIndex == _minZIndex)
                    {
                        // if the element is already in the back do nothing.
                        return;
                    }

                    if ((int)zIndex == _maxZIndex)
                    {
                        _maxZIndex--;
                    }
                }
                elem.GetStyle().SetZIndex(--_minZIndex);
            }
        }

		# endregion
        
		# region Internal

		protected HtmlEditor Editor 
		{
			get 
			{
				return htmlEditor;
			}
		}

		/// <summary>
		/// For some internal procedures we need the current element only and we trust that
		/// the element is the last one picked up with the mouse. External callers use
		/// the synchronized version which is much more robust.
		/// </summary>
		/// <returns></returns>
		public IElement GetUnsynchronizedElement()
		{
			if (Elements.Count > 0)
			{
				IEnumerator ElEnum = Elements.GetEnumerator();
				ElEnum.MoveNext();
				return ElEnum.Current as IElement;
			}
			return null;
		}

		internal ICollection Items 
		{
			get 
			{
				return _items;
			}
		}

		/// <summary>
		/// Returns the MSHTML selection object (IHTMLTxtRange or IHTMLControlRange)
		/// Does not synchronize the selection!!!  Uses the selection from the last synchronization
		/// </summary>
		protected internal object MSHTMLSelection 
		{
			get 
			{
				return _mshtmlSelection;
			}
		}

		protected internal virtual void OnSelectionChanged(ElementCollection elements) 
		{
			if (SelectionChanged != null) 
			{
				SelectionChanged(this, new SelectionChangedEventArgs(elements));
			}
		}

		protected internal virtual void OnSelectionChanged(IElement element) 
		{
			if (SelectionChanged != null) 
			{
				ElementCollection elList = new ElementCollection();
				elList.Add(element);
				SelectionChanged(this, new SelectionChangedEventArgs(elList));
			}
		}

		/// <summary>
		/// This method returns a native element instance.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		internal Control CreateElementWrapper(Interop.IHTMLElement element) 
		{
			return htmlEditor.GenericElementFactory.CreateElement(element);
		}

		# endregion

		# region Events

		/// <summary>
		/// Fired if the selection has changed. 
		/// </summary>
		/// <remarks>
		/// If element selections are made which cannot recognized as selection the event may return only the body element.
        /// <para>The event fires usually before <see cref="IHtmlEditor.HtmlElementChanged">HtmlElementChanged</see> is being fired. In case of a deselection after
        /// that no longer any elements being selected the event is not fired again.</para>
		/// </remarks>
        /// <seealso cref="IHtmlEditor.HtmlElementChanged">HtmlElementChanged</seealso>
		public event SelectionChangedEventHandler SelectionChanged;

		# endregion

		# region Element Access 

		/// <summary>
		/// Number of selected elements or characters.
		/// </summary>
		/// <remarks>
		/// This property returns the number of selected elements, if the current <see cref="SelectionType"/>
		/// is <see cref="HtmlSelectionType.ElementSelection"/>, otherwise the current number of selected
		/// characters. If an element is selected, it has handles for move/resize and if a character is selected
		/// it becomes negative (by default, white font on black background) and accepts various commands.
		/// <para>
		/// Most formatting operations in the <see cref="ISelection"/> interface need a valid element selection.
		/// The ability of retrieving the text selection length does not implies a usual way to format text.
		/// Please refer to <see cref="ITextFormatting"/> for text formatting purposes.
		/// </para>
		/// <seealso cref="ITextFormatting"/>
		/// <seealso cref="SelectionType"/>
		/// <seealso cref="HtmlSelectionType"/>
		/// </remarks>
		public int Length 
		{
			get 
			{   
				SynchronizeSelection(); 
				return _selectionLength;
			}
		}

		/// <summary>
		/// Indicates if all items in the selection have the same parent element
		/// </summary>
		private bool SameParent 
		{
			get 
			{
				if (!_sameParentValid) 
				{
					IntPtr primaryParentElementPtr = Interop.NullIntPtr;

					foreach (Interop.IHTMLElement elem in _items) 
					{
						//Check if all items have the same parent by doing pointer equality
					    Interop.IHTMLElement parentElement = elem.GetParentElement();
						IntPtr parentElementPtr = Marshal.GetIUnknownForObject(parentElement);
						//If we haven't gotten a primary parent element (ie, this is the first time through the loop)
						//Remember what the this parent element is
						if (primaryParentElementPtr == Interop.NullIntPtr) 
						{
							primaryParentElementPtr = parentElementPtr;
						}
						else 
						{
							//Check the pointers
							if (primaryParentElementPtr != parentElementPtr) 
							{
								Marshal.Release(parentElementPtr);
								if (primaryParentElementPtr != Interop.NullIntPtr) 
								{
									Marshal.Release(primaryParentElementPtr);
								}
								_sameParentValid = false;
								return _sameParentValid;
							}
							Marshal.Release(parentElementPtr);
						}
					}
					if (primaryParentElementPtr != Interop.NullIntPtr) 
					{
						Marshal.Release(primaryParentElementPtr);
					}
					_sameParentValid = true;
				}
				return _sameParentValid;
			}
		}

		/// <summary>
		/// Get the current element after synchronizing the current selection.
		/// </summary>
		public IElement Element
		{
			get
			{
				this.SynchronizeSelection();
				return GetUnsynchronizedElement();
			}
		}

		/// <summary>
		/// Gets the list of selected elements as Native Type. Normally, if only one element is 
		/// selected, use the <see cref="Element"/> method instead.
		/// </summary>
		public ElementCollection Elements 
		{
            get
            {
                this.SynchronizeSelection();
                CreateElementCollection();
                return _elements;
            }
		}

        private void CreateElementCollection()
        {
            _elements = new ElementCollection();
            if (_items != null)
            {
                foreach (Interop.IHTMLElement element in _items)
                {
                    Control wrapper = CreateElementWrapper(element);
                    if (wrapper != null)
                    {
                        _elements.Add(wrapper);
                    }
                }
            }
        }

		/// <summary>
		/// Change the complete HTML including the outer HTML
		/// </summary>
		/// <param name="outerHtml"></param>
		public void SetOuterHtml(string outerHtml) 
		{
			if (Items.Count != 1 || ((Interop.IHTMLElement)_items[0]).GetTagName() == "BODY")
				return;
			else
				((Interop.IHTMLElement)_items[0]).SetOuterHTML(outerHtml);
		}

		/// <summary>
		/// Returns the outer HTML of the current selection. This includes the tag of he outermost element themselfes.
		/// </summary>
		/// <returns>HTML code as string</returns>
		public string GetOuterHtml() 
		{
			string outerHtml = String.Empty;
			this.SynchronizeSelection();
			try 
			{
				// HACK: Call this twice because, in the first call, MSHTML will call OnContentSave, which calls SetInnerHtml, but
				// the outer HTML it returns does not include that new inner HTML.
				outerHtml = ((Interop.IHTMLElement)_items[0]).GetOuterHTML();
				outerHtml = ((Interop.IHTMLElement)_items[0]).GetOuterHTML();
			}
			catch 
			{
			}
			return outerHtml;
		}

		/// <summary>
		/// Gets the list of direct children of the given element. The collection does not reflect the current 
		/// selection to make this function usable for all elements.
		/// </summary>
		/// <param name="o">IElement element</param>
		/// <returns><see cref="ElementCollection"/> of children, can be empty if there are no children or null if element was not recognized</returns>
		public ElementCollection GetChildHierarchy(IElement o)
		{
			SynchronizeSelection();
		    Interop.IHTMLElement current = o.GetBaseElement();
			if (current == null) 
			{
				return null;
			}
			ElementCollection childs = new ElementCollection();
		    Interop.IHTMLElementCollection children = (Interop.IHTMLElementCollection) current.GetChildren();
			for (int i = 0; i < children.GetLength(); i++)
			{
				childs.Add(CreateElementWrapper(children.Item(i, i) as Interop.IHTMLElement));
			}
			return childs;
		}

		/// <summary>
		/// Get the list of elements building the parent of the given element.
		/// </summary>
		/// <remarks>
        /// The collection does not reflect the current selection to make this function usable for all elements.
		/// </remarks>
		/// <param name="o">The Element as <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement">IElement</see> derived object.</param>
		/// <returns>The <see cref="GuruComponents.Netrix.WebEditing.Elements.ElementCollection">ElementCollection</see> of parent element or null, if there are no more parents.</returns>
		public ElementCollection GetParentHierarchy(IElement o) 
		{
			SynchronizeSelection();
		    Interop.IHTMLElement current = o.GetBaseElement();
			ElementCollection ancestors = new ElementCollection();
			if (current != null && o.TagName != "BODY") 
			{
				current = current.GetParentElement();
				while ((current != null) && (current.GetTagName().Equals("BODY") == false)) 
				{
					// take the shallow copy clone to prevent callers from killing the internal instance
					Control element = CreateElementWrapper(current);
                    if (element is IElement)
                    {
                        if (IsSelectableElement(element as IElement))
                        {
                            ancestors.Add(element);
                        }
                    }
					current = current.GetParentElement();
				}
				// Don't add the body tag to the hierarchy if we aren't in full document mode
				if (current != null) 
				{
                    Control element = CreateElementWrapper(current);
                    if (element is IElement)
                    {
                        if (IsSelectableElement(element as IElement))
                        {
                                ancestors.Add(element);
                        }
                    }
				}
			}
			return ancestors;
		}

		internal IElement GetCurrentElement()
		{
			if (Elements.Count > 0)
			{
				IEnumerator elements = Elements.GetEnumerator();
				if (elements.MoveNext())
				{
					return elements.Current as IElement;
				}
			} 
			return null;
		}

		/// <summary>
		/// Determines if the element can have handles, e.g. is a selectable element.
		/// </summary>
		/// <remarks>
		/// Typical elements like TABLE or IMG have handles. DIV can have handles if either width or height is defined.		
		/// </remarks>
		/// <param name="element">The element to check.</param>
		/// <returns>Returns <c>true</c> if the element can have handles.</returns>
		public bool IsSelectableElement(IElement element) 
		{
            if (element == null) return false;
			Interop.IHTMLControlElement ce = element.GetBaseElement() as Interop.IHTMLControlElement;

			if (ce == null || element is BodyElement || element is TableCellElement || element is TableRowElement)
				return false;
			else
				return true;
		}


		/// <summary>
		/// Checks if the element is currently selected.
		/// </summary>
		/// <remarks>
		/// This methods returns always <c>False</c> if the element isn't a selectable block element or
        /// the element parameter is <c>null</c> (<c>Nothing</c> in VB.NET).
		/// One can check the selectable property by calling <see cref="IsSelectableElement"/> method.
		/// The method does not change the current selection.
		/// </remarks>
		/// <param name="element">The element to be checked.</param>
		/// <returns>Either <c>True</c> or <c>False</c> depending on current state.</returns>
        public bool IsSelected(IElement element)
        {
            if (element == null) return false;
            if (element.Unselectable) return false;
            object o = new object();
            lock (o)
            {
                //System.Diagnostics.Debug.WriteLine("============= IsSelected =============");
                try
                {
                    Interop.IHTMLElement2 el = element.GetBaseElement() as Interop.IHTMLElement2;
                    return isSelected(el);
                }
                catch (COMException)
                {
                    //Debug.WriteLine("============= COMException 2 =============");
                    return false;
                }
            }
        }

        private static bool isSelected(Interop.IHTMLElement2 el)
        {
            // we simply check the existence of either the topleft or bottomright handle
            Interop.IHTMLRect rect = el.GetBoundingClientRect();
            Rectangle area = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            Point p = area.Location;
            if (p.X > 5 && p.Y > 5)
            {
                p.Offset(-1, -1); // check topleft, if not outside visible area
            }
            else
            {
                p.Offset(area.Right + 5, area.Bottom + 5); // check bottom right
            }

            string c;
            try
            {

                c = el.ComponentFromPoint(p.X, p.Y);
            }
            catch
            {
                //Debug.WriteLine("============= COMException 1 =============");
                return false;
            }
            if (c != null && c.StartsWith("handle"))
                return true;
            else
                return false;
        }

		# endregion

		# region Selection Access
		
		/// <summary>
		/// Indicates if the current selection can be used to assign new values using any other operation.
		/// </summary>
		public bool CanMatchSize 
		{
			get 
			{
				SynchronizeSelection();
				if (_items.Count < 2) 
				{
					return false;
				}
				if (_type == HtmlSelectionType.ElementSelection) 
				{
					foreach (Interop.IHTMLElement elem in _items) 
					{
						//Then check if none of them are locked
						if (IsElementLocked(elem)) 
						{
							return false;
						}
					}
					//Then check if they all have the same parent
					if (!SameParent) 
					{
						return false;
					}
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Returns the text contained in the selection if there is a text selection.
		/// </summary>
        /// <remarks>HTML tags are stripped out. If the containing text is an selectable element this property contains the element's 
        /// content. Child elements such as formatting tags are stripped out as well.</remarks>
		public string Text 
		{
			get 
			{
				SynchronizeSelection();
                if ((_type & HtmlSelectionType.TextSelection) == HtmlSelectionType.TextSelection) 
				{
					return _text;
				}
				return null;
			}
		}

		/// <summary>
		/// Returns the html text contained in the selection if there is a text selection.
		/// </summary>
        /// <remarks> The html tags are not stripped out. See <see cref="Text"/> for a version which strips tags.</remarks>
		public string Html
		{
			get 
			{
				SynchronizeSelection();
                if ((_type & HtmlSelectionType.TextSelection) == HtmlSelectionType.TextSelection) 
				{
					return _html;
				}
				return null;
			}
		}

		/// <summary>
		/// This propertys returns true, if the control has currently selected text in it.
		/// </summary>
		public bool HasTextSelection
		{
			get
			{
                return ((SelectionType & HtmlSelectionType.TextSelection) == HtmlSelectionType.TextSelection);
			}
		}

		/// <summary>
		/// The type of the selection.
		/// </summary>
        /// <remarks>Can be none (Empty), text or html (Text </remarks>
		public HtmlSelectionType SelectionType 
		{
			get 
			{
				SynchronizeSelection();
				return _type;
			}
		}

        private void GetAllElements(Interop.IHTMLElement startElement, ref ElementCollection childCollection)
        {
            Interop.IHTMLElementCollection coll = (Interop.IHTMLElementCollection) startElement.GetChildren();
            if (coll != null && coll.GetLength() > 0)
            {
                for (int i = 0; i < coll.GetLength(); i++)
                {
                    Interop.IHTMLElement item = (Interop.IHTMLElement) coll.Item(i, i);
                    childCollection.Add(htmlEditor.GenericElementFactory.CreateElement(item));
                    GetAllElements(item, ref childCollection);
                }
            }
        }

        /// <summary>
        /// Selects all elements in the document if they are absolutely positioned.
        /// </summary>
        public bool SelectAll()
        {
            if (SelectionChanging != null)
            {
                SelectionChanging(this, EventArgs.Empty);
            }
            try
            {
                Interop.IHTMLElement body = htmlEditor.GetBodyThreadSafe(false) as Interop.IHTMLElement;
                ElementCollection childCollection = new ElementCollection();
                this.GetAllElements(body, ref childCollection);
                if (childCollection.Count > 0)
                {
                    Interop.IHTMLTextContainer container = body as Interop.IHTMLTextContainer;
                    object controlRange = container.createControlRange();
                    Interop.IHTMLControlRange htmlControlRange = controlRange as Interop.IHTMLControlRange;
                    if (htmlControlRange == null)
                    {
                        return false;
                    }
                    foreach (Element ielement in childCollection)
                    {
                        Interop.IHTMLControlElement ce = ielement.GetBaseElement() as Interop.IHTMLControlElement;
                        if (ce != null)
                        {
                            if (IsElement2DPositioned((Interop.IHTMLElement)ce))
                            {
                                htmlControlRange.add(ce);
                            }
                            // TODO: Make configurable
                        }
                    }
                    htmlControlRange.select();
                }
                return true;
            }
            finally
            {
                SynchronizeSelection();
            }
        }

        /// <summary>
        /// Searches for an element and returns true, if the element exists within the page. The element remains selected.
        /// </summary>
        /// <param name="element">The element one is looking for.</param>
        /// <returns>Returns <c>true</c> if the element is within the page and was selected.</returns>
        public bool SelectElement(IElement element) 
        {
            ArrayList list = new ArrayList(1);
            list.Add(element);
            return SelectElements(list);
        }

        /// <summary>
        /// Searches for elements and returns true, if the element exists within the page. The elements remain selected.
        /// </summary>
        /// <param name="elements">The elements one is looking for.</param>
        /// <returns>Returns <c>true</c> if all elements are within the page and were selected.</returns>
        public bool SelectElements(ICollection elements)
        {
            if (SelectionChanging != null)
            {
                SelectionChanging(this, EventArgs.Empty);
            }
            try
            {
                if (htmlEditor.ThreadSafety)
                {
                    return InvokeSelectElements(elements);
                }
                else
                {
                    return SelectElementsThreadSafe(elements);
                }
            }
            finally
            {
                SynchronizeSelection();
            }
        }

        private delegate bool SelectElementsDelegate(ICollection elements);

        private bool InvokeSelectElements(ICollection elements)
        {
            if (htmlEditor.InvokeRequired)
            {
                SelectElementsDelegate d = new SelectElementsDelegate(InvokeSelectElements);
                return Convert.ToBoolean(htmlEditor.Invoke(d, elements));
            }
            else
            {
                return SelectElementsThreadSafe(elements);
            }
        }

        private bool SelectElementsThreadSafe(ICollection elements) 
        {
            Interop.IHTMLElement body = htmlEditor.GetBodyThreadSafe(false) as Interop.IHTMLElement;
            Interop.IHTMLTextContainer container = body as Interop.IHTMLTextContainer;
            object controlRange = container.createControlRange();
            Interop.IHTMLControlRange htmlControlRange = controlRange as Interop.IHTMLControlRange;
            if (htmlControlRange == null) 
            {
                return false;
            }
            Interop.IHTMLControlRange2 htmlControlRange2 = controlRange as Interop.IHTMLControlRange2;
            if (htmlControlRange2 == null) 
            {
                return false;
            }
            int hr = 0;
            foreach (IElement o in elements) 
            {
                if (o == null) continue;
                Interop.IHTMLElement element = o.GetBaseElement();
                if (element == null) continue;
                hr = htmlControlRange2.addElement(element);
                if (hr != Interop.S_OK) 
                {
                    break;
                }
            }
            if (hr == Interop.S_OK) 
            {
                //If it succeeded, simply select the control range
                htmlControlRange.select();
            }
            else 
            {
                // elements like DIV and SPAN, w/o layout, cannot be added to a control selection.
                Interop.IHtmlBodyElement bodyElement = (Interop.IHtmlBodyElement)body;
                Interop.IHTMLTxtRange textRange = bodyElement.createTextRange();
                if (textRange != null) 
                {
                    foreach (IElement o in elements) 
                    {
                        try 
                        {
                            Interop.IHTMLElement element = o.GetBaseElement();
                            if (element == null) 
                            {
                                return false;
                            }
                            textRange.MoveToElementText(element);
                        }
                        catch 
                        {
                        }
                    }
                    textRange.Select();
                }
            }
            return true;
        }

        /// <summary>
        /// Removes the selection the user has made but does not delete the content.
        /// </summary>
        /// <remarks>
        /// This method works only with a block element selection in 2D-mode. Use the text formatting 
        /// features to select/deselect the selection of text.
        /// </remarks>
        public void ClearSelection() 
        {
            Interop.IHTMLSelectionObject selectionObj = htmlEditor.GetActiveDocument(false).GetSelection();
            try 
            {
                selectionObj.Empty();
            }
            finally
            { 
                SynchronizeSelection();
            }
        }

        /// <summary>
        /// Removes the selected elements and all its content.
        /// </summary>
        /// <remarks>
        /// This method works only with a block element selection in 2D-mode and only if block elements are
        /// selected. In case of text selection the method ignores the action.
        /// </remarks>
        public void DeleteSelection() 
        {
            Interop.IHTMLSelectionObject selectionObj = htmlEditor.GetActiveDocument(false).GetSelection();
            try 
            {
                if (selectionObj.GetSelectionType().Equals("Control"))
                {
                    selectionObj.Clear();
                }
            }
            finally
            { 
                SynchronizeSelection();
            }
        }

        /// <summary>
        /// Synchronizes the selection state held in this object with the selection state in MSHTML.
        /// </summary>
        /// <remarks>
        /// This method fires the <see cref="SelectionChanged"/> event if the current selection is different from the previous one.
        /// <para>
        /// This method recognizes only standard HTML elements. It may fail for VML, XML, or other extended elements, 
        /// which might be selectable, indeed. Instead using the element collection directly it's recommended to check the
        /// existence of handles (e.g. the selected state) directly by using <see cref="IsSelected"/> property.
        /// </para>
        /// </remarks>
        /// <returns><c>True</c> if the selection has changed and the event was fired.</returns>
        public bool SynchronizeSelection() 
        {
            // Get the selection object from the MSHTML document
            Interop.IHTMLSelectionObject selectionObj = htmlEditor.GetActiveDocument(false).GetSelection();
            //Get the current selection from that selection object
            object currentSelection = null;
            try 
            {                
                // first try to look into the controlrange, if any exists                
                if (selectionObj.GetSelectionType().Equals("Control"))
                {
                    _type = HtmlSelectionType.ElementSelection;
                    currentSelection = selectionObj.CreateRange();
                    Interop.IHTMLControlRange cc = currentSelection as Interop.IHTMLControlRange;
                    if (cc != null && cc.length > 0)
                    {
                        _items = new List<Interop.IHTMLElement>();
                        for (int i = 0; i < cc.length; i++)
                        {
                            _items.Add(cc.item(i));
                        }
                        return FireSelectionChangedEvent(false);
                    }
                }
            }
            catch 
            { 
                return false;
            }
            _oldItems = _items;
            HtmlSelectionType oldType = _type;
            int oldLength = _selectionLength;
            //Default to an empty selection
            _type = HtmlSelectionType.Empty;
            _selectionLength = 0;
            try {
                currentSelection = selectionObj.CreateRange();
            } catch { }
            if (currentSelection != null) 
            {
                _mshtmlSelection = currentSelection;
                _items = new List<Interop.IHTMLElement>();
                //If it's a text selection
                if (currentSelection is Interop.IHTMLTxtRange) 
                {
                    Interop.IHTMLTxtRange textRange = (Interop.IHTMLTxtRange) currentSelection;
                    Interop.IHTMLElement parentElement = textRange.ParentElement();
                    // If the document is in full document mode or we're selecting a non-body tag, allow it to select
                    // otherwise, leave the selection as empty (since we don't want the body tag to be selectable on an ASP.NET
                    // User Control
					if (IsSelectableElement(htmlEditor.GenericElementFactory.CreateElement(parentElement) as IElement)) 
					{
						if (parentElement != null) 
						{
							_items.Add(parentElement);
						}
						_type = HtmlSelectionType.ElementSelection;
					} 
                    // Try getting text even if it's primary an element selection
                    if (parentElement != null)
                    {
                        _text = textRange.GetText();
                        if (_text != null)
                        {
                            _selectionLength = _text.Length;
                            _type |= HtmlSelectionType.TextSelection;
                            _html = textRange.GetHtmlText();
                        }
                        else
                        {
                            _selectionLength = 0;
                        }
                    }
                }
                    //If it's a control selection
                else if (currentSelection is Interop.IHTMLControlRange) 
                {
                    Interop.IHTMLControlRange controlRange = (Interop.IHTMLControlRange) currentSelection;
                    int selectedCount = controlRange.length;
                    //Add all elements selected
                    if (selectedCount > 0) 
                    {
                        _type = HtmlSelectionType.ElementSelection;
                        for (int i = 0; i < selectedCount; i++) 
                        {
                            Interop.IHTMLElement currentElement = controlRange.item(i);
                            _items.Add(currentElement);
                        }
                        _selectionLength = selectedCount;
                    }
                }
            }
            _sameParentValid = false;
            return FireSelectionChangedEvent((_type != oldType) || (_selectionLength != oldLength));
        }

        private bool FireSelectionChangedEvent(bool forceChange)
        {
            //Now check if there was a change of selection
            //If the two selections have different lengths, then the selection has changed
            if (_items != null && _oldItems != null && _oldItems.Count == _items.Count)
            {
                //If the two selections have a different element, then the selection has changed
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i] != _oldItems[i])
                    {
                        forceChange = true;
                        break;
                    }
                }
            }
            if (_items == null || _oldItems == null || _oldItems.Count != _items.Count)
            {
                forceChange = true;
            }
            _oldItems = _items;
            if (forceChange && this.htmlEditor.IsReady)
            {
                CreateElementCollection();
                // finally, if anything was changed, fire the event and read the final caret position, when the selection
                // is in a table and the table designer is on, otherwise the current element is the correct one
                OnSelectionChanged(_elements); // don't use property, will result in stack overflow
                //Set _elements to null so no one can retrieve a dirty copy of the last selection
                _elements = null;
                return true;
            }
            return false;
        }

		# endregion

		# region Wrap
		/// <summary>
		/// Indicates if the current selection can be wrapped in HTML tags.
		/// </summary>
		[Obsolete("This method is depreciated. Use same method from HtmlDocument class instead (using HtmlEditor.Document property).")]
        public bool CanWrapSelection 
		{
			get 
			{
				if ((_selectionLength != 0) && (SelectionType == HtmlSelectionType.TextSelection)) 
				{
					return true;
				}
				return false;
			}
		}

        /// <summary>
        /// Creates a new tag around the current HTML selection.
        /// </summary>
        /// <remarks>
        /// This method does not check
        /// for the validity of the new HTML. If the generated HTML is not valid the call causes
        /// unexpected results.
        /// </remarks>
        /// <param name="tag">The tag name which is used to build the tag, e.g. "DIV" for a division.</param>        
        [Obsolete("This method is depreciated. Use same method from HtmlDocument class instead (using HtmlEditor.Document property).")]
        public void WrapSelection(string tag) 
        {
            WrapSelection(tag, null);
        }

        /// <summary>
        /// Creates a new tag around the current HTML selection.
        /// </summary>
        /// <remarks>
        /// The textselection method used internally is "clever" in some ways and tries to keep the HTML valid
        /// and set the new element as expected. This can result in element duplication.
        /// <para>
        /// This HTML <c>&lt;b>stro[ng&lt;/b>&lt;i>ita]lic&lt;</c>, where [] covers the selected text range, 
        /// would become <c>&lt;b>stro&lt;/b>&lt;span>&lt;b>ng&lt;/b>&lt;i>ita&lt;/span>lic&lt;</c> after using the
        /// method with the tag SPAN. As you can see, the &lt;b> tag is duplicated to keep the HTML valid.         
        /// </para>
        /// After the selection has been changed, the method removes the selection and sets the caret to the
        /// end of range.
        /// </remarks>
        /// <param name="tag">The tag name which is used to build the tag, e.g. "DIV" for a division.</param>
        /// <param name="attributes">A collection of attributes, expected as name/value pairs. Can be <c>null</c> (<c>Nothing</c> in VB.NET) if no attributes needed.</param>
        [Obsolete("This method is depreciated. Use same method from HtmlDocument class instead (using HtmlEditor.Document property).")]
        public void WrapSelection(string tag, IDictionary attributes) 
        {
            htmlEditor.Document.WrapSelection(tag, attributes);
        }

        /// <summary>
        /// Takes the selection and put an &lt;div&gt; around it.
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to insert elements around selected text. If there is no selection the
        /// element will still be inserted, but it remains invisible if no styles are applied. The host application
        /// should be aware of inserting empty elements and should provide some help through the user interface
        /// to make such elements visible or reachable.
        /// </remarks>
        [Obsolete("This method is depreciated. Use same method from HtmlDocument class instead (using HtmlEditor.Document property).")]
        public void WrapSelectionInDiv() 
        {
            WrapSelection("div");
        }

        /// <summary>
        /// Takes the selection and put an &lt;span&gt; around it.
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to insert elements around selected text. If there is no selection the
        /// element will still be inserted, but it remains invisible if no styles are applied. The host application
        /// should be aware of inserting empty elements and should provide some help through the user interface
        /// to make such elements visible or reachable.
        /// </remarks>
        [Obsolete("This method is depreciated. Use same method from HtmlDocument class instead (using HtmlEditor.Document property).")]
        public void WrapSelectionInSpan() 
        {
            WrapSelection("span");
        }

        /// <summary>
        /// Takes the selection and put an &lt;blockquote&gt; around it.
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to insert elements around selected text. If there is no selection the
        /// element will still be inserted, but it remains invisible if no styles are applied. The host application
        /// should be aware of inserting empty elements and should provide some help through the user interface
        /// to make such elements visible or reachable.
        /// </remarks>
        [Obsolete("This method is depreciated. Use same method from HtmlDocument class instead (using HtmlEditor.Document property).")]
        public void WrapSelectionInBlockQuote() 
        {
            WrapSelection("blockquote");
        }

        /// <summary>
        /// Takes the selection and put an &lt;hyperlink&gt; around it.
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to insert elements around selected text. If there is no selection the
        /// element will still be inserted, but it remains invisible if no styles are applied. The host application
        /// should be aware of inserting empty elements and should provide some help through the user interface
        /// to make such elements visible or reachable.
        /// </remarks>
        [Obsolete("This method is depreciated. Use same method from HtmlDocument class instead (using HtmlEditor.Document property).")]
        public void WrapSelectionInHyperlink(string url) 
        {
            htmlEditor.Exec(Interop.IDM.HYPERLINK, url);
        }

        
		# endregion

		# region Remove Methods

		/// <summary>
        /// Remove a hyperlink where the caret is placed in.
        /// </summary>
        /// <remarks>
        /// The text which the hyperlink contains will not be removed.
        /// </remarks>
        public void RemoveHyperlink() 
        {
            htmlEditor.Exec(Interop.IDM.UNLINK);
        }

		/// <summary>
		/// Indicates if the current selection can have it's hyperlink removed.
		/// </summary>
		/// <remarks>
		/// Use this property to update the user interface.
		/// </remarks>
        public bool CanRemoveHyperlink 
		{
			get 
			{
				return htmlEditor.IsCommandEnabled(Interop.IDM.UNLINK);
			}
		}

		/// <summary>
		/// This method removes the current element.</summary>
		/// <remarks>
		/// This method remove the current element (as shown in the property grid as result of the last 
		/// HtmlElementChanged event). Normally this is where the caret is located. If the parameter 
		/// preserveContent is true the content (text, child elements, …) is preserved. Removing an 
		/// element will be reflected in the DOM immediately. Undo works as expected. Redo can throw 
		/// an exception, if the removed element is added twice or more and the DOM does not allow this 
		/// element to be nested at the current location. It is recommended to disable Redo after calling 
		/// RemoveCurrentElement for at least one keystroke to avoid adding nested elements, which 
		/// doesn’t make sense in HTML under most circumstances.
		/// </remarks>
		/// <param name="preserveContent">True if the content and children should not be deleted.</param>
		public void RemoveCurrentElement(bool preserveContent)
		{
		    Interop.IHTMLDOMNode node = (Interop.IHTMLDOMNode) ((Element)this.htmlEditor.Selection.Element).GetBaseElement();
			node.removeNode(!preserveContent);
		}

        # endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (this._items != null)
                this._items.Clear();
            if (this._elements != null)
                this._elements.Clear();
        }

        #endregion

        #region ISelectionService Member

        public object PrimarySelection
        {
            get
            {
                EnsureSelection();
                if (_elements == null || _elements.Count == 0)
                {
                    return null;
                }
                IEnumerator iEnumerator = _elements.GetEnumerator();
                try
                {
                    if (iEnumerator.MoveNext())
                    {
                        object local2 = iEnumerator.Current;
                        return local2;
                    }
                }
                finally
                {
                    IDisposable iDisposable = iEnumerator as IDisposable;
                    if (iDisposable != null)
                    {
                        iDisposable.Dispose();
                    }
                }
                return null;
            }
        }

        public event System.EventHandler SelectionChanging;

        public bool GetComponentSelected(object component)
        {
            EnsureSelection();
            if (_elements == null || _elements.Count == 0)
            {
                return false;
            }
            IEnumerator iEnumerator = _elements.GetEnumerator();
            try
            {
                while (iEnumerator.MoveNext())
                {
                    if (iEnumerator.Current == component)
                    {
                        bool flag = true;
                        return flag;
                    }
                }
            }
            finally
            {
                IDisposable iDisposable = iEnumerator as IDisposable;
                if (iDisposable != null)
                {
                    iDisposable.Dispose();
                }
            }
            return false;
        }

        /// <summary>
        /// Set the given components as selected.
        /// </summary>
        /// <param name="components"></param>
        /// <param name="selectionType"></param>
        public void SetSelectedComponents(ICollection components, System.ComponentModel.Design.SelectionTypes selectionType)
        {
            if (selectionType != SelectionTypes.Normal && selectionType != SelectionTypes.Click && selectionType != SelectionTypes.Auto)
            {
                throw new ArgumentOutOfRangeException("selectionType");
            }
            if (SelectionChanging != null)
            {
                SelectionChanging(this, EventArgs.Empty);
            }
            _elements = new ElementCollection();
            _elements.AddRange(components);
            FireSelectionChangedEvent(false);
        }

        void System.ComponentModel.Design.ISelectionService.SetSelectedComponents(ICollection components)
        {
            SetSelectedComponents(components, SelectionTypes.Normal);
        }

        /// <summary>
        /// Get the selected components.
        /// </summary>
        /// <returns></returns>
        public ICollection GetSelectedComponents()
        {
            EnsureSelection();
            return _elements as ICollection;
        }

        /// <summary>
        /// Get the number of selected components.
        /// </summary>
        public int SelectionCount
        {
            get
            {
                return _elements.Count;
            }
        }

        /// <summary>
        /// Hook up the regular designer based event to the netrix base implementation.
        /// </summary>
        event System.EventHandler ISelectionService.SelectionChanged
        {
            add
            {
                this.SelectionChanged += new SelectionChangedEventHandler(value);
            }
            remove
            {
                this.SelectionChanged -= new SelectionChangedEventHandler(value);
            }
        }

        #endregion

        private void EnsureSelection()
        {
            if ((_elements == null || _elements.Count == 0))
            {
                IDesignerHost designerHost = (IDesignerHost) htmlEditor.ServiceProvider.GetService(typeof(IDesignerHost));
                if (designerHost.RootComponent != null)
                {
                    _elements = new ElementCollection();
                    _elements.Add(designerHost.RootComponent);
                }
            }
        }
    }
}