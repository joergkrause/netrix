using System;
using System.Runtime.InteropServices;
using System.Threading;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Networking
{
    /// <summary>
    /// This base class handles the basic download procedure.
    /// </summary>
    /// <remarks>
    /// This is the basic implementation of the main interface exposed by an asynchronous pluggable protocol. 
    /// This interface and the IInternetProtocolSink interface communicate with each other very closely during download 
    /// operations.
    /// <para>
    /// Implementers who want to add there own asynchronous pluggable protocol should inherit from this class and
    /// override at least the following methods: <see cref="Start"/>, <see cref="Read"/>, and <see cref="Seek"/>. 
    /// </para>
    /// </remarks>
    [ClassInterface(ClassInterfaceType.None)]
    public class GenericProtocolHandler : Interop.IInternetProtocol
    {
        //System.IO.Stream _httpStream;
        string _url = null;
        Interop.IInternetProtocolSink _pSink;
        byte[] _data = null;
        int _position = 0;
        bool _sync;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="sync"></param>
        public GenericProtocolHandler(bool sync)
        {
            _sync = sync;
            d = new DownloadHandlerDelegate(DownloadDataFromHandler);
        }

        /// <summary>
        /// Async delegate
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pSink"></param>
        /// <returns></returns>
        public delegate byte[] DownloadDataInvoker(string url, Interop.IInternetProtocolSink pSink);
        /// <summary>
        /// Fired if next download starts.
        /// </summary>
        public event DownloadDataInvoker DownloadData;
        DownloadHandlerDelegate d;

        void DownloadDataFromHandler()
        {
            try
            {
                _data = DownloadData(_url, _pSink);

                _pSink.ReportData(Interop.BSCF.BSCF_FIRSTDATANOTIFICATION | Interop.BSCF.BSCF_LASTDATANOTIFICATION | Interop.BSCF.BSCF_DATAFULLYAVAILABLE, 0, _data == null ? 0 : (uint)_data.Length);
                _pSink.ReportResult(Interop.S_OK, 0, string.Empty);
            }
            catch (Exception ex)
            {
                _pSink.ReportResult(Interop.E_UNEXPECTED, 0, ex.Message);

            }
        }

        delegate void DownloadHandlerDelegate();

        /// <summary>
        /// Start downloading
        /// </summary>
        /// <param name="szUrl"></param>
        /// <param name="pOIProtSink"></param>
        /// <param name="pOIBindInfo"></param>
        /// <param name="grfPI"></param>
        /// <param name="dwReserved"></param>
        /// <returns></returns>
        public int Start(string szUrl, Interop.IInternetProtocolSink pOIProtSink,
            Interop.IInternetBindInfo pOIBindInfo, uint grfPI, uint dwReserved)
        {
            _url = szUrl;
            _pSink = pOIProtSink;

            // we do it sync!
            if (_sync)
            {
                DownloadDataFromHandler();
                return Interop.S_OK;
            }
            else
            {
                d.Invoke();
                return Interop.E_PENDING;
            }
        }
        /// <summary>
        /// Continue
        /// </summary>
        /// <param name="pProtocolData"></param>
        public void Continue(ref Interop.PROTOCOLDATA pProtocolData)
        {
            //	m_IEProtocolImpl.Continue(ref pProtocolData);
        }
        /// <summary>
        /// Abort
        /// </summary>
        /// <param name="hrReason"></param>
        /// <param name="dwOptions"></param>
        public void Abort(int hrReason, uint dwOptions)
        {
            //	m_IEProtocolImpl.Abort(hrReason, dwOptions);
            _data = null;
            //inv.EndInvoke(result);
        }
        /// <summary>
        /// Terminate
        /// </summary>
        /// <param name="dwOptions"></param>
        public void Terminate(uint dwOptions)
        {
            _data = null;
            //inv.EndInvoke(result);
        }
        /// <summary>
        /// Suspend. Not used.
        /// </summary>
        public void Suspend() { }
        /// <summary>
        /// Resume. Not used.
        /// </summary>
        public void Resume() { }
        /// <summary>
        /// Read
        /// </summary>
        /// <param name="pv"></param>
        /// <param name="cb"></param>
        /// <param name="pcbRead"></param>
        /// <returns></returns>
        public int Read(IntPtr pv, uint cb, out uint pcbRead)
        {
            if (_data == null)
            {
                pcbRead = 0;
                return Interop.S_FALSE;
            }
            int readLength = Convert.ToInt32(cb);
            if (_position + readLength > _data.Length)
            {
                readLength = _data.Length - _position;
            }
            if (readLength > 0)
            {
                Marshal.Copy(_data, _position, pv, readLength);
                _position = _position + readLength;
                pcbRead = Convert.ToUInt32(readLength);
            }
            else
            {
                pcbRead = 0;
            }
            if (_position >= _data.Length)
            {
                return Interop.S_FALSE;
            }
            else
            {
                return Interop.S_OK;
            }
        }
        /// <summary>
        /// Seek
        /// </summary>
        /// <param name="dlibMove"></param>
        /// <param name="dwOrigin"></param>
        /// <param name="plibNewPosition"></param>
        public void Seek(Interop.LARGE_INTEGER dlibMove, uint dwOrigin, out
			Interop.ULARGE_INTEGER plibNewPosition)
        {
            plibNewPosition = new Interop.ULARGE_INTEGER();
            plibNewPosition.QuadPart = 0;
            //	m_IEProtocolImpl.Seek(dlibMove, dwOrigin, out plibNewPosition);
        }
        /// <summary>
        /// Lock
        /// </summary>
        /// <param name="dwOptions"></param>
        public void LockRequest(uint dwOptions)
        {
            if (_data != null)
            {
                Monitor.Enter(_data);
                Monitor.Enter(_position);
            }
        }
        /// <summary>
        /// Unlock
        /// </summary>
        public void UnlockRequest()
        {
            if (_data != null)
            {
                Monitor.Exit(_data);
                Monitor.Exit(_position);
            }
        }

    }

}
