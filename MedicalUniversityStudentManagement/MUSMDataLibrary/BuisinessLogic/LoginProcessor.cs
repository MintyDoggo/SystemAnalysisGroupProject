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
    public static class LoginProcessor
    {
        public static async Task<int> CreateStaffLoginAndReturnIdAsync(string connectionString, string username, string password)
        {
            // Name of our stored procedure to execute
            string procedureName = "spStaff_CreateAndOutputId";


            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inUsername", username, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@inPassword", password, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);

            // Return the outputed Id from the stored procedure
            return parameters.Get<int>("@outId");
        }

        public static async Task<int> CreateStudentLoginAndReturnIdAsync(string connectionString, string username, string password, int staffId)
        {
            // Name of our stored procedure to execute
            string procedureName = "spStudent_CreateAndOutputId";


            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inUsername", username, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@inPassword", password, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@inStaffId", staffId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);

            // Return the outputed Id from the stored procedure
            return parameters.Get<int>("@outId");
        }



        // Does not provide ability to change the Login's database id (for good reason)
        public static async Task UpdateLoginByIdAsync(string connectionString, int id, LoginModel login)
        {
            // Name of our stored procedure to execute
            string procedureName = "spLogin_UpdateById";


            // Create the Data Table representation of the user defined Login table
            DataTable loginTable = new DataTable("@inLogin");
            loginTable.Columns.Add("Username", typeof(string));
            loginTable.Columns.Add("Password", typeof(string));
            loginTable.Columns.Add("UserType", typeof(int));

            // Fill in the data
            loginTable.Rows.Add(login.Username, login.Password, login.UserType);

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inId", id, DbType.Int32);
            parameters.Add("@inLogin", loginTable.AsTableValuedParameter("udtLogin"), DbType.Object);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);
        }

        public static async Task<int> DeleteLoginByIdAsync(string connectionString, int id)
        {
            string procedureName = "spLogin_DeleteById";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inId", id, DbType.Int32);

            return await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);
        }


        public static async Task<IEnumerable<LoginModel>> GetLoginsAsync(string connectionString)
        {
            string procedureName = "spLogin_SelectAll";

            return await SqlDataAccess.LoadDataAsync<LoginModel>(connectionString, procedureName);    
        }

        public static async Task<LoginModel> GetLoginByUsernameAndPassword(string connectionString, string username, string password)
        {
            string procedureName = "spLogin_SelectByUsernameAndPassword";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            //parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@inUsername", username, DbType.String);
            parameters.Add("@inPassword", password, DbType.String);


            // Execute our stored procedure with the parameters we made
            IEnumerable<LoginModel> logins = await SqlDataAccess.LoadDataAsync<LoginModel>(connectionString, procedureName, parameters);
            if (logins.Any())
            {
                return logins.First();
            }

            return null;
        }


    }
}
