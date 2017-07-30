using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Entities.Classification;
using MusicApp.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MusicApp.Domain.Service
{
    public class ClassificationCategoryService
    {
        internal const string CollectionName = "ClassificationCategoryCollection";
        private DocumentClientFactory _documentClientFactory;

        public ClassificationCategoryService(DocumentClientFactory factory)
        {
            _documentClientFactory = factory;
        }

        public async Task<List<ClassificationCategory>> GetByUserId(string userId)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<ClassificationCategory> categoryQuery = client.CreateDocumentQuery<ClassificationCategory>(
                CreateDocumentCollectionUri(), queryOptions)
                .Where(u => u.UserId == userId);

            return categoryQuery.ToList();
        }

        public async Task<ClassificationCategory> GetById(string id)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<ClassificationCategory> categoryQuery = client.CreateDocumentQuery<ClassificationCategory>(
                CreateDocumentCollectionUri(), queryOptions)
                .Where(u => u.Id == id);

            var results = categoryQuery.ToList();
            return results.SingleOrDefault();
        }

        public async Task<List<ClassificationCategory>> GetByName(string name)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<ClassificationCategory> categoryQuery = client.CreateDocumentQuery<ClassificationCategory>(
                CreateDocumentCollectionUri(), queryOptions)
                .Where(u => u.Name == name);

            return categoryQuery.ToList();
        }

        public ClassificationCategory CreateNew(Entities.User user)
        {
            return new ClassificationCategory
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow
            };
        }

        public ClassificationTag CreateNewTag()
        {
            return new ClassificationTag
            {
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow
            };
        }

        public async Task AddOrUpdate(ClassificationCategory category)
        {
            DocumentClient client = await _documentClientFactory.GetClient();
            try
            {
                await client.ReadDocumentAsync(CreateDocumentUri(category));

                // If we successfully read then call update
                await Update(category);
            }
            catch (DocumentClientException dce)
            {
                if (dce.StatusCode == HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentAsync(CreateDocumentCollectionUri(), category);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task Update(ClassificationCategory category)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            category.ModifiedDate = DateTime.UtcNow;
            await client.ReplaceDocumentAsync(CreateDocumentUri(category), category);
        }

        public async Task Delete(ClassificationCategory category)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            await client.DeleteDocumentAsync(CreateDocumentUri(category));
        }

        // Private Methods
        private Uri CreateDocumentUri(ClassificationCategory category)
        {
            return UriFactory.CreateDocumentUri(MusicService.DatabaseName, CollectionName, category.Id);
        }

        private Uri CreateDocumentCollectionUri()
        {
            return UriFactory.CreateDocumentCollectionUri(MusicService.DatabaseName, CollectionName);
        }

    }
}
