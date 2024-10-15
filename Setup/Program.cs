using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Setup
{
    internal class Program
    {
        public static readonly List<Package> Packages = new List<Package>();
        
        static void Main(string[] args)
        {
            string json = File.ReadAllText("packages.json");

            string[] paths = JsonConvert.DeserializeObject<string[]>(json);
            
            foreach (string path in paths)
            {
                Packages.Add(JsonConvert.DeserializeObject<Package>(File.ReadAllText($"../{path}")));
            }
            
            Console.Write(JsonConvert.SerializeObject(Packages, Formatting.Indented));
        }
    }
}