using System.Windows.Input;

namespace AsistenciaTecnica
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Ayuda = new RoutedUICommand
            ("Ayuda",
             "Ayuda",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F1,ModifierKeys.Control)
            }
            );
        public static readonly RoutedUICommand Salir = new RoutedUICommand
            ("Salir",
            "Salir",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F4,ModifierKeys.Alt)
            }
            );
        public static readonly RoutedUICommand Utilidades = new RoutedUICommand
            ("Utilidades",
            "Utilidades",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.U,ModifierKeys.Control)
            }
            );
        public static readonly RoutedUICommand Editar = new RoutedUICommand
            ("Editar",
            "Editar",
            typeof(CustomCommands)
            );
        public static readonly RoutedUICommand Insertar = new RoutedUICommand
            ("Insertar",
             "Insertar",
             typeof(CustomCommands)
            );
        public static readonly RoutedUICommand Borrar = new RoutedUICommand
          ("Borrar",
           "Borrar",
           typeof(CustomCommands)
          );
        public static readonly RoutedUICommand GuardarCambios = new RoutedUICommand
            ("GuardarCambios",
            "GuardarCambios",
            typeof(CustomCommands)
            );
        public static readonly RoutedUICommand Cancelar = new RoutedUICommand
            ("Cancelar",
            "Cancelar",
            typeof(CustomCommands)
            );
        public static readonly RoutedUICommand Aceptar = new RoutedUICommand
            ("Aceptar",
            "Aceptar",
            typeof(CustomCommands)
            );

        public static readonly RoutedUICommand Filtrar = new RoutedUICommand
            ("Filtrar",
            "Filtrar",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S,ModifierKeys.Control)
            }
            );
        public static readonly RoutedUICommand Cargo = new RoutedUICommand
            ("Cargo",
            "Cargo",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
               new KeyGesture(Key.G,ModifierKeys.Alt)
            }
            );
        public static readonly RoutedUICommand Departamento = new RoutedUICommand
            ("Departamento",
            "Departamento",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
               new KeyGesture(Key.D,ModifierKeys.Alt)
            }
            );
        public static readonly RoutedUICommand Perfil = new RoutedUICommand
            ("Perfil",
            "Perfil",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
               new KeyGesture(Key.E,ModifierKeys.Alt)
            }
            );
        public static readonly RoutedUICommand Provincia = new RoutedUICommand
            ("Provincia",
            "Provincia",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.P,ModifierKeys.Alt)
            }
            );
        public static readonly RoutedUICommand Empleado = new RoutedUICommand
            ("Empleado",
            "Empleado",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.M,ModifierKeys.Alt)
            }
            );
        public static readonly RoutedUICommand Usuario = new RoutedUICommand
           ("Usuario",
           "Usuario",
           typeof(CustomCommands),
           new InputGestureCollection()
           {
                 new KeyGesture(Key.R,ModifierKeys.Alt)
           }
           );
        public static readonly RoutedUICommand Password = new RoutedUICommand
           ("Password",
           "Password",
           typeof(CustomCommands),
           new InputGestureCollection()
           {
               new KeyGesture(Key.W,ModifierKeys.Alt)
           }
           );
        public static readonly RoutedUICommand CambioUsuario = new RoutedUICommand
           ("CambioUsuario",
           "CambioUsuario",
           typeof(CustomCommands),
           new InputGestureCollection()
           {
              new KeyGesture(Key.T,ModifierKeys.Alt)
           }
           );
        public static readonly RoutedUICommand Producto = new RoutedUICommand
           ("Producto",
           "Producto",
           typeof(CustomCommands),
           new InputGestureCollection()
           {
                      new KeyGesture(Key.E,ModifierKeys.Control)
           }
           );
        public static readonly RoutedUICommand Pedido = new RoutedUICommand
            ("Pedido",
            "Pedido",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E,ModifierKeys.Control)
            }
            );
        public static readonly RoutedUICommand Buscar = new RoutedUICommand
            ("Buscar",
             "Buscar",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                        new KeyGesture(Key.F,ModifierKeys.Control)
            }
           );
    }

}
