using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Mvc;
using BanklinksDotNet.IPizzaProvider;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly BanklinkApi _banklinkApi;

        public HomeController(BanklinkApi banklinkApi)
        {
            _banklinkApi = banklinkApi;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult AcceptPayment()
        {
            NameValueCollection parameters = System.Web.HttpContext.Current.Request.Params;
            IPizzaPaymentResponse result = _banklinkApi.ParseIPizzaPaymentResponse(parameters);

            TempData["blablabla"] = result;
            return RedirectToAction("BankResponse");
        }

        [HttpPost]
        public ActionResult AcceptAuth()
        {
            NameValueCollection parameters = System.Web.HttpContext.Current.Request.Params;
            IPizzaAuthResponse result = _banklinkApi.ParseIPizzaAuthResponse(parameters);

            TempData["blablabla"] = result;
            return RedirectToAction("BankResponse");
        }

        public ActionResult BankResponse()
        {
            return View(TempData["blablabla"]);
        }

        [HttpPost]
        public ActionResult BankPayment(string bankId)
        {
            BankRequest request = _banklinkApi.CreateIPizzaPaymentRequest(new IPizzaPaymentRequestParams
            {
                Amount = 10.15M,
                BankId = bankId,
                Stamp = new Random((int)DateTime.Now.Ticks).Next(1234567).ToString(),
                ErrorReturnUrl = Url.Action("AcceptPayment", "Home", new { }, this.Request.Url.Scheme),
                SuccessReturnUrl = Url.Action("AcceptPayment", "Home", new{}, this.Request.Url.Scheme),
                PaymentMessage = "Shut up and take my $$$!",
            });
           
            return View("BankRequest", request);
        }

        [HttpPost]
        public ActionResult BankAuth(string bankId)
        {
            BankRequest request = _banklinkApi.CreateIPizzaAuthRequest(new IPizzaAuthRequestParams
            {
                Language = "EST",
                RequestStartDateTime = DateTime.ParseExact("2015-04-04T22:23:30+0300", "yyyy-MM-ddTHH:mm:sszz00", CultureInfo.InvariantCulture),
                RequestEncoding = "utf-8",
                BankId = "GENIPIZZA",
                RequestId = "1428175410690",
                ReturnUrl = Url.Action("AcceptAuth", "Home", new { }, this.Request.Url.Scheme),
                Nonce = Guid.NewGuid().ToString()
            });

            return View("BankRequest", request);
        }
    }
}