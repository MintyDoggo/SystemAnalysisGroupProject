using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace MUSMModelsLibrary
{
    public class ArtifactModel
    {
        public int Id { get; set; }
        public int RequiredArtifactId { get; set; }
        public int StudentId { get; set; }

        public byte[] Document { get; set; }
        public bool CheckedOff { get; set; }

    }
}
