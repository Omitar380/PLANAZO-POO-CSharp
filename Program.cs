using PLANAZO_POO_CSharp.Servicios;

class Program
{
    static void Main(string[] args)
    {
        GestorReservas gestor = new GestorReservas();
        bool continuar = true;

        while (continuar)
        {
            MostrarMenu();
            string? entrada = Console.ReadLine();

            if (entrada == null)
            {
                break;
            }

            if (!int.TryParse(entrada, out int opcion))
            {
                Console.WriteLine("\nError: Por favor ingrese un número válido.");
                continue;
            }

            try
            {
                switch (opcion)
                {
                    case 1:
                        gestor.ListarEventos();
                        break;
                    case 2:
                        ConsultarEvento(gestor);
                        break;
                    case 3:
                        ReservarEntradas(gestor);
                        break;
                    case 4:
                        gestor.VerReservas();
                        break;
                    case 5:
                        CancelarReserva(gestor);
                        break;
                    case 6:
                        gestor.VerResumen();
                        break;
                    case 7:
                        ConsultarReservasUsuario(gestor);
                        break;
                    case 0:
                        Console.WriteLine("\n¡Gracias por usar PLANAZO! Hasta pronto.");
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("\nOpción no válida. Intente de nuevo.");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nError de validación: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\nOperación no permitida: {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"\nNo se encontró el elemento: {ex.Message}");
            }
        }
    }

    private static void MostrarMenu()
    {
        Console.WriteLine("\n================================");
        Console.WriteLine("        PLANAZO - EVENTOS       ");
        Console.WriteLine("================================");
        Console.WriteLine("1. Ver eventos disponibles");
        Console.WriteLine("2. Consultar evento");
        Console.WriteLine("3. Reservar entradas");
        Console.WriteLine("4. Ver reservas realizadas");
        Console.WriteLine("5. Cancelar una reserva");
        Console.WriteLine("6. Ver resumen de reservas");
        Console.WriteLine("7. Consultar reservas por usuario");
        Console.WriteLine("0. Salir");
        Console.Write("\nSeleccione una opción: ");
    }

    private static void ConsultarEvento(GestorReservas gestor)
    {
        Console.Write("Ingrese el ID del evento a consultar: ");
        if (!int.TryParse(Console.ReadLine(), out int idConsulta))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var evento = gestor.BuscarEvento(idConsulta);
        if (evento == null)
        {
            Console.WriteLine("El evento con ese ID no existe.");
            return;
        }

        Console.WriteLine($"\nDetalles del evento:\n{evento}");
    }

    private static void ReservarEntradas(GestorReservas gestor)
    {
        gestor.ListarEventos();
        Console.Write("\nIngrese el ID del evento que desea reservar: ");
        if (!int.TryParse(Console.ReadLine(), out int idEvento))
        {
            Console.WriteLine("ID de evento inválido.");
            return;
        }

        Console.Write("Ingrese su nombre completo: ");
        string nombre = Console.ReadLine() ?? string.Empty;

        Console.Write("Ingrese la cantidad de entradas: ");
        if (!int.TryParse(Console.ReadLine(), out int cantidad))
        {
            Console.WriteLine("La cantidad de entradas debe ser un número entero.");
            return;
        }

        gestor.RealizarReserva(idEvento, nombre, cantidad);
    }

    private static void CancelarReserva(GestorReservas gestor)
    {
        gestor.VerReservas();
        Console.Write("\nIngrese el ID de la reserva que desea cancelar: ");
        if (!int.TryParse(Console.ReadLine(), out int idReserva))
        {
            Console.WriteLine("ID de reserva inválido.");
            return;
        }

        gestor.CancelarReserva(idReserva);
    }

    private static void ConsultarReservasUsuario(GestorReservas gestor)
    {
        Console.Write("Ingrese el nombre del usuario: ");
        string nombre = Console.ReadLine() ?? string.Empty;
        gestor.VerReservasPorUsuario(nombre);
    }
}
