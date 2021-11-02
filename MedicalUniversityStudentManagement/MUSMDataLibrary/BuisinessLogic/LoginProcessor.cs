﻿using System;
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
        public static async Task<int> CreateLoginAndReturnIdAsync(string connectionString, LoginModel login)
        {
            // Name of our stored procedure to execute
            string procedureName = "spLogin_CreateAndOutputId";


            // Create the Data Table representation of the user defined Login table
            DataTable loginTable = new DataTable("@inLogin");
            loginTable.Columns.Add("Username", typeof(string));
            loginTable.Columns.Add("Password", typeof(string));
            loginTable.Columns.Add("UserType", typeof(int));

            // Fill in the data
            loginTable.Rows.Add(login.Username, login.Password, login.UserType);

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inLogin", loginTable.AsTableValuedParameter("udtLogin"), DbType.Object);
            parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);

            // Return the outputed id from the stored procedure
            return parameters.Get<int>("@outId");
        }

        public static async Task<int> DeleteLoginByIdAsync(string connectionString, int id)
        {
            string procedureName = "spLogin_DeleteById";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inId", id, dbType: DbType.Int32);

            return await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);
        }

        public static async Task<IEnumerable<LoginModel>> GetLoginsAsync(string connectionString)
        {
            string procedureName = "spLogin_SelectAll";

            return await SqlDataAccess.LoadDataAsync<LoginModel>(connectionString, procedureName);    
        }

        public static async Task<int> ReturnLoginIdByUsernameAndPassword(string connectionString, LoginModel login)
        {
            string procedureName = "spLogin_ReturnLoginIdByUsernameAndPassword";

            // Create the Data Table representation of the user defined Login table
            DataTable loginTable = new DataTable("@inLogin");
            loginTable.Columns.Add("Username", typeof(string));
            loginTable.Columns.Add("Password", typeof(string));
            loginTable.Columns.Add("UserType", typeof(int));

            // Fill in the data
            loginTable.Rows.Add(login.Username, login.Password, 0); // UserType not needed for the stored procedure so we'll just put 0

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inLogin", loginTable.AsTableValuedParameter("udtLogin"), DbType.Object);
            parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);

            // Return the outputed id from the stored procedure
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


    }
}
