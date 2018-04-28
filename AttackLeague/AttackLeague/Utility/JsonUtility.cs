using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        protected static string GetSolutionFSPath()
        {
            return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        }

        public static T LoadJson<T>(string aFileName)
        {
            try
            {
                using (StreamReader r = new StreamReader(GetSolutionFSPath() + "/Json/" + aFileName + ".json"))
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
            using (StreamWriter r = new StreamWriter(GetSolutionFSPath() + "/Json/" + aFileName + ".json"))
            {
                r.Write(JsonConvert.SerializeObject(aObject, Formatting.Indented));
            }
        }
    }
}
