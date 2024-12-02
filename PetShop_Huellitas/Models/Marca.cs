namespace PetShop_Huellitas.Models
{
    public class Marca
    {
        public int IdMarca { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }

}
