using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;


namespace SwipeType.Example
{
    public static class PointManager 
    {
        public static void AddInput(String path, PointPattern pointPattern)
        {
            try
            {
                if(File.Exists(path))
                {
                    string jsonFile;
                    using (var reader = new StreamReader(path))
                    {
                        jsonFile = reader.ReadToEnd();
                    }

                    var list = JsonConvert.DeserializeObject<List<PointPattern>>(jsonFile);
                    list.Add(pointPattern);

                    File.Delete(path);

                    jsonFile = JsonConvert.SerializeObject(list);

                    WriteToFile(path, jsonFile);

                } else
                {
                    var patterns = new PointPattern[] { pointPattern };
                    string JsonFile = JsonConvert.SerializeObject(patterns);
                    WriteToFile(path,JsonFile);
                }
                
            }
            catch
            {
                throw;
            }
            
        }

        public static List<PointPattern> ReadFile(string path)
        {
            List<PointPattern> list2=null;
            try
            {
                if (File.Exists(path))
                {
                    string jsonFile;
                    using (var reader = new StreamReader(path))
                    {
                        jsonFile = reader.ReadToEnd();
                    }
                    var list = JsonConvert.DeserializeObject<List<PointPattern>>(jsonFile);

                    return list;

                    // Iterate through the list to get what you want
                }
                else
                {
                    return list2;
                    
                    // Message if there is no file
                }
            

            }
            catch
            {
                throw;
            }
        }

        private static void WriteToFile(string path, string file)
        {
            using (var stream = new StreamWriter(path, true))
            {
                stream.WriteLine(file.ToString());
                stream.Close();
            }
        }
    }
}
