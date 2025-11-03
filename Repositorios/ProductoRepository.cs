using Microsoft.Data.Sqlite;
using EspacioProductos;
public class ProductoRepository
{
    private string stringConnection = "Data Source = tienda.db";

    public void CrearProducto(Productos ProdInsertar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = "INSERT INTO Productos (idProducto, Descripcion, Precio) VALUES (@idProducto,@Descripcion,@Precio)";

        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idProducto", ProdInsertar.IdProducto));
        comando.Parameters.Add(new SqliteParameter("@Descripcion", ProdInsertar.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@Precio", ProdInsertar.Precio));

        comando.ExecuteNonQuery();
    }

    public bool ModificarProducto(int idBuscar, Productos prodActualizar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = "UPDATE Productos SET idProducto = @idProducto, Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @idBuscar";
        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idProducto", prodActualizar.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@Descripcion", prodActualizar.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@Precio", prodActualizar.Precio));
        comando.Parameters.Add(new SqliteParameter("@idBuscar", idBuscar));

        return comando.ExecuteNonQuery() > 0;
    }
    
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
                IdProducto = lector.GetInt32(lector.GetOrdinal("idProducto")),
                Descripcion = lector.GetString(lector.GetOrdinal("Descripcion")),
                Precio = lector.GetDouble(lector.GetOrdinal("Precio"))
            };
            productos.Add(p);
        }
        return productos;
    }


    public Productos? ObtenerPorId(int idBuscar)
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
                IdProducto = lector.GetInt32(lector.GetOrdinal("idProducto")),
                Descripcion = lector.GetString(lector.GetOrdinal("Descripcion")),
                Precio = lector.GetDouble(lector.GetOrdinal("Precio"))
            };
            return productoRetorno;
        }
        return null;
    }

    public bool EliminarProducto(int idBuscar)
    {
        using var conexion = new SqliteConnection(stringConnection);
        conexion.Open();

        string query = "DELETE FROM Productos WHERE idProducto = @idBuscar";
        using var comando = new SqliteCommand(query, conexion);

        comando.Parameters.Add(new SqliteParameter("@idBuscar", idBuscar));

        return comando.ExecuteNonQuery() > 0;   
    }
}