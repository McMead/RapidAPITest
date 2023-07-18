using Eway.Rapid.Abstractions.Interfaces;
using Eway.Rapid.Abstractions.Models;
using Eway.Rapid.Abstractions.Request;
using Eway.Rapid.Abstractions.Response;
using RapidAPIProject.Models;

namespace RapidAPIProject.Services
{
    public interface IPaymentService
    {
        Task<DirectPaymentResponse> CreateTransaction(BasicRequestModel request);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IRapidClient _client;

        public PaymentService(IRapidClient client)
        {
            _client = client;
        }

        public async Task<DirectPaymentResponse> CreateTransaction(BasicRequestModel request)
        {
            var transaction = GetDirectPaymentRequest(request);
            return await _client.CreateTransaction(transaction);
        }

        private DirectPaymentRequest GetDirectPaymentRequest(BasicRequestModel req)
        {
            DirectPaymentRequest transaction = new DirectPaymentRequest()
            {
                Customer = new DirectTokenCustomer()
                {
                    CardDetails = new CardDetails()
                    {
                        Name = req.Customer.CardDetails.Name,
                        Number = req.Customer.CardDetails.Number,
                        ExpiryMonth = req.Customer.CardDetails.ExpiryMonth,
                        ExpiryYear = req.Customer.CardDetails.ExpiryYear,
                        CVN = req.Customer.CardDetails.CVN
                    }
                },
                Payment = new Payment()
                {
                    TotalAmount = req.Payment.TotalAmount,
                },
                TransactionType = TransactionTypes.Purchase
            };

            return transaction;
        }
    }

}
