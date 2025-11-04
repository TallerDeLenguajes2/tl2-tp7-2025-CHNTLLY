using EspacioPresupustosDetalle;
using EspacioProductos;
using Microsoft.AspNetCore.Mvc;

namespace EspacioProductosControllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class ProductosController : ControllerBase
    {
        private ProductoRepository accesoProductos;
        public ProductosController()
        {
            accesoProductos = new ProductoRepository();
        }

        [HttpPost("CrearProductos")]
        public IActionResult CrearProducto(Productos producto)
        {
            try
            {
                bool resultado = accesoProductos.CrearProducto(producto);
                if (resultado) { return Created(); };
                return BadRequest("No se pudo crear el producto");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}"); //pequeña recomendacion que me dio el bendito chatgpt ;)
            }
        }

        [HttpPut("CambiarNombreProducto")]
        public IActionResult CambiarNombreProducto(int idProd, string nombreProd)
        {
            Productos? prodBuscar = accesoProductos.ObtenerPorId(idProd);
            if (prodBuscar == null) { return BadRequest("No existe un producto con el ID ingresado"); };
            prodBuscar.Descripcion = nombreProd;
            bool resultado_query = accesoProductos.ModificarProducto(idProd, prodBuscar);
            if (!resultado_query) { return BadRequest("No se pudo modificar correctamente el nombre del producto."); };
            return Ok("Nombre del producto modificado correctamente");
        }

        [HttpGet("ListarProductos")]
        public IActionResult ListarProductos()
        {
            List<Productos> listado = accesoProductos.GetProductos();
            if (listado == null) { return Ok("El listado de productos esta vacio"); }
            ;
            return Ok(listado);
        }

        [HttpGet("DetallesPorId")]
        public IActionResult DetallesPorId(int idProd)
        {
            try
            {
                Productos? prodBuscado = accesoProductos.ObtenerPorId(idProd);
                if (prodBuscado == null) { return BadRequest("No existe un producto con el ID buscado en la base de datos"); }
                ;
                return (Ok(prodBuscado));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        [HttpDelete("ElimminarProducto")]
        public IActionResult EliminarProducto(int idBuscar)
        {
            try
            {
                bool resultado_query = accesoProductos.EliminarProducto(idBuscar);
                if (!resultado_query) { return NotFound("No existe un producto con el ID buscado"); };
                return Ok("Producto borrado exitosamente");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}