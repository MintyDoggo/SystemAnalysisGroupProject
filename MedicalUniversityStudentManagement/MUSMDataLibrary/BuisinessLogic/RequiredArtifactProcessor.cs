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
            parameters.Add("@inRequiredArtifact", artifactTable.AsTableValuedParameter("udtRequiredArtifact"), DbType.Object);
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
            parameters.Add("@inId", id, DbType.Int32);

            return await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);
        }

        public static async Task<IEnumerable<RequiredArtifactModel>> GetRequiredArtifactsAsync(string connectionString)
        {
            string procedureName = "spRequiredArtifact_SelectAll";

            return await SqlDataAccess.LoadDataAsync<RequiredArtifactModel>(connectionString, procedureName);
        }


    }
}
