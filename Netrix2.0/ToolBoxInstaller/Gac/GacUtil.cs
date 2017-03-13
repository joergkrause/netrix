using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Comzept.Genesis.NetRix.ComInterop;

namespace Comzept.Genesis.NetRix.Gac
{
    /// <summary>
    /// A utility class used by Installer to move assemblies to the GAC.
    /// </summary>
    public sealed class GacUtil
    {
        // we've only statics here
        private GacUtil()
        {
        }

        /// <exclude/>
        public static bool AddAssembly(string assemblyFileName)
        {
            if ((assemblyFileName == null) || (assemblyFileName.Length == 0))
            {
                throw new ArgumentNullException("assemblyFileName");
            }
            if (!File.Exists(assemblyFileName))
            {
                throw new ArgumentException("assemblyFileName");
            }
            SystemOp.IAssemblyCache cache1 = null;
            if ((SystemOp.CreateAssemblyCache(out cache1, 0) == 0) && (cache1.InstallAssembly(0, assemblyFileName, IntPtr.Zero) == 0))
            {
                return true;
            }
            return false;
        }

        /// <exclude/>
        public static bool ContainsAssembly(AssemblyName assemblyName)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException("assemblyName");
            }
            return GacUtil.ContainsAssembly(assemblyName.FullName);
        }

        /// <exclude/>
        public static bool ContainsAssembly(string assemblyFullName)
        {
            if ((assemblyFullName == null) || (assemblyFullName.Length == 0))
            {
                throw new ArgumentNullException("assemblyFullName");
            }
            SystemOp.IAssemblyCache cache1 = null;
            if ((SystemOp.CreateAssemblyCache(out cache1, 0) == 0) && (cache1.QueryAssemblyInfo(0, assemblyFullName, IntPtr.Zero) == 0))
            {
                return true;
            }
            return false;
        }

        /// <exclude/>
        public static ICollection GetAssemblies()
        {
            ArrayList list1 = new ArrayList();
            SystemOp.IAssemblyEnum enum1 = null;
            SystemOp.IAssemblyName name1 = null;
            int num1 = SystemOp.CreateAssemblyEnum(out enum1, IntPtr.Zero, null, 2, 0);
            while (num1 == 0)
            {
                num1 = enum1.GetNextAssembly(IntPtr.Zero, out name1, 0);
                if (num1 == 0)
                {
                    uint num2 = 0;
                    name1.GetDisplayName(IntPtr.Zero, ref num2, 0);
                    if (num2 > 0)
                    {
                        IntPtr ptr1 = IntPtr.Zero;
                        string text1 = null;
                        try
                        {
                            ptr1 = Marshal.AllocHGlobal((int)((num2 + 1) * 2));
                            name1.GetDisplayName(ptr1, ref num2, 0);
                            text1 = Marshal.PtrToStringUni(ptr1);
                        }
                        finally
                        {
                            if (ptr1 != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(ptr1);
                            }
                        }
                        if (text1 != null)
                        {
                            AssemblyName name2 = new AssemblyName();
                            string[] textArray1 = text1.Split(new char[] { ',' });
                            if (textArray1.Length >= 4)
                            {
                                try
                                {
                                    name2.Name = textArray1[0];
                                    name2.Version = new Version(textArray1[1].Substring(textArray1[1].IndexOf('=') + 1));
                                    string text2 = textArray1[3].Substring(textArray1[3].IndexOf('=') + 1);
                                    if (!text2.Equals("null"))
                                    {
                                        byte[] buffer1 = new byte[text2.Length / 2];
                                        for (int num3 = 0; num3 < buffer1.Length; num3++)
                                        {
                                            buffer1[num3] = byte.Parse(text2.Substring(num3 * 2, 2), NumberStyles.HexNumber);
                                        }
                                        name2.SetPublicKeyToken(buffer1);
                                    }
                                    name2.CodeBase = GacUtil.GetFusionString(name1, 13);
                                    string text3 = GacUtil.GetFusionString(name1, 8);
                                    name2.CultureInfo = new CultureInfo(text3);
                                    list1.Add(name2);
                                    continue;
                                }
                                catch (Exception)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            return list1;
        }

        private static string GetFusionString(SystemOp.IAssemblyName assemblyName, uint item)
        {
            string text1 = string.Empty;
            uint num1 = 0;
            if ((assemblyName.GetProperty(item, IntPtr.Zero, ref num1) == 0) && (num1 > 0))
            {
                IntPtr ptr1 = IntPtr.Zero;
                try
                {
                    ptr1 = Marshal.AllocHGlobal((int)((num1 + 1) * 2));
                    if (assemblyName.GetProperty(item, ptr1, ref num1) == 0)
                    {
                        text1 = Marshal.PtrToStringUni(ptr1);
                    }
                }
                finally
                {
                    if (ptr1 != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ptr1);
                    }
                }
            }
            return text1;
        }

        /// <exclude/>
        public static bool RemoveAssembly(AssemblyName assemblyName)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException("assemblyName");
            }
            return GacUtil.RemoveAssembly(assemblyName.FullName);
        }

        /// <exclude/>
        public static bool RemoveAssembly(string assemblyFullName)
        {
            uint num2;
            if ((assemblyFullName == null) || (assemblyFullName.Length == 0))
            {
                throw new ArgumentNullException("assemblyFullName");
            }
            SystemOp.IAssemblyCache cache1 = null;
            if (SystemOp.CreateAssemblyCache(out cache1, 0) != 0)
            {
                return false;
            }
            if (cache1.UninstallAssembly(0, assemblyFullName, IntPtr.Zero, out num2) != 0)
            {
                return !GacUtil.ContainsAssembly(assemblyFullName);
            }
            return true;
        }



    }

}