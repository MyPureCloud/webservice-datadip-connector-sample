using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace inin.Bridge.WebServices.Datadip.Impl.Model
{
    [DataContract]
    public class CodeRequest
    {
        [DataMember]
        public string Code { get; set; }
    }
}
