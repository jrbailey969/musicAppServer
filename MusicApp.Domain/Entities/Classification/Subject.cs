using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Entities.Classification
{
    public class Subject
    {
        public Subject()
        {
            this.Classifications = new List<SubjectClassification>();
        }

        public List<SubjectClassification> Classifications { get; set; }
    }
}
