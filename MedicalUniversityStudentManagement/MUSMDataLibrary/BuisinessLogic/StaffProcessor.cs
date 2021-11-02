using Dapper;
using MUSMDataLibrary.DataAccess;
using MUSMModelsLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUSMDataLibrary.BuisinessLogic
{
    public static class StaffProcessor
    {
        public static async Task<int> CreateStaffAndReturnIdAsync(string connectionString, StaffModel staff)
        {
            // Name of our stored procedure to execute
            string procedureName = "spStaff_CreateAndOutputId";


            // Create the Data Table representation of the user defined Staff table
            DataTable staffTable = new DataTable("@inStaff");
            staffTable.Columns.Add("FirstName", typeof(string));
            staffTable.Columns.Add("LastName", typeof(string));
            // Fill in the data
            staffTable.Rows.Add(staff.FirstName, staff.LastName);

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inStaff", staffTable.AsTableValuedParameter("udtStaff"), DbType.Object);
            parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);

            // Return the outputed id from the stored procedure
            return parameters.Get<int>("@outId");
        }

        public static async Task<int> DeleteStaffByIdAsync(string connectionString, int id)
        {
            string procedureName = "spStaff_DeleteById";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inId", id, DbType.Int32);

            return await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);
        }

        public static async Task<IEnumerable<StaffModel>> GetStaffsAsync(string connectionString)
        {
            string procedureName = "spStaff_SelectAll";

            return await SqlDataAccess.LoadDataAsync<StaffModel>(connectionString, procedureName);
        }


    }
}
