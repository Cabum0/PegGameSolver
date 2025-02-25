using PegGameSolverLibrary;
using PegGameSolverLibrary.Helpers;

namespace PegSolverAPI.Models
{
    public class TablePegResponse
    {
        
        public int numeroHoyos;
        public int totalClavijas;
        public int totalMovimientos;
        public string[] mensajesStatus;
        public List<Movimiento> MovimientosOutput;
        public List<string[][]?> HistoricoEstadosTablero;

        public TablePegResponse(PegSolver? pegSolver)
        {
            
            numeroHoyos = pegSolver.numeroHoyos;
            totalClavijas = pegSolver.totalClavijas;
            totalMovimientos = pegSolver.totalMovimientos;
            mensajesStatus = pegSolver.mensajesStatus.ToArray();
            MovimientosOutput = pegSolver.MovimientosOutput;
            HistoricoEstadosTablero = pegSolver.HistoricoEstadosTablero;
        }

        public TablePegResponse(dynamic? pegSolver)
        {
            List<Movimiento> lista_temporal_movs = [];

            foreach (var item in pegSolver.Movimientos)
            {
                lista_temporal_movs.Add(new Movimiento(item.origen, item.destino));
            }

            List<string> lista_temporal_mensajes = [];
            lista_temporal_mensajes.Add("Esta solucion fue recuperada de la base de datos ya no fue necesario calcular.");
            foreach (var item in pegSolver.mensajesStatus)
            {
                lista_temporal_mensajes.Add(item);
            }

            List<string[][]?> lista_historicoEstadosTablero = [];
            foreach (var item in pegSolver.HistoricoEstadosTablero)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                lista_historicoEstadosTablero.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<string[][]>(json));
            }

            
            numeroHoyos = (int)pegSolver.numeroHoyos;
            totalClavijas = (int)pegSolver.totalClavijas;
            totalMovimientos = (int)pegSolver.totalMovimientos;
            mensajesStatus = lista_temporal_mensajes.ToArray();
            MovimientosOutput = lista_temporal_movs;
            HistoricoEstadosTablero = lista_historicoEstadosTablero;
        }
    }
}
