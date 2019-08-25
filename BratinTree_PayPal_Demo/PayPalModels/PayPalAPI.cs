using Braintree;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BratinTree_PayPal_Demo.PayPalModels
{
    
    public class PayPalAPI
    {
        #region Files
        private string Environment { get; set; }
        private string MerchantId { get; set; }
        private string PublicKey { get; set; }
        private string PrivateKey { get; set; }
        #endregion
        
        //Create Braintree Gateway
        public BraintreeGateway CreateGateway()
        {
            Environment = System.Environment.GetEnvironmentVariable("BraintreeEnvironment");
            MerchantId = System.Environment.GetEnvironmentVariable("BraintreeMerchantId");
            PublicKey = System.Environment.GetEnvironmentVariable("BraintreePublicKey");
            PrivateKey = System.Environment.GetEnvironmentVariable("BraintreePrivateKey");

            if (MerchantId == null || PublicKey == null || PrivateKey == null)
            {
                Environment = ConfigurationManager.AppSettings["BraintreeEnvironment"];
                MerchantId = ConfigurationManager.AppSettings["BraintreeMerchantId"];
                PublicKey = ConfigurationManager.AppSettings["BraintreePublicKey"];
                PrivateKey = ConfigurationManager.AppSettings["BraintreePrivateKey"];
            }
            return new BraintreeGateway(Environment, MerchantId, PublicKey, PrivateKey);
        }

        //Get PayPal TOKEN
        public string GetPayPalClientToken()
        {
            BraintreeGateway gateway = CreateGateway();
            string ClientToken = gateway.ClientToken.Generate();
            return ClientToken;
        }

        public string SendNonce(decimal amount, string nonce)
        {
            BraintreeGateway gateway = CreateGateway();
            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                },
                CustomerId="14",
                ShippingAddress = new AddressRequest
                {
                    Company = "base",
                    CountryName = "china",
                    LastName = "ming",
                    FirstName = "X"
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                //return RedirectToAction("Show", new { id = transaction.Id });  //9wrj2n45
            }
            else if (result.Transaction != null)
            {
              //  return RedirectToAction("Show", new { id = result.Transaction.Id });
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
               // TempData["Flash"] = errorMessages;
                //return RedirectToAction("New");
            }
            Transaction transaction2 = gateway.Transaction.Find("9wrj2n45");
            return null;
        }


    }

    
}