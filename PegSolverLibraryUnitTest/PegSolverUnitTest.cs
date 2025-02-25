using PegGameSolverLibrary;
using PegGameSolverLibrary.Helpers;

namespace PegSolverLibraryUnitTest
{
    [TestFixture]
    public class PegSolverUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(1, true)]
        [TestCase(3, true)]
        [TestCase(6, true)]
        [TestCase(10, true)]
        [TestCase(15, true)]
        [TestCase(21, true)]
        [TestCase(28, true)]
        [TestCase(36, true)]
        [TestCase(45, true)]
        [TestCase(55, true)]
        [TestCase(-1, false)]
        [TestCase(2, false)]
        [TestCase(4, false)]
        [TestCase(5, false)]
        [TestCase(7, false)]
        [TestCase(8, false)]
        [TestCase(9, false)]
        [TestCase(11, false)]
        [TestCase(14, false)]
        [TestCase(20, false)]
        [TestCase(22, false)]
        [TestCase(30, false)]
        public void EsNumeroTriangular_ValoresDePrueba_DebeRetornarResultadoEsperado(int numero, bool resultadoEsperado)
        {
            // Act
            bool resultado = PegSolver.EsNumeroTriangular(numero);

            // Assert
            Assert.AreEqual(resultadoEsperado, resultado, $"El número {numero} debería retornar {resultadoEsperado}.");
        }

        [TestCase(0, 0, 0, 0, true)]
        [TestCase(1, 2, 1, 2, true)]
        [TestCase(3, 4, 3, 5, false)]
        [TestCase(6, 7, 6, 7, true)]
        [TestCase(8, 9, 9, 8, false)]
        public void PosicionesIguales_ValoresDePrueba_DebeRetornarResultadoEsperado(int fila1, int col1, int fila2, int col2, bool resultadoEsperado)
        {
            var p1 = new Posicion(fila1, col1);
            var p2 = new Posicion(fila2, col2);

            bool resultado = PegSolver.PosicionesIguales(p1, p2);

            Assert.AreEqual(resultadoEsperado, resultado, $"Las posiciones ({fila1}, {col1}) y ({fila2}, {col2}) deberían retornar {resultadoEsperado}.");
        }

        public static bool PosicionesIguales(Posicion p1, Posicion p2) => p1.fila == p2.fila && p1.columna == p2.columna;

        [TestCase(0, 0, 1, 1, 0, 0, 1, 1, true)]
        [TestCase(2, 3, 4, 5, 2, 3, 4, 5, true)]
        [TestCase(1, 2, 3, 4, 1, 2, 4, 3, false)]
        [TestCase(5, 6, 7, 8, 5, 6, 7, 9, false)]
        public void MovimientosIguales_ValoresDePrueba_DebeRetornarResultadoEsperado(int oFila1, int oCol1, int dFila1, int dCol1, int oFila2, int oCol2, int dFila2, int dCol2, bool resultadoEsperado)
        {
            var m1 = (new Posicion(oFila1, oCol1), new Posicion(dFila1, dCol1));
            var m2 = (new Posicion(oFila2, oCol2), new Posicion(dFila2, dCol2));

            bool resultado = PegSolver.MovimientosIguales(m1, m2);

            Assert.AreEqual(resultadoEsperado, resultado, $"Los movimientos [{oFila1}, {oCol1}] -> [{dFila1}, {dCol1}] y [{oFila2}, {oCol2}] -> [{dFila2}, {dCol2}] deberían retornar {resultadoEsperado}.");
        }

        [Test]
        public async Task MoverOrigenDestinoAsync_ValoresDePrueba_DebeActualizarTablero()
        {
            string clavija = "x";
            string hueco = "o";
            string[][] tableroPublico =
                [
                ["x", "x", "x", "x", "x"],
                ["x", "x", "x", "x"],
                ["x", "x", "x"],
                ["x", "x"],
                ["o"]
                ];

            var movimiento = (new Posicion(4, 0), new Posicion(2, 2));
            string[][]? resultado = await PegSolver.MoverOrigenDestinoAsync(movimiento, tableroPublico);

            Assert.AreEqual(clavija, resultado[4][0]);
            Assert.AreEqual(hueco, resultado[3][1]);
            Assert.AreEqual(hueco, resultado[2][2]);
        }

        [Test]
        public async Task RunAsync_ConNumeroTriangularMenorA10_DebeRetornarFalso()
        {
            var solver = new PegSolver(6);
            bool resultado = await solver.RunAsync();
            Assert.IsFalse(resultado);
            Assert.Contains("Aunque el número 6 es triangular, se requiere un número mayor para hacer los movimientos.", solver.mensajesStatus);
        }

        [Test]
        public async Task RunAsync_ConNumeroNoTriangular_DebeRetornarFalso()
        {
            var solver = new PegSolver(8);
            bool resultado = await solver.RunAsync();
            Assert.IsFalse(resultado);
            Assert.Contains("El número 8 no es triangular.", solver.mensajesStatus);
        }

        [Test]
        public async Task RunAsync_ConNumeroValido_DebeRetornarExito()
        {
            var solver = new PegSolver(10);
            bool resultado = await solver.RunAsync();
            Assert.IsTrue(resultado);
            Assert.Contains("Se ha finalizado con éxito la ejecución.", solver.mensajesStatus);
        }

    }
}
