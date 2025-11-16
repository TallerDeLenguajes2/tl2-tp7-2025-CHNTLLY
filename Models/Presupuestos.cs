using EspacioPresupustosDetalle;
namespace EspacioPresupuestos
{
    public class Presupuestos
    {
        public int IdPresupuesto { get; set; }
        public string? NombreDestinatario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<PresupuestoDetalle>? Detalle { get; set; }
        decimal MontoPresupuesto()
        {
            decimal retorno = 0;
            if (Detalle == null) { return retorno; };
            for (int i = 0; i < Detalle.Count(); i++)
            {
                if(Detalle[i].Producto != null)
                {
                    retorno += Detalle[i].Producto.Precio * Detalle[i].cantidad;
                }
            }
            return (retorno);
        }
        decimal MontoPresupuestoConIva()
        {
            decimal retorno = 0;
            if (Detalle == null) { return retorno; };
            for (int i = 0; i < Detalle.Count(); i++)
            {
                if(Detalle[i].Producto != null)
                {
                    retorno += Detalle[i].Producto.Precio * Detalle[i].cantidad;
                }
            }
            retorno *= Convert.ToDecimal(1.21);
            return (retorno);
        }
        int CantidadProductos()
        {
            int retorno = 0;
            if (Detalle == null) { return 0; };
            for (int i = 0 ; i < Detalle.Count() ; i++)
            {
                retorno += Detalle[i].cantidad;
            }
            return(retorno);
        }
    }
}