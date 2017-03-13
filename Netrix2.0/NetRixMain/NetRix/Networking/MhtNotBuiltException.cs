using System;

namespace GuruComponents.Netrix.Networking
{
    /// <summary>
    /// Exception thrown if no MHT content available.
    /// </summary>
    public class MhtNotBuiltException : Exception
    {

        public MhtNotBuiltException(string message) : base(message)
        {
        }

    }
}
