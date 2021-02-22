﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    public class LoginVM : INotifyPropertyChanged
    {
        private readonly ServicioBaseDatos bbdd;
        public Usuario USUARIOSELECCIONADO { get; set; }
    
        public LoginVM()
        {
            bbdd = new ServicioBaseDatos();
            USUARIOSELECCIONADO = new Usuario();
        }

        public void Cancelar()
        {
            USUARIOSELECCIONADO = new Usuario();
        }
        public void BuscarUsuario(string login,string password)
        {
            if (bbdd.BuscarUsuario(login, password) == 1)
            {
                USUARIOSELECCIONADO = bbdd.LeerUsuario(login);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
