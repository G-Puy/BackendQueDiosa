using Azure.Core;
using DTOS.DTOSProductoFrontBack;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using static System.Net.Mime.MediaTypeNames;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class MercadoPagoController : ControllerBase
    {
        public MercadoPagoController()
        {
            MercadoPagoConfig.AccessToken = "TEST - 1609974477177647 - 010314 - aa1a201be14a912fb990aaa24584a10b - 128881622";
        }

        [HttpPost]
        public async Task<IActionResult> algo(DTOVentaEnvio dataEnvio)
        {
            // Cria o objeto de request da preferência
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Id = dataEnvio.Id.ToString(),
                        Title = dataEnvio.Nombre,
                        CurrencyId = "UYU",
                        Description = "Descrição do Item",
                        CategoryId = "art",
                        Quantity = dataEnvio.Cantidad,
                        UnitPrice = dataEnvio.Precio
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
                    ExcludedPaymentMethods = new List<PreferencePaymentMethodRequest>
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
                  },
                    ExcludedPaymentTypes = new List<PreferencePaymentTypeRequest>
                   {
                       new PreferencePaymentTypeRequest
                       {
                           Id = "ticket"
                       }
                   },
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

            return Ok(preference.Id);
        }

    }
}
