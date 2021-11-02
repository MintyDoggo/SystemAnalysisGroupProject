using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MUSMDataLibrary.BuisinessLogic;
using MUSMModelsLibrary;

namespace MUSMDatabaseServicesAPI
{
    public static class ArtifactController
    {
        /**
         * 
         * 
Example request body:

{
    "Artifact": {
        "RequiredArtifactId": 1,
        "StudentId": 2,
        "DocumentReference": "my document is here",
        "CheckedOff": false
    }
}

         * 
         */
        [Function("CreateArtifactAndReturnId")]
        public static async Task<HttpResponseData> CreateArtifactAndReturnId([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("ArtifactController");

            // Get the body of the request
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the ArtifactModel from the request body
            ArtifactModel artifact;
            try
            {
                artifact = JsonSerializer.Deserialize<ArtifactModel>(jsonBody.GetProperty("Artifact").GetRawText());
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Request didn't meet syntax requirements (make sure you include everything and have the correct property types)");
                return badRequestResponse;
            }

            // Call on the data processor and return the Id
            int retVal;
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
                retVal = await ArtifactProcessor.CreateArtifactAndReturnIdAsync(connectionString, artifact);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var conflictResponse = req.CreateResponse(HttpStatusCode.Conflict);
                await conflictResponse.WriteStringAsync("Conflict when inserting into the database");
                return conflictResponse;
            }


            // Successfully added the Artifact to the database
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(retVal);
            return response;
        }

        /**
         * 
         * 
Example request body:

{
    "Id": 2,
    "Artifact": {
        "RequiredArtifactId": 1,
        "StudentId": 2,
        "DocumentReference": "my document is there",
        "CheckedOff": true
    }
}

         * 
         */
        [Function("UpdateArtifactById")]
        public static async Task<HttpResponseData> UpdateArtifactById([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("ArtifactController");

            // Get the body of the request
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id and ArtifactModel from the request body
            int id;
            ArtifactModel artifact;
            try
            {
                id = jsonBody.GetProperty("Id").GetInt32();
                artifact = JsonSerializer.Deserialize<ArtifactModel>(jsonBody.GetProperty("Artifact").GetRawText());
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Request didn't meet syntax requirements (make sure you include everything and have the correct property types)");
                return badRequestResponse;
            }

            // Call on the data processor
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
                await ArtifactProcessor.UpdateArtifactByIdAsync(connectionString, id, artifact);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var conflictResponse = req.CreateResponse(HttpStatusCode.Conflict);
                await conflictResponse.WriteStringAsync("Conflict when inserting into the database");
                return conflictResponse;
            }


            // Successfully updated the Artifact in the database
            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [Function("DeleteArtifactById")]
        public static async Task<HttpResponseData> DeleteArtifactById([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("ArtifactController");

            // Get the body of the request and deserialize it to json
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id of the Artifact to delete from the request body
            int id = jsonBody.GetInt32();

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

                await ArtifactProcessor.DeleteArtifactByIdAsync(connectionString, id);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var conflictResponse = req.CreateResponse(HttpStatusCode.Conflict);
                await conflictResponse.WriteStringAsync("Tried to delete row that didn't exist");
                return conflictResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Row successfully deleted");
            return response;
        }

        [Function("GetArtifactsByStudentId")]
        public static async Task<HttpResponseData> GetArtifactsByStudentId([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("ArtifactController");

            // Get the body of the request and deserialize it to json
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id of the Student from the request body
            int studentId = jsonBody.GetInt32();


            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            IEnumerable<ArtifactModel> Artifacts = await ArtifactProcessor.GetArtifactsByStudentIdAsync(connectionString, studentId);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<ArtifactModel>>(Artifacts);
            return response;
        }
        [Function("GetArtifactsByRequiredArtifactId")]
        public static async Task<HttpResponseData> GetArtifactsByRequiredArtifactId([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("ArtifactController");

            // Get the body of the request and deserialize it to json
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id of the Required Artifact from the request body
            int requiredArtifactId = jsonBody.GetInt32();


            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            IEnumerable<ArtifactModel> Artifacts = await ArtifactProcessor.GetArtifactsByRequiredArtifactIdAsync(connectionString, requiredArtifactId);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<ArtifactModel>>(Artifacts);
            return response;
        }


    }
}
