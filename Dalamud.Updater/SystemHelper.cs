using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using Dalamud.Updater.Properties;
using Microsoft.Win32;

namespace Dalamud.Updater
{
    public sealed class SystemHelper
    {
        private SystemHelper() { }
        /// <summary>
            /// 프로그램을 부팅 시 자동 실행하도록 설정
            /// </summary>
            /// <param name="strAppPath">응용 프로그램 exe가 있는 폴더 경로</param>
            /// <param name="strAppName">응용 프로그램 exe 이름</param>
            /// <param name="bIsAutoRun">자동 실행 상태</param>
        public static void SetAutoRun(string strAppPath, string strAppName, bool bIsAutoRun)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strAppPath)
                || string.IsNullOrWhiteSpace(strAppName))
                {
                    throw new Exception("응용 프로그램 경로/이름이 비어 있습니다.");
                }
                RegistryKey reg = Registry.CurrentUser;
                RegistryKey run = reg.CreateSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\");
                if (bIsAutoRun)
                {
                    run.SetValue(strAppName, strAppPath);
                }
                else
                {
                    if (null != run.GetValue(strAppName))
                    {
                        run.DeleteValue(strAppName);
                    }
                }
                run.Close();
                reg.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
            /// 부팅 시 자동 실행되는지 확인
            /// </summary>
            /// <param name="strAppPath">응용 프로그램 경로</param>
            /// <param name="strAppName">응용 프로그램 이름</param>
            /// <returns></returns>
        public static bool IsAutoRun(string strAppPath, string strAppName)
        {
            try
            {
                RegistryKey reg = Registry.CurrentUser;
                RegistryKey software = reg.OpenSubKey(@"SOFTWARE");
                RegistryKey run = reg.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\");
                object key = run.GetValue(strAppName);
                software.Close();
                run.Close();
                if (null == key || !strAppPath.Equals(key.ToString()))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int processId);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr hObject);
    }
}
