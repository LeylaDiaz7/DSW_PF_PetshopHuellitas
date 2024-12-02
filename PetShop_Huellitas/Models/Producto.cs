namespace PetShop_Huellitas.Models
{
    public class Producto
    {
        public int IdPro { get; set; }
        public string Marca { get; set; }
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Detalles { get; set; }
        public string UrlImg { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }

        public Categoria Categoria { get; set; }
    }

}
