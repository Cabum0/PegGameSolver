using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using PegGameSolverLibrary;
using PegSolverAPI.Helpers;
using PegSolverAPI.Models;
using System.Dynamic;

namespace PegSolverAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PegSolverController(IOptions<DatabaseSettings> dbSettings) : ControllerBase
    {
        private readonly string connectionString = dbSettings.Value.ConnectionString;
        private readonly string database = dbSettings.Value.Database;
        private readonly string collection = dbSettings.Value.Collection;

        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented, // Formato legible
            NullValueHandling = NullValueHandling.Ignore, // Ignorar valores nulos
        };

        [HttpGet(Name = "CalculatePeg")]
        public async Task<IActionResult> CalculatePeg(int numeroHoyos = 0)
        {
            var escenario = RecuperarEscenario(numeroHoyos)
                .Select(ConvertBsonDocumentToDynamic)
                .FirstOrDefault();

            if (escenario?.totalClavijas == 1)
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(new TablePegResponse(escenario), jsonSettings));

            PegSolver result = await (new PegSolverPool(numeroHoyos)).Run();

            if (escenario == null || result.totalClavijas < escenario.totalClavijas)
            {
                if (escenario != null)
                    BorrarEscenario(numeroHoyos);

                GuardarMovimientosDB(result);
            }

            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(new TablePegResponse(result), jsonSettings));
        }

        private void GuardarMovimientosDB(PegSolver pegSolver)
        {
            MongoRepository repo = new(connectionString, database, collection);

            var movimientosArray = new BsonArray(
                pegSolver.historialMovimientos.Select(m => new BsonDocument
                {
                { "origen", $"{m.origen.fila},{m.origen.columna}" },
                { "destino", $"{m.destino.fila},{m.destino.columna}" }
                })
                );

            var mensajesArray = new BsonArray(
               pegSolver.mensajesStatus.Select(m => new BsonDocument
               {
                   { "mensaje", $"{m}" }
               })
               );

            var nuevoDocumento = new BsonDocument
        {
           
            { "numeroHoyos", pegSolver.numeroHoyos },
            { "totalClavijas", pegSolver.totalClavijas },
            { "totalMovimientos", pegSolver.totalMovimientos },
            { "mensajesStatus", new BsonArray(pegSolver.mensajesStatus.ToArray()) },
            { "Movimientos", movimientosArray },
            { "HistoricoEstadosTablero", new BsonArray( pegSolver.HistoricoEstadosTablero) }
 
        };
            if (pegSolver.numeroHoyos >= 10)
            {
                repo.Create(nuevoDocumento);
            }

        }

        private List<BsonDocument>? RecuperarEscenario(int numeroHoyos)
        {
            MongoRepository repo = new MongoRepository(connectionString, database, collection);
            var filtro = Builders<BsonDocument>.Filter.Eq("numeroHoyos", numeroHoyos);
            var documentos = repo.Read(filtro);
            return documentos;
        }

        private void BorrarEscenario(int numeroHoyos)
        {
            MongoRepository repo = new MongoRepository(connectionString, database, collection);
            var filtro = Builders<BsonDocument>.Filter.Eq("numeroHoyos", numeroHoyos);
            repo.Delete(filtro);
        }

        private static dynamic ConvertBsonDocumentToDynamic(BsonDocument bsonDocument)
        {
            string json = bsonDocument.ToJson();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(json);
        }

        private void TesTMongoDBConnection()
        {
            MongoRepository repo = new(connectionString, database, collection);

            // Leer e imprimir documentos
            var documentos = repo.Read();
            Console.WriteLine("Documentos en la colección:");
            foreach (var doc in documentos)
            {
                Console.WriteLine(doc);
            }


            //var filtro = Builders<BsonDocument>.Filter.Eq("numeroHoyos", 15);
            //var actualizacion = Builders<BsonDocument>.Update.Set("Movimientos", "(2,0) ---> (2,2)");
            //repo.Update(filtro, actualizacion);

        }
    }
}