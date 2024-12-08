namespace PetShop_Huellitas.Models
{
    public class Cliente
    {
        public int IdCli { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public int? Telefono { get; set; }
        public int? Dni { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }

}
