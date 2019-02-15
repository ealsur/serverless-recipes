namespace cosmosdbinputbinding.Models
{
    using System.Runtime.Serialization;

    // Define the class that will get sent on the HTTP body
    // DataContract is used to support XML
    [DataContract(Name = "Query", Namespace = "http://functions")]
    public class Query
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string PartitionKey { get; set; }
    }
}