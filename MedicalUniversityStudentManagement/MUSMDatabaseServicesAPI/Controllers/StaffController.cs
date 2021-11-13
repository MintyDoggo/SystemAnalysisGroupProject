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
    public static class StaffController
    {
        /**
         * 
         * 
Example request body:

{
    "FirstName": "Paul",
    "LastName": "Morton"
}

         * 
         */
        [Function("CreateStaffAndReturnId")]
        public static async Task<HttpResponseData> CreateStaffAndReturnId([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StaffController");

            // Get the body of the request
            string requestBody = await req.ReadAsStringAsync();

            // Get the StaffModel from the request body
            StaffModel staff;
            try
            {
                staff = JsonSerializer.Deserialize<StaffModel>(requestBody);
                if (staff is null)
                {
                    throw new JsonException();
                }
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
                retVal = await StaffProcessor.CreateStaffAndReturnIdAsync(connectionString, staff);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var conflictResponse = req.CreateResponse(HttpStatusCode.Conflict);
                await conflictResponse.WriteStringAsync("Conflict when inserting into the database");
                return conflictResponse;
            }


            // Successfully added the Staff to the database
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
        [Function("DeleteStaffById")]
        public static async Task<HttpResponseData> DeleteStaffById([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StaffController");

            // Get the body of the request and deserialize it to json
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id of the Staff to delete from the request body
            int id = jsonBody.GetProperty("Id").GetInt32();

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

                await StaffProcessor.DeleteStaffByIdAsync(connectionString, id);
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
         * Get all Staff
         */
        [Function("GetStaff")]
        public static async Task<HttpResponseData> GetStaff([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StaffController");

            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");    
            IEnumerable<StaffModel> Staff = await StaffProcessor.GetStaffAsync(connectionString);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<StaffModel>>(Staff);
            return response;
        }

        /**
         * 
         * 
Example query parameters:

?loginId=3
         * 
         */
        [Function("GetStaffByLoginId")]
        public static async Task<HttpResponseData> GetStaffByLoginId([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext, int loginId)
        {
            ILogger logger = executionContext.GetLogger("StaffController");


            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            StaffModel staff = await StaffProcessor.GetStaffByLoginIdAsync(connectionString, loginId);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<StaffModel>(staff);
            return response;
        }








        [Function("UpdateStaffById")]
        public static async Task<HttpResponseData> UpdateStaffById([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StaffController");

            // Get the body of the request
            string requestBody = await req.ReadAsStringAsync();

            // Get the Id and StaffModel from the request body
            int id = -1;
            StaffModel staff;
            try
            {
                staff = JsonSerializer.Deserialize<StaffModel>(requestBody);
                if (staff is null)
                {
                    throw new JsonException();
                }

                id = staff.Id;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                return badRequestResponse;
            }

            // Call on the data processor
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
                await StaffProcessor.UpdateStaffByIdAsync(connectionString, id, staff);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var conflictResponse = req.CreateResponse(HttpStatusCode.Conflict);
                await conflictResponse.WriteStringAsync("Conflict when inserting into the database");
                return conflictResponse;
            }


            // Successfully updated the Staff in the database
            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }


    }
}
