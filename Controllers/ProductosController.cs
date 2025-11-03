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
                if (resultado) {return Created();};
                return BadRequest("No se pudo crear el producto");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}"); //pequeña recomendacion que me dio el bendito chatgpt ;)
            }
        }
    }
}