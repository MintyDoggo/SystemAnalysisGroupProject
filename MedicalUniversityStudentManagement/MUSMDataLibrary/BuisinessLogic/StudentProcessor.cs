using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MUSMDataLibrary.DataAccess;
using MUSMModelsLibrary;

namespace MUSMDataLibrary.BuisinessLogic
{
    public class StudentProcessor
    {
        public static async Task<int> CreateStudentAndReturnIdAsync(string connString, StudentModel student)
        {
            // Name of our stored procedure to execute
            string procedureName = "spStudent_CreateAndOutputId";


            // Create the Data Table representation of the user defined Student table
            DataTable studentTable = new DataTable("@inStudent");
            studentTable.Columns.Add("StudentIdNumber", typeof(int));
            studentTable.Columns.Add("FirstName", typeof(string));
            studentTable.Columns.Add("LastName", typeof(string));
            studentTable.Columns.Add("Birthday", typeof(DateTime));
            studentTable.Columns.Add("Address", typeof(string));
            studentTable.Columns.Add("Major", typeof(string));
            studentTable.Columns.Add("FirstYearEnrolled", typeof(int));
            studentTable.Columns.Add("HighSchoolAttended", typeof(string));
            studentTable.Columns.Add("UndergraduateSchoolAttended", typeof(string));
            // Fill in the data
            studentTable.Rows.Add(student.StudentIdNumber, student.FirstName, student.LastName, student.Birthday, student.Address, student.Major, student.FirstYearEnrolled, student.HighSchoolAttended, student.UndergraduateSchoolAttended);

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inStudent", studentTable.AsTableValuedParameter("udtStudent"), DbType.Object);
            parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connString, procedureName, parameters);

            // Return the outputed id from the stored procedure
            return parameters.Get<int>("@outId");
        }

        public static async Task<int> DeleteStudentByIdAsync(string connString, int id)
        {
            string procedureName = "spStudent_DeleteById";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inId", id, dbType: DbType.Int32);

            return await SqlDataAccess.ModifyDataAsync(connString, procedureName, parameters);
        }

        public static async Task<IEnumerable<StudentModel>> GetStudentsAsync(string connString)
        {
            throw new NotImplementedException();
        }

    }
}
