using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BratinTree_PayPal_Demo.PayPalModels
{
    public class ShippingAddress
    {
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string Email { get; set; }
       public string City { get; set; }
       public string Line1 { get; set; }
       public string Line2 { get; set; }
        public string PostalCode { get; set; }
}
}