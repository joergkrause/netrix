﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using WindowsFormsAero.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsAero
{
    [DefaultEvent("KeyDown")]
    [DefaultProperty("Enabled")]
    [System.ComponentModel.DesignerCategory("code")]
    public class LowLevelKeyboardHook : Component
    {
        private Boolean _enabled;

        private GCHandle _hookRoot;
        private SafeWindowsHookHandle _hookHandle;
        private KeyboardLowLevelHookProc _hookProc;

        private readonly SendOrPostCallback SendKeyUp;
        private readonly SendOrPostCallback SendKeyDown;
        private readonly SendOrPostCallback SendKeyPress;

        public LowLevelKeyboardHook()
        {
            SendKeyUp = state => OnKeyUp((KeyEventArgs)(state));
            SendKeyDown = state => OnKeyDown((KeyEventArgs)(state));
            SendKeyPress = state => OnKeyPress((KeyPressEventArgs)(state));
        }

        public LowLevelKeyboardHook(IContainer container)
            : this()
        {
            container.Add(this);
        }

        public event KeyEventHandler KeyUp
        {
            add { Events.AddHandler(KeyUpEvent, value); }
            remove { Events.RemoveHandler(KeyUpEvent, value); }
        }

        public event KeyEventHandler KeyDown
        {
            add { Events.AddHandler(KeyDownEvent, value); }
            remove { Events.RemoveHandler(KeyDownEvent, value); }
        }

        public event KeyPressEventHandler KeyPress
        {
            add { Events.AddHandler(KeyPressEvent, value); }
            remove { Events.RemoveHandler(KeyPressEvent, value); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;

                    if (!DesignMode)
                    {
                        if (value)
                        {
                            _hookProc = new KeyboardLowLevelHookProc(HookProc);
                            _hookHandle = NativeMethods.SetWindowsHookEx(_hookProc);

                            if (_hookHandle.IsInvalid)
                            {
                                throw new Win32Exception();
                            }

                            _hookRoot = GCHandle.Alloc(this);
                        }
                        else
                        {
                            if (_hookHandle != null)
                            {
                                _hookHandle.Dispose();
                                _hookHandle = null;
                            }

                            if (_hookRoot.IsAllocated)
                            {
                                _hookRoot.Free();
                            }
                        }
                    }
                }
            }
        }

        public void Start()
        {
            Enabled = true;
        }

        public void Stop()
        {
            Enabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_hookHandle != null)
                {
                    _hookHandle.Dispose();
                }

                if (_hookRoot.IsAllocated)
                {
                    _hookRoot.Free();
                }
            }

            _hookHandle = null;
            base.Dispose(disposing);
        }

        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            var handler = Events[KeyUpEvent] as KeyEventHandler;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            var handler = Events[KeyDownEvent] as KeyEventHandler;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnKeyPress(KeyPressEventArgs e)
        {
            var handler = Events[KeyPressEvent] as KeyPressEventHandler;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private IntPtr HookProc(Int32 nCode, IntPtr wParam, KeyboardLowLevelHookInfo info)
        {
            if (nCode == 0)
            {
                var args = new KeyEventArgs(info.KeyData);
                var handler = SendKeyDown;

                if (info.IsKeyUp)
                {
                    handler = SendKeyUp;
                }

                SynchronizationContext.Current.Send(handler, args);

                if (info.IsKeyDown && !args.SuppressKeyPress && Events[KeyPressEvent] != null)
                {
                    string str = ToUnicode(info);

                    if (!string.IsNullOrEmpty(str))
                    {
                        foreach (char ch in str)
                        {
                            SynchronizationContext.Current.Send(SendKeyPress, new KeyPressEventArgs(ch));
                        }
                    }
                }

                if (args.Handled)
                {
                    return new IntPtr(-1);
                }
            }

            return NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, info);
        }

        private static string ToUnicode(KeyboardLowLevelHookInfo info)
        {
            string result = null;

            var keyState = new byte[256];
            var buffer = new StringBuilder(128);

            if (!NativeMethods.GetKeyboardState(keyState))
            {
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            int count = ToUnicode(info.KeyCode, info.ScanCode, keyState, buffer);

            if (count > 0)
            {
                result = buffer.ToString(0, count);

                if (_lastDeadKey != null)
                {
                    ToUnicode(_lastDeadKey.KeyCode,
                              _lastDeadKey.ScanCode,
                              _lastDeadKey.KeyboardState,
                              buffer);

                    _lastDeadKey = null;
                }
            }
            else if (count < 0)
            {
                _lastDeadKey = new DeadKeyInfo(info, keyState);

                while (count < 0)
                {
                    count = ToUnicode(Keys.Decimal, buffer);
                }
            }

            return result;
        }

        private static int ToUnicode(Keys vk, StringBuilder buffer)
        {
            return ToUnicode(vk, ToScanCode(vk), new byte[256], buffer);
        }

        private static int ToUnicode(Keys vk, uint sc, byte[] keyState, StringBuilder buffer)
        {
            return NativeMethods.ToUnicode(vk, sc, keyState, buffer, buffer.Capacity, 0);
        }

        private static uint ToScanCode(Keys vk)
        {
            return NativeMethods.MapVirtualKey((uint)(vk), VirtualKeyMapType.VirtualKeyToScanCode);
        }

        private static readonly object KeyUpEvent = new object();
        private static readonly object KeyDownEvent = new object();
        private static readonly object KeyPressEvent = new object();

        private static DeadKeyInfo _lastDeadKey;

        private sealed class DeadKeyInfo
        {
            public DeadKeyInfo(KeyboardLowLevelHookInfo info, byte[] keyState)
            {
                KeyCode = info.KeyCode;
                ScanCode = info.ScanCode;

                KeyboardState = keyState;
            }

            public readonly Keys KeyCode;
            public readonly UInt32 ScanCode;
            public readonly Byte[] KeyboardState;
        }
    }

}
