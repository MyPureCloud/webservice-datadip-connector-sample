using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace inin.Bridge.WebServices.Datadip.Impl.Model
{
    [DataContract]
    public class CodeStatus
    {
        [DataMember(EmitDefaultValue = false)]
        public string status { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string prize { get; set; }
    }
}
