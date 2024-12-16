using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PetShop_Huellitas.Models;

namespace PetShop_Huellitas.Controllers
{
    public class UsuarioController : Controller
    {
        public readonly IConfiguration _config;
        public UsuarioController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }//Fin del IACtion Index

        IEnumerable<Usuario> Usuarios()
        {
            List<Usuario> usu = new List<Usuario>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_listar_usuario", cn);
                cn.Open();
                //Realizamos la ejecucion
                SqlDataReader dr = cmd.ExecuteReader();
                //Bucle while
                while (dr.Read())
                {
                    usu.Add(new Usuario()
                    {
                        IdUsuario = dr.GetInt32(0),
                        Nombres = dr.GetString(1),
                        Apellidos = dr.GetString(2),
                        Correo = dr.GetString(3),
                        Password = dr.GetString(4),
                        FechaRegistro = dr.GetDateTime(5)
                    });//Fin add
                }//Fin del while
            }//Fin del using
            return usu;

        }//Fin del IE NUMERABLE 


        //Retornar lista de todos los usuarios de la BD

        public async Task<IActionResult> ListadoUsuario()
        {
            return View(await Task.Run(() => Usuarios()));
        }

        //Registrar Usuario

        public IActionResult RegistrarUsuario()
        {
            return View();
        }

        //Registrarlo en la BD
        [HttpPost]
        public IActionResult RegistrarUsuario(Usuario nuevoUsuario)
        {
            using(SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_registrar_usuario", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombres", nuevoUsuario.Nombres);
                cmd.Parameters.AddWithValue("@apellidos", nuevoUsuario.Apellidos);
                cmd.Parameters.AddWithValue("@correo", nuevoUsuario.Correo);
                cmd.Parameters.AddWithValue("@password", nuevoUsuario.Password);

                cn.Open();
                cmd.ExecuteNonQuery();
            }//Fin del using
            return RedirectToAction("ListadoUsuario");
        }//Fin del IActio Reg Usua

        public IActionResult EditarUsuario(int id)
        {
            Usuario usuario = null;
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("SELECT idUusario,nombres, apellidos, correo,password, fechaRegistro FROM Tbl_Usuario WHERE idUusario = @IdUsuario", cn);
                cmd.Parameters.AddWithValue("@IdUsuario", id);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    usuario = new Usuario()
                    {
                        IdUsuario = dr.GetInt32(0),
                        Nombres = dr.GetString(1),
                        Apellidos = dr.GetString(2),
                        Correo = dr.GetString(3),
                        Password = dr.GetString(4),
                        FechaRegistro = dr.GetDateTime(5)
                    };
                }//Fin del if
                dr.Close();
            }//Fin Using USU
            return View(usuario);
        }//fin IACt Editar Usu


        [HttpPost]
        public IActionResult EditarUsuario(Usuario usuario)
        {
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_actualizar_usuario",cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario",usuario.IdUsuario);
                cmd.Parameters.AddWithValue("@nombres", usuario.Nombres);
                cmd.Parameters.AddWithValue("@apellidos",usuario.Apellidos);
                cmd.Parameters.AddWithValue("@correo",usuario.Correo);
                cmd.Parameters.AddWithValue("@password",usuario.Password);
                cn.Open();
                cmd.ExecuteNonQuery();

            }//Fin del using
            return RedirectToAction("ListadoUsuario");
        }//Fin IAC Edit USU POST

        // método para mostrar la vista de confirmación de eliminación
        public IActionResult Delete(int id)
        {
            Usuario usuario= null;
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("SELECT idUusario,nombres, apellidos, correo,password, fechaRegistro FROM Tbl_Usuario WHERE idUusario = @IdUsuario", cn);

                cmd.Parameters.AddWithValue("@IdUsuario", id);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    usuario = new Usuario()
                    {
                        IdUsuario = dr.GetInt32(0),
                        Nombres = dr.GetString(1),
                        Apellidos= dr.GetString(2),
                        Correo=dr.GetString(3),
                        Password=dr.GetString(4)
                    };
                }
                dr.Close();
            }
            return View(usuario);
        }//FIN IACT ELIMINAR USU

        // método para eliminar una categoría en la BD
        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
                {
                    SqlCommand cmd = new SqlCommand("sp_eliminar_usuario", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", id);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                return Json(new { success = true, message = "Usuario eliminado correctamente." });

            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }//FIN IACTI Eliminar USU




    }// FIN DE LA CLASE USURIO CONTROLLER
}//FIN DEL NAMESPACE
