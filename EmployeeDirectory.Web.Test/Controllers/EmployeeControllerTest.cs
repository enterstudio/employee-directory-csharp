using EmployeeDirectory.Web.Controllers;
using EmployeeDirectory.Web.Services;
using EmployeeDirectory.Web.Test.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Twilio.Mvc;
using Twilio.TwiML.Mvc;

namespace EmployeeDirectory.Web.Test.Controllers
{
    [TestClass]
    public class EmployeeControllerTest
    {
        [TestMethod]
        public async Task EmployeeController_Lookup_should_return_not_found_for_unknown_name()
        {
            var xml = await GetXmlResultFromLookupAsync("unknown");
            var expectedMessage = string.Format(EmployeeController.NotFoundMessage, "unknown");
            Assert.IsTrue(xml.Contains(expectedMessage));
        }

        private async Task<string> GetXmlResultFromLookupAsync(string lookupString)
        {
            var ctrl = GetTestController();
            var result = (await ctrl.Lookup(new SmsRequest { Body = lookupString })) as TwiMLResult;
            Assert.IsNotNull(result);

            return GetXmlFromTwiMLResult(result);
        }

        private EmployeeController GetTestController()
        {
            return new EmployeeController(new EmployeeDirectoryService(TestEmployeeDbContext.GetTestDbContext()));
        }

        private string GetXmlFromTwiMLResult(TwiMLResult result)
        {
            var field = typeof(TwiMLResult).GetField("data", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            var xml = field.GetValue(result).ToString();
            Trace.WriteLine(xml);
            return xml;
        }

        [TestMethod]
        public async Task EmployeeController_Lookup_should_return_joe_record()
        {
            var xml = await GetXmlResultFromLookupAsync("Joe Programmer");
            Assert.IsTrue(xml.Contains("Joe Programmer"));
            Assert.IsTrue(xml.Contains("joe@example.com"));
            Assert.IsTrue(xml.Contains("joe.png"));
        }
    }
}
