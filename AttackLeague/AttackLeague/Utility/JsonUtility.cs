using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility
{
    class JsonUtility
    {
        /*
        public class Item
        {
            public int millis;
            public string stamp;
            public DateTime datetime;
            public string light;
            public float temp;
            public float vcc;
        }
        */

        [Serializable]
        private struct SerializedData
        {
            public string typename;
            public object data;
        }

        protected static string GetSolutionFSPath()
        {
            return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        }

        public static object LoadJsonTyped(string aFileName)
        {
            try
            {
                // 1. What class do we have
                // 2. Send data as JSON string and deserialize as intended object yes code yes

                SerializedData data = LoadJson<SerializedData>(aFileName);
                string className = data.typename;
                object classData = data.data;

                string serialized = JsonConvert.SerializeObject(classData /*Data*/, Formatting.Indented);


                Type type = Type.GetType(className, false);
                var JSONConvert = typeof(JsonConvert);
                var parameterTypes = new[] { typeof(string) };
                MethodInfo deserializer = JSONConvert.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(i => i.Name.Equals("DeserializeObject", StringComparison.InvariantCulture))
                    .Where(i => i.IsGenericMethod)
                    .Where(i => i.GetParameters().Select(a => a.ParameterType).SequenceEqual(parameterTypes))
                    .Single();
                deserializer = deserializer.MakeGenericMethod(type);
                return deserializer.Invoke(null, new object[] { serialized });

            }
            catch (Exception e)
            {
                Console.Write("We did wrong" + e.ToString());
                throw;
            }
        }

        private static T LoadJson<T>(string aFileName)
        {
            try
            {
                using (StreamReader r = new StreamReader(GetSolutionFSPath() + "/.." + "/Json/" + aFileName + ".json"))
                {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception e)
            {
                Console.Write("We did wrong" + e.ToString());
                throw;
            }
        }

        public static void SaveJson(string aFileName, object aObject)
        {
           SerializedData Data;
           Data.data = aObject;
           Data.typename = aObject.GetType().ToString();


            using (StreamWriter r = new StreamWriter(GetSolutionFSPath() + "/.." + "/Json/" + aFileName + ".json"))
            {
                r.Write(JsonConvert.SerializeObject(Data /*Data*/, Formatting.Indented));
            }
        }
    }
}
