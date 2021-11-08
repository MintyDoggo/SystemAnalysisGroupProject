using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace MUSMModelsLibrary
{
    public class StudentModel
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int StudentIdNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string Major { get; set; }
        public int FirstYearEnrolled { get; set; }
        public string HighSchoolAttended { get; set; }
        public string UndergraduateSchoolAttended { get; set; }

    }
}
