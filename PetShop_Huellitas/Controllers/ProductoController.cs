using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetShop_Huellitas.Dao;
using PetShop_Huellitas.Models;

namespace PetShop_Huellitas.Controllers
{
    public class ProductoController : Controller
    {

        //definir variable de solo lectura para el dao en net core
        private readonly ProductoDAO dao_product;

        // Inyección de dependencias
        public ProductoController(ProductoDAO product)
        {
            dao_product = product;
        }

        public IActionResult IndexProductos()
        {
            var listado = dao_product.getProductos();
            return View(listado);
        }

        [HttpGet]
        public IActionResult CreateProducto()
        {
            ViewBag.categorias = new SelectList(
                dao_product.getCategorias(), "IdCategoria", "Nombre");

            return View(new Producto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProducto(Producto obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["mensaje"] = dao_product.GrabarProducto(obj);
                    return RedirectToAction(nameof(IndexProductos));
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            //
            ViewBag.categorias = new SelectList(
                dao_product.getCategorias(), "IdCategoria", "Nombre");
            //
            return View(obj);

        }

        // Detalles del producto
        private Producto BuscarProducto(int codigo)
        {
            var buscado = dao_product.getProductos()
                                 .Find(c => c.IdPro.Equals(codigo));

            if (buscado == null)
            {
                throw new Exception("Producto no encontrado.");
            }
            return buscado!;
        }


        public IActionResult DetailsProducto(int id)
        {
            return View(BuscarProducto(id));
        }

        // Editar Producto (EditProducto)
        public IActionResult EditProducto(int id)
        {
            ViewBag.categorias = new SelectList(
                dao_product.getCategorias(), "IdCategoria", "Nombre");
            //
            return View(BuscarProducto(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProducto(int id, Producto obj)
        {
            try
            {
                obj.IdPro = id;
                if (ModelState.IsValid)
                {
                    TempData["mensaje"] =
                        dao_product.ActualizarProducto(obj);
                    //
                    return RedirectToAction(nameof(IndexProductos));
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            //
            ViewBag.categorias = new SelectList(
                dao_product.getCategorias(), "IdCategoria", "Nombre");
            //
            return View();
        }



        // GET: Eliminar producto 
        public IActionResult DeleteProducto(int id)
        {
            return View(BuscarProducto(id));
        }

        // POST: Eliminar producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProducto(int id, IFormCollection collection)
        {
            try
            {
                TempData["mensaje"] = dao_product.EliminarProducto(id);
                
                return RedirectToAction(nameof(IndexProductos));
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

    }
}
