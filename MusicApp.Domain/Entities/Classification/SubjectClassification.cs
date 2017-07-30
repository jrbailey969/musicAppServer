using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Entities.Classification
{
    public class SubjectClassification
    {
        public string ClassificationCategoryId { get; set; }
        public string ClassificationTagId { get; set; }
        public int? RangeValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
