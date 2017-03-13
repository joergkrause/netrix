using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Infralution.Licensing;
using System.ComponentModel;

namespace GuruComponents.Netrix.Licensing
{
    /// <summary>
    /// Class used for License of HTML Editor
    /// </summary>
    public partial class InfralutionLicense
    {

        /// <summary>
        /// License Parametre to get License key
        /// </summary> 



        //this is testing Params Created by "Test" password
        //                    const string LICENSE_PARAMETERS =
        //                       @"<EncryptedLicenseParameters>
        //            	      <ProductName>My Product</ProductName>
        //            	      <RSAKeyValue>
        //            	      <Modulus>zX4VW8ukM8aBMgIeYOhBsH6s+UlbYM3jv3kGy59NA5vBDxaCRclowIXkHC+Ue+ua0am7brgWss/N7PetcaleXWUJacRaisC5yjl3WK2UWoPQ37HjKijXM++eCOq+mEProZYO7Ux2Q8aA13glAXn5Ry9OePA3YmD4f+658k0x1AE=</Modulus>
        //            	      <Exponent>AQAB</Exponent>
        //            	      </RSAKeyValue>
        //            	      <DesignSignature>VyWFO92791568I4C/iw5bNi8y4yhv0GE4/m+5MWMsAlOdIe7GWr3v57rxtMo9iPyfMyHJhLLJHBkuANsI0gWUiaAfyMhpHdM893mkc4PEz8KS+ZSvUCVklNWiPWPQUT0cDd6FSxCVjujBoZmIGYe4KglA8vjd2YxtHm7ZgTPYG4=</DesignSignature>
        //            	      <RuntimeSignature>YCO+TnF/qnmLuY+NiINXPal3uSZiBuCvlI6RUDtsoVxSV5eld0P4ava+m9+viKYMBLd/tOIiNH+Jh1ZQIMeWtDNndxyoRr2S1eRp0fb5XYHk8bvUUJB81a9eSrJ366YgZwzXuL2oNw4hilZBQCgUgDWEwD4c2meD0dkE5MkTOdI=</RuntimeSignature>
        //            	      <KeyStrength>7</KeyStrength>
        //            	      </EncryptedLicenseParameters>";


        const string LICENSE_PARAMETERS =
           @"<EncryptedLicenseParameters>
	  <ProductName>netrix</ProductName>
	  <RSAKeyValue>
	    <Modulus>sU/Cmbsa2km8mvUpD1iBL/3E1xj97l4X0TYSnu+ePKyL+ttmEzORkRlnzVbuffOqAQuBpXdyxwTy9zc/ztq8s2HffCroK0GIy8YsTjGk+RPiYUNtwfGQfmflv7xAY0WJnVYO+fQ/H9C+fhmGRC1/oZJqeR7/El61XQkMCgj259s=</Modulus>
	    <Exponent>AQAB</Exponent>
	  </RSAKeyValue>
	  <DesignSignature>nSLPRpAeZ9AhfeIew52PG2KthBUSiE/XA4QcSmNg0tEAC5/eD0HXGLU836klf5cKtDF8GTjZnYM6eOE1ZU1e09u3LOGAzXrBoBFfJ9gPNZUZREJ5mTb3gFR836CsNxLFUbnhs/v1Z6WXWIyvXUeg80cnipZfEV7fX2kZmSHpuzA=</DesignSignature>
	  <RuntimeSignature>iQydy2sD+Dl9LHs0eQTfwm1s3RWmVXSHV4pXWUbHbtIbAs2y2sJBV3fA1r0juVIjE7acaCRpeYAJkErP6WlRynuIc/Pn1Y1GWTbukgnJEvmv3EaX9b2WvW2iFERsHkfWaQjdEfyTHf42oU0BJp9YN9py7jPMHfyZ/inqnMXwZHE=</RuntimeSignature>
	  <KeyStrength>7</KeyStrength>
	</EncryptedLicenseParameters>";



        /// <summary>
        /// The name of the file to store the license key in - the sub directory is created
        /// automatically by ILS
        /// </summary>
        /// 

        static string _licenseFile = Application.StartupPath + @"\LicensedApp\LicensedApp.lic";
        static EncryptedLicense _license;

        //Comment Add for other path.
        //static string _licenseFile =Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\LicensedApp\LicensedApp.lic";
        //static string _licenseFile = AppDomain.CurrentDomain.BaseDirectory + @"\LicensedApp\LicensedApp.lic";
        //static string _licenseFile = Application.ExecutablePath + @"\LicensedApp\LicensedApp.lic";


        /// <summary>
        /// Function used for Check the License
        /// </summary>
        /// <returns>bool</returns>
        public static bool CheckLicense()
        {
            if (LicenseManager.CurrentContext.UsageMode == LicenseUsageMode.Runtime)
            {
                return true;
            }
            Application.EnableVisualStyles();

            // check if there is a valid license for the application

            EncryptedLicenseProvider provider = new EncryptedLicenseProvider();
            _license = provider.GetLicense(LICENSE_PARAMETERS, _licenseFile);
            // if there is no installed license then display the evaluation dialog until
            // the user installs a license or selects Exit or Continue

            while (_license == null)
            {
                EvaluationMonitor evaluationMonitor = new RegistryEvaluationMonitor("MyEvaluationPassword");
                EvaluationDialog evaluationDialog = new EvaluationDialog(evaluationMonitor, "Guru Components");
                evaluationDialog.EvaluationMessage = " Thank for you evaluating Guru Components Netrix Professional v2.0. To continue your evaluation please click the Continue Evaluation button";
                evaluationDialog.ExtendedTrialDays = 60;
                evaluationDialog.TrialDays = 60;
                EvaluationDialogResult dialogResult = evaluationDialog.ShowDialog();
                if (dialogResult == EvaluationDialogResult.Exit) return false;    //exit the app
                if (dialogResult == EvaluationDialogResult.Continue) break;      //exit the loop 
                if (dialogResult == EvaluationDialogResult.InstallLicense)
                {
                    EncryptedLicenseInstallForm licenseForm = new EncryptedLicenseInstallForm();
                    _license = licenseForm.ShowDialog("HTML EDITOR", _licenseFile, null);
                }
            }

            return true;
        }
    }
}
//public static bool CheckLicense()
//{
//    return true;
//}



