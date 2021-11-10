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
    public static class RequiredArtifactController
    {
        /**
         * 
         * 
Example request body:

{
    "Name": "Final High School Transcript"
}

         * 
         */
        [Function("CreateRequiredArtifactAndReturnId")]
        public static async Task<HttpResponseData> CreateRequiredArtifactAndReturnId([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("RequiredArtifactController");

            // Get the body of the request
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            string reqName;
            try
            {
                reqName = jsonBody.GetProperty("Name").GetString();
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
                RequiredArtifactModel NewRequiredArtifactModel = new RequiredArtifactModel { Name = reqName };
                retVal = await RequiredArtifactProcessor.CreateRequiredArtifactAndReturnIdAsync(connectionString, NewRequiredArtifactModel);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var conflictResponse = req.CreateResponse(HttpStatusCode.Conflict);
                await conflictResponse.WriteStringAsync("Conflict when inserting into the database");
                return conflictResponse;
            }


            // Successfully added the RequiredArtifact to the database
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(retVal);
            return response;
        }

        /**
         * 
         * 
Example request body:

{
    "Id": 1
}

         * 
         */
        [Function("DeleteRequiredArtifactById")]
        public static async Task<HttpResponseData> DeleteRequiredArtifactById([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, FunctionContext executionContext, int requiredArtifactId)
        {
            ILogger logger = executionContext.GetLogger("RequiredArtifactController");

           
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

                await RequiredArtifactProcessor.DeleteRequiredArtifactByIdAsync(connectionString, requiredArtifactId);
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



        /**
         * Get all Required Artifacts
         */
        [Function("GetRequiredArtifacts")]
        public static async Task<HttpResponseData> GetGetRequiredArtifacts([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("RequiredArtifactController");

            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            IEnumerable<RequiredArtifactModel> RequiredArtifacts = await RequiredArtifactProcessor.GetRequiredArtifactsAsync(connectionString);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<RequiredArtifactModel>>(RequiredArtifacts);
            return response;
        }
    }
}
