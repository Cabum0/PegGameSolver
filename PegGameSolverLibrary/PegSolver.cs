using PegGameSolverLibrary.Helpers;

namespace PegGameSolverLibrary
{
    public class PegSolver(int numeroHoyos, List<(Posicion origen, Posicion destino)>? historialMovimientosInicial = null)
    {
        public List<string> mensajesStatus = [];
        public string[][]? tablero_inicial;
        public List<Movimiento> MovimientosOutput = [];
        public readonly int numeroHoyos = numeroHoyos;
        private string[][]? tablero;
        private static readonly string clavija = "x";
        private static readonly string hueco = "o";
        public List<(Posicion origen, Posicion destino)> historialMovimientos = [];
        private readonly List<(Posicion origen, Posicion destino)> historialMovimientosInicial = historialMovimientosInicial;
        private int numeroPaso = 0;
        public List<string[][]?> HistoricoEstadosTablero = [];

        #region Parametros para medir eficiencia del recorrido
        public int totalClavijas = numeroHoyos;
        public int totalMovimientos = numeroHoyos;
        #endregion

        public async Task<bool> RunAsync()
        {
            if (numeroHoyos == 0)
            {
                mensajesStatus.Add($"El número de hoyos debe ser diferente a cero.");
                return false;
            }

            if (!EsNumeroTriangular(numeroHoyos))
            {
                mensajesStatus.Add($"El número {numeroHoyos} no es triangular.");
                return false;
            }

            if (numeroHoyos < 10)
            {
                mensajesStatus.Add($"Aunque el número {numeroHoyos} es triangular, se requiere un número mayor para hacer los movimientos.");
                return false;
            }

            CalcularMatrizTrianguloInverso(numeroHoyos);
            var listadoPosiciones = await BuscarOrigenAsync();

            if (listadoPosiciones.Count == 0)
            {
                mensajesStatus.Add("No se encontró un origen válido.");
                return false;
            }

            Random random = new Random();
            int intentos = 0;
            while (listadoPosiciones.Count > 0)
            {
                try
                {
                    int indiceAleatorio = random.Next(listadoPosiciones.Count);
                    (Posicion origen, Posicion destino) = listadoPosiciones[indiceAleatorio];
                    await MoverOrigenDestinoAsync(origen, destino);
                    listadoPosiciones = await BuscarOrigenAsync();
                }
                catch (Exception ex)
                {
                    mensajesStatus.Add($"Error en el intento {intentos + 1}: {ex.Message}");
                }
                intentos++;
            }

            ConvertirMovimientosFormatoSalida();
            await CalcularEficienciaAsync();
            mensajesStatus.Add($"Se resolvio el escenario con {totalMovimientos} movimientos y {totalClavijas} clavijas al final.");
            mensajesStatus.Add("Se ha finalizado con éxito la ejecución.");
            return true;
        }

        public static bool EsNumeroTriangular(int numero)
        {
            if (numero < 0) return false;
            int test = 8 * numero + 1;
            int raiz = (int)Math.Sqrt(test);
            return raiz * raiz == test;
        }

        public static async Task<string[][]?> MoverOrigenDestinoAsync((Posicion origen, Posicion destino) movimiento, string[][]? tableroPublico)
        {
            return await Task.Run(() =>
            {
                tableroPublico[movimiento.origen.fila][movimiento.origen.columna] = clavija;
                tableroPublico[(movimiento.origen.fila + movimiento.destino.fila) / 2][(movimiento.origen.columna + movimiento.destino.columna) / 2] = hueco;
                tableroPublico[movimiento.destino.fila][movimiento.destino.columna] = hueco;
                return tableroPublico;
            });
        }

        private async Task MoverOrigenDestinoAsync(Posicion origen, Posicion destino)
        {
            await Task.Run(() =>
            {
                tablero[origen.fila][origen.columna] = clavija;
                tablero[(origen.fila + destino.fila) / 2][(origen.columna + destino.columna) / 2] = hueco;
                tablero[destino.fila][destino.columna] = hueco;
                historialMovimientos.Add((origen, destino));
                HistoricoEstadosTablero.Add(tablero.Select(row => row.ToArray()).ToArray());
                numeroPaso++;
            });
        }

        private void CalcularMatrizTrianguloInverso(int n)
        {
            int filas = (int)((Math.Sqrt(1 + 8 * n) - 1) / 2);
            tablero = new string[filas][];

            for (int i = filas; i >= 1; i--)
            {
                int rowIndex = filas - i;
                tablero[rowIndex] = new string[i];

                for (int j = 0; j < i; j++)
                {
                    tablero[rowIndex][j] = (i == 1) ? hueco : clavija;
                }
            }
            // Copia profunda del tablero
            tablero_inicial = tablero.Select(row => row.ToArray()).ToArray();
            HistoricoEstadosTablero.Add(tablero_inicial);
        }

        private async Task<List<(Posicion origen, Posicion destino)>> BuscarOrigenAsync()
        {
            return await Task.Run(() => BuscarOrigen());
        }

        private List<(Posicion origen, Posicion destino)> BuscarOrigen()
        {
            var result = new List<(Posicion origen, Posicion destino)>();

            if (tablero == null) return result;

            for (int i = 0; i < tablero.Length; i++)
            {
                for (int j = 0; j < tablero[i].Length; j++)
                {
                    if (tablero[i][j] != hueco) continue;

                    var posicionOrigen = new Posicion(i, j);
                    var posicionesDestinoValidas = PosicionesValidas(posicionOrigen);

                    if (posicionesDestinoValidas.Count == 0) continue;

                    // Si solo hay una opción, agregarla directamente
                    if (posicionesDestinoValidas.Count == 1 || historialMovimientosInicial == null)
                    {
                        result.Add(posicionesDestinoValidas.First());
                        continue;
                    }

                    // Filtrar movimientos históricos
                    try
                    {
                        var movimientoHistorico = historialMovimientosInicial[numeroPaso];
                        var movimientosfiltrados = posicionesDestinoValidas.Where(mov => !MovimientosIguales(movimientoHistorico, mov)).ToList();
                        result = movimientosfiltrados;
                    }
                    catch
                    {
                        result = posicionesDestinoValidas;
                    }

                }
            }
            return result;
        }

        private List<(Posicion origen, Posicion destino)> PosicionesValidas(Posicion origen)
        {
            var offsets = new (int dx, int dy)[]
            {
            (0, 2), (0, -2), (2, 0), (-2, 0),
            (2, 2), (-2, -2), (-2, 2), (2, -2)
            };

            var intermedios = new (int dx, int dy)[]
            {
            (0, 1), (0, -1), (1, 0), (-1, 0),
            (1, 1), (-1, -1), (-1, 1), (1, -1)
            };

            var posicionesDestino = offsets
                .Select((offset, index) => new
                {
                    Final = new Posicion(origen.fila + offset.dx, origen.columna + offset.dy),
                    Intermedia = new Posicion(origen.fila + intermedios[index].dx, origen.columna + intermedios[index].dy)
                })
                .Where(p => EsValidaConIntermedia(p.Final, p.Intermedia))
                .Select(p => p.Final)
                .ToList();

            List<(Posicion origen, Posicion destino)> result = [];

            foreach (var posicionDestino in posicionesDestino) { result.Add((origen, posicionDestino)); }

            return result;
        }

        private bool EsValidaConIntermedia(Posicion final, Posicion intermedia) => EsValida(final, clavija) && EsValida(intermedia, clavija);

        private bool EsValida(Posicion pos, string estado)
        {
            // Verificar si matriz es null
            if (tablero == null || pos.fila < 0 || pos.fila >= tablero.Length)
                return false;

            // Verificar si la fila específica es null o está fuera de rango en 'y'
            if (tablero[pos.fila] == null || pos.columna < 0 || pos.columna >= tablero[pos.fila].Length)
                return false;

            // Comparar con el estado deseado
            return tablero[pos.fila][pos.columna] == estado;
        }

        public static bool MovimientosIguales((Posicion origen, Posicion destino) m1, (Posicion origen, Posicion destino) m2) =>
                           PosicionesIguales(m1.origen, m2.origen) &&
                            PosicionesIguales(m1.destino, m2.destino);

        public static bool PosicionesIguales(Posicion p1, Posicion p2) => p1.fila == p2.fila && p1.columna == p2.columna;

        private async Task CalcularEficienciaAsync()
        {
            await Task.Run(() =>
            {
                totalClavijas = tablero?.Sum(fila => fila.Count(celda => celda == clavija)) ?? 0;
                totalMovimientos = historialMovimientos.Count;
            });
        }

        private void ConvertirMovimientosFormatoSalida()
        {
            foreach (var movimiento in historialMovimientos)
            {
                MovimientosOutput.Add(new Movimiento(movimiento.origen, movimiento.destino));
            }
        }

    }
}