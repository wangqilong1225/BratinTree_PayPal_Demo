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
            return View();
        }

        //获取TOKEN
        public string GetPayPalClienToken()
        {
            ViewBag.ClientToken = payPalAPI.GetPayPalClientToken();
            return payPalAPI.GetPayPalClientToken();
        }

        public string SendNonce()
        {
            decimal amount=0.0m;
            //获取收货地址信息
            ShippingAddress shippingAddress = new ShippingAddress();
            try
            {
                amount =Convert.ToDecimal(Request["amount"]);
                shippingAddress = new ShippingAddress {
                    FirstName = Request["FirstName"],
                    LastName= Request["LastName"],
                    Email= Request["Email"],
                    City= Request["City"],
                    Line1= Request["Line1"],
                    Line2= Request["Line2"],
                    PostalCode= Request["PostalCode"]
                };
            }
            catch (FormatException e)
            {               
                return e.Message;
            }

            //获取nonce
            var nonce = Request["payment_method_nonce"];

            return payPalAPI.SendNonce(amount, nonce, shippingAddress);
        }
    }
}