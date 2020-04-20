using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HZSoft.Util
{
    /// <summary>
    /// 日志
    /// </summary>
    public class LogHelper
    {
        const string path = "/log/";
        public static void AddLog(string msg)
        {
            msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + msg;
            string fullPath = HttpContext.Current.Server.MapPath(path);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            string filename = DateTime.Now.ToString("yyyyMMdd") + ".txt";

            using (StreamWriter sw = new StreamWriter(fullPath + filename, true))
            {
                sw.WriteLine(msg);
            }
        }
    }
}
