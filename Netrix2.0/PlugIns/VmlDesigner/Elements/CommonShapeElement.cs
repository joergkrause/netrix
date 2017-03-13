using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using GuruComponents.Netrix.ComInterop;
using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.VmlDesigner.DataTypes;
using System.Runtime.InteropServices.ComTypes;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This is a class implementing all the Common Stuff of the ShapeElement and the ShapeTypeElement.
	/// </summary>
	public class CommonShapeElement : PredefinedElement{

        public CommonShapeElement(string type, IHtmlEditor editor)
            : base(type, editor)
        {
        }
        
        internal CommonShapeElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor) { }

        #region DesignTimeSupport

        /// <summary>
        /// List of adjustment values for parameterized paths.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public VgAdjustments Adj {
            get { 
                object objAdj = GetAttribute("adj");
                if (objAdj is IVgAdjustments) {
                    return new VgAdjustments(objAdj as IVgAdjustments);
                }
                else {
                    return null;
                }
            }			
        }

        private VgPath path;

        /// <summary>
        /// String with command set describing a path.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public VgPath Path {
            get { 
                object pathObj = base.GetAttribute("path");
                if (pathObj == null) return null;
                // find the name of the interface
                IntPtr ptr = Marshal.GetIDispatchForObject(pathObj);
                object o = Marshal.GetObjectForIUnknown(ptr);
                if (o is string)
                {
                    return null;
                }
                else
                {
                    Interop.IDispatch idisp = (Interop.IDispatch)o;

                    ITypeInfo ti;
                    idisp.GetTypeInfo(0, 0, out ti);

                    string name, doc, f;
                    int h;
                    ((ITypeInfo)ti).GetDocumentation(-1, out name, out doc, out h, out f);

                    // cast in case of the right interface
                    if (name == "IVgPath")
                    {
                        if (path == null)
                        {
                            try
                            {
                                path = new VgPath((IVgPath)pathObj);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                        }
                        return path;
                    }
                }
                // unknown?
                return null; 
            }
        }

        #endregion

	}
}
