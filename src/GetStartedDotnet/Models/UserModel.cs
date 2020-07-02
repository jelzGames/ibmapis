using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetStartedDotnet.Models
{
    public class UserModel
    {
        public Guid _id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string direccion { get; set; }
        public string colonia { get; set; }
        public string ciudad { get; set; }
        public string tel { get; set; }
        [JsonIgnore]
        public string _rev { get; set; }
        
    }
}
