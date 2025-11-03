using AccesoADatos;
using EspacioCadete;
using EspacioInformeCadete;
using EspacioInformeCadeteria;
using EspacioPedido;
using EspacioCadeteria;
using Microsoft.AspNetCore.Mvc;

namespace EspacioCadeteriaControllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class CadeteriaController : ControllerBase
    {
        private AccesoADatosJSON accesoADatos;
        public CadeteriaController()
        {
            accesoADatos = new AccesoADatosJSON();
        }
        [HttpGet("GetPedidos")]
        public IActionResult GetPedidos()
        {
            List<Pedido> listadoPedidos = accesoADatos.LeerPedidos("./data/datosPedidos.json");
            return Ok(listadoPedidos);
        }
        [HttpGet("GetCadetes")]
        public IActionResult GetCadetes()
        {
            List<Cadete> listadoCadetes = accesoADatos.LeerCadetes("./data/datosCadetes.json");
            return Ok(listadoCadetes);
        }
        [HttpGet("GetInforme")]
        public IActionResult GetInforme()
        {
            List<InformeCadete> listadoInformes = accesoADatos.LeerInformeCadetes("./data/datosInformes.json");
            InformeGeneral InformeTotal = Cadeteria.GenerarInforme(listadoInformes);
            return Ok(InformeTotal);
        }

        [HttpPost("AgregarPedidos")]
        public IActionResult AgregarPedidos(Pedido pedido)
        {
            Pedido pedidoCreated = accesoADatos.AddPedido(pedido, "./data/datosPedidos.json");
            return Created();
        }

        [HttpPut("AsignarPedido")]
        public IActionResult AsignarPedido(int idPedido, int idCadete)
        {
            var pedidoBuscado = accesoADatos.GetPedidoById(idPedido, "./data/datosPedidos.json");
            var cadeteBuscado = accesoADatos.GetCadeteById(idCadete, "./data/datosCadetes.json");
            if (pedidoBuscado == null && cadeteBuscado == null) return NotFound("pedido y cadete no encontrados");
            if (pedidoBuscado == null) return NotFound("pedido no encontrado");
            if (cadeteBuscado == null) return NotFound("cadete no encontrado");
            var ListadoPedidos = accesoADatos.LeerPedidos("./data/datosPedidos.json");
            foreach (var pedido in ListadoPedidos)
            {
                if (pedido.numero == idPedido)
                {
                    pedido.cadete = cadeteBuscado;
                }
            }
            accesoADatos.escribirPedidosJSON(ListadoPedidos, "./data/datosPedidos.json");
            return Ok("Cadete asignado a Pedido correctamente");
        }

        [HttpPut("CambiarEstadoPedido")]
        public IActionResult CambiarEstadoPedido(int idPedido, bool NuevoEstado)
        {
            var pedidoBuscado = accesoADatos.GetPedidoById(idPedido, "./data/datosPedidos.json");
            if (pedidoBuscado == null) return NotFound("pedido no encontrado");
            var ListadoPedidos = accesoADatos.LeerPedidos("./data/datosPedidos.json");
            foreach (var pedido in ListadoPedidos)
            {
                if (pedido.numero == idPedido)
                {
                    pedido.estado = NuevoEstado;
                }
            }
            accesoADatos.escribirPedidosJSON(ListadoPedidos, "./data/datosPedidos.json");
            return Ok("Estado del pedido cambiado correctamente");
        }
        [HttpPut("CambiarCadetePedido")]
        public IActionResult CambiarCadetePedido(int idPedido, int idNuevoCadete)
        {
            var pedidoBuscado = accesoADatos.GetPedidoById(idPedido, "./data/datosPedidos.json");
            var cadeteBuscado = accesoADatos.GetCadeteById(idNuevoCadete, "./data/datosCadetes.json");
            if (pedidoBuscado == null && cadeteBuscado == null) return NotFound("pedido y cadete no encontrados");
            if (pedidoBuscado == null) return NotFound("pedido no encontrado");
            if (cadeteBuscado == null) return NotFound("cadete no encontrado");
            var ListadoPedidos = accesoADatos.LeerPedidos("./data/datosPedidos.json");
            foreach (var pedido in ListadoPedidos)
            {
                if (pedido.numero == idPedido)
                {
                    pedido.cadete = cadeteBuscado;
                }
            }
            accesoADatos.escribirPedidosJSON(ListadoPedidos, "./data/datosPedidos.json");
            return Ok("Cadete asignado a Pedido correctamente");
        }
    }
}