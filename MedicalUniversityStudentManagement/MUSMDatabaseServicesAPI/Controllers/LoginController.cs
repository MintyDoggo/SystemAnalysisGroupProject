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
    public static class LoginController
    {
        /**
         * 
         * 
Example request body:

{update
    "Username": "PaulMorton27",
    "Password": "ihaveasecurepassword",
    "UserType": 2
}

         * 
         */
        [Function("CreateLoginAndReturnId")]
        public static async Task<HttpResponseData> CreateLoginAndReturnId([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("LoginController");

            // Get the body of the request
            string requestBody = await req.ReadAsStringAsync();
            string queryParameters = req.Url.Query;

            // Get the LoginModel from the request body
            LoginModel login;
            try
            {
                login = JsonSerializer.Deserialize<LoginModel>(requestBody);
                if (login is null)
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

                switch (login.UserType)
                {
                    case MUSMModelsLibrary.Enums.UserType.Staff:
                        retVal = await LoginProcessor.CreateStaffLoginAndReturnIdAsync(connectionString, login.Username, login.Password);
                        break;
                    case MUSMModelsLibrary.Enums.UserType.Student:
                        if (queryParameters.Contains("?staffId="))
                        {
                            // TODO: This query parameter thing sucks
                            int staffId = int.Parse(queryParameters.Remove(0, "?staffId=".Length));
                            retVal = await LoginProcessor.CreateStudentLoginAndReturnIdAsync(connectionString, login.Username, login.Password, staffId);
                            break;
                        }
                        var badRequestResponse1 = req.CreateResponse(HttpStatusCode.BadRequest);
                        return badRequestResponse1;

                    default:
                        var badRequestResponse2 = req.CreateResponse(HttpStatusCode.BadRequest);
                        return badRequestResponse2;
                }

            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var conflictResponse = req.CreateResponse(HttpStatusCode.Conflict);
                await conflictResponse.WriteStringAsync("Conflict when inserting into the database");
                return conflictResponse;
            }


            // Successfully added the Login to the database
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(retVal);
            return response;
        }

        /**
         * 
         * 
Example request body:

{
    "Id": 3,
    "Username": "PaulMorton27",
    "Password": "nowmypasswordisreallysecure",
    "UserType": 2
}

         * 
         */
        [Function("UpdateLoginById")]
        public static async Task<HttpResponseData> UpdateLoginById([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("LoginController");

            // Get the body of the request
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id and LoginModel from the request body
            int id;
            LoginModel login;
            try
            {
                login = JsonSerializer.Deserialize<LoginModel>(requestBody);
                if (login is null)
                {
                    throw new JsonException();
                }

                id = login.Id;
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
                await LoginProcessor.UpdateLoginByIdAsync(connectionString, id, login);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var conflictResponse = req.CreateResponse(HttpStatusCode.Conflict);
                await conflictResponse.WriteStringAsync("Conflict when inserting into the database");
                return conflictResponse;
            }


            // Successfully updated the Login in the database
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
        [Function("DeleteLoginById")]
        public static async Task<HttpResponseData> DeleteLoginById([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("LoginController");

            // Get the body of the request and deserialize it to json
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id of the Login to delete from the request body
            int id = jsonBody.GetProperty("Id").GetInt32();

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

                await LoginProcessor.DeleteLoginByIdAsync(connectionString, id);
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
         * Get all Logins
         */
        [Function("GetLogins")]
        public static async Task<HttpResponseData> GetLogins([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("LoginController");


            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            IEnumerable<LoginModel> Logins = await LoginProcessor.GetLoginsAsync(connectionString);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<LoginModel>>(Logins);
            return response;
        }

        /**
         * 
         * 
Example query parameters:

?username=PaulMorton27&password=ihaveasecurepassword
         * 
         */
        [Function("GetLoginByUsernameAndPassword")]
        public static async Task<HttpResponseData> GetLoginByUsernameAndPassword([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext, string username, string password)
        {
            ILogger logger = executionContext.GetLogger("LoginController");


            // Call on the data processor and return the Id
            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            LoginModel login = await LoginProcessor.GetLoginByUsernameAndPassword(connectionString, username, password);
            if (login is null)
            {
                var noContentResponse = req.CreateResponse(HttpStatusCode.NoContent);
                await noContentResponse.WriteStringAsync("No logins found with the given username and password");
                return noContentResponse;
            }

            // Successfully looked up the Id
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(login);
            return response;
        }

    }
}
