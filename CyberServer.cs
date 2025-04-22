using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CyberServer
{
    class CyberServer
    {
        static void Main(string[] args)
        {
            FileInfo fi = new FileInfo("../../log4net.xml");

            log4net.Config.XmlConfigurator.Configure(fi);

            LogService.Init("CyberServer");

            LogService.Info("[数据库] 尝试连接");

            if (!DBManager.Connect("cyber", "127.0.0.1", 3306, "root", "Zll001109"))
            {
                LogService.Error("[数据库] 连接失败");
                return;
            }

            LogService.Info("[数据库] 连接成功");

            NetManager.StartLoop(8888);
        }
    }
}
