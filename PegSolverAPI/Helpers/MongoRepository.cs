using MongoDB.Bson;
using MongoDB.Driver;

namespace PegSolverAPI.Helpers
{
    public class MongoRepository
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public MongoRepository(string connectionString, string database, string collection)
        {
            var cliente = new MongoClient(connectionString);
            var _database = cliente.GetDatabase(database);
            _collection = _database.GetCollection<BsonDocument>(collection);
        }

        public void Create(BsonDocument documento)
        {
            _collection.InsertOne(documento);
            Console.WriteLine("Documento insertado.");
        }

        public List<BsonDocument> Read(FilterDefinition<BsonDocument> filter) => _collection.Find(filter).ToList();

        public List<BsonDocument> Read() => _collection.Find(new BsonDocument()).ToList();

        public void Update(FilterDefinition<BsonDocument> filtro, UpdateDefinition<BsonDocument> actualizacion)
        {
            var resultado = _collection.UpdateOne(filtro, actualizacion);
            if (resultado.ModifiedCount > 0)
                Console.WriteLine("Documento actualizado.");
            else
                Console.WriteLine("No se encontró ningún documento para actualizar.");
        }

        public void Delete(FilterDefinition<BsonDocument> filtro)
        {
            var resultado = _collection.DeleteOne(filtro);
            if (resultado.DeletedCount > 0)
                Console.WriteLine("Documento eliminado.");
            else
                Console.WriteLine("No se encontró ningún documento para eliminar.");
        }
    }
}
