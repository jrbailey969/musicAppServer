using MusicApp.Domain.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Enumerations = MusicApp.Domain.Entities.Enumerations;

namespace MusicApp.Domain.Entities.Classification
{
    public class ClassificationCategory
    {
        public ClassificationCategory()
        {
            this.Tags = new List<ClassificationTag>();
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int ClassificationCategoryTypeId { get; set; }
        public int? RangeMin { get; set; }
        public int? RangeMax { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public List<ClassificationTag> Tags { get; set; }

        [JsonIgnore]
        public Enumerations.ClassificationCategoryType ClassificationCategoryType
        {
            get { return (Enumerations.ClassificationCategoryType)this.ClassificationCategoryTypeId; }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, JsonHelper.GetSerializerSettings());
        }
    }
}
