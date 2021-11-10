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
    public static class StudentProcessor
    {
        public static async Task<int> CreateStudentAndReturnIdAsync(string connectionString, StudentModel student)
        {
            // Name of our stored procedure to execute
            string procedureName = "spStudent_CreateAndOutputId";


            // Create the Data Table representation of the user defined Student table
            DataTable studentTable = new DataTable("@inStudent");
            studentTable.Columns.Add("StaffId", typeof(int));
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
            studentTable.Rows.Add(student.StaffId, student.StudentIdNumber, student.FirstName, student.LastName, student.Birthday, student.Address, student.Major, student.FirstYearEnrolled, student.HighSchoolAttended, student.UndergraduateSchoolAttended);

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inStudent", studentTable.AsTableValuedParameter("udtStudent"), DbType.Object);
            parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);

            // Return the outputed id from the stored procedure
            return parameters.Get<int>("@outId");
        }

        public static async Task UpdateStudentByIdAsync(string connectionString, int id, StudentModel student)
        {
            // Name of our stored procedure to execute
            string procedureName = "spStudent_UpdateById";


            // Create the Data Table representation of the user defined Student table
            DataTable studentTable = new DataTable("@inStudent");
            studentTable.Columns.Add("StaffId", typeof(int));
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
            studentTable.Rows.Add(student.StaffId, student.StudentIdNumber, student.FirstName, student.LastName, student.Birthday, student.Address, student.Major, student.FirstYearEnrolled, student.HighSchoolAttended, student.UndergraduateSchoolAttended);

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inId", id, DbType.Int32);
            parameters.Add("@inStudent", studentTable.AsTableValuedParameter("udtStudent"), DbType.Object);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);
        }

        public static async Task<int> DeleteStudentByIdAsync(string connectionString, int id)
        {
            string procedureName = "spStudent_DeleteById";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inId", id, DbType.Int32);

            return await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);
        }

        public static async Task<IEnumerable<StudentModel>> GetStudentsAsync(string connectionString)
        {
            string procedureName = "spStudent_SelectAll";

            return await SqlDataAccess.LoadDataAsync<StudentModel>(connectionString, procedureName);    
        }

        public static async Task<IEnumerable<StudentModel>> GetStudentsByStaffIdAsync(string connectionString, int staffId)
        {
            string procedureName = "spStudent_SelectByStaffId";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inStaffId", staffId, DbType.Int32);

            return await SqlDataAccess.LoadDataAsync<StudentModel>(connectionString, procedureName, parameters);
        }
        

        public static async Task<StudentModel> GetStudentByLoginIdAsync(string connectionString, int loginId)
        {
            string procedureName = "spStudent_SelectByLoginId";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inLoginId", loginId, DbType.Int32);


            IEnumerable<StudentModel> students = await SqlDataAccess.LoadDataAsync<StudentModel>(connectionString, procedureName, parameters);
            if (students.Any())
            {
                return students.First();
            }

            return null;

        }

    }
}
