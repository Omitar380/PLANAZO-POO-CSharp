namespace PLANAZO_POO_CSharp.Modelos
{
    public class Reserva
    {
        public int IdReserva { get; }
        public Usuario Usuario { get; }
        public string NombrePersona => Usuario.Nombre;
        public Evento EventoReservado { get; }
        public int CantidadEntradas { get; }
        public decimal ValorTotal { get; }
        public bool Activa { get; private set; }

        public Reserva(int idReserva, Usuario usuario, Evento eventoReservado, int cantidadEntradas)
        {
            if (idReserva <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idReserva), "El identificador debe ser mayor a cero.");
            }

            ArgumentNullException.ThrowIfNull(usuario);
            ArgumentNullException.ThrowIfNull(eventoReservado);

            if (cantidadEntradas <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(cantidadEntradas), "La cantidad de entradas debe ser mayor a cero.");
            }

            IdReserva = idReserva;
            Usuario = usuario;
            EventoReservado = eventoReservado;
            CantidadEntradas = cantidadEntradas;
            ValorTotal = eventoReservado.CalcularPrecioFinal() * cantidadEntradas;
            Activa = true;
        }

        public void Cancelar()
        {
            if (!Activa)
            {
                throw new InvalidOperationException("La reserva ya se encuentra cancelada previamente.");
            }

            EventoReservado.LiberarCupos(CantidadEntradas);
            Activa = false;
        }

        public override string ToString()
        {
            string estado = Activa ? "Activa" : "Cancelada";
            return $"Reserva #{IdReserva} | Persona: {Usuario.Nombre} | Evento: {EventoReservado.Nombre} | " +
                   $"Entradas: {CantidadEntradas} | Total: ${ValorTotal:N2} | Estado: {estado}";
        }
    }
}
