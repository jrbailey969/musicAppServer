using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Infrastructure;
using MusicApp.Domain.TransferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Entities = MusicApp.Domain.Entities;

namespace MusicApp.Domain.Service
{
    public class ArtistService
    {
        internal const string CollectionName = "ArtistCollection";
        private DocumentClientFactory _documentClientFactory;

        public ArtistService(DocumentClientFactory factory)
        {
            _documentClientFactory = factory;
        }

        public async Task<List<Artist>> Search(string userId, ArtistSearchCriteria criteria)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = 20 };

            StringBuilder builder = new StringBuilder();

            builder.Append("SELECT a.Id, a.Name FROM a ");
            for (int i = 0; i < criteria.Classifications.Count; i++)
            {
                builder.Append($"JOIN c{i} IN a.Classifications ");
            }
            builder.Append($"WHERE CONTAINS(UPPER(a.Name), UPPER('{criteria.Name}')) ");
            for (int i = 0; i < criteria.Classifications.Count; i++)
            {
                var classification = criteria.Classifications[i];
                builder.Append($"AND c{i}.ClassificationCategoryId = '{classification.ClassificationCategoryId}' ");
                if (!string.IsNullOrWhiteSpace(classification.ClassificationTagId))
                {
                    builder.Append($"AND c{i}.ClassificationTagId = '{classification.ClassificationTagId}' ");
                }
                if (classification.RangeValue.HasValue)
                {
                    builder.Append($"AND c{i}.RangeValue = {classification.RangeValue} ");
                }
            }
            builder.Append("ORDER BY a.Name");


            IQueryable<Artist> artistQuery = client.CreateDocumentQuery<Artist>(
                CreateDocumentCollectionUri(), builder.ToString(), queryOptions);

            return artistQuery.ToList();

        }

        public async Task<List<Artist>> GetByUserId(string userId)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = 20 };

            IQueryable<Artist> artistQuery = client.CreateDocumentQuery<Artist>(
                CreateDocumentCollectionUri(), queryOptions)
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.Name);

            return artistQuery.ToList();
        }

        public async Task<Artist> GetById(string id)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Artist> artistQuery = client.CreateDocumentQuery<Artist>(
                CreateDocumentCollectionUri(), queryOptions)
                .Where(a => a.Id == id);

            var results = artistQuery.ToList();
            return results.SingleOrDefault();
        }

        //public async Task<List<Artist>> GetByArtistname(string name)
        //{
        //    DocumentClient client = await _documentClientFactory.GetClient();

        //    FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

        //    IQueryable<Artist> artistQuery = client.CreateDocumentQuery<Artist>(
        //        CreateDocumentCollectionUri(), queryOptions)
        //        .Where(a => a.Name == name);

        //    return artistQuery.ToList();
        //}

        public Artist CreateNew(Entities.User user)
        {
            return new Artist
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow
            };
        }
        public async Task AddOrUpdate(Artist artist)
        {
            DocumentClient client = await _documentClientFactory.GetClient();
            try
            {
                await client.ReadDocumentAsync(CreateDocumentUri(artist));

                // If we successfully read then call update
                await Update(artist);
            }
            catch (DocumentClientException dce)
            {
                if (dce.StatusCode == HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentAsync(CreateDocumentCollectionUri(), artist);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task Update(Artist artist)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            artist.ModifiedDate = DateTime.UtcNow;
            await client.ReplaceDocumentAsync(CreateDocumentUri(artist), artist);
        }

        public async Task Delete(Artist artist)
        {
            DocumentClient client = await _documentClientFactory.GetClient();

            await client.DeleteDocumentAsync(CreateDocumentUri(artist));
        }

        // Private Methods
        private Uri CreateDocumentUri(Artist artist)
        {
            return UriFactory.CreateDocumentUri(MusicService.DatabaseName, CollectionName, artist.Id);
        }

        private Uri CreateDocumentCollectionUri()
        {
            return UriFactory.CreateDocumentCollectionUri(MusicService.DatabaseName, CollectionName);
        }

    }
}
