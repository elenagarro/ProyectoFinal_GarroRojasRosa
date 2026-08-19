using Google.Cloud.Firestore;

namespace ProyectoFinal_GarroRojasRosa.Services
{
    public class FirestoreService
    {
        private readonly FirestoreDb _firestoreDb;

        public FirestoreService(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;
        }

        // Crear o actualizar un documento
        public async Task GuardarDocumentoAsync(
            string coleccion,
            string idDocumento,
            Dictionary<string, object> datos)
        {
            DocumentReference documento =
                _firestoreDb
                    .Collection(coleccion)
                    .Document(idDocumento);

            await documento.SetAsync(datos);
        }

        // Obtener todos los documentos de una colección
        public async Task<List<Dictionary<string, object>>>
            ObtenerColeccionAsync(string coleccion)
        {
            QuerySnapshot snapshot =
                await _firestoreDb
                    .Collection(coleccion)
                    .GetSnapshotAsync();

            var resultados =
                new List<Dictionary<string, object>>();

            foreach (DocumentSnapshot documento in snapshot.Documents)
            {
                var datos = documento.ToDictionary();

                datos["Id"] = documento.Id;

                resultados.Add(datos);
            }

            return resultados;
        }

        // Eliminar un documento
        public async Task EliminarDocumentoAsync(
            string coleccion,
            string idDocumento)
        {
            await _firestoreDb
                .Collection(coleccion)
                .Document(idDocumento)
                .DeleteAsync();
        }
    }
}