using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace AsistenciaTecnica
{
    class ServicioBaseDatos
    {
        const int MAX_SESIONES_POR_SALA = 3;
        private readonly SqlConnection conexion;
        public SqlCommand comando;

        public ServicioBaseDatos()
        {
            try
            {
                conexion = new SqlConnection(@"Server=(local);Database=dbasistencia;Trusted_Connection=Yes;MultipleActiveResultSets=True");

            }
            catch (Exception ex)
            {
                throw new MisExcepciones(ex.Message + "\nNo se ha podido establecer conexión con la base de datos");
            }
        }
        public ObservableCollection<Sala> ObtenerSalas(bool soloDisponibles, bool insertarFilaVacia)
        {
            ObservableCollection<Sala> salas = new ObservableCollection<Sala>();
            if (insertarFilaVacia)
            {
                salas.Add(new Sala());
                salas[0].NUMERO = "Todas";
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * from salas ";
            // Si solo disponibles, además comprobaremos que no tengan más de MAX_SESIONES_POR_SALA
            if (soloDisponibles)
            {
                comando.CommandText += "WHERE disponible = 1 " +
                                       "and (select count(*) from sesiones where sala = idSala) < " + MAX_SESIONES_POR_SALA;
            }
            comando.CommandText += " ORDER BY numero";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    salas.Add(new Sala(lector.GetInt32(0), lector.GetString(1),
                                       lector.GetInt32(2), lector.GetBoolean(3)));
                }
            }
            lector.Close();
            conexion.Close();
            return salas;
        }
        public void InsertarSala(Sala salaFormulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "INSERT INTO Salas VALUES(@numero,@capacidad,@disponible)";
            comando.Parameters.Add("@numero", SqlDbType.NVarChar);
            comando.Parameters["@numero"].Value = salaFormulario.NUMERO;
            comando.Parameters.Add("@capacidad", SqlDbType.Int);
            comando.Parameters["@capacidad"].Value = salaFormulario.CAPACIDAD;
            comando.Parameters.Add("@disponible", SqlDbType.Int);
            comando.Parameters["@disponible"].Value = salaFormulario.DISPONIBLE;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void ActualizarSala(Sala salaFormulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "UPDATE salas SET numero = @numero, capacidad = @capacidad, disponible = @disponible" +
                " WHERE idSala = @idSala";
            comando.Parameters.Add("@idSala", SqlDbType.Int);
            comando.Parameters["@idSala"].Value = salaFormulario.IDSALA;
            comando.Parameters.Add("@numero", SqlDbType.NVarChar);
            comando.Parameters["@numero"].Value = salaFormulario.NUMERO;
            comando.Parameters.Add("@capacidad", SqlDbType.Int);
            comando.Parameters["@capacidad"].Value = salaFormulario.CAPACIDAD;
            comando.Parameters.Add("@disponible", SqlDbType.Int);
            comando.Parameters["@disponible"].Value = salaFormulario.DISPONIBLE;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        // No puede haber dos salas con el mismo número
        public bool ExisteSala(Sala salaFormulario)
        {
            bool existe = false;
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT numero FROM salas WHERE numero = @numero AND idSala <> @idSala";
            comando.Parameters.Add("@numero", SqlDbType.NVarChar);
            comando.Parameters["@numero"].Value = salaFormulario.NUMERO;
            comando.Parameters.Add("@idSala", SqlDbType.Int);
            comando.Parameters["@idSala"].Value = salaFormulario.IDSALA;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read() && !existe)
                {
                    existe = true;
                }
            }
            lector.Close();
            conexion.Close();
            return existe;
        }

        public ObservableCollection<string> ObtenerDatosFiltro(string campo)
        {
            ObservableCollection<string> datos = new ObservableCollection<string>();
            datos.Add("Todas");
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "select " + campo + " from peliculas " +
                      "GROUP BY " + campo +
                      " ORDER BY " + campo;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    datos.Add(lector.GetString(0));
                }
            }
            lector.Close();
            conexion.Close();
            return datos;
        }

        public bool ExisteProvincia(Provincia formulario)
        {
            bool existe = false;
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT idProvincia FROM provincias WHERE idProvincia = @idProvincia";
            comando.Parameters.Add("@idProvincia", SqlDbType.NVarChar);
            comando.Parameters["@idProvincia"].Value = formulario.IDPROVINCIA;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read() && !existe)
                {
                    existe = true;
                }
            }
            lector.Close();
            conexion.Close();
            return existe;
        }
        public int BuscarUsuario(string login, string password)
        {
            int retorno = 0;
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "select count(*) from usuarios where login = @login " +
                                  "and password = HASHBYTES('SHA2_512', @password)";
            comando.Parameters.Add("@login", SqlDbType.NVarChar);
            comando.Parameters["@login"].Value = login;
            comando.Parameters.Add("@password", SqlDbType.VarChar);
            comando.Parameters["@password"].Value = password;
            if ((int)comando.ExecuteScalar() > 0)
                retorno = 1;
            conexion.Close();
            return retorno;
        }
        public Usuario LeerUsuario(string login)
        {
            Usuario usuario = new Usuario();
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "select * from usuarios where login = @login";
            comando.Parameters.Add("@login", SqlDbType.VarChar);
            comando.Parameters["@login"].Value = login;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    usuario.LOGIN = lector.GetString(0);
                    usuario.EMPLEADO = lector.GetInt32(2);
                }
            }
            conexion.Close();
            return usuario;
        }
        public Empleado BuscarEmpleado(int idEmpleado)
        {
            Empleado empleado = new Empleado();
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "select * from empleados where idEmpleado = @idEmpleado";
            comando.Parameters.Add("@idEmpleado", SqlDbType.VarChar);
            comando.Parameters["@idEmpleado"].Value = idEmpleado;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    empleado.IDEMPLEADO = lector.GetInt32(0);
                    empleado.NOMBRE = lector.GetString(1);
                    empleado.APELLIDOS = lector.GetString(2);
                }
            }
            conexion.Close();
            return empleado;
        }

        public ObservableCollection<Cargo> ObtenerCargos(bool insertarFilaVacia)
        {
            ObservableCollection<Cargo> cargos = new ObservableCollection<Cargo>();
            if (insertarFilaVacia)
            {
                cargos.Add(new Cargo());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * from cargos";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    cargos.Add(new Cargo(lector.GetInt32(0), lector.GetString(1)));
                }
            }
            lector.Close();
            conexion.Close();
            return cargos;
        }
        public ObservableCollection<Departamento> ObtenerDepartamentos(bool insertarFilaVacia)
        {
            ObservableCollection<Departamento> departamentos = new ObservableCollection<Departamento>();
            if (insertarFilaVacia)
            {
                departamentos.Add(new Departamento());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * from departamentos ORDER BY nombre";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    departamentos.Add(new Departamento(lector.GetInt32(0), lector.GetString(1)));
                }
            }
            lector.Close();
            conexion.Close();
            return departamentos;
        }
        public void InsertarDepartamento(Departamento deparatmentoFormulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "INSERT INTO departamentos VALUES(@nombre)";
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar);
            comando.Parameters["@nombre"].Value = deparatmentoFormulario.NOMBRE;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void ActualizarDepartamento(Departamento deparatmentoFormulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "UPDATE departamentos SET nombre = @nombre" +
                                  " WHERE idDepartamento = @idDepartamento";
            comando.Parameters.Add("@idDepartamento", SqlDbType.Int);
            comando.Parameters["@idDepartamento"].Value = deparatmentoFormulario.IDDEPARTAMENTO;
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar);
            comando.Parameters["@nombre"].Value = deparatmentoFormulario.NOMBRE;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void BorrarDepartamento(Departamento departamentoFormulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "DELETE FROM departamentos WHERE idDepartamento = @idDepartamento";
            comando.Parameters.Add("@idDepartamento", SqlDbType.Int);
            comando.Parameters["@idDepartamento"].Value = departamentoFormulario.IDDEPARTAMENTO;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void InsertarCargo(Cargo cargoFormulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "INSERT INTO cargos VALUES(@nombre)";
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar);
            comando.Parameters["@nombre"].Value = cargoFormulario.NOMBRE;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void ActualizarCargo(Cargo cargoFormulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "UPDATE cargos SET nombre = @nombre" +
                                  " WHERE idCargo = @idCargo";
            comando.Parameters.Add("@idCargo", SqlDbType.Int);
            comando.Parameters["@idCargo"].Value = cargoFormulario.IDCARGO;
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar);
            comando.Parameters["@nombre"].Value = cargoFormulario.NOMBRE;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void BorrarCargo(Cargo cargoFormulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "DELETE FROM cargos WHERE idCargo = @idCargo";
            comando.Parameters.Add("@idCargo", SqlDbType.Int);
            comando.Parameters["@idCargo"].Value = cargoFormulario.IDCARGO;
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        public void InsertarPerfil(Perfil formulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "INSERT INTO perfiles VALUES(@nombre,@descripcionAmpliada)";
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar);
            comando.Parameters["@nombre"].Value = formulario.NOMBRE;
            comando.Parameters.Add("@descripcionAmpliada", SqlDbType.NVarChar);
            comando.Parameters["@descripcionAmpliada"].Value = formulario.NOMBRE;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void ActualizarPerfil(Perfil formulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "UPDATE perfiles SET nombre = @nombre, descripcionAmpliada = @descripcionAmpliada" +
                                  " WHERE idPerfil = @idPerfil";
            comando.Parameters.Add("@idPerfil", SqlDbType.Int);
            comando.Parameters["@idPerfil"].Value = formulario.IDPERFIL;
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar);
            comando.Parameters["@nombre"].Value = formulario.NOMBRE;
            comando.Parameters.Add("@descripcionAmpliada", SqlDbType.NVarChar);
            comando.Parameters["@descripcionAmpliada"].Value = formulario.DESCRIPCIONAMPLIADA;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void BorrarPerfil(Perfil formulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "DELETE FROM perfiles WHERE idPerfil = @idPerfil";
            comando.Parameters.Add("@idPerfil", SqlDbType.Int);
            comando.Parameters["@idPerfil"].Value = formulario.IDPERFIL;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public ObservableCollection<Perfil> ObtenerPerfiles(bool insertarFilaVacia)
        {
            ObservableCollection<Perfil> perfiles = new ObservableCollection<Perfil>();
            if (insertarFilaVacia)
            {
                perfiles.Add(new Perfil());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * from perfiles";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    perfiles.Add(new Perfil(lector.GetInt32(0), lector.GetString(1), lector.GetString(2)));
                }
            }
            lector.Close();
            conexion.Close();
            return perfiles;
        }
        public void InsertarProvincia(Provincia formulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "INSERT INTO provincias VALUES(@idProvincia,@nombre)";
            comando.Parameters.Add("@idProvincia", SqlDbType.NVarChar);
            comando.Parameters["@idProvincia"].Value = formulario.IDPROVINCIA;
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar);
            comando.Parameters["@nombre"].Value = formulario.NOMBRE;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void ActualizarProvincia(Provincia formulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "UPDATE provincias SET nombre = @nombre" +
                                  " WHERE idProvincia = @idProvincia";
            comando.Parameters.Add("@idProvincia", SqlDbType.Int);
            comando.Parameters["@idProvincia"].Value = formulario.IDPROVINCIA;
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar);
            comando.Parameters["@nombre"].Value = formulario.NOMBRE;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void BorrarProvincia(Provincia formulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "DELETE FROM provincias WHERE idProvincia = @idProvincia";
            comando.Parameters.Add("@idProvincia", SqlDbType.Int);
            comando.Parameters["@idProvincia"].Value = formulario.IDPROVINCIA;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public ObservableCollection<Provincia> ObtenerProvincias(bool insertarFilaVacia)
        {
            ObservableCollection<Provincia> provincias = new ObservableCollection<Provincia>();
            if (insertarFilaVacia)
            {
                provincias.Add(new Provincia());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * from provincias";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    provincias.Add(new Provincia(lector.GetString(0), lector.GetString(1)));
                }
            }
            lector.Close();
            conexion.Close();
            return provincias;
        }

        public ObservableCollection<Empleado> ObtenerEmpleados(bool insertarFilaVacia)
        {
            ObservableCollection<Empleado> empleados = new ObservableCollection<Empleado>();
            if (insertarFilaVacia)
            {
                empleados.Add(new Empleado());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT idEmpleado,em.nombre,apellidos,telefono,direccion,poblacion," +
                "codigoPostal," +
                "provincia,pr.nombre," +
                "mail," +
                "cargo,ca.nombre," +
                "departamento,de.nombre,imagen " +
                "from empleados em JOIN departamentos de " +
                 " ON em.departamento = de.idDepartamento" +
                " JOIN provincias pr " +
                 " ON em.provincia = pr.idProvincia" +
                " JOIN cargos ca" +
                " ON em.cargo = ca.idCargo";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    string imagen;
                    if (lector.IsDBNull(14))
                        imagen = "";
                    else
                        imagen = lector.GetString(14);
                    empleados.Add(new Empleado(lector.GetInt32(0), lector.GetString(1), lector.GetString(2),
                        lector.GetString(3), lector.GetString(4), lector.GetString(5), lector.GetString(6),
                        new Provincia(lector.GetString(7), lector.GetString(8)),
                        lector.GetString(9),
                        new Cargo(lector.GetInt32(10), lector.GetString(11)),
                        new Departamento(lector.GetInt32(12), lector.GetString(13)), imagen));
                }
            }
            lector.Close();
            conexion.Close();
            return empleados;
        }
    }
}
