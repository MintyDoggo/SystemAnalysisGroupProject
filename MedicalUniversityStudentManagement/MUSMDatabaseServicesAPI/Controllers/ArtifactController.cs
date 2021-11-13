using System;
using System.Collections.Generic;
using System.IO;
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
         * Binary request body with query parameters.
Example query parameters:

?requiredArtifactId=1&studentId=2&checkedOff=false
         * 
         */
        [Function("CreateArtifactAndReturnId")]
        public static async Task<HttpResponseData> CreateArtifactAndReturnId([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext executionContext, int requiredArtifactId, int studentId, bool checkedOff)
        {
            ILogger logger = executionContext.GetLogger("ArtifactController");

            // Get the ArtifactModel from the query parameters
            ArtifactModel artifact = new();
            artifact.RequiredArtifactId = requiredArtifactId;
            artifact.StudentId = studentId;

            // artifact.Document
            {
                MemoryStream bodyStream = (MemoryStream)(req.Body);
                if (bodyStream is not null)
                {
                    artifact.Document = bodyStream.ToArray();
                }
                else
                {
                    using (MemoryStream memoryStream = new())
                    {
                        await req.Body.CopyToAsync(memoryStream);
                        artifact.Document = memoryStream.ToArray();
                    }
                }
            }

            artifact.CheckedOff = checkedOff;


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
         * Binary request body with query parameters.
Example query parameters:

?id=2&requiredArtifactId=1&studentId=2&checkedOff=true
         * 
         */
        [Function("UpdateArtifactById")]
        public static async Task<HttpResponseData> UpdateArtifactById([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req, FunctionContext executionContext, int id, int requiredArtifactId, int studentId, bool checkedOff)
        {
            ILogger logger = executionContext.GetLogger("ArtifactController");

            // Get the ArtifactModel from the query parameters
            ArtifactModel artifact = new();
            artifact.RequiredArtifactId = requiredArtifactId;
            artifact.StudentId = studentId;

            // artifact.Document
            {
                try
                {
                    MemoryStream bodyStream = (MemoryStream)(req.Body);
                    if (bodyStream is not null)
                    {
                        artifact.Document = bodyStream.ToArray();
                    }
                    else
                    {
                        using (MemoryStream memoryStream = new())
                        {
                            await req.Body.CopyToAsync(memoryStream);
                            artifact.Document = memoryStream.ToArray();
                        }
                    }
                }
                catch (Exception)
                {
                    using (MemoryStream memoryStream = new())
                    {
                        await req.Body.CopyToAsync(memoryStream);
                        artifact.Document = memoryStream.ToArray();
                    }
                }

            }

            artifact.CheckedOff = checkedOff;


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

        /**
         * 
         * 
Example request body:

{
    "Id": 1
}

         * 
         */
        [Function("DeleteArtifactById")]
        public static async Task<HttpResponseData> DeleteArtifactById([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, FunctionContext executionContext, int artifactId)
        {
            ILogger logger = executionContext.GetLogger("ArtifactController");

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

                await ArtifactProcessor.DeleteArtifactByIdAsync(connectionString, artifactId);
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
         * Get all Artifacts
         */
        [Function("GetArtifacts")]
        public static async Task<HttpResponseData> GetArtifacts([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("ArtifactController");

            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            IEnumerable<ArtifactModel> artifacts = await ArtifactProcessor.GetArtifactsAsync(connectionString);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<ArtifactModel>>(artifacts);
            return response;
        }

        /**
         * 
         * 
Example query parameters:

?studentId=2
         * 
         */
        [Function("GetArtifactsByStudentId")]
        public static async Task<HttpResponseData> GetArtifactsByStudentId([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext, int studentId)
        {
            ILogger logger = executionContext.GetLogger("ArtifactController");


            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            IEnumerable<ArtifactModel> Artifacts = await ArtifactProcessor.GetArtifactsByStudentIdAsync(connectionString, studentId);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<ArtifactModel>>(Artifacts);
            return response;
        }

        /**
         * 
         * 
Example query parameters:

?requiredArtifactId=1
         * 
         */
        [Function("GetArtifactsByRequiredArtifactId")]
        public static async Task<HttpResponseData> GetArtifactsByRequiredArtifactId([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext, int requiredArtifactId)
        {
            ILogger logger = executionContext.GetLogger("ArtifactController");


            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            IEnumerable<ArtifactModel> Artifacts = await ArtifactProcessor.GetArtifactsByRequiredArtifactIdAsync(connectionString, requiredArtifactId);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<ArtifactModel>>(Artifacts);
            return response;
        }


    }
}
