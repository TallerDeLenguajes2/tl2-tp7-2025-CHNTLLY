using Microsoft.Data.Sqlite;
using EspacioProductos;
public class ProductoRepository
{
    private string stringConnection = "Data Source = tienda.db";

    public bool CrearProducto(Productos ProdInsertar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = "INSERT INTO Productos (idProducto, Descripcion, Precio) VALUES (@idProducto,@Descripcion,@Precio)";

        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idProducto", ProdInsertar.IdProducto));
        comando.Parameters.Add(new SqliteParameter("@Descripcion", ProdInsertar.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@Precio", ProdInsertar.Precio));

        return comando.ExecuteNonQuery() > 0;
    }

    public bool ModificarProducto(int idBuscar, Productos prodActualizar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @idBuscar";
        using var comando = new SqliteCommand(query, conexion);

       //comando.Parameters.Add(new SqliteParameter("@idProducto", prodActualizar.IdProducto));
        comando.Parameters.Add(new SqliteParameter("@Descripcion", (object)prodActualizar.Descripcion ?? DBNull.Value));
        comando.Parameters.Add(new SqliteParameter("@Precio", prodActualizar.Precio));
        comando.Parameters.Add(new SqliteParameter("@idBuscar", idBuscar));

        return comando.ExecuteNonQuery() > 0;
    }
    
    public List<Productos> GetProductos()
    {
        List<Productos> productos = new List<Productos>();
        string query = "SELECT * FROM Productos";

        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();
        
        using var comando = new SqliteCommand(query, conexion);

        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            var p = new Productos
            {
                IdProducto = lector.GetInt32(lector.GetOrdinal("idProducto")),
                Descripcion = lector.GetString(lector.GetOrdinal("Descripcion")),
                Precio = lector.GetDecimal(lector.GetOrdinal("Precio"))
            };
            productos.Add(p);
        }
        return productos;
    }


    public Productos? ObtenerPorId(int idBuscar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();
        string query = "SELECT idProducto,Descripcion,Precio FROM Productos WHERE idProducto = @idBuscar";
        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idBuscar", idBuscar));

        using var lector = comando.ExecuteReader();

        if (lector.Read())
        {
            Productos productoRetorno = new Productos
            {
                IdProducto = lector.GetInt32(lector.GetOrdinal("idProducto")),
                Descripcion = lector.GetString(lector.GetOrdinal("Descripcion")),
                Precio = lector.GetDecimal(lector.GetOrdinal("Precio"))
            };
            return productoRetorno;
        }
        return null;
    }

    public bool EliminarProducto(int idBuscar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();
        using var transaccion = conexion.BeginTransaction(); //consultar
        try
        {
            string queryDetalles = "DELETE FROM PresupuestosDetalle WHERE idProducto = @idProducto";
            using (var comandoDetalles = new SqliteCommand(queryDetalles, conexion, transaccion))
            {
                comandoDetalles.Parameters.Add(new SqliteParameter("@idProducto",idBuscar));
                comandoDetalles.ExecuteNonQuery();
            }
            string queryProducto = "DELETE FROM Productos WHERE idProducto = @idProducto";
            int filasAfectadas = 0;
            using (var comandoProducto = new SqliteCommand(queryProducto, conexion, transaccion))
            {
                comandoProducto.Parameters.Add(new SqliteParameter("@idProducto", idBuscar));
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