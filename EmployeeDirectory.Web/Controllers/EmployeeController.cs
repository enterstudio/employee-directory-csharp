using EmployeeDirectory.Web.Models;
using EmployeeDirectory.Web.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Twilio.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace EmployeeDirectory.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public const string NotFoundMessage = "We could not find an employee matching '{0}'";
        private EmployeeDirectoryService _service;

        public EmployeeController() : base()
        {
            _service = new EmployeeDirectoryService(new EmployeeDbContext());
        }

        public EmployeeController(EmployeeDirectoryService service) : base()
        {
            _service = service;
        }

        // POST: Employee/Lookup
        [HttpPost]
        public async Task<ActionResult> Lookup(SmsRequest request)
        {
            var employee = (await _service.FindByNamePartialAsync(request.Body))
                .FirstOrDefault();

            var response = new TwilioResponse();
            if (employee == null)
            {
                response.Message(string.Format(NotFoundMessage, request.Body));
            }
            else
            {
                response.Message($"{employee.FullName} - {employee.Email} - {employee.PhoneNumber}", new string[] { employee.ImageUrl }, null);
            }
            return new TwiMLResult(response);
        }
    }
}
