using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using inin.Bridge.WebServices.Datadip.Impl.Model;

namespace inin.Bridge.WebServices.Datadip.Impl
{
    [ServiceContract(Name = "RESTCustomAction")]
    interface ICustomAction
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "GetCodeStatus", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        CodeStatus GetCodeStatus(CodeRequest cr);
    }
}
