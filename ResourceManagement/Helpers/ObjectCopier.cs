using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ResourceManagement.Helpers
{
    public static class ObjectCopier
    {
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T CloneJson<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }

        public static void CopyTo(this object S, object T)
        {
            foreach (var pS in S.GetType().GetProperties())
            {
                foreach (var pT in T.GetType().GetProperties())
                {
                    if (pT.Name != pS.Name) continue;
                    (pT.GetSetMethod()).Invoke(T, new object[]
                    { pS.GetGetMethod().Invoke( S, null ) });
                }
            };
        }

        public static object CloneObject(object opSource)
        {
            //grab the type and create a new instance of that type
            Type opSourceType = opSource.GetType();
            object opTarget = CreateInstanceOfType(opSourceType);

            //grab the properties
            PropertyInfo[] opPropertyInfo = opSourceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //iterate over the properties and if it has a 'set' method assign it from the source TO the target
            foreach (PropertyInfo item in opPropertyInfo)
            {
                if (item.CanWrite)
                {
                    //value types can simply be 'set'
                    if (item.PropertyType.IsValueType || item.PropertyType.IsEnum || item.PropertyType.Equals(typeof(System.String)))
                    {
                        item.SetValue(opTarget, item.GetValue(opSource, null), null);
                    }
                    //object/complex types need to recursively call this method until the end of the tree is reached
                    else
                    {
                        object opPropertyValue = item.GetValue(opSource, null);
                        if (opPropertyValue == null)
                        {
                            item.SetValue(opTarget, null, null);
                        }
                        else
                        {
                            item.SetValue(opTarget, CloneObject(opPropertyValue), null);
                        }
                    }
                }
            }
            //return the new item
            return opTarget;
        }

        private static object CreateInstanceOfType(Type opSourceType)
        {
            throw new NotImplementedException();
        }
    }

    public class Person : ICloneable
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

    public static class SerializeHelper
    {
        //Michael White, Holly Springs Consulting, 2009
        //michael@hollyspringsconsulting.com
        public static T DeserializeXML<T>(string xmlData) where T : new()
        {
            if (string.IsNullOrEmpty(xmlData))
                return default(T);

            TextReader tr = new StringReader(xmlData);
            T DocItms = new T();
            XmlSerializer xms = new XmlSerializer(DocItms.GetType());
            DocItms = (T)xms.Deserialize(tr);

            return DocItms == null ? default(T) : DocItms;
        }

        public static string SeralizeObjectToXML<T>(T xmlObject)
        {
            StringBuilder sbTR = new StringBuilder();
            XmlSerializer xmsTR = new XmlSerializer(xmlObject.GetType());
            XmlWriterSettings xwsTR = new XmlWriterSettings();

            XmlWriter xmwTR = XmlWriter.Create(sbTR, xwsTR);
            xmsTR.Serialize(xmwTR, xmlObject);

            return sbTR.ToString();
        }

        public static T CloneObject<T>(T objClone) where T : new()
        {
            string GetString = SerializeHelper.SeralizeObjectToXML<T>(objClone);
            return SerializeHelper.DeserializeXML<T>(GetString);
        }
    }
}
