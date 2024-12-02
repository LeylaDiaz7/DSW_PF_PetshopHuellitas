namespace PetShop_Huellitas.Models
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public int IdCliente { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }

        public Cliente Cliente { get; set; }
        public Producto Producto { get; set; }
    }

}
