using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PetShop_Huellitas.Models;

namespace PetShop_Huellitas.Controllers
{
    public class ClienteController : Controller
    {

        public readonly IConfiguration _config;

        public ClienteController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListarClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            string mensaje;

            // nos conectamos con la bd

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_listarclientes", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Cliente cliente = new Cliente
                        {
                            IdCli = dr.GetInt32(0),
                            Nombres = dr.GetString(1),
                            Apellidos = dr.GetString(2),
                            Direccion = dr.IsDBNull(3) ? null : dr.GetString(3),
                            Telefono = dr.IsDBNull(4) ? (int?)null : dr.GetInt32(4),
                            Dni = dr.IsDBNull(5) ? (int?)null : dr.GetInt32(5),
                            Correo = dr.GetString(6),
                            FechaRegistro = dr.GetDateTime(7)
                        };
                        clientes.Add(cliente);
                    }

                    mensaje = $"Se encontraron {clientes.Count} clientes.";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
            }

            ViewBag.Mensaje = mensaje;
            return View(clientes);
        }


        [HttpGet]
        public IActionResult CrearCliente()
        {
            return View();
        } // fin del get crear



        [HttpPost]
        public async Task<IActionResult> CrearCliente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
                {
                    try
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("sp_registrarcliente", cn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@nombres", cliente.Nombres ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@apellidos", cliente.Apellidos ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@direccion", cliente.Direccion ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@telefono", cliente.Telefono ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@dni", cliente.Dni ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@correo", cliente.Correo ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@password", cliente.Password ?? (object)DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = $"Error: {ex.Message}";
                        return View(cliente);
                    }
                } // fin del using

                return RedirectToAction("ListarClientes");
            }

            return View(cliente);
        } // fin del crear


        [HttpGet]
        public IActionResult EditarCliente(int id)
        {
            Cliente cliente = null;

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Tbl_Cliente WHERE idCli = @idCli", cn);
                    cmd.Parameters.AddWithValue("@idCli", id);
                    cn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        cliente = new Cliente
                        {
                            IdCli = dr.GetInt32(0),
                            Nombres = dr.GetString(1),
                            Apellidos = dr.GetString(2),
                            Direccion = dr.IsDBNull(3) ? null : dr.GetString(3),
                            Telefono = dr.IsDBNull(4) ? (int?)null : dr.GetInt32(4),
                            Dni = dr.IsDBNull(5) ? (int?)null : dr.GetInt32(5),
                            Correo = dr.GetString(6),
                            Password = dr.IsDBNull(7) ? null : dr.GetString(7),
                            FechaRegistro = dr.GetDateTime(8)
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            } // fin del using

            if (cliente == null)
                return NotFound();

            return View(cliente);
        } // fin del get editar


        [HttpPost]
        public async Task<IActionResult> EditarCliente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
                {
                    try
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("sp_actualizarcliente", cn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idCli", cliente.IdCli);
                        cmd.Parameters.AddWithValue("@nombres", cliente.Nombres ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@apellidos", cliente.Apellidos ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@direccion", cliente.Direccion ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@telefono", cliente.Telefono ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@dni", cliente.Dni ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@correo", cliente.Correo ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@password", cliente.Password ?? (object)DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = $"Error: {ex.Message}";
                        return View(cliente);
                    }
                } // fin del using

                return RedirectToAction("ListarClientes");
            }

            return View(cliente);
        } // fin del post editar

        [HttpPost]
        public IActionResult EliminarCliente(int idCli)
        {
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_eliminarcliente", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCli", idCli);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
                catch (Exception ex)
                {
                    ViewBag.Mensaje = $"Error: {ex.Message}";
                    return RedirectToAction("ListarClientes");
                }
            } // fin del using

            return RedirectToAction("ListarClientes");
        } // fin de eliminar

    } // fin de controlador
}
