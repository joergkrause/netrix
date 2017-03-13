using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using ComTypes = GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.ComInterop
{
    /// <summary>
    /// Connect events helper class.
    /// </summary>
    [ComVisible(false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
    public class ConnectionPointCookie : IDisposable
    {
        private IConnectionPoint connectionPoint;
        private int cookie;

        /// <summary>
        /// Creates a connection point to of the given interface type,
        /// which will call on a managed code sink that implements that interface.
        /// </summary>
        /// <param name='source'>
        /// The object that exposes the events.  This object must implement IConnectionPointContainer or an InvalidCastException will be thrown.
        /// </param>
        /// <param name='sink'>
        /// The object to sink the events.  This object must implement the interface eventInterface, or an InvalidCastException is thrown.
        /// </param>
        /// <param name='eventInterface'>
        /// The event interface to sink.  The sink object must support this interface and the source object must expose it through it's ConnectionPointContainer.
        /// </param>
        public ConnectionPointCookie(object source, object sink, Type eventInterface) :
            this(source, sink, eventInterface.GUID, true) 
        {
        }

        /// <summary>
        /// Creates a connection point to of the given interface type,
        /// which will call on a managed code sink that implements that interface.
        /// </summary>
        /// <param name='source'>
        /// The object that exposes the events.  This object must implement IConnectionPointContainer or an InvalidCastException will be thrown.
        /// </param>
        /// <param name='sink'>
        /// The object to sink the events.  This object must implement the interface eventInterface, or an InvalidCastException is thrown.
        /// </param>
        /// <param name='eventInterface'>
        /// The event interface to sink.  The sink object must support this interface and the source object must expose it through it's ConnectionPointContainer.
        /// </param>
        /// <param name="throwException"></param>
        public ConnectionPointCookie(object source, object sink, Type eventInterface, bool throwException)
            : 
            this(source, sink, eventInterface.GUID, throwException)
        {
            
        }


        /// <summary>
        /// Creates a connection point to of the given interface type,
        /// which will call on a managed code sink that implements that interface.
        /// </summary>
        /// <param name='source'>
        /// The object that exposes the events. This object must implement IConnectionPointContainer or an InvalidCastException will be thrown.
        /// </param>
        /// <param name='sink'>
        /// The object to sink the events. This object must implement the interface eventInterface, or an InvalidCastException is thrown.
        /// </param>
        /// <param name='iid'>
        /// The GUID of the event interface to sink. The sink object must support this interface and the source object must expose it through it's ConnectionPointContainer.
        /// </param>
        /// <param name='throwException'>
        /// If true, exceptions described will be thrown, otherwise object will silently fail to connect.
        /// </param>
        public ConnectionPointCookie(object source, object sink, Guid iid, bool throwException) 
        {
            Exception ex = null;
            if (source is IConnectionPointContainer) 
            {
                IConnectionPointContainer cpc = (IConnectionPointContainer) source;
                IEnumConnectionPoints checkPoints;
                cpc.EnumConnectionPoints(out checkPoints);
                try 
                {
                    Guid tmp = iid;
                    cpc.FindConnectionPoint(ref tmp, out connectionPoint);
                } 
                catch(Exception) 
                {
                    connectionPoint = null;
                }

                if (connectionPoint == null) 
                {
                    ex = new ArgumentException("The source object does not expose the " + iid + " event interface");
                }
//                else if (!Type.GetTypeFromCLSID(iid).IsInstanceOfType(sink) && !(iid.Equals(typeof(Interop.IPropertyNotifySink).GUID))) 
//                {
//                    ex = new InvalidCastException("The sink object does not implement the eventInterface");
//                }
                else 
                {
                    try 
                    {
                        connectionPoint.Advise(sink, out cookie);
                    }
                    catch (Exception)
                    {
                        cookie = 0;
                        connectionPoint = null;
                        ex = new Exception("IConnectionPoint::Advise failed for event interface '" + iid + "'");
                    }
                }
            }
            else 
            {
                ex = new InvalidCastException("The source object does not expost IConnectionPointContainer");
            }

            if (throwException && (connectionPoint == null || cookie == 0)) 
            {
                if (ex == null) 
                {
                    throw new ArgumentException("Could not create connection point for event interface '" + iid + "'");
                }
                else 
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Disconnect the current connection point.  If the object is not connected,
        /// this method will do nothing.
        /// </summary>
        public void Disconnect() 
        {
            if (connectionPoint != null && cookie != 0) 
            {
                try
                {
                    connectionPoint.Unadvise(cookie);                    
                }
                catch (Exception)
                {
                }
                finally
                {
                    cookie = 0;
                    int RefCount;
                    do
                    {
                        RefCount = Marshal.ReleaseComObject(connectionPoint);
                    } while (RefCount >= 0);
                    Marshal.FinalReleaseComObject(connectionPoint);
                    connectionPoint = null;
                }
            }
        }


        #region IDisposable Members

        /// <summary>
        /// Force disconnect before disposing.
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }

        #endregion
    }


}
