using System;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{
	/// <summary>
	/// Utility class.
	/// </summary>
	public class Utils
	{
		private Utils()
		{
		}

		/// <summary>
		/// Check that the supplied enum value belongs to the supplied System.Type.<br></br>
		/// If not throw an InvalidEnumArgumentException.
		/// </summary>
		/// <param name="argumentName">The name of the argument.</param>
		/// <param name="enumValue">The enum value.</param>
		/// <param name="enumClass">The type of the enum that 'enumValue' should belong to.</param>
		public static void CheckValidEnumValue( string argumentName, object enumValue, System.Type enumClass )
		{
			if( !Enum.IsDefined(enumClass, enumValue) )
			{
				throw new System.ComponentModel.InvalidEnumArgumentException( argumentName, (int)enumValue, enumClass );
			}
		}
	}
}
