using System;
using System.IO;

using GuruComponents.CodeEditor.Library.Collections.Generic;


namespace GuruComponents.CodeEditor.Library.IO
{
    public class FilesCache
    {
        private KeyedCollection<Stream> _files;
        private LightCollection<string> _freeFiles;

        private long _maxBufferSize;
        private long _currentBufferSize;
        private long _maxOverrideSize;
        private bool _isOnQuotaOverride;

        public void Dispose()
        {
            if(_files!=null)
                _files.Clear();
            if(_freeFiles!=null)
                _freeFiles.Clear();
        }

        public FilesCache():this(52428800,20971520) // 50 MB, 20 MB
        {
        }
        public FilesCache(long maxBufferSize):this(maxBufferSize,20971520)
        {
        }
        public FilesCache(long maxBufferSize,long maxOverrideSize)
        {
            _maxBufferSize = maxBufferSize;
            _maxOverrideSize = maxOverrideSize;
            _files = new KeyedCollection<Stream>();
            _freeFiles = new LightCollection<string>();
            _isOnQuotaOverride = false;
        }

        public long MaxBufferSize
        {
            get
            {
                if (_isOnQuotaOverride)
                {
                    return _maxBufferSize + _maxOverrideSize;
                }
                return _maxBufferSize;
            }
            set
            {
                _maxBufferSize = value;
            }
        }
        public long MaxOverrideSize
        {
            get
            {
                return _maxOverrideSize;
            }
            set
            {
                _maxOverrideSize = value;
            }
        }

        public Stream OpenFile(string filename)
        {
            return OpenFile(filename, false);
        }
        public Stream OpenFile(string filename,bool forceReload)
        {
            if (!_files.Contains(filename) || forceReload)
            {
                StreamReader sr = new StreamReader(filename);
                _currentBufferSize += sr.BaseStream.Length;
                EnsureCapacity();
                _files.Add(filename, CloneStream(sr.BaseStream));
                sr.Close();
            }
            return _files[filename];
        }
        public void CloseFile(string filename)
        {
            if (_files.Contains(filename))
            {
                //_currentBufferSize -= _files[filename].Length;
                //_files.Remove(filename);
                //EnsureCapacity();
                if (!_freeFiles.Contains(filename))
                {
                    _freeFiles.Add(filename);
                }
            }
        }
        public long GetUsedBufferSize()
        {
            return _currentBufferSize;
        }
        public long GetFreeBufferSize()
        {
            if (_isOnQuotaOverride)
            {
                return (_maxBufferSize + _maxOverrideSize) - _currentBufferSize;
            }
            return _maxBufferSize - _currentBufferSize;
        }
        public void FreeCache()
        {
            _files.Clear();
            _freeFiles.Clear();
            _isOnQuotaOverride = false;
            _currentBufferSize = 0;
        }
        public bool FreeCachedFile(string filename)
        {
            if (_files.Contains(filename))
            {
                _currentBufferSize -= _files[filename].Length;
                _files.Remove(filename);
                EnsureCapacity();
                _freeFiles.Remove(filename);
                return true;
            }
            _freeFiles.Remove(filename);
            return false;
        }
        public bool IsCachedFile(string filename)
        {
            return _files.Contains(filename);
        }
        public bool IsOnQuotaOverride()
        {
            return _isOnQuotaOverride;
        }
        public string[] CachedFiles
        {
            get
            {
                return _files.Keys;
            }
        }

        private void EnsureCapacity()
        {
            if (_currentBufferSize > _maxBufferSize)
            {
                if (_currentBufferSize > _maxBufferSize + _maxOverrideSize)
                {
                    if (_freeFiles.Count == 0)
                    {
                        throw new Exception("Cache quota exceeded");
                    }
                    else
                    {
                        _files.Remove(_freeFiles[0]);
                        EnsureCapacity();
                    }
                }
                else
                {
                    _isOnQuotaOverride = true;
                }
            }
            else
            {
                _isOnQuotaOverride = false;
            }
        }
        private Stream CloneStream(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return new MemoryStream(buffer);
        }

    }
}
