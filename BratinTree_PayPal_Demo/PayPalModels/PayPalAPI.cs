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

        //Create Braintree Gateway Braintree网关初始化
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

        //支付
        public string SendNonce(decimal amount, string nonce, ShippingAddress shippingAddress)
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
                ShippingAddress = new AddressRequest
                {
                    FirstName = shippingAddress.FirstName,
                    LastName = shippingAddress.LastName,
                    PostalCode= shippingAddress.PostalCode,
                    StreetAddress= shippingAddress.Line1+ shippingAddress.Line2,
                }
            };
            //网关支付
            Result<Transaction> result = gateway.Transaction.Sale(request);
            //支付成功
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
            }
            else if (result.Transaction != null)
            {
                return "Transaction is null,Id=" + result.Transaction.Id;
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
                return errorMessages;
            }
            return "支付完成！";
        }
    }
}