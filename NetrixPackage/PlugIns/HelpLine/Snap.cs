using System;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Behaviors;

namespace GuruComponents.Netrix.HelpLine
{
	/// <summary>
	/// Snap provides a static function to calculate the snap rectangle of current element depending on current helpline settings.
	/// </summary>	
public class Snap
{

	// we odn't want any inctance here, because the function is deterministic.
	private Snap()
	{
	}

    /// <summary>
    /// Calculate the snap position based on the given properties.
    /// </summary>
    /// <param name="rect">Snap position.</param>
    /// <param name="pVar">Zone control.</param>
    /// <param name="properties">Read properties that influence the calculation.</param>
    /// <param name="scrollPos">Current scroll position used to correct the snap's position.</param>
	public static void SnapRectToHelpLine(ref System.Drawing.Rectangle rect, SnapZone pVar, HelpLineProperties properties, System.Drawing.Point scrollPos)
	{
        if (properties.SnapElements)
        {
            int x = properties.X + scrollPos.X;
            int y = properties.Y + scrollPos.Y;
            int snapzone = properties.SnapZone;
            switch (pVar)
            {
                case SnapZone.None:
                    if (IsBetween(rect.Left, x - snapzone, x + snapzone))
                    {
                        rect.X = x;
                    }
                    if (IsBetween(rect.Right, x- snapzone,x + snapzone))
                    {
                        rect.X = (x - rect.Width);
                    }
                    if (IsBetween(rect.Top, y - snapzone, y + snapzone))
                    {
                        rect.Y = y;
                    }
                    if (IsBetween(rect.Bottom, y - snapzone, y + snapzone))
                    {
                        rect.Y = (y - rect.Height);
                    }
                    return;

                case SnapZone.ZoneTop:
                    if (properties.SnapOnResize && IsBetween(rect.X, y - snapzone, y + snapzone))
                    {
                        rect.Y = properties.Y;
                    }
                    return;

                case SnapZone.ZoneLeft:
                    if (properties.SnapOnResize && IsBetween(rect.Left, x - snapzone, x + snapzone))
                    {
                        rect.X = x;
                    }
                    return;

                case SnapZone.ZoneBottom:
                    if (properties.SnapOnResize && IsBetween(rect.Bottom, y - snapzone, y + snapzone))
                    {
                        rect.Y = (y - rect.Height);
                    }
                    return;

                case SnapZone.ZoneRight:
                    if (properties.SnapOnResize && IsBetween(rect.Right, x - snapzone, x + snapzone))
                    {
                        rect.X = (x - rect.Width);
                    }
                    return;

                case SnapZone.CornerTopLeft:
                    if (properties.SnapOnResize && (IsBetween(rect.Top, y- snapzone, y + snapzone) || IsBetween(rect.Left, x - snapzone, x + snapzone)))
                    {
                        rect.Y = y;
                        rect.X = x;
                    }
                    return;

                case SnapZone.CornerTopRight:
                    if (properties.SnapOnResize && (IsBetween(rect.Top, y - snapzone, y + snapzone) || IsBetween(rect.Right, x - snapzone, x + snapzone)))
                    {
                        rect.Y = y;
                        rect.X = (x - rect.Width);
                    }
                    return;

                case SnapZone.CornerBottomLeft:
                    if (properties.SnapOnResize && (IsBetween(rect.Bottom, properties.Y - snapzone, y + snapzone) || IsBetween(rect.Left, x - snapzone, x + snapzone)))
                    {
                        rect.Y = (y - rect.Height);
                        rect.X = x;
                    }
                    return;

                case SnapZone.CornerBottomRight:
                    if (properties.SnapOnResize && (IsBetween(rect.Bottom, y - snapzone, y + snapzone) || IsBetween(rect.Right, x - snapzone, x + snapzone)))
                    {
                        rect.Y = (y - rect.Height);
                        rect.X = (x - rect.Width);
                    }
                    return;

                default:
                    return;
            }
        }
	}

	/// <summary>
	/// Checks id a value is between to boundaries (including the boundary value).
	/// </summary>
	/// <param name="check">Value to check</param>
	/// <param name="min">Lower boundary</param>
	/// <param name="max">Upper boundary</param>
	/// <returns></returns>
	private static bool IsBetween(int check, int min, int max)
	{
		if (check <= max && check >= min)
			return true;
		else
			return false;
	}

}
}
