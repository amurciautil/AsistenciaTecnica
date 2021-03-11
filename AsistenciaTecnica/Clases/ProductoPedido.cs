using System.ComponentModel;

namespace AsistenciaTecnica
{
    class ProductoPedido : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int IDPEDIDO { get; set; }
        public Pedido PEDIDO { get; set; }
        public Producto PRODUCTO { get; set; }
        public double CANTIDAD { get; set; }
        public double PRECIOVENTA { get; set; }
        public string NOMBREPRODUCTO { get; set; }
        public string IDPRODUCTO { get; set; }

        public ProductoPedido(Pedido pedido)
        {
            PEDIDO = pedido;
            Producto producto = new Producto();
            PRODUCTO = producto;
        }
        public ProductoPedido()
        {
        }
        public ProductoPedido(int id, Pedido pedido, Producto producto, double cantidad, double precioventa)
        {
            ID = id;
            PEDIDO = pedido;
            PRODUCTO = producto;
            CANTIDAD = cantidad;
            PRECIOVENTA = precioventa;
            NOMBREPRODUCTO = producto.IDCODIGO + "-" + producto.DESCRIPCION;
        }
        public ProductoPedido(ProductoPedido productoPedido)
        {
            ID = productoPedido.ID;
            PEDIDO = productoPedido.PEDIDO;
            PRODUCTO = productoPedido.PRODUCTO;
            CANTIDAD = productoPedido.CANTIDAD;
            PRECIOVENTA = productoPedido.PRECIOVENTA;
            NOMBREPRODUCTO = productoPedido.NOMBREPRODUCTO;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
