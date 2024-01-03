using Azure.Core;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class MercadoPagoController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> algo([FromForm] IFormCollection dataEnvio)
        {
            List<PreferencePaymentMethodRequest> ExcludedPaymentMethodsList = new List<PreferencePaymentMethodRequest>
            {
                new PreferencePaymentMethodRequest
                {
                    Id = "amex"
                },
                new PreferencePaymentMethodRequest
                {
                    Id = "master"
                },
                new PreferencePaymentMethodRequest
                {
                    Id = "creditel"
                }
            };
            List<PreferencePaymentTypeRequest> ExcludedPaymentTypesList = new List<PreferencePaymentTypeRequest> 
            {
                new PreferencePaymentTypeRequest
                {
                    Id = "ticket"
                }
            };


            // Cria o objeto de request da preferência
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Id = "item-ID-1234",
                        Title = "Meu produto",
                        CurrencyId = "BRL",
                        PictureUrl = "https://www.mercadopago.com/org-img/MP3/home/logomp3.gif",
                        Description = "Descrição do Item",
                        CategoryId = "art",
                        Quantity = 1,
                        UnitPrice = 75.76m
                    }
                },
                Payer = new PreferencePayerRequest
                {
                    Name = "João",
                    Surname = "Silva",
                    Email = "user@email.com",
                    Phone = new PhoneRequest
                    {
                        AreaCode = "11",
                        Number = "4444-4444"
                    },
                    Identification = new IdentificationRequest
                    {
                        Type = "CPF",
                        Number = "19119119100"
                    },
                    Address = new AddressRequest
                    {
                        StreetName = "Street",
                        StreetNumber = "123",
                        ZipCode = "06233200"
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://www.success.com",
                    Failure = "http://www.failure.com",
                    Pending = "http://www.pending.com"
                },
                AutoReturn = "approved",
                PaymentMethods = new PreferencePaymentMethodsRequest
                {
                    ExcludedPaymentMethods = ExcludedPaymentMethodsList,
                    ExcludedPaymentTypes = ExcludedPaymentTypesList,
                    Installments = 1
                },
                NotificationUrl = "https://www.your-site.com/ipn",
                StatementDescriptor = "MEUNEGOCIO",
                ExternalReference = "Reference_1234",
                Expires = true,
                ExpirationDateFrom = DateTime.Parse("2016-02-01T12:00:00.000-04:00"),
                ExpirationDateTo = DateTime.Parse("2016-02-28T12:00:00.000-04:00")
            };

            // Cria a preferência usando o client
            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);

            return Ok(preference);
        }




    }
}
