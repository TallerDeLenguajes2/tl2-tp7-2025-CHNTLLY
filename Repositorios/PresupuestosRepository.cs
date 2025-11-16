using Microsoft.Data.Sqlite;
using EspacioPresupuestos;
using EspacioPresupustosDetalle;
using EspacioProductos;
public class PresupuestoRepository
{
    private string stringConnection = "Data Source = tienda.db";

    public bool CrearPresupuesto(Presupuestos PresupInsertar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = "INSERT INTO Presupuestos (idPresupuesto, NombreDestinatario, FechaCreacion) VALUES (@idPresupuesto,@NombreDestinatario,@FechaCreacion)";

        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", PresupInsertar.IdPresupuesto));
        comando.Parameters.Add(new SqliteParameter("@NombreDestinatario", PresupInsertar.NombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@FechaCreacion", PresupInsertar.FechaCreacion));

        return comando.ExecuteNonQuery() > 0;
    }
    
    public List<Presupuestos> GetPresupuestos()
    {
        List<Presupuestos> presupuestos = new List<Presupuestos>();
        string query = "SELECT * FROM presupuestos";

        using var conexion = new SqliteConnection(stringConnection);

        conexion.Open();

        using var comando = new SqliteCommand(query, conexion);

        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            var p = new Presupuestos
            {
                IdPresupuesto = lector.GetInt32(lector.GetOrdinal("idPresupuesto")),
                NombreDestinatario = lector.GetString(lector.GetOrdinal("NombreDestinatario")),
                FechaCreacion = lector.GetDateTime(lector.GetOrdinal("FechaCreacion")),
                Detalle = new List<PresupuestoDetalle>()
            };

            presupuestos.Add(p);
        }

        return presupuestos;
    }


    public Presupuestos ObtenerPorId(int idBuscar)                                          
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = @"SELECT p.idPresupuesto, p.NombreDestinatario, p.FechaCreacion,
                                pd.idProducto, pd.Cantidad,
                                pro.Descripcion, pro.Precio
                                FROM Presupuestos as p
                                INNER JOIN PresupuestosDetalle as pd 
                                ON pd.idPresupuesto = p.idPresupuesto
                                INNER JOIN Productos as pro
                                ON pro.idProducto = pd.idProducto
                                WHERE p.idPresupuesto = @idBuscar";

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.Add(new SqliteParameter("@idBuscar", idBuscar));
        using var lector = comando.ExecuteReader();

        Presupuestos presupuestoRetorno = new Presupuestos();

        if(lector.Read())
        {
            presupuestoRetorno.IdPresupuesto = lector.GetInt32(lector.GetOrdinal("idPresupuesto"));
            presupuestoRetorno.NombreDestinatario = lector.GetString(lector.GetOrdinal("NombreDestinatario"));
            presupuestoRetorno.FechaCreacion = lector.GetDateTime(lector.GetOrdinal("FechaCreacion"));
            presupuestoRetorno.Detalle = new List<PresupuestoDetalle>();

            do //no considera que no haya detalles en este presupuesto -- CORRECCIÓN -- solo trae cuando hay coincidencias al ser inner join
            {
                PresupuestoDetalle p = new PresupuestoDetalle
                {
                    Producto = new Productos
                    {
                        IdProducto = lector.GetInt32(lector.GetOrdinal("idProducto")),
                        Descripcion = lector.GetString(lector.GetOrdinal("Descripcion")),
                        Precio = lector.GetDecimal(lector.GetOrdinal("Precio"))
                    },
                    cantidad = lector.GetInt32(lector.GetOrdinal("Cantidad"))
                };
                presupuestoRetorno.Detalle.Add(p);
            } while (lector.Read());
        }
        return (presupuestoRetorno);
    }
    public bool AgregarPresupuesto(int idBuscar, int idProducto, int Cantidad)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = @"INSERT INTO PresupuestosDetalle
                         (idPresupuesto, idProducto, cantidad)
                         VALUES (@idPresupuesto, @idProducto, @Cantidad)";

        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idBuscar));
        comando.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
        comando.Parameters.Add(new SqliteParameter("@Cantidad", Cantidad));

        return comando.ExecuteNonQuery() > 0;
    }
    public bool EliminarPresupuesto(int idBuscar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();
        using var transaccion = conexion.BeginTransaction(); //consultar
        try
        {
            string queryDetalles = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto";
            using (var comandoDetalles = new SqliteCommand(queryDetalles, conexion, transaccion))
            {
                comandoDetalles.Parameters.Add(new SqliteParameter("@idPresupuesto",idBuscar));
                comandoDetalles.ExecuteNonQuery();
            }
            string queryPresupuesto = "DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";
            int filasAfectadas = 0;
            using (var comandoProducto = new SqliteCommand(queryPresupuesto, conexion, transaccion))
            {
                comandoProducto.Parameters.Add(new SqliteParameter("@idPresupuesto", idBuscar));
                filasAfectadas = comandoProducto.ExecuteNonQuery();
            }
            transaccion.Commit(); //no se ejecuta si alguna de las lineas antriores tuvo un error, por eso esta el transaccion.Rollback()
            return filasAfectadas > 0;
        }
        catch (Exception ex)
        {
            transaccion.Rollback(); //se ejecuta si algun ExecuteNonQuery falla
            return false;
        }
    }
}