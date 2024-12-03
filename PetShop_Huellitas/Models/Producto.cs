using System.ComponentModel;

namespace PetShop_Huellitas.Models
{
    public class Producto
    {
        [DisplayName("Codigo")]
        public int IdPro { get; set; }

        [DisplayName("Marca")]
        public String Marca { get; set; } = "";
        public int IdCategoria { get; set; }

        [DisplayName("Categoría")]
        public String NombreCategoria { get; set; } = "";

        [DisplayName("Producto")]
        public String Nombre { get; set; } = "";

        public String Detalles { get; set; } = "";

        [DisplayName("Imagen")]
        public String UrlImg { get; set; } = "";

        [DisplayName("Fecha de registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int Stock { get; set; }
        public decimal Precio { get; set; }

        [DisplayName("¿Está Activo?")]
        public bool Activo { get; set; }

        public Categoria? Categoria { get; set; }
    }

}
