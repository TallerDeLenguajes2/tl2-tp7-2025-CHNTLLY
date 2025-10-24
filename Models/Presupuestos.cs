using EspacioPresupustosDetalle;
namespace EspacioPresupuestos
{
    public class Presupuestos
    {
        int idProducto { get; set; }
        string nombreDestinatario { get; set; }
        DateTime FechaCreacion;
        List<PresupuestoDetalle> detalle;
        double MontoPresupuesto()
        {
            return (0);
        }
        double MontoPresupuestoConIva()
        {
            return (0);
        }
        int CantidadProductos()
        {
            return (0);
        }
    }
}