﻿namespace WebAPI.Model
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int? CategoriaId { get; set; }
    }
}