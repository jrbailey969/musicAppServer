using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Domain.Service;
using MusicApp.Domain.Entities;
using MusicApp.Domain.TransferModels;
using Newtonsoft.Json;
using MusicApp.Domain.Entities.Classification;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicApp.Api.Controllers
{
    [Route("api/[controller]")]
    public class ArtistController : Controller
    {
        private readonly ArtistService _artistService;

        public ArtistController(ArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet("search")]
        public async Task<List<Artist>> Search(string criteria)
        {
            ArtistSearchCriteria searchCritiera = JsonConvert.DeserializeObject<ArtistSearchCriteria>(criteria);
            searchCritiera.Classifications = searchCritiera.Classifications ?? new List<SubjectClassification>();

            return await _artistService.Search("0e54b6da-caef-492f-bc95-b0669a8957d5", searchCritiera);
        }

        [HttpGet]
        public async Task<List<Artist>> Get()
        {
            return await _artistService.GetByUserId("0e54b6da-caef-492f-bc95-b0669a8957d5");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<Artist> Get(string id)
        {
            return await _artistService.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public async Task<Artist> Post([FromBody]Artist artist)
        {
            var newArtist = _artistService.CreateNew(new User { Id = "0e54b6da-caef-492f-bc95-b0669a8957d5" });
            newArtist.Name = artist.Name;
            newArtist.Classifications = artist.Classifications;
            await _artistService.AddOrUpdate(newArtist);

            return newArtist;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody]Artist artist)
        {
            artist.Id = id;
            await _artistService.Update(artist);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var artist = await _artistService.GetById(id);

            if (artist != null)
            {
                await _artistService.Delete(artist);
            }
        }
    }
}
