using DTOS;
using Microsoft.AspNetCore.Http;

namespace Dominio.Entidades
{
    public class Producto
    {
        public long Id { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public double PrecioActual { get; set; } = 0;
        public double PrecioAnterior { get; set; } = 0;
        public long IdTipoProducto { get; set; } = 0;
        public bool VisibleEnWeb { get; set; } = false;
        public bool Nuevo { get; set; } = false;
        public bool BajaLogica { get; set; } = false;
        public List<Stock> Stocks { get; set; } = new List<Stock>();
        public List<Stream> Imagenes { get; set; } = new List<Stream>();

        public void cargarDeDTO(DTOProducto dtoProducto)
        {
            this.Id = dtoProducto.Id;
            this.Nombre = dtoProducto.Nombre;
            this.Descripcion = dtoProducto.Descripcion;
            this.PrecioActual = dtoProducto.PrecioActual;
            this.PrecioAnterior = dtoProducto.PrecioAnterior;
            this.IdTipoProducto = dtoProducto.IdTipoProducto;
            this.VisibleEnWeb = dtoProducto.VisibleEnWeb;
            this.Nuevo = dtoProducto.Nuevo;
            this.BajaLogica = dtoProducto.BajaLogica;
            this.Stocks = cargarStocks(dtoProducto.Stocks);
            this.Imagenes = dtoProducto.Imagenes;
        }

        public DTOProducto darDto()
        {
            DTOProducto dtoProducto = new DTOProducto();
            dtoProducto.Id = this.Id;
            dtoProducto.Nombre = this.Nombre;
            dtoProducto.Descripcion = this.Descripcion;
            dtoProducto.PrecioActual = this.PrecioActual;
            dtoProducto.PrecioAnterior = this.PrecioAnterior;
            dtoProducto.IdTipoProducto = this.IdTipoProducto;
            dtoProducto.VisibleEnWeb = this.VisibleEnWeb;
            dtoProducto.Nuevo = this.Nuevo;
            dtoProducto.BajaLogica = this.BajaLogica;

            foreach (var stock in this.Stocks)
            {
                dtoProducto.Stocks.Add(stock.darDto());
            }

            dtoProducto.Imagenes = this.Imagenes;

            return dtoProducto;
        }

        private List<Stock> cargarStocks(List<DTOStock> dtos)
        {
            List<Stock> stocks = new List<Stock>();
            foreach (var dto in dtos)
            {
                Stock stock = new Stock();
                stock.cargarDeDTO(dto);
                stocks.Add(stock);
            }
            return stocks;
        }
    }
}
