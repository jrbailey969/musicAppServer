using MusicApp.Domain.Entities.Classification;
using MusicApp.Domain.Helpers;
using Newtonsoft.Json;
using System;

namespace MusicApp.Domain.Entities
{
    public class Artist : Subject
    {
        public Artist()
            : base()
        {
        }
        
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, JsonHelper.GetSerializerSettings());
        }
    }
}
