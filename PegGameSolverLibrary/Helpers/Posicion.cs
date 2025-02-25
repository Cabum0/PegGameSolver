namespace PegGameSolverLibrary.Helpers
{
    public readonly struct Posicion
    {
        public int fila { get; }
        public int columna { get; }
        public Posicion(int fila, int columna) => (this.fila, this.columna) = (fila, columna);
    }
}
