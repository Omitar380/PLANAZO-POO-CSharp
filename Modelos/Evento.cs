namespace PLANAZO_POO_CSharp.Modelos
{
    public abstract class Evento
    {
        public int Id { get; }
        public string Nombre { get; }
        public DateTime Fecha { get; }
        public decimal PrecioBase { get; }
        public int CapacidadMaxima { get; }
        public int CuposDisponibles { get; private set; }

        protected Evento(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "El identificador debe ser mayor a cero.");
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del evento es obligatorio.", nameof(nombre));
            }

            if (precioBase < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precioBase), "El precio base no puede ser negativo.");
            }

            if (capacidadMaxima <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacidadMaxima), "La capacidad debe ser mayor a cero.");
            }

            Id = id;
            Nombre = nombre.Trim();
            Fecha = fecha;
            PrecioBase = precioBase;
            CapacidadMaxima = capacidadMaxima;
            CuposDisponibles = capacidadMaxima;
        }

        public abstract decimal CalcularPrecioFinal();

        public void ReservarCupos(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad debe ser mayor a cero.");
            }

            if (cantidad > CuposDisponibles)
            {
                throw new InvalidOperationException(
                    $"No hay suficientes cupos disponibles. Cupos actuales: {CuposDisponibles}");
            }

            CuposDisponibles -= cantidad;
        }

        public void LiberarCupos(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad debe ser mayor a cero.");
            }

            if (cantidad > CapacidadMaxima - CuposDisponibles)
            {
                throw new InvalidOperationException("La cantidad de cupos liberados supera la capacidad del evento.");
            }

            CuposDisponibles += cantidad;
        }

        public override string ToString()
        {
            return $"[{Id}] {Nombre} - Fecha: {Fecha.ToShortDateString()} - " +
                   $"Precio Final: ${CalcularPrecioFinal():N2} - Cupos: {CuposDisponibles}/{CapacidadMaxima}";
        }
    }
}
