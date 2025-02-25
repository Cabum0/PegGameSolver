namespace PegGameSolverLibrary
{
    public class PegSolverPool(int numeroHoyos)
    {
        public readonly int numeroHoyos = numeroHoyos;
        private int TotalClavijasMasBajo = 100;
        private readonly int MaximoPegSolverPool = 100;

        public async Task<PegSolver?> Run()
        {
            var listaCaminos = new List<PegSolver> { new PegSolver(numeroHoyos) };

            for (int i = 0; i < MaximoPegSolverPool; i++)
            {
                var caminoActual = listaCaminos[i];
                await caminoActual.RunAsync();

                if (numeroHoyos == 0)
                {
                    break;
                }

                TotalClavijasMasBajo = caminoActual.totalClavijas;

                if (TotalClavijasMasBajo == 1)
                {
                    break;
                }

                listaCaminos.Add(new PegSolver(numeroHoyos, caminoActual.historialMovimientos));
            }
            listaCaminos = listaCaminos.OrderBy(x => x.totalClavijas).ToList();
            var mejorresultado = listaCaminos.FirstOrDefault();
            return mejorresultado;
        }
    }
}