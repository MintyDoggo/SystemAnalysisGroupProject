using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUSMModelsLibrary;

namespace MUSMDataLibrary.BuisinessLogic
{
    public class StudentProcessor
    {
        public static async Task<int> CreateStudentAndReturnIdAsync(string connString, string firstName, string lastName)
        {
            throw new NotImplementedException();
        }

        public static async Task<int> DeleteStudentByIdAsync(string connString, int id)
        {
            throw new NotImplementedException();
        }

        public static async Task<IEnumerable<StudentModel>> GetStudentsAsync(string connString)
        {
            throw new NotImplementedException();
        }

    }
}
