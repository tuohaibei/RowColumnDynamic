using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
//***************************************************************************************
//编制人:**
//编制时间：2015-09-15
//编制作用：序列化对象帮助类
//编制单位：TCL通讯
//***************************************************************************************
namespace TCL.Resources.Common
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public class SerializeHelper
    {
        /// <summary>          
        /// 序列化对象
        /// </summary> 
        /// <param name="obj">待序列化的对象</param>     
        /// <returns>序列化后的2进制数据</returns>          
        public static byte[] Serialize(object obj)
        { 
            using (MemoryStream ms = new MemoryStream()) { 
                BinaryFormatter bf = new BinaryFormatter(); 
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        /// <summary>          
        /// 序列化对象
        /// </summary> 
        /// <param name="obj">待序列化的对象</param>     
        /// <returns>序列化后的2进制数据</returns>          
        public static string SerializeToString(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                return Encoding.Default.GetString(Serialize(obj));
            }
        }
        /// <summary>          
        /// 反序列化          
        /// </summary>          
        /// <param name="data">2进制数据</param>          
        /// <returns>反序列化生成的对象</returns>          
        public static object DeSerialize(byte[] data)   
        {              
            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return bf.Deserialize(ms);
            }
        }
        /// <summary>          
        /// 反序列化          
        /// </summary>          
        /// <param name="data">2进制数据</param>          
        /// <returns>反序列化生成的对象</returns>          
        public static T DeSerialize<T>(string data)
        {
            return (T)DeSerialize(Encoding.Default.GetBytes(data));
        }
        /// <summary>          
        /// Json序列化          
        /// </summary>          
        /// <param name="obj">待序列化的对象</param>
        /// <returns>json数据</returns>
        public static string JsonSerialize(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder result = new StringBuilder();
            jss.Serialize(obj, result);
            return result.ToString();
        }            
        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns>反序列化生成的对象</returns>
        public static T JsonDeSerialize<T>(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder result = new StringBuilder();
            return jss.Deserialize<T>(json);
        }  
    }
}
