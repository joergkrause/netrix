using System;

namespace GuruComponents.Netrix
{
    /// <summary>
    /// Provides state information about internet connection where
    /// the control is running. This is a bitfield.
    /// </summary>
    [Flags()]
    public enum InternetConnectState
    {
        ConnectionModem     = 0x01,
        ConnectionLAN       = 0x02,
        ConnectionProxy     = 0x04,
        RASInstalled        = 0x10,
        Offline             = 0x20,
        Configured          = 0x40,
    }
}
