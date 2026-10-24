using System;
using System.Collections.Generic;
using System.Linq;
using PLANAZO_POO_CSharp.Modelos;

namespace PLANAZO_POO_CSharp.Servicios
{
    public class GestorReservas
    {
        private readonly List<Evento> eventos;
        private readonly List<Reserva> reservas;
        private readonly List<Usuario> usuarios;
        private int contadorReservas = 1;
        private int contadorUsuarios = 1;

        public GestorReservas()
        {
            eventos = new List<Evento>();
            reservas = new List<Reserva>();
            usuarios = new List<Usuario>();
            CargarEventosIniciales();
        }

        private void CargarEventosIniciales()
        {
            // Requisito: iniciar con al menos 6 eventos (2 películas, 2 conciertos, 2 talleres)
            eventos.Add(new Cine(1, "Estreno: Sci-Fi Multiverso", DateTime.Now.AddDays(5), 20000m, 50));
            eventos.Add(new Cine(2, "Clásico de Terror en 35mm", DateTime.Now.AddDays(7), 15000m, 40));
            
            eventos.Add(new Concierto(3, "Rock Sinfónico en Vivo", DateTime.Now.AddDays(10), 60000m, 100));
            eventos.Add(new Concierto(4, "Festival Urbano Summer", DateTime.Now.AddDays(15), 80000m, 150));
            
            eventos.Add(new Taller(5, "Taller de Introducción a C#", DateTime.Now.AddDays(3), 30000m, 20));
            eventos.Add(new Taller(6, "Taller de Robótica con Arduino", DateTime.Now.AddDays(8), 45000m, 15));

            eventos.Add(new EventoDeportivo(7, "Final de fútbol local", DateTime.Now.AddDays(20), 50000m, 200));
        }

        public void ListarEventos()
        {
            Console.WriteLine("\n--- EVENTOS DISPONIBLES ---");
            foreach (var ev in eventos)
            {
                Console.WriteLine(ev);
            }
        }

        public Evento? BuscarEvento(int id)
        {
            return eventos.FirstOrDefault(e => e.Id == id);
        }

        public void RealizarReserva(int idEvento, string persona, int cantidad)
        {
            if (string.IsNullOrWhiteSpace(persona))
            {
                throw new ArgumentException("El nombre de la persona es obligatorio.", nameof(persona));
            }

            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad de entradas debe ser mayor a cero.");
            }

            Evento? evento = BuscarEvento(idEvento);
            if (evento == null)
            {
                throw new KeyNotFoundException("El evento seleccionado no existe.");
            }

            if (cantidad > evento.CuposDisponibles)
            {
                throw new InvalidOperationException(
                    $"No hay suficientes cupos disponibles. Cupos actuales: {evento.CuposDisponibles}");
            }

            Usuario usuario = ObtenerOCrearUsuario(persona);
            Reserva nuevaReserva = new Reserva(contadorReservas, usuario, evento, cantidad);
            evento.ReservarCupos(cantidad);
            reservas.Add(nuevaReserva);
            contadorReservas++;

            Console.WriteLine($"\n ¡Reserva exitosa! Se han reservado {cantidad} entrada(s) para '{evento.Nombre}'. ID de Reserva: {nuevaReserva.IdReserva}");
        }

        public List<Reserva> BuscarReservasPorUsuario(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                throw new ArgumentException("El nombre del usuario es obligatorio.", nameof(nombreUsuario));
            }

            Usuario? usuario = usuarios.FirstOrDefault(
                u => string.Equals(u.Nombre, nombreUsuario.Trim(), StringComparison.OrdinalIgnoreCase));

            return usuario == null
                ? new List<Reserva>()
                : reservas.Where(r => r.Usuario.Id == usuario.Id).ToList();
        }

        private Usuario ObtenerOCrearUsuario(string nombre)
        {
            string nombreNormalizado = nombre.Trim();
            Usuario? usuario = usuarios.FirstOrDefault(
                u => string.Equals(u.Nombre, nombreNormalizado, StringComparison.OrdinalIgnoreCase));

            if (usuario != null)
            {
                return usuario;
            }

            usuario = new Usuario(contadorUsuarios++, nombreNormalizado);
            usuarios.Add(usuario);
            return usuario;
        }

        public void VerReservas()
        {
            Console.WriteLine("\n--- LISTA DE RESERVAS ---");
            if (reservas.Count == 0)
            {
                Console.WriteLine("No hay reservas registradas.");
                return;
            }

            foreach (var r in reservas)
            {
                Console.WriteLine(r);
            }
        }

        public void CancelarReserva(int idReserva)
        {
            Reserva? reserva = reservas.FirstOrDefault(r => r.IdReserva == idReserva);
            if (reserva == null)
            {
                throw new KeyNotFoundException("La reserva especificada no existe.");
            }

            reserva.Cancelar();

            Console.WriteLine($"\n La reserva #{idReserva} ha sido cancelada con éxito y se han devuelto los cupos.");
        }

        public void VerResumen()
        {
            var activas = reservas.Where(r => r.Activa).ToList();
            int totalReservasActivas = activas.Count;
            int totalEntradasReservadas = activas.Sum(r => r.CantidadEntradas);
            decimal valorTotalActivas = activas.Sum(r => r.ValorTotal);

            Console.WriteLine("\n================================");
            Console.WriteLine("     RESUMEN DE RESERVAS");
            Console.WriteLine("================================");
            Console.WriteLine($"Reservas activas: {totalReservasActivas}");
            Console.WriteLine($"Entradas reservadas (activas): {totalEntradasReservadas}");
            Console.WriteLine($"Valor total recaudado (activas): ${valorTotalActivas:N2}");
            Console.WriteLine("================================");
        }

        public void VerReservasPorUsuario(string nombreUsuario)
        {
            List<Reserva> reservasUsuario = BuscarReservasPorUsuario(nombreUsuario);
            Console.WriteLine($"\n--- RESERVAS DE {nombreUsuario.Trim()} ---");

            if (reservasUsuario.Count == 0)
            {
                Console.WriteLine("No hay reservas registradas para ese usuario.");
                return;
            }

            foreach (Reserva reserva in reservasUsuario)
            {
                Console.WriteLine(reserva);
            }
        }
    }
}