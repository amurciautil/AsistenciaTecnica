using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace AsistenciaTecnica
{
    class ServicioBaseDatos
    {
        private readonly SqlConnection conexion;
        public SqlCommand comando;
        public int situacionCierre = Properties.Settings.Default.situacionCierre;
        public ServicioBaseDatos()
        {
            try
            {
                string server = Properties.Settings.Default.servidorBBDD;
                string user = Properties.Settings.Default.usuarioBBDD;
                string password = Properties.Settings.Default.claveBBDD;
                string database = Properties.Settings.Default.databaseBBDD;
                string connString;
                //server    dataserverincidencias.database.windows.net
                //user      adminserver
                //password  Damfp2019
                //database  bdproyecto
                if (server == "local")
                {
                    conexion = new SqlConnection(@"Server=(local);Database=dbasistencia;Trusted_Connection=Yes;MultipleActiveResultSets=True");
                }
                else
                {
                    connString = $"server={server};user id={user};password={password};database={database};";
                    conexion = new SqlConnection(connString);
                }
            }
            catch (Exception ex)
            {
                throw new MisExcepciones(ex.Message + "\nNo se ha podido establecer conexión con la base de datos");
            }
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
        public int BuscarUsuario(string login, string password)
        {
            try
            {
                int retorno = 0;
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "select count(*) from usuarios where login = @login " +
                                "and password = HASHBYTES('SHA2_512', '" + password + "')";
                comando.Parameters.Add("@login", SqlDbType.NVarChar);
                comando.Parameters["@login"].Value = login;
                if ((int)comando.ExecuteScalar() > 0)
                    retorno = 1;
                conexion.Close();
                return retorno;
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public Usuario LeerUsuario(string login)
        {
            Usuario usuario = new Usuario();
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "select " +
                "login," +
                "perfil," +
                "empleado," +
                "activo," +
                "em.nombre," +
                "em.apellidos, " +
                "pe.descripcion from usuarios us" +
                " JOIN empleados em ON us.empleado = em.idEmpleado" +
                " JOIN perfiles pe ON us.perfil = pe.idPerfil" +
                " where login = @login";
            comando.Parameters.Add("@login", SqlDbType.VarChar);
            comando.Parameters["@login"].Value = login;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    usuario.LOGIN = lector.GetString(0);
                    usuario.IDEMPLEADO = lector.GetInt32(2);
                    usuario.PERFIL = new Perfil(lector.GetInt32(1), lector.GetString(6));
                    usuario.EMPLEADO = new Empleado(lector.GetInt32(2), lector.GetString(4), lector.GetString(5));
                    usuario.ACTIVO = lector.GetBoolean(3);
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

        public ObservableCollection<TablaAyuda> ObtenerTablaAyuda(bool insertarFilaVacia)
        {
            ObservableCollection<TablaAyuda> lista = new ObservableCollection<TablaAyuda>();
            if (insertarFilaVacia)
            {
                lista.Add(new TablaAyuda());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * from ayuda";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    lista.Add(new TablaAyuda(lector.GetString(0), lector.GetString(1), lector.GetString(2)));
                }
            }
            lector.Close();
            conexion.Close();
            return lista;
        }
        public void InsertarTablaAyuda(TablaAyuda formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                string ampliacion;
                if (formulario.AMPLIACION != null)
                    ampliacion = formulario.AMPLIACION;
                else
                    ampliacion = "";
                comando.CommandText = "INSERT INTO ayuda VALUES(@codigo,@descripcion,@ampliacion)";
                comando.Parameters.Add("@codigo", SqlDbType.VarChar);
                comando.Parameters["@codigo"].Value = formulario.CODIGO;
                comando.Parameters.Add("@descripcion", SqlDbType.NVarChar);
                comando.Parameters["@descripcion"].Value = formulario.DESCRIPCION;
                comando.Parameters.Add("@ampliacion", SqlDbType.NVarChar);
                comando.Parameters["@ampliacion"].Value = ampliacion;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void ActualizarTablaAyuda(TablaAyuda formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "UPDATE ayuda SET descripcion = @descripcion," +
                                      "ampliacion = @ampliacion" +
                                      " WHERE codigo = @codigo";
                comando.Parameters.Add("@codigo", SqlDbType.VarChar);
                comando.Parameters["@codigo"].Value = formulario.CODIGO;
                comando.Parameters.Add("@descripcion", SqlDbType.NVarChar);
                comando.Parameters["@descripcion"].Value = formulario.DESCRIPCION;
                comando.Parameters.Add("@ampliacion", SqlDbType.NVarChar);
                comando.Parameters["@ampliacion"].Value = formulario.AMPLIACION;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
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
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "INSERT INTO departamentos VALUES(@nombre)";
                comando.Parameters.Add("@nombre", SqlDbType.NVarChar);
                comando.Parameters["@nombre"].Value = deparatmentoFormulario.NOMBRE;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void ActualizarDepartamento(Departamento deparatmentoFormulario)
        {
            try
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
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void BorrarDepartamento(Departamento departamentoFormulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM departamentos WHERE idDepartamento = @idDepartamento";
                comando.Parameters.Add("@idDepartamento", SqlDbType.Int);
                comando.Parameters["@idDepartamento"].Value = departamentoFormulario.IDDEPARTAMENTO;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
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
        public void InsertarCargo(Cargo cargoFormulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "INSERT INTO cargos VALUES(@nombre)";
                comando.Parameters.Add("@nombre", SqlDbType.NVarChar);
                comando.Parameters["@nombre"].Value = cargoFormulario.NOMBRE;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void ActualizarCargo(Cargo cargoFormulario)
        {
            try
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
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void BorrarCargo(Cargo cargoFormulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM cargos WHERE idCargo = @idCargo";
                comando.Parameters.Add("@idCargo", SqlDbType.Int);
                comando.Parameters["@idCargo"].Value = cargoFormulario.IDCARGO;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }

        public void InsertarPerfil(Perfil formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "INSERT INTO perfiles VALUES(@descripcion,@descripcionAmpliada)";
                comando.Parameters.Add("@descripcion", SqlDbType.NVarChar);
                comando.Parameters["@descripcion"].Value = formulario.NOMBRE;
                comando.Parameters.Add("@descripcionAmpliada", SqlDbType.NVarChar);
                comando.Parameters["@descripcionAmpliada"].Value = formulario.NOMBRE;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void ActualizarPerfil(Perfil formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "UPDATE perfiles SET descripcion = @descripcion, descripcionAmpliada = @descripcionAmpliada" +
                                      " WHERE idPerfil = @idPerfil";
                comando.Parameters.Add("@idPerfil", SqlDbType.Int);
                comando.Parameters["@idPerfil"].Value = formulario.IDPERFIL;
                comando.Parameters.Add("@descripcion", SqlDbType.NVarChar);
                comando.Parameters["@descripcion"].Value = formulario.NOMBRE;
                comando.Parameters.Add("@descripcionAmpliada", SqlDbType.NVarChar);
                comando.Parameters["@descripcionAmpliada"].Value = formulario.DESCRIPCIONAMPLIADA;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void BorrarPerfil(Perfil formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM perfiles WHERE idPerfil = @idPerfil";
                comando.Parameters.Add("@idPerfil", SqlDbType.Int);
                comando.Parameters["@idPerfil"].Value = formulario.IDPERFIL;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
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
            try
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
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void ActualizarProvincia(Provincia formulario)
        {
            try
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
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void BorrarProvincia(Provincia formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM provincias WHERE idProvincia = @idProvincia";
                comando.Parameters.Add("@idProvincia", SqlDbType.Int);
                comando.Parameters["@idProvincia"].Value = formulario.IDPROVINCIA;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
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

        public ObservableCollection<Empleado> ObtenerEmpleados(bool insertarFilaVacia, int departamento)
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
            if (departamento > 0)
                comando.CommandText += " WHERE departamento = " + departamento;
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
        public void BorrarEmpleado(Empleado seleccionado)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM empleados WHERE idEmpleado = @idEmpleado";
                comando.Parameters.Add("@idEmpleado", SqlDbType.Int);
                comando.Parameters["@idEmpleado"].Value = seleccionado.IDEMPLEADO;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void InsertarEmpleado(Empleado formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "INSERT INTO empleados " +
                        "VALUES(@nombre,@apellidos,@telefono,@direccion,@poblacion," +
                               "@codigoPostal,@provincia,@mail,@cargo,@departamento," +
                               "@imagen)";
                comando = PreparaDatosEmpleado(comando, formulario);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void ActualizarEmpleado(Empleado formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "UPDATE empleados SET " +
                    "nombre = @nombre," +
                    "apellidos = @apellidos," +
                    "telefono = @telefono," +
                    "direccion = @direccion," +
                    "poblacion = @poblacion," +
                    "codigoPostal = @codigoPostal," +
                    "provincia = @provincia," +
                    "mail = @mail," +
                    "cargo = @cargo," +
                    "departamento = @departamento," +
                    "imagen = @imagen" +
                    " WHERE idEmpleado = @idEmpleado";
                comando = PreparaDatosEmpleado(comando, formulario);
                comando.Parameters.Add("@idEmpleado", SqlDbType.Int);
                comando.Parameters["@idEmpleado"].Value = formulario.IDEMPLEADO;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public SqlCommand PreparaDatosEmpleado(SqlCommand com, Empleado formulario)
        {
            SqlCommand cmd = com;
            // Es necesario controlar los nulos para grabar "" si el valor capturado en pantalla es nulo.
            string apellidos, telefono, direccion, poblacion, codigoPostal, mail, imagen;
            if (formulario.APELLIDOS == null)
                apellidos = "";
            else
                apellidos = formulario.APELLIDOS;
            if (formulario.TELEFONO == null)
                telefono = "";
            else
                telefono = formulario.TELEFONO;
            if (formulario.DIRECCION == null)
                direccion = "";
            else
                direccion = formulario.DIRECCION;
            if (formulario.POBLACION == null)
                poblacion = "";
            else
                poblacion = formulario.POBLACION;
            if (formulario.CODIGOPOSTAL == null)
                codigoPostal = "";
            else
                codigoPostal = formulario.CODIGOPOSTAL;
            if (formulario.MAIL == null)
                mail = "";
            else
                mail = formulario.MAIL;
            if (formulario.IMAGEN == null)
                imagen = "";
            else
                imagen = formulario.IMAGEN;
            cmd.Parameters.Add("@nombre", SqlDbType.NVarChar);
            cmd.Parameters["@nombre"].Value = formulario.NOMBRE;
            cmd.Parameters.Add("@apellidos", SqlDbType.NVarChar);
            cmd.Parameters["@apellidos"].Value = apellidos;
            cmd.Parameters.Add("@telefono", SqlDbType.NVarChar);
            cmd.Parameters["@telefono"].Value = telefono;
            cmd.Parameters.Add("@direccion", SqlDbType.NVarChar);
            cmd.Parameters["@direccion"].Value = direccion;
            cmd.Parameters.Add("@poblacion", SqlDbType.NVarChar);
            cmd.Parameters["@poblacion"].Value = poblacion;
            cmd.Parameters.Add("@codigoPostal", SqlDbType.NVarChar);
            cmd.Parameters["@codigoPostal"].Value = codigoPostal;
            cmd.Parameters.Add("@provincia", SqlDbType.NVarChar);
            cmd.Parameters["@provincia"].Value = ObtenerClavesPrimarias("idProvincia", "provincias", "nombre",
                                                                        formulario.NOMBREPROVINCIA,
                                                                        "S", false);
            cmd.Parameters.Add("@mail", SqlDbType.NVarChar);
            cmd.Parameters["@mail"].Value = mail;
            cmd.Parameters.Add("@cargo", SqlDbType.NVarChar);
            cmd.Parameters["@cargo"].Value = ObtenerClavesPrimarias("idCargo", "cargos", "nombre",
                                                                        formulario.NOMBRECARGO,
                                                                        "I", false);
            cmd.Parameters.Add("@departamento", SqlDbType.NVarChar);
            cmd.Parameters["@departamento"].Value = ObtenerClavesPrimarias("idDepartamento", "departamentos", "nombre",
                                                                        formulario.NOMBREDEPARTAMENTO,
                                                                        "I", false);
            cmd.Parameters.Add("@imagen", SqlDbType.NVarChar);
            cmd.Parameters["@imagen"].Value = imagen;
            return cmd;
        }
        // Método para obtener la clave primaria dado el valor seleccionado en un combobox
        // Recibe: 
        // nombre del campo de la clave primaria
        // nombre de la tabla
        // nombre del campo para la condición WHERE
        // valor a buscar
        // tipo de variable del campo de la clave primaria (I integer, S String)
        // abrimos conexión BBDD: true / false. No abrir conexión si ya tenemos una abierta
        public string ObtenerClavesPrimarias(string codigo, string tabla, string nombre,
                                             string valor, string tipoclave, bool openconexion)
        {
            string clavePrimaria = "";
            if (openconexion)
                conexion.Open();
            SqlCommand cmd;
            cmd = conexion.CreateCommand();
            cmd.CommandText = "Select TOP 1 " + codigo + " from " + tabla + " where " + nombre + " = @" +
                nombre;
            cmd.Parameters.Add("@" + nombre, SqlDbType.NVarChar);
            cmd.Parameters["@" + nombre].Value = valor;
            SqlDataReader lector = cmd.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    switch (tipoclave)
                    {
                        case "I":
                            clavePrimaria = lector.GetInt32(0).ToString();
                            break;
                        case "S":
                            clavePrimaria = lector.GetString(0);
                            break;
                        default:
                            clavePrimaria = lector.GetString(0);
                            break;
                    }
                }
            }
            lector.Close();
            if (openconexion)
                conexion.Close();
            return clavePrimaria;
        }
        public ObservableCollection<Usuario> ObtenerUsuarios(bool insertarFilaVacia)
        {
            ObservableCollection<Usuario> usuarios = new ObservableCollection<Usuario>();
            if (insertarFilaVacia)
            {
                usuarios.Add(new Usuario());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "select " +
                "login," +
                "perfil," +
                "empleado," +
                "activo," +
                "em.nombre," +
                "em.apellidos, " +
                "pe.descripcion from usuarios us" +
                " JOIN empleados em ON us.empleado = em.idEmpleado" +
                " JOIN perfiles pe ON us.perfil = pe.idPerfil";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    usuarios.Add(new Usuario(lector.GetString(0), "",
                                 new Perfil(lector.GetInt32(1), lector.GetString(6)),
                                 new Empleado(lector.GetInt32(2), lector.GetString(4), lector.GetString(5)),
                                 lector.GetBoolean(3)));
                }
            }
            conexion.Close();
            return usuarios;
        }
        public void BorrarUsuario(Usuario seleccionado)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM usuarios WHERE login = @login";
                comando.Parameters.Add("@login", SqlDbType.NVarChar);
                comando.Parameters["@login"].Value = seleccionado.LOGIN;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void InsertarUsuario(Usuario formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "INSERT INTO usuarios " +
                        "VALUES(@login,HASHBYTES('SHA2_512', '" + formulario.PASSWORD + "'),@perfil,@empleado,@activo)";
                comando = PreparaDatosUsuario(comando, formulario);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void ActualizarUsuario(Usuario formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "UPDATE usuarios SET " +
                    "activo = @activo," +
                    "perfil = @perfil," +
                    "empleado = @empleado" +
                    " WHERE login = @login";
                comando = PreparaDatosUsuario(comando, formulario);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public SqlCommand PreparaDatosUsuario(SqlCommand com, Usuario formulario)
        {
            SqlCommand cmd = com;
            com.Parameters.Add("@login", SqlDbType.NVarChar);
            com.Parameters["@login"].Value = formulario.LOGIN;
            cmd.Parameters.Add("@perfil", SqlDbType.NVarChar);
            cmd.Parameters["@perfil"].Value = ObtenerClavesPrimarias("idPerfil", "perfiles", "descripcion",
                                                                        formulario.NOMBREPERFIL,
                                                                        "I", false);
            cmd.Parameters.Add("@empleado", SqlDbType.Int);
            cmd.Parameters["@empleado"].Value = formulario.IDEMPLEADO;
            cmd.Parameters.Add("@activo", SqlDbType.Int);
            cmd.Parameters["@activo"].Value = formulario.ACTIVO;
            return cmd;
        }
        public void ActualizarPassword(string login, string password)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "UPDATE usuarios SET " +
                "password = HASHBYTES('SHA2_512', '" + password + "')" +
                " WHERE login = @login";
            comando.Parameters.Add("@login", SqlDbType.NVarChar);
            comando.Parameters["@login"].Value = login;
        //    comando.Parameters.Add("@password", SqlDbType.NVarChar);
        //    comando.Parameters["@password"].Value = password;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public SqlBinary BuscarPassword(string login)
        {
            SqlBinary palabra;
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT password from usuarios " +
                                 " WHERE login = @login";
            comando.Parameters.Add("@login", SqlDbType.NVarChar);
            comando.Parameters["@login"].Value = login;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    palabra = lector.GetSqlBinary(0);
                }
            }
            conexion.Close();
            return palabra;
        }
        public ObservableCollection<Producto> ObtenerProductos(bool insertarFilaVacia, bool soloActivos)
        {
            ObservableCollection<Producto> productos = new ObservableCollection<Producto>();
            if (insertarFilaVacia)
            {
                productos.Add(new Producto());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT pr.codigo," +
                "pr.descripcion," +
                "pr.descripcionAmpliada," +
                "pr.precioVenta," +
                "pr.tipoProducto," +
                "pr.controlExistencias," +
                "pr.existencias," +
                "pr.activo," +
                "tp.nombre" +
                " from productos pr JOIN tipoproducto tp " +
                 " ON pr.tipoProducto = tp.idTipo";
            if (soloActivos)
                comando.CommandText += " WHERE activo = 1";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    productos.Add(new Producto(lector.GetString(0),
                        lector.GetString(1),
                        lector.GetString(2),
                        (double)lector.GetDecimal(3),
                        new TipoProducto(lector.GetInt32(4), lector.GetString(8)),
                        lector.GetBoolean(5),
                        (double)lector.GetDecimal(6),
                        lector.GetBoolean(7)));
                }
            }
            lector.Close();
            conexion.Close();
            return productos;
        }
        public void BorrarProducto(Producto seleccionado)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM productos WHERE codigo = @codigo";
                comando.Parameters.Add("@codigo", SqlDbType.VarChar);
                comando.Parameters["@codigo"].Value = seleccionado.IDCODIGO;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public ObservableCollection<TipoProducto> ObtenerTipoProducto(bool insertarFilaVacia)
        {
            ObservableCollection<TipoProducto> lista = new ObservableCollection<TipoProducto>();
            if (insertarFilaVacia)
            {
                lista.Add(new TipoProducto());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * from tipoProducto";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    lista.Add(new TipoProducto(lector.GetInt32(0), lector.GetString(1)));
                }
            }
            lector.Close();
            conexion.Close();
            return lista;
        }
        public void InsertarProducto(Producto formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "INSERT INTO productos " +
                        "VALUES(@codigo,@descripcion,@descripcionAmpliada,@precioVenta,@tipoProducto,@controlExistencias," +
                        "@existencias,@activo)";
                comando = PreparaDatosProducto(comando, formulario);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void ActualizarProducto(Producto formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "UPDATE productos SET " +
                    "descripcion = @descripcion," +
                    "descripcionAmpliada = @descripcionAmpliada," +
                    "precioVenta = @precioVenta," +
                    "tipoProducto = @tipoProducto," +
                    "controlExistencias = @controlExistencias," +
                    "existencias = @existencias," +
                    "activo = @activo" +
                    " WHERE codigo = @codigo";
                comando = PreparaDatosProducto(comando, formulario);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }

        public SqlCommand PreparaDatosProducto(SqlCommand com, Producto formulario)
        {
            SqlCommand cmd = com;
            // Es necesario controlar los nulos para grabar "" si el valor capturado en pantalla es nulo.
            string descripcionAmpliada;
            if (formulario.DESCRIPCIONAMPLIADA == null)
                descripcionAmpliada = "";
            else
                descripcionAmpliada = formulario.DESCRIPCIONAMPLIADA;
            com.Parameters.Add("@codigo", SqlDbType.NVarChar);
            com.Parameters["@codigo"].Value = formulario.IDCODIGO;
            cmd.Parameters.Add("@descripcion", SqlDbType.NVarChar);
            com.Parameters["@descripcion"].Value = formulario.DESCRIPCION;
            cmd.Parameters.Add("@descripcionAmpliada", SqlDbType.NVarChar);
            com.Parameters["@descripcionAmpliada"].Value = descripcionAmpliada;
            cmd.Parameters.Add("@precioVenta", SqlDbType.Decimal);
            com.Parameters["@precioVenta"].Value = formulario.PRECIOVENTA;
            cmd.Parameters.Add("@tipoProducto", SqlDbType.Int);
            com.Parameters["@tipoProducto"].Value = formulario.IDTIPOPRODUCTO;
            cmd.Parameters.Add("@controlExistencias", SqlDbType.Int);
            com.Parameters["@controlExistencias"].Value = formulario.CONTROLEXISTENCIAS;
            cmd.Parameters.Add("@existencias", SqlDbType.Decimal);
            com.Parameters["@existencias"].Value = formulario.EXISTENCIAS;
            cmd.Parameters.Add("@activo", SqlDbType.Int);
            com.Parameters["@activo"].Value = formulario.ACTIVO;
            return cmd;
        }
        public bool ExisteProducto(Producto formulario)
        {
            bool existe = false;
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT codigo FROM productos WHERE codigo = @codigo";
            comando.Parameters.Add("@codigo", SqlDbType.VarChar);
            comando.Parameters["@codigo"].Value = formulario.IDCODIGO;
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
        public AyudaEnLinea LeerAyuda(string codigo)
        {
            AyudaEnLinea ayuda = new AyudaEnLinea();
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * FROM ayuda WHERE codigo = @codigo";
            comando.Parameters.Add("@codigo", SqlDbType.VarChar);
            comando.Parameters["@codigo"].Value = codigo;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    ayuda = new AyudaEnLinea(lector.GetString(0), lector.GetString(1), lector.GetString(2));
                }
            }
            lector.Close();
            conexion.Close();
            return ayuda;
        }
        public ObservableCollection<Pedido> ObtenerPedidos(string filtro, bool insertarFilaVacia)
        {
            ObservableCollection<Pedido> pedidos = new ObservableCollection<Pedido>();
            if (insertarFilaVacia)
            {
                pedidos.Add(new Pedido());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT pe.idPedido," +
                "pe.descripcion," +
                "pe.telefono," +
                "pe.nombre," +
                "pe.apellidos," +
                "pe.direccion," +
                "pe.poblacion," +
                "pe.codigoPostal," +
                "pe.provincia, " +
                "pe.mail, " +
                "pe.usuario, " +
                "pe.fechaIntroduccion, " +
                "pe.fechacierre, " +
                "pe.situacion, " +
                "pe.tipoPedido, " +
                "pe.enGarantia, " +
                "pr.nombre, " +
                "si.descripcion, " +
                "tp.descripcion " +
                " from pedidos pe JOIN provincias pr " +
                " ON pe.provincia = pr.idProvincia " +
                " JOIN situacionpedidos si " +
                " ON pe.situacion = si.idSituacion " +
                " JOIN tipopedido tp " +
                " ON pe.tipoPedido = tp.idTipo " +
                filtro;
            if (filtro == " WHERE 1=1")
            {
                comando.CommandText += " AND pe.situacion < " + situacionCierre;
            }
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    DateTime fechaCierre;
                    if (lector.IsDBNull(lector.GetOrdinal("fechaCierre")))
                    {
                        fechaCierre = DateTime.MinValue;
                    }
                    else
                    {
                        fechaCierre = lector.GetDateTime(12);
                    }
                    pedidos.Add(new Pedido(lector.GetInt32(0),
                              lector.GetString(1),
                              lector.GetString(2),
                              lector.GetString(3),
                              lector.GetString(4),
                              lector.GetString(5),
                              lector.GetString(6),
                              lector.GetString(7),
                              new Provincia(lector.GetString(8), lector.GetString(16)),
                              lector.GetString(9),
                              lector.GetString(10),
                              lector.GetDateTime(11),
                              fechaCierre,
                              new SituacionPedido(lector.GetInt32(13), lector.GetString(17)),
                              new TipoPedido(lector.GetInt32(14), lector.GetString(18)),
                              lector.GetBoolean(15)));
                }
            }
            lector.Close();
            conexion.Close();
            return pedidos;
        }
        public ObservableCollection<TipoPedido> ObtenerTipoPedido(bool insertarFilaVacia)
        {
            ObservableCollection<TipoPedido> lista = new ObservableCollection<TipoPedido>();
            if (insertarFilaVacia)
            {
                lista.Add(new TipoPedido());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * from tipopedido";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    lista.Add(new TipoPedido(lector.GetInt32(0), lector.GetString(1)));
                }
            }
            lector.Close();
            conexion.Close();
            return lista;
        }
        public ObservableCollection<SituacionPedido> ObtenerSituacionPedidos(bool insertarFilaVacia)
        {
            ObservableCollection<SituacionPedido> lista = new ObservableCollection<SituacionPedido>();
            if (insertarFilaVacia)
            {
                lista.Add(new SituacionPedido());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * from situacionpedidos";
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    lista.Add(new SituacionPedido(lector.GetInt32(0), lector.GetString(1)));
                }
            }
            lector.Close();
            conexion.Close();
            return lista;
        }
        public void InsertarPedido(Pedido formulario)
        {
            try
            {
                int situacionAnterior = formulario.IDSITUACION;
                string fechaCierre;
                conexion.Open();
                comando = conexion.CreateCommand();
                if (formulario.IDSITUACION == situacionCierre)
                    fechaCierre = DateTime.Now.ToString("yyyy/MM/dd");
                else
                    fechaCierre = "01/01/0001";
                comando.CommandText = "INSERT INTO pedidos " +
                        "(descripcion,telefono," +
                        "nombre," +
                        "apellidos," +
                        "direccion," +
                        "poblacion," +
                        "codigoPostal," +
                        "provincia," +
                        "mail," +
                        "usuario," +
                        "fechaIntroduccion," +
                        "fechaCierre," +
                        "situacion," +
                        "tipoPedido," +
                        "enGarantia" +
                        ") VALUES (@descripcion," +
                        "@telefono," +
                        "@nombre," +
                        "@apellidos," +
                        "@direccion," +
                        "@poblacion," +
                        "@codigoPostal," +
                        "@provincia," +
                        "@mail," +
                        "@usuario," +
                        "@fechaIntroduccion,'" + fechaCierre + "'," +
                        "@situacion," +
                        "@tipoPedido," +
                        "@enGarantia)";
                comando = PreparaDatosPedido(comando, formulario, "A", situacionAnterior);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public int BuscarSituacionPedido(int idPedido)
        {
            int situacionPedido = 0;
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "Select situacion FROM pedidos WHERE idPedido = @idPedido";
            comando.Parameters.Add("@idPedido", SqlDbType.Int);
            comando.Parameters["@idPedido"].Value = idPedido;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    situacionPedido = lector.GetInt32(0);
                }
            }
            lector.Close();
            conexion.Close();
            return situacionPedido;
        }
        public void ActualizarPedido(Pedido formulario)
        {
            try
            {
                int situacionAnterior = BuscarSituacionPedido(formulario.IDPEDIDO);
                //DateTime fechaCierre = new DateTime();
                string fechaCierre = "";
                if (situacionAnterior != situacionCierre && formulario.IDSITUACION == situacionCierre)
                {
                    //fechaCierre = DateTime.Today;
                    fechaCierre = DateTime.Now.ToString("yyyy/MM/dd");
                }
                if (situacionAnterior == situacionCierre && formulario.IDSITUACION != situacionCierre)
                {
                    //fechaCierre = DateTime.Parse("01/01/0001");
                    fechaCierre = "01/01/0001";
                }
                conexion.Open();
                // Si nueva situacion es cancelada o cerrada cerraremos todos los partes que esten abiertos
                if (formulario.IDSITUACION >= situacionCierre)
                {
                    CerrarPartesDelPedido(formulario.IDPEDIDO);
                }
                comando = conexion.CreateCommand();
                comando.CommandText = "UPDATE pedidos SET " +
                    "descripcion = @descripcion," +
                    "telefono = @telefono," +
                    "nombre = @nombre," +
                    "apellidos = @apellidos," +
                    "direccion = @direccion," +
                    "poblacion = @poblacion," +
                    "codigoPostal = @codigoPostal," +
                    "provincia = @provincia," +
                    "mail = @mail," +
                    "situacion = @situacion," +
                    "fechaCierre = '" + fechaCierre + "'," +
                    "tipoPedido = @tipoPedido," +
                    "enGarantia = @enGarantia" +
                    " WHERE idPedido = @idPedido";

                comando = PreparaDatosPedido(comando, formulario, "M", situacionAnterior);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void CerrarPartesDelPedido(int idPedido)
        {
            SqlCommand cmd = conexion.CreateCommand();
            string fechaCierre = DateTime.Now.ToString("yyyy/MM/dd");
            cmd = conexion.CreateCommand();
            cmd.CommandText = "UPDATE partes SET " +
                "cerrado = 1," +
                "fechaCierre = '" + fechaCierre + "'" +
                " WHERE pedido = " + @idPedido +
                " AND cerrado = 0";
            cmd.ExecuteNonQuery();
        }

        public SqlCommand PreparaDatosPedido(SqlCommand com, Pedido formulario, string operacion, int situacionAnterior)
        {
            SqlCommand cmd = com;
            // Es necesario controlar los nulos para grabar "" si el valor capturado en pantalla es nulo.
            string codigoPostal, apellidos, mail, usuario;
            if (formulario.CODIGOPOSTAL == null)
                codigoPostal = "";
            else
                codigoPostal = formulario.CODIGOPOSTAL;
            if (formulario.APELLIDOS == null)
                apellidos = "";
            else
                apellidos = formulario.APELLIDOS;
            if (formulario.MAIL == null)
                mail = "";
            else
                mail = formulario.MAIL;
            if (formulario.USUARIO == null)
                usuario = "";
            else
                usuario = formulario.USUARIO;
            cmd.Parameters.Add("@idPedido", SqlDbType.Int);
            cmd.Parameters["@idPedido"].Value = formulario.IDPEDIDO;
            cmd.Parameters.Add("@descripcion", SqlDbType.NVarChar);
            cmd.Parameters["@descripcion"].Value = formulario.DESCRIPCION;
            cmd.Parameters.Add("@telefono", SqlDbType.NVarChar);
            cmd.Parameters["@telefono"].Value = formulario.TELEFONO;
            cmd.Parameters.Add("@nombre", SqlDbType.NVarChar);
            cmd.Parameters["@nombre"].Value = formulario.NOMBRE;
            cmd.Parameters.Add("@apellidos", SqlDbType.NVarChar);
            cmd.Parameters["@apellidos"].Value = apellidos;
            cmd.Parameters.Add("@direccion", SqlDbType.NVarChar);
            cmd.Parameters["@direccion"].Value = formulario.DIRECCION;
            cmd.Parameters.Add("@poblacion", SqlDbType.NVarChar);
            cmd.Parameters["@poblacion"].Value = formulario.POBLACION;
            cmd.Parameters.Add("@codigoPostal", SqlDbType.NVarChar);
            cmd.Parameters["@codigoPostal"].Value = codigoPostal;
            cmd.Parameters.Add("@provincia", SqlDbType.NVarChar);
            cmd.Parameters["@provincia"].Value = formulario.IDPROVINCIA;
            cmd.Parameters.Add("@mail", SqlDbType.NVarChar);
            cmd.Parameters["@mail"].Value = mail;
            cmd.Parameters.Add("@usuario", SqlDbType.NVarChar);
            cmd.Parameters["@usuario"].Value = usuario;

            // Gestionamos el valor de la fecha de cierre en función de la situacion anterior y la nueva
            if (operacion == "A")
            {
                cmd.Parameters.Add("@fechaIntroduccion", SqlDbType.DateTime);
                cmd.Parameters["@fechaIntroduccion"].Value = DateTime.Today;
            }
            cmd.Parameters.Add("@situacion", SqlDbType.Int);
            cmd.Parameters["@situacion"].Value = formulario.IDSITUACION;
            cmd.Parameters.Add("@tipoPedido", SqlDbType.Int);
            cmd.Parameters["@tipoPedido"].Value = formulario.IDTIPOPEDIDO;
            cmd.Parameters.Add("@enGarantia", SqlDbType.Int);
            cmd.Parameters["@enGarantia"].Value = formulario.ENGARANTIA;
            return cmd;
        }
        public void BorrarPedido(Pedido seleccionado)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM pedidos WHERE idPedido = @idPedido";
                comando.Parameters.Add("@idPedido", SqlDbType.Int);
                comando.Parameters["@idPedido"].Value = seleccionado.IDPEDIDO;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public Telefono ObtenerTelefono(string telefonoABuscar)
        {
            Telefono telefono = new Telefono();
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT " +
                "tl.telefono," +
                "tl.nombre," +
                "tl.apellidos," +
                "tl.direccion," +
                "tl.poblacion," +
                "tl.codigoPostal," +
                "tl.provincia, " +
                "tl.mail, " +
                "pr.nombre " +
                " from telefonos tl JOIN provincias pr " +
                " ON tl.provincia = pr.idProvincia" +
                " WHERE telefono = @telefono";
            comando.Parameters.Add("@telefono", SqlDbType.VarChar);
            comando.Parameters["@telefono"].Value = telefonoABuscar;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    telefono = new Telefono(lector.GetString(0),
                               lector.GetString(1),
                               lector.GetString(2),
                               lector.GetString(3),
                               lector.GetString(4),
                               lector.GetString(5),
                               new Provincia(lector.GetString(6), lector.GetString(8)),
                               lector.GetString(7));
                }
            }
            lector.Close();
            conexion.Close();
            return telefono;
        }
        public double ObtenerProducto(string productoABuscar)
        {
            double precio = 0;
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT " +
                "precioVenta " +
                " from productos " +
                " WHERE codigo = @codigo";
            comando.Parameters.Add("@codigo", SqlDbType.VarChar);
            comando.Parameters["@codigo"].Value = productoABuscar;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    precio = (double)lector.GetDecimal(0);
                }
            }
            lector.Close();
            conexion.Close();
            return precio;
        }
        public void BorrarProductoPedido(ProductoPedido seleccionado)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM productosPedido WHERE id = @id";
                comando.Parameters.Add("@id", SqlDbType.Int);
                comando.Parameters["@id"].Value = seleccionado.ID;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void InsertarProductoPedido(ProductoPedido formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "INSERT INTO productosPedido " +
                        "(" +
                        "pedido," +
                        "producto," +
                        "cantidad," +
                        "precioVenta" +
                        ") VALUES (" +
                        "@pedido," +
                        "@producto," +
                        "@cantidad," +
                        "@precioVenta)";
                comando = PreparaDatosProductoPedido(comando, formulario);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void ActualizarProductoPedido(ProductoPedido formulario)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "UPDATE productosPedido SET " +
                    "producto = @producto," +
                    "cantidad = @cantidad," +
                    "precioVenta = @precioVenta" +
                    " WHERE id = @id";
                comando = PreparaDatosProductoPedido(comando, formulario);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public SqlCommand PreparaDatosProductoPedido(SqlCommand com, ProductoPedido formulario)
        {
            SqlCommand cmd = com;
            com.Parameters.Add("@id", SqlDbType.Int);
            com.Parameters["@id"].Value = formulario.ID;
            com.Parameters.Add("@pedido", SqlDbType.Int);
            com.Parameters["@pedido"].Value = formulario.PEDIDO.IDPEDIDO;
            cmd.Parameters.Add("@producto", SqlDbType.NVarChar);
            com.Parameters["@producto"].Value = formulario.PRODUCTO.IDCODIGO;
            cmd.Parameters.Add("@cantidad", SqlDbType.Int);
            com.Parameters["@cantidad"].Value = formulario.CANTIDAD;
            cmd.Parameters.Add("@precioVenta", SqlDbType.Decimal);
            com.Parameters["@precioVenta"].Value = formulario.PRECIOVENTA;
            return cmd;
        }
        public ObservableCollection<ProductoPedido> ObtenerProductoPedido(bool insertarFilaVacia, int idPedido)
        {
            ObservableCollection<ProductoPedido> lista = new ObservableCollection<ProductoPedido>();
            if (insertarFilaVacia)
            {
                lista.Add(new ProductoPedido());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT " +
                "pd.id," +  
                "pd.pedido," + 
                "pd.producto," + 
                "pd.cantidad," + 
                "pd.precioVenta," + 
                "pr.descripcion," +
                "pr.descripcionAmpliada," + 
                "pr.precioVenta," + 
                "tp.idTipo," + 
                "pr.controlExistencias," + 
                "pr.existencias," + 
                "pr.activo," + 
                "tp.nombre," + 
                "pe.descripcion," + 
                "pe.telefono," + 
                "pe.nombre," + 
                "pe.apellidos," + 
                "pe.direccion," + 
                "pe.poblacion," + 
                "pe.codigoPostal," + 
                "pe.provincia," + 
                "pe.mail," + 
                "pe.usuario," + 
                "pe.fechaIntroduccion," + 
                "pe.fechacierre as fechacierrePe," + 
                "pe.situacion," + 
                "pe.tipoPedido," + 
                "pe.enGarantia, " + 
                "prov.nombre," + 
                "si.descripcion," + 
                "tipe.descripcion" + 
                " FROM productosPedido pd" + 
                " JOIN pedidos pe " +
                " ON pe.idPedido = pd.pedido " +
                " JOIN productos pr" +
                " ON pr.codigo = pd.producto" +
                " JOIN tipoproducto tp" +
                " ON tp.idTipo = pr.tipoproducto" +
                  " JOIN provincias prov " +
                " ON pe.provincia = prov.idProvincia " +
                " JOIN situacionpedidos si " +
                " ON pe.situacion = si.idSituacion " +
                " JOIN tipopedido tipe " +
                " ON pe.tipoPedido = tipe.idTipo " +
                " WHERE pe.idPedido = " + idPedido;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    DateTime fechaCierrePe;
                    if (lector.IsDBNull(lector.GetOrdinal("fechaCierrePe")))
                        fechaCierrePe = DateTime.MinValue;
                    else
                        fechaCierrePe = lector.GetDateTime(24);
                    lista.Add(new ProductoPedido(lector.GetInt32(0),
                              new Pedido(lector.GetInt32(1),
                              lector.GetString(13),
                              lector.GetString(14),
                              lector.GetString(15),
                              lector.GetString(16),
                              lector.GetString(17),
                              lector.GetString(18),
                              lector.GetString(19),
                              new Provincia(lector.GetString(20), lector.GetString(28)),
                              lector.GetString(21),
                              lector.GetString(22),
                              lector.GetDateTime(23),
                              fechaCierrePe,
                              new SituacionPedido(lector.GetInt32(25), lector.GetString(29)),
                              new TipoPedido(lector.GetInt32(26), lector.GetString(30)),
                              lector.GetBoolean(27)),
                              new Producto(lector.GetString(2),
                              lector.GetString(5),
                              lector.GetString(6),
                               (double)lector.GetDecimal(7),
                              new TipoProducto(lector.GetInt32(8), lector.GetString(12)),
                              lector.GetBoolean(9),
                               (double)lector.GetDecimal(10),
                              lector.GetBoolean(11)),
                               (double)lector.GetDecimal(3),
                               (double)lector.GetDecimal(4)
                              ));
                }
            }
            lector.Close();
            conexion.Close();
            return lista;
        }
        public ObservableCollection<Parte> ObtenerPartes(string filtro, int idPedido, bool insertarFilaVacia)
        {
            ObservableCollection<Parte> lista = new ObservableCollection<Parte>();
            if (insertarFilaVacia)
            {
                lista.Add(new Parte());
            }
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT pe.idPedido," +
                "pe.descripcion," +
                "pe.telefono," +
                "pe.nombre," +
                "pe.apellidos," +
                "pe.direccion," +
                "pe.poblacion," +
                "pe.codigoPostal," +
                "pe.provincia," +
                "pe.mail," +
                "pe.usuario," +
                "pe.fechaIntroduccion," +
                "pe.fechacierre as fechacierrePe," +
                "pe.situacion," +
                "pe.tipoPedido," +
                "pe.enGarantia," +
                "pr.nombre," +
                "si.descripcion," +
                "tp.descripcion," +
                "pa.idEmpleado," +
                "em.nombre," +
                "em.apellidos," +
                "pa.idParte," +
                "pa.fechaIntroduccion," +
                "pa.fechacierre as fechacierrePa," +
                "pa.cerrado," +
                "pa.fechaprevista," +
                "pa.observaciones," +
                "pa.incidencias" +
                " from partes pa JOIN empleados em " +
                " ON pa.idEmpleado = em.idEmpleado " +
                " JOIN pedidos pe " +
                " ON pe.idPedido = pa.pedido " +
                " JOIN provincias pr " +
                " ON pe.provincia = pr.idProvincia " +
                " JOIN situacionpedidos si " +
                " ON pe.situacion = si.idSituacion " +
                " JOIN tipopedido tp " +
                " ON pe.tipoPedido = tp.idTipo " +
                filtro;
            if (idPedido > 0)
                comando.CommandText += " AND pe.idPedido = " + idPedido;
            else
                if (filtro == " WHERE 1=1")
                comando.CommandText += " AND pe.situacion < " + situacionCierre;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    DateTime fechaCierrePe;
                    DateTime fechaCierrePa;
                    if (lector.IsDBNull(lector.GetOrdinal("fechaCierrePe")))
                    {
                        fechaCierrePe = DateTime.MinValue;
                    }
                    else
                    {
                        fechaCierrePe = lector.GetDateTime(12);
                    }
                    if (lector.IsDBNull(lector.GetOrdinal("fechaCierrePa")))
                    {
                        fechaCierrePa = DateTime.MinValue;
                    }
                    else
                    {
                        fechaCierrePa = lector.GetDateTime(24);
                    }

                    lista.Add(new Parte(lector.GetInt32(22),
                        new Pedido(lector.GetInt32(0),
                              lector.GetString(1),
                              lector.GetString(2),
                              lector.GetString(3),
                              lector.GetString(4),
                              lector.GetString(5),
                              lector.GetString(6),
                              lector.GetString(7),
                              new Provincia(lector.GetString(8), lector.GetString(16)),
                              lector.GetString(9),
                              lector.GetString(10),
                              lector.GetDateTime(11),
                              fechaCierrePe,
                              new SituacionPedido(lector.GetInt32(13), lector.GetString(17)),
                              new TipoPedido(lector.GetInt32(14), lector.GetString(18)),
                              lector.GetBoolean(15)),
                              lector.GetDateTime(23),
                              fechaCierrePa,
                              lector.GetBoolean(25),
                              new Empleado(lector.GetInt32(19), lector.GetString(20), lector.GetString(21)),
                              lector.GetString(27),
                              lector.GetDateTime(26),
                              lector.GetString(28)
                              ));
                }
            }
            lector.Close();
            conexion.Close();
            return lista;
        }
        public void BorrarParte(Parte seleccionado)
        {
            try
            {
                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM partes WHERE idParte = @idParte";
                comando.Parameters.Add("@idParte", SqlDbType.Int);
                comando.Parameters["@idParte"].Value = seleccionado.IDPARTE;
                comando.ExecuteNonQuery();
                if (ContarPartes(seleccionado.IDPEDIDO, "") > 0)
                    ActualizarSituacionPedido(seleccionado.CERRADO, true, seleccionado.IDPEDIDO);
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void InsertarParte(Parte formulario)
        {
            try
            {
                bool situacionAnterior = formulario.CERRADO;
                string fechaCierre;
                conexion.Open();
                comando = conexion.CreateCommand();
                if (formulario.CERRADO)
                    fechaCierre = DateTime.Now.ToString("yyyy/MM/dd");
                else
                    fechaCierre = "01/01/0001";
                comando.CommandText = "INSERT INTO partes " +
                        "(pedido," +
                        "fechaIntroduccion," +
                        "fechaCierre," +
                        "cerrado," +
                        "idEmpleado," +
                        "observaciones," +
                        "fechaprevista," +
                        "incidencias" +
                        ") VALUES (@pedido," +
                        "@fechaIntroduccion, '" + fechaCierre + "'," +
                        "@cerrado," +
                        "@idEmpleado," +
                        "@observaciones," +
                        "@fechaprevista," +
                        "@incidencias)";
                comando = PreparaDatosParte(comando, formulario, "A", situacionAnterior);
                comando.ExecuteNonQuery();
                ActualizarSituacionPedido(situacionAnterior, formulario.CERRADO, formulario.IDPEDIDO);
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public void ActualizarParte(Parte formulario)
        {
            try
            {
                bool situacionAnterior = BuscarSituacionParte(formulario.IDPARTE);
                string fechaCierre = "";
                if (!situacionAnterior && formulario.CERRADO)
                {
                    fechaCierre = DateTime.Now.ToString("yyyy/MM/dd");
                }
                if (situacionAnterior && !formulario.CERRADO)
                {
                    fechaCierre = "01/01/0001";
                }

                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "UPDATE partes SET " +
                    "cerrado = @cerrado," +
                    "idEmpleado = @idEmpleado," +
                    "observaciones = @observaciones," +
                    "incidencias = @incidencias," +
                    "fechaprevista = @fechaprevista," +
                    "fechacierre = '" + fechaCierre + "'" +
                    " WHERE idParte = @idParte";
                comando = PreparaDatosParte(comando, formulario, "M", situacionAnterior);
                comando.ExecuteNonQuery();
                ActualizarSituacionPedido(situacionAnterior, formulario.CERRADO, formulario.IDPEDIDO);
                conexion.Close();
            }
            catch (Exception e)
            {
                conexion.Close();
                throw new MisExcepciones(e.Message);
            }
        }
        public SqlCommand PreparaDatosParte(SqlCommand com, Parte formulario, string operacion, bool situacionAnterior)
        {
            SqlCommand cmd = com;
            // Es necesario controlar los nulos para grabar "" si el valor capturado en pantalla es nulo.
            string observaciones, incidencias;
            if (formulario.OBSERVACIONES == null)
                observaciones = "";
            else
                observaciones = formulario.OBSERVACIONES;
            if (formulario.INCIDENCIAS == null)
                incidencias = "";
            else
                incidencias = formulario.INCIDENCIAS;
            com.Parameters.Add("@idParte", SqlDbType.Int);
            com.Parameters["@idParte"].Value = formulario.IDPARTE;
            com.Parameters.Add("@pedido", SqlDbType.Int);
            com.Parameters["@pedido"].Value = formulario.IDPEDIDO;
            if (operacion == "A")
            {
                cmd.Parameters.Add("@fechaIntroduccion", SqlDbType.DateTime);
                com.Parameters["@fechaIntroduccion"].Value = DateTime.Today;
            }
            cmd.Parameters.Add("@cerrado", SqlDbType.Int);
            com.Parameters["@cerrado"].Value = formulario.CERRADO;
            cmd.Parameters.Add("@idEmpleado", SqlDbType.Int);
            com.Parameters["@idEmpleado"].Value = formulario.IDEMPLEADO;
            cmd.Parameters.Add("@observaciones", SqlDbType.NVarChar);
            com.Parameters["@observaciones"].Value = observaciones;
            cmd.Parameters.Add("@incidencias", SqlDbType.NVarChar);
            com.Parameters["@incidencias"].Value = incidencias;
            cmd.Parameters.Add("@fechaprevista", SqlDbType.DateTime);
            com.Parameters["@fechaprevista"].Value = formulario.FECHAPREVISTA;
            return cmd;
        }
        public bool BuscarSituacionParte(int idParte)
        {
            bool situacionParte = false;
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "Select cerrado FROM partes WHERE idParte = @idParte";
            comando.Parameters.Add("@idParte", SqlDbType.Int);
            comando.Parameters["@idParte"].Value = idParte;
            SqlDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    situacionParte = lector.GetBoolean(0);
                }
            }
            lector.Close();
            conexion.Close();
            return situacionParte;
        }
        public void ActualizarSituacionPedido(bool cerradoAntes, bool cerradoDespues, int idPedido)
        {
            string fechaCierre = "";
            bool actualizar = false;
            // Obtener situaciones de cerrado y en reparacion
            int situacionCerrado = Properties.Settings.Default.situacionCierre;
            int situacionEnReparacion = Properties.Settings.Default.situacionEnReparacion;
            // Si cerramos el parte y no quedan partes abiertos se cierra el pedido
            // Si se abre o crea un parte nuevo como abirto se abre el pedido
            // Antes y ahora abierto o antes cerrado y despues abierto => ABRIMOS PEDIDO
            if (!cerradoAntes == !cerradoDespues ||
               (cerradoAntes && !cerradoDespues))
            {
                fechaCierre = "01/01/0001";
            }
            if (!cerradoAntes && cerradoDespues && ContarPartes(idPedido, "AND cerrado = 0") == 0)
            {
                fechaCierre = DateTime.Now.ToString("yyyy/MM/dd");
            }
            SqlCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "Update pedidos SET " +
                            "fechaCierre = '" + @fechaCierre + "'," +
                            "situacion = @situacion " +
                            " WHERE idPedido = @idPedido";
            cmd.Parameters.Add("@idPedido", SqlDbType.Int);
            cmd.Parameters["@idPedido"].Value = idPedido;
            cmd.Parameters.Add("@situacion", SqlDbType.Int);
            // Antes y ahora abierto o antes cerrado y despues abierto => ABRIMOS PEDIDO
            if (!cerradoAntes == !cerradoDespues ||
               (cerradoAntes && !cerradoDespues))
            {
                cmd.Parameters["@situacion"].Value = situacionEnReparacion;
                actualizar = true;
            }
            // Antes abiero y ahora cerrado y no hay partes abiertos de este pedido => CERRAMOS PEDIDO
            if (!cerradoAntes && cerradoDespues && ContarPartes(idPedido, "AND cerrado = 0") == 0)
            {
                cmd.Parameters["@situacion"].Value = situacionCerrado;
                actualizar = true;
            }
            // Actualizamos pedido
            if (actualizar)
                cmd.ExecuteNonQuery();
        }
        public int ContarPartes(int idPedido,string estado)
        {
            int partesAbiertos = 0;
            SqlCommand com = conexion.CreateCommand();
            com.CommandText = "select count(*) from partes where pedido = @idPedido " + estado;
            com.Parameters.Add("@idPedido", SqlDbType.Int);
            com.Parameters["@idPedido"].Value = idPedido;
            partesAbiertos = (Int32)com.ExecuteScalar();
            return partesAbiertos;
        }
    }


}
