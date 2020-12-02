﻿using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Young.Core.Common
{
    /// <summary>
    /// CLR Version: 4.0.30319.239
    /// NameSpace: com.magicalhorse.arch.Framework.Extension
    /// FileName: JsonExtension
    ///
    /// Created at 1/10/2012 6:50:15 PM
    /// </summary>
    public static class JsonExtension
    {
        #region fields

        #endregion

        #region .ctor

        #endregion

        #region properties

        #endregion

        #region methods

        /// <summary>
        /// Json序列化,用于发送到客户端
        /// 对象 必须有[DataContractAttribute]
        /// </summary>
        /// <param name="item">where item  have [DataContractAttribute]</param>
        /// <returns></returns>
        public static string ToJson_(object item)
        {
            var serializer = new DataContractJsonSerializer(item.GetType());
            var sb = new StringBuilder();
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, item);
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Json反序列化,用于接收客户端Json后生成对应的对象
        /// 对象 必须有[DataContractAttribute]
        /// </summary>
        /// <typeparam name="T">where T have [DataContractAttribute]</typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T FromJson_<T>( string jsonString)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            T jsonObject;

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                jsonObject = (T)ser.ReadObject(ms);
            }

            return jsonObject;
        }

        /// <summary>
        /// 使用 JavaScriptSerializer To
        /// </summary>
        /// <param name="obj">where obj have [SerializerAttribute]</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            var js = new JavaScriptSerializer();

            return js.Serialize(obj);
        }

        /// <summary>
        /// 使用 JavaScriptSerializer From
        /// </summary>
        /// <typeparam name="T">where T have [SerializerAttribute]</typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string jsonString)
        {
            var js = new JavaScriptSerializer();

            return js.Deserialize<T>(jsonString);
        }

        #endregion
    }

    public static class XmlExtension
    {
        #region Xml

        /// <summary>
        /// 将制定对象转换为XML字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <returns>序列化之后的XML字符串</returns>
        public static string ToXml(this object target)
        {
            XmlWriterSettings settings = new XmlWriterSettings { Encoding = Encoding.UTF8 };
            DataContractSerializer serializer = new DataContractSerializer(target.GetType());

            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    serializer.WriteObject(writer, target);
                    writer.Flush();
                }
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// 将指定的XML字符串转换为T类型的对象
        /// </summary>
        /// <typeparam name="T">所生成对象的类型</typeparam>
        /// <param name="input">要进行反序列化的XML字符串</param>
        /// <returns>反序列化的对象</returns>
        public static T DeserializeXml<T>(this string input)
            where T : class
        {
            Encoding encoding = Encoding.UTF8;
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (Stream stream2 = new MemoryStream(encoding.GetBytes(input)))
            {
                T target = serializer.ReadObject(stream2) as T;
                return target;
            }
        }
        #endregion
    }
}
