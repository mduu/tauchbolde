using ApprovalTests;
using Newtonsoft.Json;

namespace Tauchbolde.Tests.TestingTools
{
    public static class ApprovalsExtensions
    {
        public static void VerifyObjectJson(object obj) => 
            Approvals.VerifyJson(JsonConvert.SerializeObject(obj));
    }
}