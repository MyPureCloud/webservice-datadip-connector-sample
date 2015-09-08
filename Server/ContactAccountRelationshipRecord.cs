using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace inin.Bridge.WebServices.Datadip.Impl
{
    [DataContract]
    public class ContactAccountRelationshipRecord
    {
        [DataMember]
        public List<ContactAccountRelationship> ContactAccountRelationship = new List<ContactAccountRelationship>();
    }
}
