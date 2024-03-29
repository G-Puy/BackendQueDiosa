﻿using Azure.Core;
using Dominio.Entidades;
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
using System.Data.SqlClient;
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
        private IRepositorioVenta ManejadorVenta { get; set; }

        public MercadoPagoController([FromServices] IRepositorioProducto repInj, IRepositorioStock repStock, IRepositorioVenta manejadorVenta)
        {
            // MercadoPagoConfig.AccessToken = "TEST-1609974477177647-010314-aa1a201be14a912fb990aaa24584a10b-128881622"; OFICIAL
            MercadoPagoConfig.AccessToken = "TEST-4047227634484086-011912-91f30428b5ab88c487b8c5f895a08224-1620680375";
            this.ManejadorProducto = repInj;
            this.ManejadorStock = repStock;
            this.ManejadorVenta = manejadorVenta;
        }

        [HttpPost("crearPreferencia")]
        public async Task<IActionResult> CrearPreferencia(DTOOrderData orderDataEnvio)
        {
            var persona = orderDataEnvio.datosPersona;
            var dataProductos = orderDataEnvio.datosProductos;

            List<DTOProducto> ids = new List<DTOProducto>();
            List<DTOStock> stocks = new List<DTOStock>();
            try
            {
                Venta venta = new Venta();
                venta.NombreComprador = persona.nombre;
                venta.CorreoComprador = persona.mail;
                venta.Direccion = persona.direccion;
                venta.Telefono = persona.telefono;
                venta.ApellidoComprador = persona.apellido;
                venta.Envio = persona.enviar;
                venta.Fecha = DateTime.Now;
                venta.Notas = persona.notas;

                foreach (var data in dataProductos)
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
                    VentaProducto ventaProducto = new VentaProducto();
                    ventaProducto.IdProducto = data.Id;
                    ventaProducto.IdColor = data.IdColor;
                    ventaProducto.IdTalle = data.IdTalle;
                    ventaProducto.Cantidad = data.Cantidad;
                    venta.ProductosVendidos.Add(ventaProducto);
                }

                List<DTOProducto> productos = ManejadorProducto.BuscarPorIds(ids);
                if (productos.Exists(p => p.BajaLogica)) return BadRequest("Producto dado de baja");
                if (stocks.Exists(s => !ManejadorStock.TieneStock(s))) return BadRequest("Producto no tiene stock");
                List<PreferenceItemRequest> preferenceItemRequests = new List<PreferenceItemRequest>();

                foreach (var item in stocks)
                {
                    DTOProducto p = productos.Find(x => x.Id == item.IdProducto);

                    Decimal precio = Convert.ToDecimal(p.PrecioActual);

                    foreach (var v in venta.ProductosVendidos)
                    {
                        if (v.IdProducto == item.IdProducto) v.Precio = precio;
                    }
                    venta.MontoTotal += precio * item.Cantidad;

                    PreferenceItemRequest preferenceItemRequest = new PreferenceItemRequest
                    {
                        Id = p.Id.ToString(),
                        Title = p.Nombre,
                        CurrencyId = "UYU",
                        Description = p.Descripcion,
                        Quantity = item.Cantidad,
                        UnitPrice = precio
                    };

                    preferenceItemRequests.Add(preferenceItemRequest);
                }

                var request = new PreferenceRequest
                {
                    Items = preferenceItemRequests,
                    Payer = new PreferencePayerRequest
                    {
                        Name = persona.nombre,
                        Surname = persona.apellido,
                        Email = persona.mail,
                        Phone = new PhoneRequest
                        {
                            Number = persona.telefono
                        },
                        Address = new AddressRequest
                        {
                            StreetName = persona.direccion
                        }
                    },
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = "https://quediosa.netlify.app/store/success",
                        Failure = "https://quediosa.netlify.app/store/failure",
                        Pending = "https://quediosa.netlify.app/store/pending"
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
                    StatementDescriptor = "Que Diosa",
                    ExternalReference = "Reference_1234",
                    Expires = true,
                    ExpirationDateFrom = DateTime.UtcNow,
                    ExpirationDateTo = DateTime.UtcNow.AddMinutes(60)
                };

                // Cria a preferência usando o client
                var client = new PreferenceClient();
                Preference preference = await client.CreateAsync(request);

                var idVenta = ManejadorStock.ActualizarStockYCrearVenta(stocks, venta.darDto());

                if (idVenta == -1) return BadRequest(false);

                DTOPreferencia dto = new DTOPreferencia();
                dto.IdPreferencia = preference.Id;
                dto.IdVenta = idVenta;

                return Ok(dto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("confirmarCompra")]
        public IActionResult ConfirmarCompra( long idVenta)
        {
            if (ManejadorVenta.Confirmar(idVenta)) return Ok(true);
            else return BadRequest(false);
        }

        [HttpGet("cancelarCompra")]
        public IActionResult CancelarCompra(long idVenta)
        {
            if (ManejadorVenta.Cancelar(idVenta)) return Ok(true);
            else return BadRequest(false);
        }

    }
}
