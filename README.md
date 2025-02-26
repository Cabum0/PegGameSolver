# PegGameSolver
PegGameSolver

The solutions has the following:

1. PegGameSolverLibrary: Is the .NET library with the logic to solve the Peg Game
2. PegSolverLibraryUnitTest: Unit test for the library
3. PegSolverAPI: Is an API using the library
4. peggamesolverweb: Is the Web aplication (React-Vite) consuming the API with the logic to solve the Peg Game

Core features of the solution:

- The algorithm supports an N-number of holes that can form a triangle.
- Results are stored in MongoDB.
- The core library has a unit test project.
- The solution includes a Docker Compose file to deploy the whole system quickly.
- The solution includes a web interface to improve the user experience.

Solution explanation:

The PegGameSolverLibrary project contains all the logic to simulate boards and perform different moves while following the game rules. The PegSolverAPI project uses this library and receives requests from clients, verifying if the optimal solution is already in the database to provide it. Otherwise, it calculates a possible solution, stores it in the database, and returns the result to the clients. The PegGameSolverWeb project provides a UI for a better user experience when interacting with the calculation engine.

Sample of the message receive from the Endpoint:

{
"numeroHoyos":21,
"totalClavijas":1,
"totalMovimientos":19,
"mensajesStatus":
[
"Se resolvio el escenario con 19 movimientos y 1 clavijas al final.",
"Se ha finalizado con éxito la ejecución."
],
"MovimientosOutput":[{"Origen":{"fila":5,"columna":0},"Destino":{"fila":3,"columna":0}},{"Origen":{"fila":3,"columna":0},"Destino":{"fila":1,"columna":2}},{"Origen":{"fila":4,"columna":0},"Destino":{"fila":2,"columna":0}},{"Origen":{"fila":3,"columna":0},"Destino":{"fila":5,"columna":0}},{"Origen":{"fila":2,"columna":1},"Destino":{"fila":4,"columna":1}},{"Origen":{"fila":4,"columna":1},"Destino":{"fila":2,"columna":3}},{"Origen":{"fila":3,"columna":1},"Destino":{"fila":1,"columna":3}},{"Origen":{"fila":3,"columna":2},"Destino":{"fila":1,"columna":0}},{"Origen":{"fila":2,"columna":3},"Destino":{"fila":0,"columna":5}},{"Origen":{"fila":2,"columna":1},"Destino":{"fila":4,"columna":1}},{"Origen":{"fila":0,"columna":5},"Destino":{"fila":0,"columna":3}},{"Origen":{"fila":2,"columna":0},"Destino":{"fila":0,"columna":2}},{"Origen":{"fila":2,"columna":2},"Destino":{"fila":2,"columna":0}},{"Origen":{"fila":1,"columna":4},"Destino":{"fila":3,"columna":2}},{"Origen":{"fila":0,"columna":2},"Destino":{"fila":0,"columna":0}},{"Origen":{"fila":2,"columna":3},"Destino":{"fila":0,"columna":5}},{"Origen":{"fila":2,"columna":1},"Destino":{"fila":2,"columna":3}},{"Origen":{"fila":1,"columna":2},"Destino":{"fila":3,"columna":0}},{"Origen":{"fila":2,"columna":2},"Destino":{"fila":0,"columna":2}}],
"HistoricoEstadosTablero":[[["x","x","x","x","x","x"],["x","x","x","x","x"],["x","x","x","x"],["x","x","x"],["x","x"],["o"]],[["x","x","x","x","x","x"],["x","x","x","x","x"],["x","x","x","x"],["o","x","x"],["o","x"],["x"]],[["x","x","x","x","x","x"],["x","x","o","x","x"],["x","o","x","x"],["x","x","x"],["o","x"],["x"]],[["x","x","x","x","x","x"],["x","x","o","x","x"],["o","o","x","x"],["o","x","x"],["x","x"],["x"]],[["x","x","x","x","x","x"],["x","x","o","x","x"],["o","o","x","x"],["x","x","x"],["o","x"],["o"]],[["x","x","x","x","x","x"],["x","x","o","x","x"],["o","x","x","x"],["x","o","x"],["o","o"],["o"]],[["x","x","x","x","x","x"],["x","x","o","x","x"],["o","x","x","o"],["x","o","o"],["o","x"],["o"]],[["x","x","x","x","x","x"],["x","x","o","o","x"],["o","x","o","o"],["x","x","o"],["o","x"],["o"]],[["x","x","x","x","x","x"],["o","x","o","o","x"],["o","o","o","o"],["x","x","x"],["o","x"],["o"]],[["x","x","x","x","x","o"],["o","x","o","o","o"],["o","o","o","x"],["x","x","x"],["o","x"],["o"]],[["x","x","x","x","x","o"],["o","x","o","o","o"],["o","x","o","x"],["x","o","x"],["o","o"],["o"]],[["x","x","x","o","o","x"],["o","x","o","o","o"],["o","x","o","x"],["x","o","x"],["o","o"],["o"]],[["x","x","o","o","o","x"],["o","o","o","o","o"],["x","x","o","x"],["x","o","x"],["o","o"],["o"]],[["x","x","o","o","o","x"],["o","o","o","o","o"],["o","o","x","x"],["x","o","x"],["o","o"],["o"]],[["x","x","o","o","o","x"],["o","o","o","o","x"],["o","o","x","o"],["x","o","o"],["o","o"],["o"]],[["o","o","x","o","o","x"],["o","o","o","o","x"],["o","o","x","o"],["x","o","o"],["o","o"],["o"]],[["o","o","x","o","o","o"],["o","o","o","o","o"],["o","o","x","x"],["x","o","o"],["o","o"],["o"]],[["o","o","x","o","o","o"],["o","o","o","o","o"],["o","x","o","o"],["x","o","o"],["o","o"],["o"]],[["o","o","x","o","o","o"],["o","o","x","o","o"],["o","o","o","o"],["o","o","o"],["o","o"],["o"]],[["o","o","o","o","o","o"],["o","o","o","o","o"],["o","o","x","o"],["o","o","o"],["o","o"],["o"]]]
}

Where we have the attributes as follow:

- numeroHoyos: As the number of holes
- totalClavijas: Total number of pegs ins the board at the end of calculation
- totalMovimientos: Total number of movements to achive that.
- mensajesStatus: Message log during execution.
- MovimientosOutput: List of movements in the inner matrix of the process.
- HistoricoEstadosTablero: States of the board during executions.

To use the docker compose file use the following command in a terminal:

docker-compose up --build

At the root folder of the solution for example:

C:\code\PegGameSolver

This will create 3 cotainers:

![image](https://github.com/user-attachments/assets/3fd43f1f-1601-426c-b46d-5837de649725)

Then you can use the web interface, go to:

http://localhost:3000/

Modify the ports in the Docker Compose file if needed.

Also you can use swagger to test the API directly:

http://localhost:5000/swagger/index.html

Finally, you can calculate different scenarios using the input box in the right corner by entering the number of holes for the board. By clicking each move in the movement list, you can visualize that scenario on the left board. The message log will provide additional information about the calculation process.

![image](https://github.com/user-attachments/assets/77d2acb8-1e77-4467-9560-406022503ab0)



