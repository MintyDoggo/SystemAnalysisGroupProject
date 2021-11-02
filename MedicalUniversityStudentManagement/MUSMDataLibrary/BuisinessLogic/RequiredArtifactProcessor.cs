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
    public static class RequiredArtifactProcessor
    {
        public static async Task<int> CreateRequiredArtifactAndReturnIdAsync(string connectionString, RequiredArtifactModel requiredArtifact)
        {
            // Name of our stored procedure to execute
            string procedureName = "spRequiredArtifact_CreateAndOutputId";


            // Create the Data Table representation of the user defined Required Artifact table
            DataTable artifactTable = new DataTable("@inRequiredArtifact");
            artifactTable.Columns.Add("Name", typeof(string));
            // Fill in the data
            artifactTable.Rows.Add(requiredArtifact.Name);

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inArtifact", artifactTable.AsTableValuedParameter("udtArtifact"), DbType.Object);
            parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);

            // Return the outputed id from the stored procedure
            return parameters.Get<int>("@outId");
        }

        public static async Task<int> DeleteRequiredArtifactByIdAsync(string connectionString, int id)
        {
            string procedureName = "spRequiredArtifact_DeleteById";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inId", id, dbType: DbType.Int32);

            return await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);
        }


    }
}
