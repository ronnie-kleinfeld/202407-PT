//stolen from http://thedailyreviewer.com/dotnet/view/c-service---authenticating-to-a-network-fileshare-104352958
using System;
using System.Security;
using System.Runtime.InteropServices;
using System.ComponentModel;
//vk 02.16
using System.Diagnostics;
using System.IO;

namespace AS400
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct _USE_INFO_2
    {
        internal string ui2_local;
        internal string ui2_remote;
        internal IntPtr ui2_password; // don't pass a string or StringBuilder here!!
        internal uint ui2_status;
        internal uint ui2_asg_type;
        internal uint ui2_refcount;
        internal uint ui2_usecount;
        internal string ui2_username;
        internal string ui2_domainname;
    }
    public class WinNet
    {
        [DllImport("netapi32", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        static extern int NetUseAdd(
        string UncServerName, // not used
        int Level, // use info struct level 1 or 2
        IntPtr Buf, // Buffer
        ref int ParmError
        );
        [DllImport("netapi32", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        static extern int NetUseDel(
        string UncServerName,
        string UseName,
        int ForceCond
        );
        const uint USE_WILDCARD = 0xFFFFFFFF;
        public string sLog = ""; //vk 05.18

        //vk 02.16
        public string MappingToPath(string m)
        {
            string ret = "";
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "net";
            start.Arguments = String.Format("use {0}:", m);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.CreateNoWindow = true;
            //start.FileName = @"C:\7za.exe"; // Specify exe name.
            //start.UseShellExecute = false;
            //start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                ret = FromProcess(process.StandardOutput, false);
                if (ret == "")
                    ret = "ERROR: " + FromProcess(process.StandardError, true);
            }
            return ret;
        }
        private string FromProcess(StreamReader reader, bool err)
        {
            string ret = "";
            string result = reader.ReadToEnd();
            char[] d = { (char)13, (char)10 };
            foreach (string Line in result.Split(d))
            {
                if (err)
                { if (ret == "") ret = Line.Replace("Remote name", "").Trim(); }
                else
                { if (Line.StartsWith("Remote name")) ret = Line.Replace("Remote name", "").Trim(); }
            }
            return ret;
        }
        public string UseRecord(string resource, string user, string password, string domain) //static
        {
            int ret = 0;
            int paramError = 0;
            _USE_INFO_2 use2 = new _USE_INFO_2();
            IntPtr pBuf = IntPtr.Zero;
            use2.ui2_password = IntPtr.Zero;
            try
            {
                pBuf = Marshal.AllocHGlobal(Marshal.SizeOf(use2));
                use2.ui2_local = null;
                use2.ui2_asg_type = USE_WILDCARD;
                use2.ui2_remote = resource;
                use2.ui2_password = Marshal.StringToHGlobalAuto(password);
                use2.ui2_username = user;
                use2.ui2_domainname = domain;
                Marshal.StructureToPtr(use2, pBuf, false);
                ret = NetUseAdd(null, 2, pBuf, ref paramError);
                if (ret != 0)
                {
                    throw new Exception(new
                    Win32Exception(Marshal.GetLastWin32Error()).Message);
                }
                return "OK"; //vk 02.16
            }
            catch (Exception e)
            {
                return e.Message; //vk 05.18
            }
            finally
            {
                Marshal.FreeHGlobal(use2.ui2_password);
                Marshal.FreeHGlobal(pBuf);
            }
        }
        public void Stop(string resource) //static
        {
            int ret = 0;
            try
            {
                ret = NetUseDel(null, resource, 2);
            }
            catch (Exception ex)
            {
                var x = ex;
            }
        }
    }
}
