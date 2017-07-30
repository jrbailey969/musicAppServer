using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using MusicApp.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Service
{
    public class MusicService
    {
        internal const string DatabaseName = "MusicDb";
        private DocumentClientFactory _documentClientFactory;

        public MusicService(DocumentClientFactory factory)
        {
            _documentClientFactory = factory;
        }

        public async Task InitializeStorage()
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseName });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName), new DocumentCollection { Id = UserService.CollectionName });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName), new DocumentCollection { Id = ArtistService.CollectionName });
        }
    }
}
