using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace TCL.Resources.Common
{
    public class Config
    {
        private static Config _instance;
        private static object _lock = new object();

        public static Config Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (_lock)
                    {
                        if (null == _instance)
                            _instance = new Config();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public string GetDbString()
        {
            return ConfigurationManager.ConnectionStrings["ResoucePlan"] == null ? string.Empty :
              ConfigurationManager.ConnectionStrings["ResoucePlan"].ToString();
        }
    }
}