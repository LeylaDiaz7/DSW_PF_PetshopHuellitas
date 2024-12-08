using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PetShop_Huellitas.Models;

namespace PetShop_Huellitas.Controllers
{
    public class CategoriaController : Controller
    {

        public readonly IConfiguration _config;

        public CategoriaController(IConfiguration config)
        {
            _config = config;
        }


        public IActionResult Index()
        {
            return View();
        }

        IEnumerable<Categoria> Categorias()
        {
            List<Categoria> cat = new List<Categoria>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_listarcategorias", cn);

                cn.Open();
                //realizamos la respectiva ejecución
                SqlDataReader dr = cmd.ExecuteReader();

                //bucle while
                while (dr.Read())
                {
                    cat.Add(new Categoria()
                    {
                        IdCategoria = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        Activo = dr.GetBoolean(2),
                        FechaRegistro = dr.GetDateTime(3)
                    }); // fin del agregar listado categoria
                } // fin del read

            } // fin del using
            return cat;
        } //fin del IEnumerable


        // retorna lista de todas las categorias registradas en la BD

        public async Task<IActionResult> ListadoCategoria()
        {
            return View(await Task.Run(() => Categorias()));
        }

        // formulario de nueva categoría
        public IActionResult RegistrarCategoria()
        {
            return View();
        }

        // registrar una nueva categoría en la BD
        [HttpPost]
        public IActionResult RegistrarCategoria(Categoria nuevaCategoria)
        {
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_registrarcategoria", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", nuevaCategoria.Nombre);
                cmd.Parameters.AddWithValue("@Activo", nuevaCategoria.Activo);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("ListadoCategoria");
        }

        public IActionResult EditarCategoria(int id)
        {
            Categoria categoria = null;
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("SELECT idCategoria, nombre, activo, fechaRegistro FROM Tbl_Categoria WHERE idCategoria = @idCategoria", cn);
                cmd.Parameters.AddWithValue("@idCategoria", id);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    categoria = new Categoria()
                    {
                        IdCategoria = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        Activo = dr.GetBoolean(2),
                        FechaRegistro = dr.GetDateTime(3)
                    };
                }
                dr.Close();
            }
            return View(categoria);
        }

        [HttpPost]
        public IActionResult EditarCategoria(Categoria categoria)
        {
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_actualizarcategoria", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategoria", categoria.IdCategoria);
                cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                cmd.Parameters.AddWithValue("@Activo", categoria.Activo);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("ListadoCategoria");
        }

        // método para mostrar la vista de confirmación de eliminación
        public IActionResult EliminarCategoria(int id)
        {
            Categoria categoria = null;
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("SELECT idCategoria, nombre FROM Tbl_Categoria WHERE idCategoria = @idCategoria", cn);
                cmd.Parameters.AddWithValue("@idCategoria", id);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    categoria = new Categoria()
                    {
                        IdCategoria = dr.GetInt32(0),
                        Nombre = dr.GetString(1)
                    };
                }
                dr.Close();
            }
            return View(categoria);
        }

        // método para eliminar una categoría en la BD
        [HttpPost, ActionName("EliminarCategoria")]
        public IActionResult EliminarCategoriaConfirmado(int id)
        {
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_eliminarcategoria", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategoria", id);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("ListadoCategoria");
        }


    }
}
