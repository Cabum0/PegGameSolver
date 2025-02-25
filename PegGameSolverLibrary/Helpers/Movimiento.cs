using System.Text.Json.Serialization;
using System.Linq;

namespace PegGameSolverLibrary.Helpers
{
    public struct Movimiento
    {
        public Posicion Origen { get; set; }
        public Posicion Destino { get; set; }

        [JsonConstructor]
        public Movimiento(Posicion origen, Posicion destino) => (Origen, Destino) = (origen, destino);

        public Movimiento(string origen, string destino)
        {
            string[] origen_piezas = origen.Split(',');
            string[] destino_piezas = destino.Split(',');
            Origen = new Posicion(int.Parse(origen_piezas[0]), int.Parse(origen_piezas[1]));
            Destino = new Posicion(int.Parse(destino_piezas[0]), int.Parse(destino_piezas[1]));
        }
    }
}