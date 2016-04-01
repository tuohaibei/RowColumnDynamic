using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using TCL.Resources.DAL;

//*********************************
//编制人： 梁国
//编制作用：数据辅助类，反射实现数据库字段到实体类映射
//编制时间：2013-05-16
//*********************************
namespace TCL.Resources.Common
{
    public class EntityHelper
    {
        // property信息字典
        private static Dictionary<Type, PropertyInfo[]> PropertyDic = new Dictionary<Type, PropertyInfo[]>();

        private static object objKey = new object();

        /// <summary>
        /// 获取类型所有属性信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static PropertyInfo[] GetPropertys(Type type)
        {
            PropertyInfo[] propertyInfos;
            if (PropertyDic.ContainsKey(type))
            {
                propertyInfos = PropertyDic[type];
            }
            else
            {
                propertyInfos = type.GetProperties();
                if (!PropertyDic.ContainsKey(type))
                {
                    lock (objKey)
                    {
                        if (!PropertyDic.ContainsKey(type))
                        {
                            PropertyDic[type] = propertyInfos;
                        }
                    }
                }
            }
            return propertyInfos;
        }

        /// <summary>
        /// sql返回结果填充到Model
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">查询的SQL语句</param>
        /// <returns></returns>
        public static List<T> FillList<T>(string sql) where T : class
        {
            PropertyInfo[] propertyInfos = GetPropertys(typeof(T));
            List<T> lst = new List<T>();
            using (IDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GetResouceConnectionString(), CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    T rowInstance = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in propertyInfos)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (reader.GetName(i) == property.Name)
                            {
                                if (reader.GetValue(i) != DBNull.Value)
                                {
                                    if (property.PropertyType.IsGenericType)
                                        property.SetValue(rowInstance, Convert.ChangeType(reader.GetValue(i), property.PropertyType.GetGenericArguments()[0]), null);
                                    else if (property.PropertyType.IsEnum)
                                        property.SetValue(rowInstance, Enum.ToObject(property.PropertyType, reader.GetValue(i)), null);
                                    else
                                        property.SetValue(rowInstance, Convert.ChangeType(reader.GetValue(i), property.PropertyType), null);
                                }
                                break;
                            }
                        }
                    }
                    lst.Add(rowInstance);
                }
            }
            return lst;
        }

        /// <summary>
        /// sql返回结果填充到Model
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">查询的SQL语句</param>
        /// <returns></returns>
        public static List<T> FillList<T>(string sql, params SqlParameter[] commandParameters) where T : class
        {
            PropertyInfo[] propertyInfos = GetPropertys(typeof(T));
            List<T> lst = new List<T>();
            using (IDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GetResouceConnectionString(), CommandType.Text, sql, commandParameters))
            {
                while (reader.Read())
                {
                    T rowInstance = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in propertyInfos)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (reader.GetName(i) == property.Name)
                            {
                                if (reader.GetValue(i) != DBNull.Value)
                                {
                                    if (property.PropertyType.IsGenericType)
                                        property.SetValue(rowInstance, Convert.ChangeType(reader.GetValue(i), property.PropertyType.GetGenericArguments()[0]), null);
                                    else if (property.PropertyType.IsEnum)
                                        property.SetValue(rowInstance, Enum.ToObject(property.PropertyType, reader.GetValue(i)), null);
                                    else
                                        property.SetValue(rowInstance, Convert.ChangeType(reader.GetValue(i), property.PropertyType), null);
                                }
                                break;
                            }
                        }
                    }
                    lst.Add(rowInstance);
                }
            }
            return lst;
        }


        /// <summary>
        /// sql返回结果填充到Model
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">查询的SQL语句</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        public static List<T> FillList<T>(string sql, string connectionString) where T : class
        {
            PropertyInfo[] propertyInfos = GetPropertys(typeof(T));
            List<T> lst = new List<T>();
            using (IDataReader reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    T rowInstance = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in propertyInfos)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (reader.GetName(i) == property.Name)
                            {
                                if (reader.GetValue(i) != DBNull.Value)
                                {
                                    if (property.PropertyType.IsGenericType)
                                        property.SetValue(rowInstance, Convert.ChangeType(reader.GetValue(i), property.PropertyType.GetGenericArguments()[0]), null);
                                    else if (property.PropertyType.IsEnum)
                                        property.SetValue(rowInstance, Enum.ToObject(property.PropertyType, reader.GetValue(i)), null);
                                    else
                                        property.SetValue(rowInstance, Convert.ChangeType(reader.GetValue(i), property.PropertyType), null);
                                }
                                break;
                            }
                        }
                    }
                    lst.Add(rowInstance);
                }
            }
            return lst;
        }

        /// <summary>
        /// 存储过程填充实例
        /// </summary>
        /// <typeparam name="T">返回的实体类型</typeparam>
        /// <param name="procName">存储过程名</param>
        /// <param name="param">存储过程参数</param>
        /// <returns></returns>
        public static List<T> FillListByProc<T>(string procName, params SqlParameter[] param)
        {
            PropertyInfo[] propertyInfos = GetPropertys(typeof(T));
            List<T> lst = new List<T>();
            using (IDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GetResouceConnectionString(), CommandType.StoredProcedure, procName, param))
            {
                while (reader.Read())
                {
                    T rowInstance = Activator.CreateInstance<T>();
                    foreach (System.Reflection.PropertyInfo property in propertyInfos)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (reader.GetName(i) == property.Name)
                            {
                                if (reader.GetValue(i) != DBNull.Value)
                                {
                                    if (property.PropertyType.IsGenericType)
                                        property.SetValue(rowInstance, Convert.ChangeType(reader.GetValue(i), property.PropertyType.GetGenericArguments()[0]), null);
                                    else if (property.PropertyType.IsEnum)
                                        property.SetValue(rowInstance, Enum.ToObject(property.PropertyType, reader.GetValue(i)), null);
                                    else
                                        property.SetValue(rowInstance, Convert.ChangeType(reader.GetValue(i), property.PropertyType), null);
                                }
                                break;
                            }
                        }
                    }
                    lst.Add(rowInstance);
                }
            }
            return lst;
        }

        #region Model数据集写入DB table modify by liupan
        /// <summary>
        /// Model数据集写入DB table
        /// </summary>
        /// <param name="entity">扩展类</param>
        /// <param name="tableName">DB 表名</param>
        /// <param name="keyColumn">主键字段名</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="piid">流程ID(主键value，只适用于主键仅为流程ID的表格)</param>
        /// <returns></returns>
        public static bool WriteDBTable(object entity, string tableName, string keyColumn, string piid, string connectionString)
        {
            PropertyInfo[] propertyInfos = GetPropertys(entity.GetType());
            string selectSql = @"select * from " + tableName + " where " + keyColumn + "='" + piid + "';";
            //删除
            string execSql = "";
            execSql = @"delete from " + tableName + " where " + keyColumn + "='" + piid + "';";
            //新增
            string strColumn = "";
            string strValue = "";
            using (IDataReader reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, selectSql))
            {
                foreach (PropertyInfo property in propertyInfos)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.GetName(i) == property.Name)
                        {
                            strColumn = strColumn + property.Name + ",";
                            if (property.GetValue(entity, null) != null)
                                strValue = strValue + "'" + property.GetValue(entity, null).ToString().Replace("'", "''") + "',";
                            else
                                strValue = strValue + "'" + property.GetValue(entity, null) + "',";
                        }
                    }
                   
                }
                execSql = execSql + "insert into " + tableName + "(" + strColumn.Trim(',') + ") values (" + strValue.Trim(',') + ");";
                return SqlHelper.ExecuteNonQuery(SqlHelper.GetResouceConnectionString(), CommandType.Text, execSql.ToString()) > 0;
            }

       }

        #endregion

    }
}
