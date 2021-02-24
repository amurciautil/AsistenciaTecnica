﻿using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;

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
                            "and password = HASHBYTES('SHA2_512', '" + password + "')";
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
            comando.CommandText = "INSERT INTO perfiles VALUES(@descripcion,@descripcionAmpliada)";
            comando.Parameters.Add("@descripcion", SqlDbType.NVarChar);
            comando.Parameters["@descripcion"].Value = formulario.NOMBRE;
            comando.Parameters.Add("@descripcionAmpliada", SqlDbType.NVarChar);
            comando.Parameters["@descripcionAmpliada"].Value = formulario.NOMBRE;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void ActualizarPerfil(Perfil formulario)
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
        public void BorrarEmpleado(Empleado seleccionado)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "DELETE FROM empleados WHERE idEmpleado = @idEmpleado";
            comando.Parameters.Add("@idEmpleado", SqlDbType.Int);
            comando.Parameters["@idEmpleado"].Value = seleccionado.IDEMPLEADO;
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void InsertarEmpleado(Empleado formulario)
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
        public void ActualizarEmpleado(Empleado formulario)
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
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "DELETE FROM usuarios WHERE login = @login";
            comando.Parameters.Add("@login", SqlDbType.NVarChar);
            comando.Parameters["@login"].Value = seleccionado.LOGIN;
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        public void InsertarUsuario(Usuario formulario)
        {
            conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "INSERT INTO usuarios " +
                    "VALUES(@login,HASHBYTES('SHA2_512', '" + formulario.PASSWORD + "'),@perfil,@empleado,@activo)";
           comando = PreparaDatosUsuario(comando, formulario);
            comando.ExecuteNonQuery();
            conexion.Close();
        }
        public void ActualizarUsuario(Usuario formulario)
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

        public SqlCommand PreparaDatosUsuario(SqlCommand com, Usuario formulario)
        {
            SqlCommand cmd = com;
            com.Parameters.Add("@login", SqlDbType.NVarChar);
            com.Parameters["@login"].Value = formulario.LOGIN;
            //cmd.Parameters.Add("@password", SqlDbType.NVarChar);
            //cmd.Parameters["@password"].Value = formulario.PASSWORD;
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
            //    "password = HASHBYTES('SHA2_512', @password)" +
                "password = HASHBYTES('SHA2_512', '"+password+"')" +
                " WHERE login = @login";
            comando.Parameters.Add("@login", SqlDbType.NVarChar);
            comando.Parameters["@login"].Value = login;
            comando.Parameters.Add("@password", SqlDbType.NVarChar);
            comando.Parameters["@password"].Value = password;
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
    }
}
