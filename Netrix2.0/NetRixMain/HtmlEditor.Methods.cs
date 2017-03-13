using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GuruComponents.Netrix
{

    public partial class HtmlEditor
    {
        # region HTML Element

        # region GetElementsByTagName() method
        /// <summary>
        /// Returns an array of element objects which contains all element with the given tag name. 
        /// </summary>
        /// <remarks>
        /// If no such elements the method will return null.
        /// </remarks>
        /// <param name="tagName">The name of the specific tag which the method should search.</param>
        /// <returns>The <see cref="ElementCollection"/> of <see cref="IElement"/> objects or null, if no elements found.</returns>
        public ElementCollection GetElementsByTagName(string tagName)
        {
            if (ThreadSafety)
            {
                return InvokeGetElementsByTagName(tagName);
            }
            else
            {
                return GetElementsByTagNameInternal(tagName);
            }
        }

        private delegate ElementCollection GetElementsByTagNameDelegate(string name);

        private ElementCollection InvokeGetElementsByTagName(string name)
        {
            if (this.InvokeRequired)
            {
                GetElementsByTagNameDelegate d = new GetElementsByTagNameDelegate(InvokeGetElementsByTagName);
                return Invoke(d, new object[] { name }) as ElementCollection;
            }
            else
            {
                return GetElementsByTagNameInternal(name);
            }
        }

        private ElementCollection GetElementsByTagNameInternal(string name)
        {
            try
            {
                Interop.IHTMLDocument3 doc3 = (Interop.IHTMLDocument3)this.GetActiveDocument(false);
                if (doc3 != null)
                {
                    Interop.IHTMLElementCollection eColl = doc3.GetElementsByTagName(name);
                    if (eColl.GetLength() > 0)
                    {
                        ElementCollection ielements = new ElementCollection();
                        for (int i = 0; i < eColl.GetLength(); i++)
                        {
                            IElement element = GenericElementFactory.CreateElement((Interop.IHTMLElement)eColl.Item(i, i)) as IElement;
                            ielements.Add(element);
                        }
                        return ielements;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        # endregion GetElementsByTagName() method

        # region GetElementById() method
        /// <summary>
        /// Returns the element with the given Id as native element object.
        /// </summary>
        /// <param name="Id">The ID value which the method should search for.</param>
        /// <returns>The element, if found or <c>null</c>, if there is no such element.</returns>
        public IElement GetElementById(string Id)
        {
            try
            {
                if (ThreadSafety)
                {
                    return InvokeGetElementById(Id);
                }
                else
                {
                    return GetElementByIdInternal(Id);
                }
            }
            catch
            {
                return null;
            }
        }

        private delegate IElement GetElementByIdDelegate(string id);

        private IElement InvokeGetElementById(string id)
        {
            if (this.InvokeRequired)
            {
                GetElementByIdDelegate d = new GetElementByIdDelegate(InvokeGetElementById);
                return Invoke(d, new object[] { id }) as IElement;
            }
            else
            {
                return GetElementByIdInternal(id);
            }
        }

        private IElement GetElementByIdInternal(string id)
        {
            try
            {
                Interop.IHTMLElement element = null;
                if (this.GetActiveDocument(false) != null)
                {
                    element = ((Interop.IHTMLDocument3)this.GetActiveDocument(false)).GetElementById(id) as Interop.IHTMLElement;
                }
                if (element != null)
                {
                    return GenericElementFactory.CreateElement((Interop.IHTMLElement)element) as IElement;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        # endregion GetElementById() method

        # region GetAllElements() method
        /// <summary>
        /// Returns all element of the current page.
        /// </summary>
        /// <returns>The collection of all elements.</returns>
        public ElementCollection GetAllElements()
        {
            try
            {
                if (ThreadSafety)
                {
                    return InvokeGetAllElements();
                }
                else
                {
                    return InvokeGetAllElements();
                }
            }
            catch
            {
                return null;
            }
        }

        private delegate ElementCollection GetAllElementsDelegate();

        private ElementCollection InvokeGetAllElements()
        {
            if (this.InvokeRequired)
            {
                GetAllElementsDelegate d = new GetAllElementsDelegate(InvokeGetAllElements);
                return Invoke(d) as ElementCollection;
            }
            else
            {
                return GetAllElementsInternal();
            }
        }

        private ElementCollection GetAllElementsInternal()
        {
            try
            {
                Interop.IHTMLElementCollection elements = null;
                if (this.GetActiveDocument(false) != null)
                {
                    elements = this.GetActiveDocument(false).GetAll();
                }
                if (elements != null && elements.GetLength() > 0 )
                {
                    ElementCollection ec = new ElementCollection();
                    for ( int i = 0; i < elements.GetLength(); i++)
                    {
                        ec.Add(GenericElementFactory.CreateElement((Interop.IHTMLElement) elements.Item(i, i)));
                    }
                    return ec;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        # endregion GetElementById() method

        # region GetBodyElement() method
        /// <summary>
        /// Returns the current BodyElement object from the document.
        /// </summary>
        /// <returns>BodyElement, if any or null if no body is present or document is not ready.</returns>
        public IElement GetBodyElement()
        {
            if (ThreadSafety)
            {
                return InvokeGetBodyElement();
            }
            else
            {
                return GetBodyElementInternal();
            }
        }

        private delegate IElement GetBodyElementDelegate();

        private IElement InvokeGetBodyElement()
        {
            if (this.InvokeRequired)
            {
                GetBodyElementDelegate d = new GetBodyElementDelegate(InvokeGetBodyElement);
                return Invoke(d, null) as IElement;
            }
            else
            {
                return GetBodyElementInternal();
            }
        }

        private IElement GetBodyElementInternal()
        {
            try
            {
                Interop.IHTMLElement element = GetBodyThreadSafe(false) as Interop.IHTMLElement;
                if (element != null)
                {
                    switch (element.GetTagName())
                    {
                        case "BODY":
                            BodyElement body = (BodyElement)GenericElementFactory.CreateElement(element);
                            return body;
                        case "FRAMESET":
                            FrameSetElement fs = (FrameSetElement)GenericElementFactory.CreateElement(element);
                            return fs;
                        case "FRAME":
                            FrameElement frame = (FrameElement)GenericElementFactory.CreateElement(element);
                            return frame;
                    }
                }
            }
            catch { };
            return null;
        }

        # endregion GetBodyElement() method

        # region ThreadSafeBody

        internal Interop.IHtmlBodyElement GetBodyThreadSafe(bool baseDocument)
        {
            if (ThreadSafety)
            {
                return InvokeGetBodyThreadSafe(baseDocument) as Interop.IHtmlBodyElement;
            }
            else
            {
                return this.GetActiveDocument(baseDocument).GetBody() as Interop.IHtmlBodyElement;
            }
        }

        private delegate Interop.IHTMLElement GetBodyThreadSafeDelegate(bool baseDocument);

        private Interop.IHTMLElement InvokeGetBodyThreadSafe(bool baseDocument)
        {
            if (this.InvokeRequired)
            {
                GetBodyThreadSafeDelegate d = new GetBodyThreadSafeDelegate(InvokeGetBodyThreadSafe);
                return Invoke(d, new object[] { baseDocument }) as Interop.IHTMLElement;
            }
            else
            {
                Interop.IHTMLDocument2 doc2 = this.GetActiveDocument(baseDocument);
                return (doc2 != null) ? doc2.GetBody() : null;
            }
        }

        # endregion

        # endregion

        /// <summary>
        /// Print the current content to the given graphics device
        /// </summary>
        /// <remarks>
        /// EXPERIMENTAL.<br/>
        /// Use this method to create an image (screenshot of the whole page).
        /// </remarks>
        /// <param name="gr">Device the method is creating the output on.</param>
        public void PrintToHdc(Graphics gr)
        {            
            Interop.IHTMLElement2 body = (Interop.IHTMLElement2)GetActiveDocument(false).GetBody();
            //bool vScroll = (this.ScrollBars == RichTextBoxScrollBars.Vertical ||
            //                   this.ScrollBars == RichTextBoxScrollBars.Both);
            //bool hScroll = (this.ScrollBars == RichTextBoxScrollBars.Horizontal ||
            //                   this.ScrollBars == RichTextBoxScrollBars.Both);
            //Rectangle r =
            //    new Rectangle(0, 0, body.GetScrollWidth() - ((vScroll) ? 32 : 0),
            //                  body.GetScrollHeight() - ((hScroll) ? 32 : 0));
            Rectangle r = new Rectangle(0, 0, body.GetScrollWidth(), body.GetScrollHeight());
            MshtmlSite.ExpandView(r);
            Interop.IHTMLElementRender render = (Interop.IHTMLElementRender) body;            
            render.DrawToDC(gr.GetHdc());
            MshtmlSite.ParentResize();
        }
    }
}