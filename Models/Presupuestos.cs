using EspacioPresupustosDetalle;
namespace EspacioPresupuestos
{
    public class Presupuestos
    {
        public int idPresupuesto { get; set; }
        public string NombreDestinatario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<PresupuestoDetalle> detalle { get; set; }
        double MontoPresupuesto()
        {
            double retorno = 0;
            for(int i = 0 ; i < detalle.Count() ; i++)
            {
                retorno += detalle[i].producto.Precio;
            }
            return (retorno);
        }
        double MontoPresupuestoConIva()
        {
            double retorno = 0;
            for (int i = 0; i < detalle.Count(); i++)
            {
                retorno += detalle[i].producto.Precio;
            }
            retorno *= 1.21;
            return (retorno);
        }
        int CantidadProductos()
        {
            return (detalle.Count());
        }
    }
}