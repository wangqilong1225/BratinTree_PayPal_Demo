using BratinTree_PayPal_Demo.PayPalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BratinTree_PayPal_Demo.Controllers
{
    public class PayPalController : Controller
    {
        PayPalAPI payPalAPI = new PayPalAPI();
        public ActionResult Index()
        {
            ViewBag.ClientToken = payPalAPI.GetPayPalClientToken();
            return View();
        }

        //GET
        public string GetPayPalClienToken()
        {
            ViewBag.ClientToken = payPalAPI.GetPayPalClientToken();
            return payPalAPI.GetPayPalClientToken();
        }

        public string SendNonce()
        {
            //var gateway = config.GetGateway();
            decimal amount=0.0m;

            try
            {
                amount =Convert.ToDecimal(Request["amount"]);
            }
            catch (FormatException e)
            {
                TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                //return RedirectToAction("New");
            }

            var nonce = Request["payment_method_nonce"];

            return payPalAPI.SendNonce(amount, nonce);
        }
    }
}