using EspacioPresupustosDetalle;
using EspacioProductos;
using EspacioPresupuestos;
using Microsoft.AspNetCore.Mvc;

namespace EspacioPresupuestosControllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class PresupuestosController : ControllerBase
    {
        private PresupuestoRepository accesoPresupuestos;
        public PresupuestosController()
        {
            accesoPresupuestos = new PresupuestoRepository();
        }

        [HttpPost("CrearPresupuesto")]
        public IActionResult CrearPresupuesto(Presupuestos presupuesto)
        {
            try
            {
                bool resultado = accesoPresupuestos.CrearPresupuesto(presupuesto);
                if (resultado) { return Created(); };
                return BadRequest("No se pudo crear el presupuesto");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}"); 
            }
        }

        [HttpPost("AgregarProductoyCantidad")]
        public IActionResult AgregarProductoyCantidad(int idPresupuesto, int idProducto, int cantidad)
        {
            try
            {
                bool resultado_query = accesoPresupuestos.AgregarPresupuesto(idPresupuesto, idProducto, cantidad);
                if (!resultado_query) { return BadRequest("No se pudo agregar los datos de producto y cantidad al presupuesto, revise las entradas"); };
                return (Ok("producto y cantidad agragados correctamnte"));
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}"); 
            }
        }

        [HttpGet("DetallesPorIdPresupuesto")]
        public IActionResult DetallesPorIdPresupuesto(int idPresupuesto)
        {
            try
            {
                Presupuestos? presupuestoBuscado = accesoPresupuestos.ObtenerPorId(idPresupuesto);
                if (presupuestoBuscado == null) { return BadRequest("No existe un presupuesto con el ID buscado en la base de datos"); }
                ;
                return (Ok(presupuestoBuscado));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("ListarPresupuestos")]
        public IActionResult ListarPresupuestos()
        {
            List<Presupuestos> listado = accesoPresupuestos.GetPresupuestos();
            if (listado == null) { return Ok("El listado de presupuestos esta vacio"); };
            return Ok(listado);
        }

        [HttpDelete("ElimminarPresupuesto")]
        public IActionResult EliminarPresupusto(int idBuscar)
        {
            try
            {
                bool resultado_query = accesoPresupuestos.EliminarPresupuesto(idBuscar);
                if (!resultado_query) { return NotFound("No existe un presupuesto con el ID buscado"); };
                return Ok("Presupuesto borrado exitosamente");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}