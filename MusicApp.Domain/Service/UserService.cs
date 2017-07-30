using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Entities = MusicApp.Domain.Entities;

namespace MusicApp.Domain.Service
{
    public class UserService
    {
        internal const string CollectionName = "UserCollection";
        private DocumentClientFactory _documentClientFactory;

        public UserService(DocumentClientFactory factory)
        {
            _documentClientFactory = factory;
        }

        public async Task<List<Entities.User>> GetByUsername(string userName)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Entities.User> userQuery = client.CreateDocumentQuery<Entities.User>(
                CreateDocumentCollectionUri(), queryOptions)
                .Where(u => u.UserName == userName);

            return userQuery.ToList();
        }

        public Entities.User CreateNew()
        {
            return new Entities.User
            {
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow
            };
        }
        public async Task AddOrUpdate(Entities.User user)
        {
            DocumentClient client = await _documentClientFactory.GetClient();
            try
            {
                await client.ReadDocumentAsync(CreateDocumentUri(user));

                // If we successfully read then call update
                await Update(user);
            }
            catch (DocumentClientException dce)
            {
                if (dce.StatusCode == HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentAsync(CreateDocumentCollectionUri(), user);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task Update(Entities.User user)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            await client.ReplaceDocumentAsync(CreateDocumentUri(user), user);
        }

        public async Task Delete(Entities.User user)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            await client.DeleteDocumentAsync(CreateDocumentUri(user));
        }

        // Private Methods
        private Uri CreateDocumentUri(Entities.User user)
        {
            return UriFactory.CreateDocumentUri(MusicService.DatabaseName, CollectionName, user.Id);
        }

        private Uri CreateDocumentCollectionUri()
        {
            return UriFactory.CreateDocumentCollectionUri(MusicService.DatabaseName, CollectionName);
        }

    }
}
