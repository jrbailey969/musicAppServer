using MusicApp.Domain.Entities.Classification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.TransferModels
{
    public class ArtistSearchCriteria
    {
        public string Name { get; set; }
        public List<SubjectClassification> Classifications { get; set; }
    }
}
