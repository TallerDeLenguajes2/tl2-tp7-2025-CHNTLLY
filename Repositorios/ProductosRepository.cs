using Microsoft.Data.Sqlite;
using EspacioProductos;
public class ProductoRepository
{
    private string stringConnection = "Data Source = tienda.db";

    public List<Productos> GetProductos()
    {
        List<Productos> productos = new List<Productos>();
        string query = "SELECT * FROM productos";

        using var conexion = new SqliteConnection(stringConnection);
        using var comando = new SqliteCommand(query, conexion);

        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            var p = new Productos
            {
                idProducto = lector.GetInt32(lector.GetOrdinal("idProducto")),
                Descripcion = lector.GetString(lector.GetOrdinal("Descripcion")),
                Precio = lector.GetDouble(lector.GetOrdinal("Precio"))
            };
            productos.Add(p);
        }

        return productos;
    }

    public void CrearProducto(Productos ProdInsertar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = "INSERT INTO productos (idProducto, Descripcion, Precio) VALUES (@idProducto,@Descripcion,@Precio)";

        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idProducto", ProdInsertar.idProducto));
        comando.Parameters.Add(new SqliteParameter("@Descripcion", ProdInsertar.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@Precio", ProdInsertar.Precio));

        comando.ExecuteNonQuery();
    }
    public void ActualizarProducto(int idBuscar, Productos prodActualizar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = "UPDATE Producto SET idProducto = @idProducto, Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @idBuscar";
        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idProducto", prodActualizar.idProducto));
        comando.Parameters.Add(new SqliteParameter("@Descripcion", prodActualizar.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@Precio", prodActualizar.Precio));
        comando.Parameters.Add(new SqliteParameter("@idBuscar", idBuscar));

        comando.ExecuteNonQuery();
    }

    public Productos ObtenerPorId(int idBuscar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        string query = "SELECT idProducto,Descripcion,Precio FROM productos WHERE idProducto = @idBuscar";
        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idBuscar", idBuscar));

        using var lector = comando.ExecuteReader();

        if (lector.Read())
        {
            Productos productoRetorno = new Productos
            {
                idProducto = lector.GetInt32(lector.GetOrdinal("idProducto")),
                Descripcion = lector.GetString(lector.GetOrdinal("Descripcion")),
                Precio = lector.GetDouble(lector.GetOrdinal("Precio"))
            };
            return productoRetorno;
        }
        return null;
    }

    public void EliminarProducto(int idBuscar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = "DELETE FROM productos WHERE idProducto = @idBuscar";
        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idBuscar", idBuscar));

        comando.ExecuteNonQuery();
        
    }

}