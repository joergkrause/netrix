using System;
using System.Diagnostics;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Configuration.Install;
using System.ComponentModel;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using Comzept.Genesis.Licensing;


namespace Comzept.Genesis.Licensing.Installer
{

    [RunInstaller(true)]
    public partial class NetrixInstaller : System.Configuration.Install.Installer
    {
        public NetrixInstaller()
        {
            InitializeComponent();
        }

        private string InstallPath = "";
        private string AssemblyName = "";
        private string ControlName = "";
		private bool envDte=true;
        

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            
            string strAssemblyFiles = "";

            try
            {
                AssemblyName = Context.Parameters["AssemblyName"];
                ControlName = Context.Parameters["ControlName"];
                InstallPath = Context.Parameters["FullPath"];

                // Add assemblies to GAC
                strAssemblyFiles = base.Context.Parameters["gacname"].ToString();
                if (strAssemblyFiles.Length > 0)
                {
                    System.EnterpriseServices.Internal.Publish publish = new System.EnterpriseServices.Internal.Publish();
                    bool GacInstall = (Context.Parameters["CHECKBOXGAC"].ToString() == "1");
                    InstallerSupport.Instance.SetNetRixRegistryValue((GacInstall) ? "1" : "0", "GAC");
                    foreach (string strAssemblyFile in strAssemblyFiles.Split('|'))
                    {
                        try
                        {
                            string[] strAssembly = strAssemblyFile.Split('*');

                            if (strAssembly[0].EndsWith(".Core.dll"))
                            {
                                //publish.GacInstall(Path.Combine(Path.Combine(InstallPath, "Control"), strAssembly[0]));
                                InstallerSupport.Instance.SetNetRixRegistryValue(InstallPath, InstallerSupport.GetProperty("InstallPathKey"));
                            }
                            if (GacInstall)
                            {
                                
                                publish.GacInstall(Path.Combine(Path.Combine(InstallPath, "Control"), strAssembly[0]));
                                //MessageBox.Show("Gac installed");
                            }
                            //   MessageBox.Show(strAssembly[0] + " : " + strAssembly[1]);
                            if (!(strAssembly[0].EndsWith(".Core.dll")))
                            {

                                Type type = GetEditorType(strAssembly[0], strAssembly[1], GacInstall);
                                if (type == null)
                                {
                                    continue;
                                }
                                else
                                {
                                    InstallerSupport.Instance.SetLICFileContent(null, "", type, "Features", false);
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }


                    }
                }
                // Toolbox
                if (Context.Parameters["CHECKBOXTOOLBOX"].ToString() == "1")
                {
					try
					{
                    AddRemoveItems AddRemoveItemsDlg = new AddRemoveItems(strAssemblyFiles);
                    AddRemoveItemsDlg.InstallPath = InstallPath;
                    AddRemoveItemsDlg.ShowDialog();
					}
					catch
					{
						envDte=false;
					}
                }
                // Check for license condition
                //if (Context.Parameters["CHECKBOXLICENSE"].ToString() == "1")
                //{
                //    LicenseManager licenseManager = new LicenseManager();
                //    licenseManager.ShowDialog();
                //    //  LicenseStorage.LaunchProg();
                //}
            } // try
            catch (Exception ex)
            {
				if (envDte!=false)
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace, "Error running Installer");
                throw new InstallException(ex.Message, ex); //rethrow
            }
            base.Install(stateSaver);
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            //#if DEBUG
            //            MessageBox.Show("Uninstalling");
            //#endif
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(InstallerSupport.GetProperty("RegKey"), true);
            AssemblyName = Context.Parameters["AssemblyName"];
            string InstallPath = rk.GetValue(InstallerSupport.GetProperty("InstallPathKey")).ToString();

            string strAssemblyFiles = base.Context.Parameters["gacname"].ToString();

            try
            {
                base.Uninstall(savedState);
                // remove from GAC

                if (strAssemblyFiles.Length > 0)
                {
                    System.EnterpriseServices.Internal.Publish publish = new System.EnterpriseServices.Internal.Publish();
                    foreach (string strAssemblyFile in strAssemblyFiles.Split('|'))
                    {
                        try
                        {
                            string[] strAssembly = strAssemblyFile.Split('*');
                           // if (!strAssembly[0].EndsWith(".Core.dll"))
                            //{

                                publish.GacRemove(Path.Combine(Path.Combine(InstallPath, "Control"), strAssembly[0]));
                            //}
                        }
                        catch { }

                    }
                }
            }
            catch { }

            try
            {
                // toolbox
                AddRemoveItems AddRemoveItemsDlg = new AddRemoveItems(strAssemblyFiles);
                AddRemoveItemsDlg.RemoveItem();
                // license
                if (strAssemblyFiles.Contains("Core.dll"))
                {
                    rk.DeleteValue("GAC");
                    rk.DeleteValue("InstallPath");
                }
            }
            catch { };
            //                rk.DeleteSubKeyTree("Features");
            rk.Close();
            //            } catch {
            //            }
        }

        private Type GetEditorType(string assembly, string type, bool gacInstall)
        {
            string full = "";
            Assembly a;
            if (!gacInstall)
            {
                if (InstallPath == null || InstallPath.Length == 0)
                {
                    full = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.GetName().Name), assembly);
                }
                else
                {
                    full = Path.Combine(Path.Combine(InstallPath, "Control"), assembly);
                }
                //  MessageBox.Show(full, "FullPath");
                a = Assembly.LoadFile(full);
            }
            else
            {
                a = System.Reflection.Assembly.LoadWithPartialName(assembly.Substring(0, assembly.Length - 4));
            }

#if DEBUG
            MessageBox.Show(type);
#endif
#if DEBUG
            //  MessageBox.Show(full, "Assembly.LoadFile");
#endif
            if (a != null)
                return a.GetType(type, true, true);

            return null;
        }


        protected override void OnAfterInstall(System.Collections.IDictionary savedState)
        {
         try
			{
			if (Context.Parameters["CHECKBOXLICENSE"].ToString() == "1")
            {
                Process.Start(Path.Combine(InstallPath, "LicenseManagerPro.exe"));
                base.OnAfterInstall(savedState);
            }
			}
			catch
			{
			}
        }

        public override void Commit(System.Collections.IDictionary savedState)
        {
            MessageBox.Show("commit");
            base.Commit(savedState);
            //if (Context.Parameters["CHECKBOXLICENSE"].ToString() == "1")
            //{
             //  LicenseManager licenseManager = new LicenseManager();
              // licenseManager.ShowDialog();
                        

#if DEBUG
            MessageBox.Show("Committing");
#endif
        }

        public override void Rollback(System.Collections.IDictionary savedState)
        {

            RegistryKey rk = Registry.CurrentUser.OpenSubKey(InstallerSupport.GetProperty("RegKey"));
            AssemblyName = Context.Parameters["AssemblyName"];
            string InstallPath = rk.GetValue(InstallerSupport.GetProperty("InstallPath")).ToString();

            string strAssemblyFiles = base.Context.Parameters["gacname"].ToString();
            try
            {
                base.Rollback(savedState);
                // remove from GAC



                if (strAssemblyFiles.Length > 0)
                {
                    System.EnterpriseServices.Internal.Publish publish = new System.EnterpriseServices.Internal.Publish();
                    foreach (string strAssemblyFile in strAssemblyFiles.Split('|'))
                    {
                        try
                        {
                            string[] strAssembly = strAssemblyFile.Split('*');
                            publish.GacRemove(Path.Combine(Path.Combine(InstallPath, "Control"), strAssembly[0]));

                        }
                        catch { }
                    }
                }
            }
            catch { }

            // toolbox
            AddRemoveItems AddRemoveItemsDlg = new AddRemoveItems(strAssemblyFiles);
            AddRemoveItemsDlg.RemoveItem();
            // license

            rk.DeleteValue("GAC");
            rk.DeleteValue("InstallPath");
            //                rk.DeleteSubKeyTree("Features");
            rk.Close();
#if DEBUG
            MessageBox.Show("Rolling back");
#endif
        }
   }
}