using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Networking
{

    /// <summary>
    /// Marks types that a factory is used to create instances instead of a public constructor.
    /// </summary>
    public interface IFactory
    {
    }


    /// <summary>
    /// Defines methods for loader classes.
    /// </summary>
	public interface IProtocolFactory : IFactory
	{
        /// <summary>
        /// Returns an instance.
        /// </summary>
        /// <returns></returns>
		Interop.IInternetProtocol GetIInternetProtocol();
	}

    /// <summary>
    /// Defines methods for session and mime type handling classes.
    /// </summary>
    public interface ISessionFactory : IFactory
    {
        /// <summary>
        /// Returns an instance.
        /// </summary>
        /// <returns></returns>
        Interop.IInternetSession GetIInternetSession();
    }
}
