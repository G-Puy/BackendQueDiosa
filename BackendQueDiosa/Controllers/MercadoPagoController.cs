using Azure.Core;
using DTOS;
using DTOS.DTOSProductoFrontBack;
using IRepositorios;
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
        private IRepositorioProducto ManejadorProducto { get; set; }
        private IRepositorioStock ManejadorStock { get; set; }

        public MercadoPagoController([FromServices] IRepositorioProducto repInj, IRepositorioStock repStock)
        {
            MercadoPagoConfig.AccessToken = "TEST-1609974477177647-010314-aa1a201be14a912fb990aaa24584a10b-128881622";
            this.ManejadorProducto = repInj;
            this.ManejadorStock = repStock;
        }

        [HttpPost]
        public async Task<IActionResult> algo(DTOOrderData dataVenta)
        {
            List<DTOProducto> ids = new List<DTOProducto>();
            List<DTOStock> stocks = new List<DTOStock>();
            foreach (var data in dataVenta.DTOOrderDataProductos)
            {
                DTOProducto producto = new DTOProducto();
                producto.Id = data.Id;
                ids.Add(producto);
                DTOStock stock = new DTOStock();
                stock.IdProducto = data.Id;
                stock.IdColor = data.IdColor;
                stock.IdTalle = data.IdTalle;
                stock.Cantidad = data.Cantidad;
                stocks.Add(stock);
            }

            List<DTOProducto> productos = ManejadorProducto.BuscarPorIds(ids);


            if (productos.Exists(p => p.BajaLogica)) return BadRequest("Producto dado de baja");
            if (stocks.Exists(s => !ManejadorStock.TieneStock(s))) return BadRequest("Producto no tiene stock"); 

            List<PreferenceItemRequest> preferenceItemRequests = new List<PreferenceItemRequest>();

            foreach (var item in productos)
            {
                int cantidad = dataVenta.DTOOrderDataProductos.Find(x => x.Id == item.Id).Cantidad;

                PreferenceItemRequest preferenceItemRequest = new PreferenceItemRequest
                {
                    Id = item.Id.ToString(),
                    Title = item.Nombre,
                    CurrencyId = "UYU",
                    Description = item.Descripcion,
                    Quantity = cantidad,
                    UnitPrice = 1
                };

                preferenceItemRequests.Add(preferenceItemRequest);
            }

            // Cria o objeto de request da preferência
            var request = new PreferenceRequest
            {
                Items = preferenceItemRequests,
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
