using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace GuruComponents.Netrix.XmlDesigner.Edx
{
    internal sealed class XhtmlResolver : XmlResolver
    {
        private ArrayList _knownResources = new ArrayList();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XhtmlResolver"/> class.
        /// </summary>
        public XhtmlResolver()
        {
        }

        /// <summary>
        /// Adds the specified entity  resource to this resolver's list of known resources.
        /// </summary>
        /// <param name="xmlUri">The identifier for the XML resource. The XML resolver uses this for <see cref="M:XmlResolver.GetEntity"/> and <see cref="M:XmlResolver.GetResolveUri"/>.</param>
        /// <param name="resourceNamespaceType">The type to use to resolve the assembly resource's assembly and namespace.</param>
        /// <param name="resourceName">The name of the assembly resource.</param>
        public void AddResource(System.Uri xmlUri, Type resourceNamespaceType, string resourceName)
        {
            if (xmlUri == null)
                throw new ArgumentNullException("xmlUri");
            if (resourceNamespaceType == null)
                throw new ArgumentNullException("resourceNamespaceType");
            if (resourceName == null)
                throw new ArgumentNullException("resourceName");
            _knownResources.Add(new XmlResource(xmlUri, resourceNamespaceType, resourceName));
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public override System.Net.ICredentials Credentials
        {
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Resolves entities using local resources if it is a known entity.
        /// </summary>
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            foreach (XmlResource resource in this._knownResources)
            {
                if (resource.XmlUri.Equals(absoluteUri))
                {
                    //Debug.WriteLine(string.Format("CustomXmlResolver.GetEntity: '{0}' stream returned successfully.", absoluteUri.ToString()));
                    return resource.GetResourceStream();
                }
            }
            //Debug.WriteLine(string.Format("CustomXmlResolver.GetEntity: '{0}' no entity available!", absoluteUri.ToString()));
            //TODO: REVIEW: Which to use, base or null ?
            return null;//return base.GetEntity(absoluteUri, role, ofObjectToReturn);
        }

        /// <summary>
        /// Resolves known URIs to known local resource URIs.
        /// </summary>
        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            foreach (XmlResource resource in this._knownResources)
            {
                //TODO: REVIEW: EndsWith is propably not suitable here. We should probably check the full string or "system ID"
                Uri relUri = new Uri("urn:" + relativeUri);
                if (resource.XmlUri.AbsolutePath.EndsWith(relUri.AbsolutePath))
                {
                    //Debug.WriteLine(string.Format("CustomXmlResolver.ResolveUri: '{0}' Resolved successfully.", relativeUri));
                    return resource.XmlUri;
                }
            }
            //Debug.WriteLine(string.Format("CustomXmlResolver.ResolveUri: '{0}' UNRESOLVED.", relativeUri));
            //TODO: REVIEW: Which to use, base or null ?
            return null; //return base.ResolveUri (baseUri, relativeUri); 
        }

        /// <summary>
        /// Used internally to track xml entity resource info...
        /// </summary>
        private struct XmlResource
        {
            public System.Uri XmlUri;
            public Type NamespaceType;
            public string ResourceName;

            public XmlResource(System.Uri xmlUri, Type namespaceType, string resourceName)
            {
                this.XmlUri = xmlUri;
                this.NamespaceType = namespaceType;
                this.ResourceName = resourceName;
            }

            /// <summary>
            /// Returns the resource stream that this object describes.
            /// </summary>
            public Stream GetResourceStream()
            {
                return this.NamespaceType.Assembly.GetManifestResourceStream(this.NamespaceType, this.ResourceName);
            }
        }
    }
}
