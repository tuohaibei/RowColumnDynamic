using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace TCL.Resources.Common
{
    public static class ExtensionMethodUnilty
    {
        /// <summary>
        /// 设置对象属性值
        /// </summary>
        /// <param name="dest">目标对象</param>
        /// <param name="source">源对象</param>
        /// <param name="filterPropertyName">过滤掉的属性名</param>
        public static void SetPropertyValue(this object dest, object source, params string[] filterPropertyName)
        {
            if (dest == null || source == null)
                throw new NullReferenceException("对象不能为空");
            PropertyInfo[] destPropertyInfos = dest.GetType().GetProperties();
            PropertyInfo[] sourcePropertyInfos = source.GetType().GetProperties();
            foreach (var sourceProperty in sourcePropertyInfos)
            {
                if (filterPropertyName != null && filterPropertyName.ToList().Any(propertyName => propertyName == sourceProperty.Name))
                {
                    continue;
                }
                destPropertyInfos.ToList().ForEach(destProperty =>
                {
                    if (destProperty.Name == sourceProperty.Name)
                    {
                        destProperty.SetValue(dest, sourceProperty.GetValue(source, null), null);
                    }

                });
            }
        }

        //private static List<T> ConvertArray<T>(Array input)
        //{
        //    return input.Cast<T>().ToList(); // Using LINQ for simplicity
        //}

        //public static object GetDeserializedObject(object obj, Type targetType)
        //{
        //    if (obj is Array)
        //    {
        //        MethodInfo convertMethod = typeof(ExtensionMethodUnilty).GetMethod("ConvertArray",
        //            BindingFlags.NonPublic | BindingFlags.Static);
        //        MethodInfo generic = convertMethod.MakeGenericMethod(new[] { targetType });
        //        return generic.Invoke(null, new object[] { obj });
        //    }
        //    return obj;
        //}

        

        private static T[] ConvertObjectToArray<T>(object input)
        {
            if(input is IEnumerable<T>)
            {
              return (input as IEnumerable<T>).Cast<T>().ToArray();
            }
            return null;
        }

        /// <summary>
        /// 将IEnumerable<T>转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTableFromIEnumerable<T>(this IEnumerable<T> list)
        {
            if (list == null)
            {
                list = (IEnumerable<T>)Activator.CreateInstance(typeof(List<T>));
            }
            List<PropertyInfo> pList = new List<PropertyInfo>();
            Type type = typeof(T);
            DataTable dt = new DataTable();
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType.IsGenericType ? p.PropertyType.GetGenericArguments()[0] : p.PropertyType); });
            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null) == null ? DBNull.Value : p.GetValue(item, null));
                dt.Rows.Add(row);
            }
            return dt;
        }

        /// <summary>
        /// Returns all types in <paramref name="assembliesToSearch"/> that directly or indirectly implement or inherit from the given type. 
        /// </summary>
        public static IEnumerable<Type> GetImplementors(this Type abstractType, params Assembly[] assembliesToSearch)
        {
            var typesInAssemblies = assembliesToSearch.SelectMany(assembly => assembly.GetTypes());
            return typesInAssemblies.Where(abstractType.IsAssignableFrom);
        }

        /// <summary>
        /// Returns the results of <see cref="GetImplementors"/> that match <see cref="IsInstantiable"/>.
        /// </summary>
        public static IEnumerable<Type> GetInstantiableImplementors(this Type abstractType, params Assembly[] assembliesToSearch)
        {
            var implementors = abstractType.GetImplementors(assembliesToSearch);
            return implementors.Where(IsInstantiable);
        }

        /// <summary>
        /// Determines whether <paramref name="type"/> is a concrete, non-open-generic type.
        /// </summary>
        public static bool IsInstantiable(this Type type)
        {
            return !(type.IsAbstract || type.IsGenericTypeDefinition || type.IsInterface);
        }

        /// <summary>
        /// [ <c>public static T GetDefault&lt; T &gt;()</c> ]
        /// <para></para>
        /// Retrieves the default value for a given Type
        /// </summary>
        /// <typeparam name="T">The Type for which to get the default value</typeparam>
        /// <returns>The default value for Type T</returns>
        /// <remarks>
        /// If a reference Type or a System.Void Type is supplied, this method always returns null.  If a value type 
        /// is supplied which is not publicly visible or which contains generic parameters, this method will fail with an 
        /// exception.
        /// </remarks>
        /// <seealso cref="GetDefault(Type)"/>
        public static T GetDefault<T>()
        {
            return (T)GetDefault(typeof(T));
        }

        /// <summary>
        /// [ <c>public static object GetDefault(Type type)</c> ]
        /// <para></para>
        /// Retrieves the default value for a given Type
        /// </summary>
        /// <param name="type">The Type for which to get the default value</param>
        /// <returns>The default value for <paramref name="type"/></returns>
        /// <remarks>
        /// If a null Type, a reference Type, or a System.Void Type is supplied, this method always returns null.  If a value type 
        /// is supplied which is not publicly visible or which contains generic parameters, this method will fail with an 
        /// exception.
        /// </remarks>
        /// <seealso cref="GetDefault&lt;T&gt;"/>
        public static object GetDefault(Type type)
        {
            // If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
            if (type == null || !type.IsValueType || type == typeof(void))
                return null;

            // If the supplied Type has generic parameters, its default value cannot be determined
            if (type.ContainsGenericParameters)
                throw new ArgumentException(
                    "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                    "> contains generic parameters, so the default value cannot be retrieved");

            // If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct), return a 
            //  default instance of the value type
            if (type.IsPrimitive || !type.IsNotPublic)
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe Activator.CreateInstance method could not " +
                        "create a default instance of the supplied value type <" + type +
                        "> (Inner Exception message: \"" + e.Message + "\")", e);
                }
            }

            // Fail with exception
            throw new ArgumentException("{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                "> is not a publicly-visible type, so the default value cannot be retrieved");
        }


    }
}
