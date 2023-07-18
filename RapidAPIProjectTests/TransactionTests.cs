using NUnit.Framework;
using Moq;
using Eway.Rapid.Abstractions.Interfaces;
using RapidAPIProject.Models;
using Eway.Rapid.Abstractions.Request;
using Eway.Rapid.Abstractions.Response;
using RapidAPIProject.Services;

namespace RapidAPIProject.Tests.Services
{
    public class PaymentServiceTests
    {
        private Mock<IRapidClient> _rapidClientMock;
        private IPaymentService _paymentService;

        [SetUp]
        public void Setup()
        {
            _rapidClientMock = new Mock<IRapidClient>();
            _paymentService = new PaymentService(_rapidClientMock.Object);
        }

        [Test]
        public async Task CreateTransaction_Should_Return_DirectPaymentResponse()
        {
            // Arrange
            var requestModel = new BasicRequestModel
            {
                Customer = new CustomerModel
                {
                    CardDetails = new CardDetailsModel
                    {
                        Name = "John Smith",
                        Number = "4444333322221111",
                        ExpiryMonth = "11",
                        ExpiryYear = "25",
                        CVN = "123"
                    }
                },
                Payment = new PaymentModel
                {
                    TotalAmount = 100
                }
            };

            var expectedResponse = new DirectPaymentResponse();

            _rapidClientMock
                .Setup(rc => rc.CreateTransaction(It.IsAny<DirectPaymentRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);


            // Act
            var response = await _paymentService.CreateTransaction(requestModel);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));
        }
    }
}
