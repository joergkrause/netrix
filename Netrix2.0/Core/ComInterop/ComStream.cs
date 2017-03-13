using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using STATSTG=System.Runtime.InteropServices.ComTypes.STATSTG;

namespace GuruComponents.Netrix.ComInterop
{
    /// <exclude />
    [ComVisible(false)]
    public class ComStream : IStream
    {
        private Stream stream;

        /// <exclude />
        public ComStream(Stream stream)
        {
            this.stream = stream;
        }

        void IStream.Clone(out IStream ppstm)
        {
            ppstm = null;
        }

        /// <exclude />
        public void Commit(int grfCommitFlags)
        {

        }

        /// <exclude />
        public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {

        }

        /// <exclude />
        public void LockRegion(long libOffset, long cb, int dwLockType)
        {

        }

        /// <exclude />
        public void Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            stream.Read(pv, (int)stream.Position, cb);
        }

        /// <exclude />
        public void Revert()
        {
        }

        /// <exclude />
        void IStream.Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            stream.Seek(dlibMove, (SeekOrigin)dwOrigin);
        }

        /// <exclude />
        public void SetSize(long libNewSize)
        {
            stream.SetLength(libNewSize);
        }

        /// <exclude />
        public void Stat(out STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new STATSTG();
        }

        /// <exclude />
        public void UnlockRegion(long libOffset, long cb, int dwLockType)
        {
        }

        /// <exclude />            
        public void Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            stream.Write(pv, 0, cb);
        }
    }

}
