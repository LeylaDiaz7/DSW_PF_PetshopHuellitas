namespace PetShop_Huellitas.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public int CantidadProducto { get; set; }
        public decimal MontoTotal { get; set; }
        public string Contacto { get; set; }
        public string Distrito { get; set; }
        public int Telefono { get; set; }
        public string Direccion { get; set; }
        public string IdTransaccion { get; set; }
        public DateTime FechaVenta { get; set; } = DateTime.Now;

        public int IdProducto { get; set; }

        public Cliente Cliente { get; set; }

        // Relación con los detalles de la venta
        public List<DetalleVenta> Detalles { get; set; }

        // Constructor para inicializar la lista de detalles
        public Venta()
        {
            Detalles = new List<DetalleVenta>();
        }
    }

}
