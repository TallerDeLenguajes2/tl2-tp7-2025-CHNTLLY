using EspacioPresupustosDetalle;
namespace EspacioPresupuestos
{
    public class Presupuestos
    {
        public int IdPresupuesto { get; set; }
        public string? NombreDestinatario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<PresupuestoDetalle>? Detalle { get; set; }
        double MontoPresupuesto()
        {
            double retorno = 0;
            if (Detalle == null) { return retorno; };
            for (int i = 0; i < Detalle.Count(); i++)
            {
                retorno += Detalle[i].Producto.Precio * Detalle[i].cantidad;
            }
            return (retorno);
        }
        double MontoPresupuestoConIva()
        {
            double retorno = 0;
            if (Detalle == null) { return retorno; };
            for (int i = 0; i < Detalle.Count(); i++)
            {
                retorno += Detalle[i].Producto.Precio * Detalle[i].cantidad;
            }
            retorno *= 1.21;
            return (retorno);
        }
        int CantidadProductos()
        {
            if (Detalle == null) { return 0; };
            return (Detalle.Count());
        }
    }
}