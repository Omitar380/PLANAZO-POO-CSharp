namespace PLANAZO_POO_CSharp.Modelos
{
    public class Usuario
    {
        public int Id { get; }
        public string Nombre { get; }

        public Usuario(int id, string nombre)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "El identificador debe ser mayor a cero.");
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del usuario es obligatorio.", nameof(nombre));
            }

            Id = id;
            Nombre = nombre.Trim();
        }

        public override string ToString() => $"[{Id}] {Nombre}";
    }
}
