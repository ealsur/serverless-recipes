using System;

namespace cosmosdbstaticclient.Models
{
    public class MyClass
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string name { get; set; }
        public string city { get; set; }
    }
}