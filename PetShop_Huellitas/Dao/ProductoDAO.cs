using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PetShop_Huellitas.Models;
using System.Data;

namespace PetShop_Huellitas.Dao
{
    //Herando la clase DBHelper para el uso de sus metodos
    public class ProductoDAO:DBHelper
    {
        private string cad_cn = "";
        //Constructor del dao recibe el iconfiguration
        public ProductoDAO(IConfiguration iconfig)
        {
            cad_cn = iconfig.GetConnectionString("cn");
        } // fin del constructor...


        // GetProductos
        public List<Producto> getProductos()
        {
            SqlDataReader dr = traerDataReader(cad_cn, "sp_listarproductos");

            var productos = new List<Producto>();

            while (dr.Read())
            {
                var obj = new Producto()
                {
                    IdPro = dr.GetInt32(0),
                    Marca = dr.IsDBNull(1) ? null : dr.GetString(1),
                    NombreCategoria = dr.GetString(2),
                    Nombre = dr.IsDBNull(3) ? null : dr.GetString(3),
                    Detalles = dr.IsDBNull(4) ? null : dr.GetString(4),
                    UrlImg = dr.IsDBNull(5) ? null : dr.GetString(5),
                    FechaRegistro = dr.GetDateTime(6),
                    Stock = dr.GetInt32(7),
                    Precio = dr.GetDecimal(8),
                    Activo = dr.GetBoolean(9)
                };
                productos.Add(obj);//Fin del add...

            }// Fin del bucle while...

            dr.Close();

            //Fin del using

            return productos;
        }


        // GrabarProducto
        public string GrabarProducto(Producto obj) 
        {
            try 
            {
                ejecutarCRUD(cad_cn, "sp_registrarproducto",
                    obj.Marca, obj.IdCategoria, obj.Nombre,
                    obj.Detalles, obj.UrlImg, obj.Stock,
                    obj.Precio, obj.Activo);
                return $"Producto  {obj.Nombre} registrado con éxito!";
            }
            catch (Exception ex) 
            {
                return ex.Message;
            }
        }

        // Eliminar producto
        public string EliminarProducto(int codigo)
        {
            try
            {
                ejecutarCRUD(cad_cn, "sp_eliminarproducto", codigo);
                return $"Se ha eliminado al producto: {codigo}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Modificar Producto
        public string ActualizarProducto(Producto obj)
        {
            try
            {
                ejecutarCRUD(cad_cn, "sp_actualizarproducto",
                obj.IdPro, obj.Marca, obj.IdCategoria, obj.Nombre,
                obj.Detalles, obj.UrlImg, obj.Stock,
                obj.Precio, obj.Activo);
                return $"Producto  {obj.Nombre} actualizado con éxito!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        //------------------- Adicianales ---------------------
        // GetProductos
        public List<Categoria> getCategorias()
        {
            SqlDataReader dr = traerDataReader(cad_cn, "sp_listarCategorias");

            var categorias = new List<Categoria>();

            while (dr.Read())
            {
                var obj = new Categoria()
                {
                    IdCategoria = dr.GetInt32(0),
                    Nombre = dr.GetString(1)
                };
                categorias.Add(obj);//Fin del add...

            }// Fin del bucle while...

            dr.Close();

            //Fin del using

            return categorias;
        }

    }
}
