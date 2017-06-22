using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using System.Web.UI;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.WebEditing.Elements;
using System.ComponentModel;
using System.Web.UI.Design;
using GuruComponents.Netrix.WebEditing.Elements.Html5;

# pragma warning disable 0618

namespace GuruComponents.Netrix
{
        /// <summary>
        /// This class wraps internal element objects into native one.
        /// </summary>
        /// <remarks>
        /// This class is widely used internally to wrap the COM interface related element objects directly
        /// into .NET native objects. The class provides an object cache, which helds all previously created 
        /// instances. The method <see cref="CreateElement">CreateElement</see>
        /// will call the <c>internal</c> constructor of each element class to instantiate the object.
        /// <para>
        /// Under normal circumstances a direct call to this classe is not appropriate. However, plug-ins may use
        /// the class to add there element types to the list of recognizable ones.
        /// </para>
        /// </remarks>
        public sealed class ElementFactory : IElementFactory
        {
            private Dictionary<string, Type> elementTypes;
            private IHtmlEditor htmlEditor;
            private DesignerHost designerHost;

            internal ElementFactory(IHtmlEditor htmlEditor)
            {
                this.htmlEditor = htmlEditor;
                ResetElementCache();
                BuildElements();
            }

            /// <summary>
            /// Clears the cache
            /// </summary>
            /// <remarks>
            /// Subsequent access to cleared elements leads to immediate recreation. However, attached
            /// event handlers and delegate methods get orphaned.
            /// </remarks>
            public void ResetElementCache()
            {
                if (htmlEditor.ServiceProvider != null)
                {
                    designerHost = (DesignerHost)htmlEditor.ServiceProvider.GetService(typeof(IDesignerHost));
                    if (designerHost != null)
                    {
                        foreach (IComponent component in designerHost.Container.Components)
                        {
                            if (component is IDisposable) ((IDisposable)component).Dispose();
                        }
                        designerHost.Clear();
                    }
                }
            }

            /// <summary>
            /// Let plug-ins register their element classes to make the control ready
            /// to handle non-HTML elements internally.
            /// </summary>
            /// <remarks>
            /// The element is recognized by alias and name, divided by colon (asp:button, html:img). The basic
            /// HTML elements have the alias html, even if the document loaded is not XHTML conform. For
            /// a HTML comment the name is "!" without an alias. Plug-Ins may define different name schemas.
            /// </remarks>
            /// <seealso cref="UnRegisterElement"/>
            /// <param name="elementName">Name of element being registered, including the alias (see remarks).</param>
            /// <param name="elementType">The type defined in an assembly, which is part of the app domain.</param>
            public void RegisterElement(string elementName, Type elementType)
            {
                //elementName = elementName.ToLower();
                if (!elementTypes.ContainsKey(elementName))
                {
                    elementTypes.Add(elementName, elementType);
                }
            }

            /// <summary>
            /// Removes an element from the list of registered types.
            /// </summary>
            /// <remarks>
            /// The element is recognized by alias and name, divided by colon (asp:button, html:img). The basic
            /// HTML elements have the alias html, even if the document loaded is not XHTML conform. For
            /// a HTML comment the name is "!" without an alias. Plug-Ins may define different name schemas.
            /// </remarks>
            /// <seealso cref="RegisterElement"/>
            /// <param name="elementName">The name of the element type, including the alias (see remarks).</param>
            public void UnRegisterElement(string elementName)
            {
                elementTypes.Remove(elementName.ToLower());
            }

            class TypeComparer : IEqualityComparer<string>
            {
                #region IEqualityComparer<string> Members

                public bool Equals(string x, string y)
                {
                    return (x.ToLower() == y.ToLower());
                }

                public int GetHashCode(string obj)
                {
                    return 0;
                }

                #endregion
            }

            /// <summary>
            /// Build elements look up table for HTML elements.
            /// </summary>
            private void BuildElements()
            {
                elementTypes = new Dictionary<string,Type>(new TypeComparer());
                elementTypes["html:!"] = typeof(CommentElement);
                elementTypes["html:a"] = typeof(AnchorElement);
                elementTypes["html:abbr"] = typeof(AbbreviationElement);
                elementTypes["html:address"] = typeof(AddressElement);
                elementTypes["html:acronym"] = typeof(AnchorElement);
                elementTypes["html:applet"] = typeof(AppletElement);
                elementTypes["html:area"] = typeof(AreaElement);
                elementTypes["html:article"] = typeof(ArticleElement); // 5
                elementTypes["html:aside"] = typeof(AsideElement); // 5
                elementTypes["html:audio"] = typeof(AudioElement); // 5
                elementTypes["html:base"] = typeof(BaseElement);
                elementTypes["html:bgsound"] = typeof(BgSoundElement);
                elementTypes["html:body"] = typeof(BodyElement);
                elementTypes["html:b"] = typeof(BoldElement);
                elementTypes["html:bdo"] = typeof(BdoElement);
                elementTypes["html:blockquote"] = typeof(BlockquoteElement);
                elementTypes["html:big"] = typeof(BigElement);
                elementTypes["html:br"] = typeof(BreakElement);
                elementTypes["html:canvas"] = typeof(CanvasElement);    // 5
                elementTypes["html:caption"] = typeof(CaptionElement);
                elementTypes["html:center"] = typeof(CenterElement);
                elementTypes["html:cite"] = typeof(CiteElement);
                elementTypes["html:code"] = typeof(CodeElement);
                elementTypes["html:command"] = typeof(CommandElement);    // 5
                elementTypes["html:col"] = typeof(ColElement);
                elementTypes["html:colgroup"] = typeof(ColgroupElement);                
                elementTypes["html:div"] = typeof(DivElement);
                elementTypes["html:dd"] = typeof(DefinitionListDefinitionElement);
                elementTypes["html:del"] = typeof(DeletedElement);
                elementTypes["html:dfn"] = typeof(DefinitionElement);
                elementTypes["html:dir"] = typeof(DirectoryElement);
                elementTypes["html:dl"] = typeof(DefinitionListEntryElement);
                elementTypes["html:dt"] = typeof(DefinitionListTermElement);
                elementTypes["html:em"] = typeof(EmphasisElement);
                elementTypes["html:embed"] = typeof(EmbedElement);
                elementTypes["html:figure"] = typeof(FigureElement);    // 5
                elementTypes["html:figcaption"] = typeof(FigCaptionElement);    // 5
                elementTypes["html:font"] = typeof(FontElement);
                elementTypes["html:footer"] = typeof(GuruComponents.Netrix.WebEditing.Elements.Html5.FooterElement);
                elementTypes["html:frame"] = typeof(FrameElement);
                elementTypes["html:frameset"] = typeof(FrameSetElement);
                elementTypes["html:hr"] = typeof(HorizontalRuleElement);
                elementTypes["html:h1"] = typeof(GuruComponents.Netrix.WebEditing.Elements.HeaderElement);
                elementTypes["html:h2"] = typeof(GuruComponents.Netrix.WebEditing.Elements.HeaderElement);
                elementTypes["html:h3"] = typeof(GuruComponents.Netrix.WebEditing.Elements.HeaderElement);
                elementTypes["html:h4"] = typeof(GuruComponents.Netrix.WebEditing.Elements.HeaderElement);
                elementTypes["html:h5"] = typeof(GuruComponents.Netrix.WebEditing.Elements.HeaderElement);
                elementTypes["html:h6"] = typeof(GuruComponents.Netrix.WebEditing.Elements.HeaderElement);
                elementTypes["html:head"] = typeof(HeadElement);
                elementTypes["html:header"] = typeof(GuruComponents.Netrix.WebEditing.Elements.Html5.HeaderElement);    // 5
                elementTypes["html:hgroup"] = typeof(GuruComponents.Netrix.WebEditing.Elements.Html5.HGroupElement);    // 5
                elementTypes["html:html"] = typeof(HtmlElement);
                elementTypes["html:i"] = typeof(ItalicElement);
                elementTypes["html:img"] = typeof(ImageElement);
                elementTypes["html:ins"] = typeof(InsertedElement);
                elementTypes["html:kbd"] = typeof(KeyboardElement);
                elementTypes["html:iframe"] = typeof(IFrameElement);
                elementTypes["html:label"] = typeof(LabelElement);            
                elementTypes["html:li"] = typeof(ListItemElement);
                elementTypes["html:link"] = typeof(LinkElement);
                elementTypes["html:label"] = typeof(LabelElement);
                elementTypes["html:mark"] = typeof(MarkElement);    // 5
                elementTypes["html:marquee"] = typeof(MarqueeElement);
                elementTypes["html:menu"] = typeof(MenuElement);
                elementTypes["html:meta"] = typeof(MetaElement);
                elementTypes["html:map"] = typeof(MapElement);
                elementTypes["html:nav"] = typeof(NavElement);    // 5
                elementTypes["html:noframes"] = typeof(NoframeElement);
                elementTypes["html:noscript"] = typeof(NoScriptElement);
                elementTypes["html:nobr"] = typeof(NoBreakElement);
                elementTypes["html:object"] = typeof(ObjectElement);
                elementTypes["html:ol"] = typeof(OrderedListElement);
                elementTypes["html:p"] = typeof(ParagraphElement);
                elementTypes["html:param"] = typeof(ParamElement);
                elementTypes["html:pre"] = typeof(PreformattedElement);
                elementTypes["html:small"] = typeof(SmallElement);
                elementTypes["html:span"] = typeof(SpanElement);
                elementTypes["html:script"] = typeof(ScriptElement);
                elementTypes["html:section"] = typeof(SectionElement);    // 5
                elementTypes["html:source"] = typeof(SourceElement);    // 5
                elementTypes["html:strike"] = typeof(StrikeElement);
                elementTypes["html:strong"] = typeof(StrongElement);
                elementTypes["html:style"] = typeof(StyleElement);
                elementTypes["html:sub"] = typeof(SubElement);
                elementTypes["html:sup"] = typeof(SupElement);
                elementTypes["html:table"] = typeof(TableElement);
                elementTypes["html:thead"] = typeof(TableHeadElement);
                elementTypes["html:tbody"] = typeof(TableBodyElement);
                elementTypes["html:tfoot"] = typeof(TableFootElement);
                elementTypes["html:time"] = typeof(TimeElement);    // 5
                elementTypes["html:title"] = typeof(TitleElement);
                elementTypes["html:tr"] = typeof(TableRowElement);
                elementTypes["html:td"] = typeof(TableCellElement);
                elementTypes["html:th"] = typeof(TableHeaderElement);
                elementTypes["html:tt"] = typeof(TeletypeElement);
                elementTypes["html:ul"] = typeof(UnorderedListElement);
                elementTypes["html:u"] = typeof(UnderlineElement);
                elementTypes["html:var"] = typeof(VariableElement);
                elementTypes["html:video"] = typeof(VideoElement);    // 5
                // Forms
                elementTypes["html:form"] = typeof(FormElement);
                elementTypes["html:fieldset"] = typeof(FieldSetElement);
                elementTypes["html:legend"] = typeof(LegendElement);			
                elementTypes["html:buttonelement"] = typeof(ButtonElement);
                elementTypes["html:button"] = typeof(InputButtonElement);
                elementTypes["html:text"] = typeof(InputTextElement);
                elementTypes["html:textarea"] = typeof(TextAreaElement);
                elementTypes["html:radio"] = typeof(InputRadioElement);
                elementTypes["html:file"] = typeof(InputFileElement);
                elementTypes["html:password"] = typeof(InputPasswordElement);
                elementTypes["html:checkbox"] = typeof(InputCheckboxElement);
                elementTypes["html:select"] = typeof(SelectElement);
                elementTypes["html:option"] = typeof(OptionElement);
                elementTypes["html:hidden"] = typeof(InputHiddenElement);
                elementTypes["html:submit"] = typeof(InputSubmitElement);
                elementTypes["html:reset"] = typeof(InputResetElement);
                elementTypes["html:image"] = typeof(InputImageElement);

            }

            /// <summary>
            /// Makes a association between an element and a native element class.
            /// </summary><remarks>
            /// In case of an already created object the object is returned instead of creating a new one.
            /// This class returns <see cref="System.Web.UI.Control">Control</see>, but all element classes internally
            /// used are using IElement, too. So in any case an internal HTML element object is being retrieved, it's
            /// possible to cast to IElement instead. In case of a plug-in this is not required, because plug-ins may handle
            /// their elements differently and such a cast is not appropriate. Consult the documentation of the plug-in to
            /// understand the requirements for object handling.
            /// </remarks>
            /// <param name="el">The element for that a native object is needed.</param>
            /// <returns>System.Web.UI.Control if creating or retrieving was successful, otherwise <c>null</c>. The caller is responsible to cast to the right class type.</returns>
            public Control CreateElement(Interop.IHTMLElement el)
            {
                if (el == null) return null;

                Type type = null;
                Control element = null;			
                // each element will be stored in a hashtable to increase performance, subsequent calls
                // to this factory will retrieve stored objects instead of creating new ones.
                string uniqueName;
                ((Interop.IHTMLUniqueName)el).uniqueID(out uniqueName);
                // look whether we have it already in the designer
                if (designerHost[uniqueName] is Control)
                {
                    Control returnEl = designerHost[uniqueName] as Control;
                    if (returnEl is IElement)
                    {
                        ((IElement)returnEl).HtmlEditor = this.htmlEditor;
                    }
                    return returnEl;
                }
                // if not, try plug-ins, and finally add
                try
                {
                    // get tag name
                    type = GetElementType(el);
                    ConstructorInfo cinfo;
                    if (type != null)
                    {
                        // default ctor
                        cinfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(Interop.IHTMLElement), typeof(IHtmlEditor) }, null);
                        // not found - get it from plug-in
                        if (cinfo == null)
                        {
                            // probably an plugin-driven control, plug-ins register their elements in designer host
                            foreach (IComponent icomp in designerHost.Container.Components)
                            {
                                IDesigner id = designerHost.GetDesigner(icomp);
                                if (id == null) continue;
                                if (id is ControlDesigner)
                                {
                                    IHtmlControlDesignerBehavior db = ((ControlDesigner)id).Behavior as IHtmlControlDesignerBehavior;
                                    if (db != null && db.DesignTimeElement.Equals(el))
                                    {
                                        element = icomp as Control;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // usually, we get and invoke here
                            element = (Control)cinfo.Invoke(new object[] { el, htmlEditor });
                        }
                        // still not found, try other ctors plugins- could usually define
                        while (element == null)
                        {
                            //throw new NotSupportedException("Element " + ns + ":" + str1 + " not supported. Did you miss a registration?");
                            cinfo = type.GetConstructor(new Type[] { typeof(Interop.IHTMLElement), typeof(IHtmlEditor) });
                            if (cinfo != null)
                            {
                                element = (Control)cinfo.Invoke(new object[] { el, htmlEditor });
                                break;
                            }
                            cinfo = type.GetConstructor(new Type[] { typeof(IHtmlEditor) });
                            if (cinfo != null)
                            {
                                element = (Control)cinfo.Invoke(new object[] { htmlEditor });
                                break;
                            }
                            cinfo = type.GetConstructor(Type.EmptyTypes);
                            if (cinfo != null)
                            {
                                element = (Control)cinfo.Invoke(null);
                                break;
                            }
                        }
                    }
                }
                catch 
                {
                    // whatever is goind wrong we don't care, returning null is "bad state" for caller
                    //return null;
                    throw;
                }
                finally
                {
                    if (element != null)
                    {
                        if (element is IElement)
                        {
                            ((IElement)element).HtmlEditor = this.htmlEditor;
                        }
                        // not there, then we add it
                        if (designerHost[uniqueName] == null)
                        {
                            designerHost.Add(element, uniqueName);
                            ((HtmlEditor)htmlEditor).OnElementCreated(element);
                        }
                    }
                }
                return element;
            }

            private Type GetElementType(Interop.IHTMLElement el)
            {
                Type type = null;
                string str1 = el.GetTagName().ToLower();
                string ns = "html";
                if (str1.IndexOf(":") != -1)
                {
                    string[] strSplit = str1.Split(':');
                    str1 = strSplit[1];
                    ns = strSplit[0];
                }
                else
                {
                    ns = ((Interop.IHTMLElement2)el).GetScopeName().ToLower();
                }
                // special handling for input tags
                if (!str1.Equals("input"))
                {
                    string fullName = String.Concat(ns, ":", str1);
                    if (elementTypes.ContainsKey(fullName))
                    {
                        type = elementTypes[fullName];
                    }
                }
                else
                {
                    object[] locals1 = new object[1];
                    el.GetAttribute("type", 0, locals1);
                    string str2 = locals1[0].ToString().ToLower();
                    if (str2.Equals("button"))
                    {
                        type = elementTypes[String.Concat(ns, ":", "buttonelement")];
                    }
                    else
                    {
                        type = elementTypes[String.Concat(ns, ":", str2)];
                    }
                }
                return type;
            }

            #region IElementFactory Members


            public Type GetElementType(string elementName)
            {
                if (elementTypes.ContainsKey(elementName))
                {
                    return elementTypes[elementName];
                } else
                {
                    return null;
                }
            }

            #endregion

            internal bool IsRegistered(string f)
            {
                if (elementTypes.ContainsKey(f))
                {
                    return (elementTypes[f] != null);
                } else
                {
                    return false;
                }
            }
        }

}
