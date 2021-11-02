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

            // Get the RequiredArtifactModel from the request body
            RequiredArtifactModel requiredArtifact;
            try
            {
                requiredArtifact = JsonSerializer.Deserialize<RequiredArtifactModel>(requestBody);
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
                retVal = await RequiredArtifactProcessor.CreateRequiredArtifactAndReturnIdAsync(connectionString, requiredArtifact);
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

        [Function("DeleteRequiredArtifactById")]
        public static async Task<HttpResponseData> DeleteRequiredArtifactById([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("RequiredArtifactController");

            // Get the body of the request and deserialize it to json
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id of the RequiredArtifact to delete from the request body
            int id = jsonBody.GetInt32();

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

                await RequiredArtifactProcessor.DeleteRequiredArtifactByIdAsync(connectionString, id);
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


    }
}
