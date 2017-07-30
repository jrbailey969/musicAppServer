using MusicApp.Domain.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Entities.Classification
{
    public class ClassificationTag : IComparable<ClassificationTag>
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public int CompareTo(ClassificationTag other)
        {
            return string.Compare(this.Name, other.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, JsonHelper.GetSerializerSettings());
        }
    }
}
