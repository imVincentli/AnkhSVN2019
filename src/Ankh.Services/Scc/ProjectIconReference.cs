﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Ankh.Scc
{
    public sealed class ProjectIconReference : IDisposable, IEquatable<ProjectIconReference>
    {
        readonly IntPtr _staticHandle;
        readonly IntPtr _imageList;
        readonly int _index;
        IntPtr _handle;
        bool _disposed;

        public ProjectIconReference(IntPtr imageList, int index)
        {
            _imageList = imageList;
            _index = index;
        }

        public ProjectIconReference(IntPtr handle)
        {
            _staticHandle = handle;
        }

        ~ProjectIconReference()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IntPtr GetHandle()
        {
            if (_disposed)
                return IntPtr.Zero;

            if (_staticHandle != IntPtr.Zero)
                return _staticHandle;
            else if (_handle != IntPtr.Zero)
                return _handle;

            return _handle = NativeMethods.ImageList_GetIcon(_imageList, _index, 1);
        }

        void Dispose(bool dispose)
        {
            if (_disposed)
                return;

            if (_handle != null)
            {
                _disposed = true;
                NativeMethods.DestroyIcon(_handle);
            }
        }

        class NativeMethods
        {
            [DllImport("comctl32.dll", SetLastError = true)]
            public static extern IntPtr ImageList_GetIcon(IntPtr himl, int i, int flags);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool DestroyIcon(IntPtr hIcon);
        }

        public bool Equals(ProjectIconReference other)
        {
            return _staticHandle == other._staticHandle && _imageList == other._imageList && _index == other._index;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectIconReference);
        }

        public override int GetHashCode()
        {
            return _staticHandle.GetHashCode() ^ _imageList.GetHashCode() ^ _index.GetHashCode();
        }
    }
}
