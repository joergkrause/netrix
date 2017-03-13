using System.Web.UI.Design;

namespace GuruComponents.Netrix.Designer
{
	/// <summary>
	/// Base interface for externally added design time enhancement modules.
	/// </summary>
    public interface IDesignTimeBehavior :         
        //GuruComponents.Netrix.WebEditing.Behaviors.IBaseBehavior,
        IHtmlControlDesignerBehavior, 
        IControlDesignerBehavior       
	{

	}
}
