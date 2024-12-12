using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PetShop_Huellitas.Models;

namespace PetShop_Huellitas.Controllers
{
    public class LoginController : Controller
    {
        public readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginAdmin(string correo, string password)
        {
            Usuario user = null;
            string connectionString = _config.GetConnectionString("cn");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_LoginUser", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Password", password);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    user = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(reader["idUusario"]),
                        Nombres = reader["nombres"].ToString(),
                        Apellidos = reader["apellidos"].ToString(),
                        Correo = reader["correo"].ToString(),
                        Password = reader["password"].ToString(),
                        FechaRegistro = Convert.ToDateTime(reader["fechaRegistro"])
                    };
                }
                con.Close();
            }

            if (user != null)
            {
                // Autenticación exitosa
                // Aquí puedes añadir lógica para establecer la sesión del usuario
                return RedirectToAction("IndexProductos", "Producto");
            }
            else
            {
                ModelState.AddModelError("", "Nombre o contraseña incorrectos.");
                return View();
            }
        }

        /*

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Usuario model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = _config.GetConnectionString("cn");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegisterUser", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombres", model.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", model.Apellidos);
                    cmd.Parameters.AddWithValue("@Correo", model.Correo);
                    cmd.Parameters.AddWithValue("@Password", model.Password);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
        */

    } // fin de la clase
} // fin del namespace
