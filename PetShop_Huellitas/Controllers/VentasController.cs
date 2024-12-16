using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PetShop_Huellitas.Models;

namespace PetShop_Huellitas.Controllers
{
    public class VentasController : Controller
    {
        public readonly IConfiguration _config;

        public VentasController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            return View();
        }


        // Listado Ventas
        public async Task<IActionResult> ListarVentas()
        {
            List<Venta> ventas = new List<Venta>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_listarventas", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    try
                    {
                        int idVenta = dr.GetInt32(0);
                        string nombres = dr.GetString(1);
                        string apellidos = dr.GetString(2);
                        int cantidadProducto = dr.IsDBNull(3) ? 0 : dr.GetInt32(3);
                        decimal montoTotal = dr.GetDecimal(4);
                        string contacto = dr.GetString(5);
                        int telefono = dr.GetInt32(6);
                        string direccion = dr.GetString(7);
                        DateTime fechaVenta = dr.GetDateTime(8);
                        int idProducto = dr.IsDBNull(9) ? 0 : dr.GetInt32(9);

                        ventas.Add(new Venta
                        {
                            IdVenta = idVenta,
                            Contacto = $"{nombres} {apellidos}",
                            CantidadProducto = cantidadProducto,
                            MontoTotal = montoTotal,
                            Telefono = telefono,
                            Direccion = direccion,
                            FechaVenta = fechaVenta,
                            IdProducto = idProducto
                        });
                    }
                    catch (InvalidCastException ex)
                    {
                        Console.WriteLine($"Error al leer los datos: {ex.Message}");
                        throw;
                    }
                }
            }
            return View(ventas);
        }

        // Vista de Registrar
        public IActionResult RegistrarVenta()
        {
            ViewBag.Productos = ObtenerProductos();
            return View();
        }

        // Registrar nueva venta
        [HttpPost]
        public IActionResult RegistrarVenta(int idCliente, int[] idProductos, int[] cantidades, string distrito, string idTransaccion)
        {
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_registrarventa", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                cmd.Parameters.AddWithValue("@idProductos", string.Join(",", idProductos));
                cmd.Parameters.AddWithValue("@cantidades", string.Join(",", cantidades));
                cmd.Parameters.AddWithValue("@idDistrito", distrito);
                cmd.Parameters.AddWithValue("@idTransaccion", idTransaccion);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("ListarVentas");
        }

        private List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("SELECT idPro, nombre, precio FROM Tbl_Producto", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    productos.Add(new Producto
                    {
                        IdPro = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        Precio = dr.GetDecimal(2)
                    });
                }
            }
            return productos;
        }

        //Método para eliminar venta
        public IActionResult Delete(int id)
        {
            Venta venta = null;

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("SELECT idVenta, contacto, cantidadProducto, montoTotal, telefono, direccion, fechaVenta FROM Tbl_Venta WHERE idVenta = @IdVenta", cn);
                cmd.Parameters.AddWithValue("@IdVenta", id);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    venta = new Venta()
                    {
                        IdVenta = dr.GetInt32(0),
                        Contacto = dr.GetString(1),
                        CantidadProducto = dr.GetInt32(2),
                        MontoTotal = dr.GetDecimal(3),
                        Telefono = dr.GetInt32(4),
                        Direccion = dr.GetString(5),
                        FechaVenta = dr.GetDateTime(6)
                    };
                }
                dr.Close();
            }

            return View(venta);
        }


        // Eliminar una Venta
        [HttpPost]
        public IActionResult EliminarVenta(int id)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
                {
                    SqlCommand cmd = new SqlCommand("sp_eliminarventa", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idVenta", id);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                return Json(new { success = true, message = "Venta eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }


        //Ver el detalle de una venta
        public IActionResult DetalleVenta(int id)
        {
            Venta venta = null;

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_verDetalleVenta", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idVenta", id);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    venta = new Venta()
                    {
                        IdVenta = dr.GetInt32(0),
                        Contacto = dr.GetString(1),
                        CantidadProducto = dr.GetInt32(2),
                        MontoTotal = dr.GetDecimal(3),
                        Direccion = dr.GetString(4),
                        FechaVenta = dr.GetDateTime(5)
                    };
                }
                dr.Close();
            }

            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

    }


}
