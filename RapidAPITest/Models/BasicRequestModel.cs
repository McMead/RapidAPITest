using Eway.Rapid.Abstractions.Models;
using Eway.Rapid.Abstractions.Request;

namespace RapidAPIProject.Models
{
    public class BasicRequestModel 
    {
        public CustomerModel Customer { get; set; }
        public PaymentModel Payment { get; set; }
        public TransactionTypesModel TransactionType { get; set; }
    }

    public class CustomerModel
    {
        public CardDetailsModel CardDetails { get; set; }
    }

    public class CardDetailsModel
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CVN { get; set; }
    }

    public class PaymentModel
    {
        public int TotalAmount { get; set; }
    }

    public enum TransactionTypesModel
    {
        Purchase       
    }
}
