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
    public static class ArtifactProcessor
    {
        public static async Task<int> CreateArtifactAndReturnIdAsync(string connectionString, ArtifactModel artifact)
        {
            // Name of our stored procedure to execute
            string procedureName = "spArtifact_CreateAndOutputId";


            // Create the Data Table representation of the user defined Artifact table
            DataTable artifactTable = new DataTable("@inArtifact");
            artifactTable.Columns.Add("RequiredArtifactId", typeof(int));
            artifactTable.Columns.Add("StudentId", typeof(int));
            artifactTable.Columns.Add("Document", typeof(byte[]));
            artifactTable.Columns.Add("CheckedOff", typeof(bool));
            // Fill in the data
            artifactTable.Rows.Add(artifact.RequiredArtifactId, artifact.StudentId, artifact.Document, artifact.CheckedOff);

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inArtifact", artifactTable.AsTableValuedParameter("udtArtifact"), DbType.Object);
            parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);

            // Return the outputed id from the stored procedure
            return parameters.Get<int>("@outId");
        }

        public static async Task UpdateArtifactByIdAsync(string connectionString, int id, ArtifactModel artifact)
        {
            // Name of our stored procedure to execute
            string procedureName = "spArtifact_UpdateById";


            // Create the Data Table representation of the user defined Artifact table
            DataTable artifactTable = new DataTable("@inArtifact");
            artifactTable.Columns.Add("RequiredArtifactId", typeof(int));
            artifactTable.Columns.Add("StudentId", typeof(int));
            artifactTable.Columns.Add("Document", typeof(byte[]));
            artifactTable.Columns.Add("CheckedOff", typeof(bool));
            // Fill in the data
            artifactTable.Rows.Add(artifact.RequiredArtifactId, artifact.StudentId, artifact.Document, artifact.CheckedOff);

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inId", id, DbType.Int32);
            parameters.Add("@inArtifact", artifactTable.AsTableValuedParameter("udtArtifact"), DbType.Object);


            // Execute our stored procedure with the parameters we made
            await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);
        }

        public static async Task<int> DeleteArtifactByIdAsync(string connectionString, int id)
        {
            string procedureName = "spArtifact_DeleteById";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inId", id, DbType.Int32);

            return await SqlDataAccess.ModifyDataAsync(connectionString, procedureName, parameters);
        }


        public static async Task<IEnumerable<ArtifactModel>> GetArtifactsAsync(string connectionString)
        {
            string procedureName = "spArtifact_SelectAll";

            return await SqlDataAccess.LoadDataAsync<ArtifactModel>(connectionString, procedureName);
        }

        public static async Task<IEnumerable<ArtifactModel>> GetArtifactsByStudentIdAsync(string connectionString, int studentId)
        {
            string procedureName = "spArtifact_SelectByStudentId";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inStudentId", studentId, DbType.Int32);

            return await SqlDataAccess.LoadDataAsync<ArtifactModel>(connectionString, procedureName, parameters);
        }

        public static async Task<IEnumerable<ArtifactModel>> GetArtifactsByRequiredArtifactIdAsync(string connectionString, int requiredArtifactId)
        {
            string procedureName = "spArtifact_SelectByRequiredArtifactId";

            // Make parameters to pass to the stored procedure
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@inRequiredArtifactId", requiredArtifactId, DbType.Int32);

            return await SqlDataAccess.LoadDataAsync<ArtifactModel>(connectionString, procedureName, parameters);
        }


    }
}
