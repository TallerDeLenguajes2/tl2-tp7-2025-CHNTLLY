using Microsoft.Data.Sqlite;
using EspacioPresupuestos;
using EspacioPresupustosDetalle;
using EspacioProductos;
public class PresupuestoRepository
{
    private string stringConnection = "Data Source = tienda.db";

    public List<Presupuestos> GetPresupuestos()
    {
        List<Presupuestos> presupuestos = new List<Presupuestos>();
        string query = "SELECT * FROM presupuestos";

        using var conexion = new SqliteConnection(stringConnection);
        using var comando = new SqliteCommand(query, conexion);

        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            var p = new Presupuestos
            {
                idPresupuesto = lector.GetInt32(lector.GetOrdinal("idPresupuesto")),
                NombreDestinatario = lector.GetString(lector.GetOrdinal("Descripcion")),
                FechaCreacion = lector.GetDateTime(lector.GetOrdinal("FechaCreacion")),
                detalle = new List<PresupuestoDetalle>()
            };
            presupuestos.Add(p);
        }

        return presupuestos;
    }

    public void CrearPresupuesto(Presupuestos PresupInsertar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = "INSERT INTO Presupuestos (idPresupuesto, NombreDestinatario, FechaCreacion) VALUES (@idPresupuesto,@NombreDestinatario,@FechaCreacion)";

        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", PresupInsertar.idPresupuesto));
        comando.Parameters.Add(new SqliteParameter("@NombreDestinatario", PresupInsertar.NombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@FechaCreacion", PresupInsertar.FechaCreacion));

        comando.ExecuteNonQuery();
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
            presupuestoRetorno.idPresupuesto = lector.GetInt32(lector.GetOrdinal("idPresupuesto"));
            presupuestoRetorno.NombreDestinatario = lector.GetString(lector.GetOrdinal("NombreDestinatario"));
            presupuestoRetorno.FechaCreacion = lector.GetDateTime(lector.GetOrdinal("FechaCreacion"));
            presupuestoRetorno.detalle = new List<PresupuestoDetalle>();

            do //no considera que no haya detalles en este presupuesto 
            {
                PresupuestoDetalle p = new PresupuestoDetalle
                {
                    producto = new Productos
                    {
                        idProducto = lector.GetInt32(lector.GetOrdinal("idProducto")),
                        Descripcion = lector.GetString(lector.GetOrdinal("Descripcion")),
                        Precio = lector.GetDouble(lector.GetOrdinal("Precio"))
                    },
                    cantidad = lector.GetInt32(lector.GetOrdinal("Cantidad"))
                };
                presupuestoRetorno.detalle.Add(p);
            } while (lector.Read());
        }
        return (presupuestoRetorno);
    }
    public bool EliminarPresupuesto(int idBuscar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = "DELETE FROM Presupuestos WHERE idPresupuesto = @idBuscar";
        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idBuscar", idBuscar));

        return comando.ExecuteNonQuery() > 0;
    }

    public void AgregarPresupuesto(int idBuscar, int idProducto, int Cantidad)
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

        comando.ExecuteNonQuery();
    }
}