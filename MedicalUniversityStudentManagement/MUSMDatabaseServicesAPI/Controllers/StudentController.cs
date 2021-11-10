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
using System.Collections.Specialized;
using System.Web;

namespace MUSMDatabaseServicesAPI
{
    public static class StudentController
    {
        /**
         * 
         * 
Example request body:

{
    "StaffId": 3,
    "StudentIdNumber": 23,
    "FirstName": "Clyde",
    "LastName": "Clyde",
    "Birthday": "1993-11-08",
    "Address": "clyde lives in miami",
    "Major": "computer",
    "FirstYearEnrolled": 2001,
    "HighSchoolAttended": "Bane Bay",
    "UndergraduateSchoolAttended": "homeschool o ya"
}

         * 
         */
        [Function("CreateStudentAndReturnId")]
        public static async Task<HttpResponseData> CreateStudentAndReturnId([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StudentController");

            // Get the body of the request
            string requestBody = await req.ReadAsStringAsync();

            // Get the StudentModel from the request body
            StudentModel student;
            try
            {
                student = JsonSerializer.Deserialize<StudentModel>(requestBody);
                if (student is null)
                {
                    throw new JsonException();
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Request body didn't meet syntax requirements. Example body:\n");
                await badRequestResponse.WriteStringAsync("\n");
                await badRequestResponse.WriteStringAsync("{\n");
                await badRequestResponse.WriteStringAsync("    \"StaffId\": 3,\n");
                await badRequestResponse.WriteStringAsync("    \"StudentIdNumber\": 23,\n");
                await badRequestResponse.WriteStringAsync("    \"FirstName\": \"Clyde\",\n");
                await badRequestResponse.WriteStringAsync("    \"LastName\": \"Clyde\",\n");
                await badRequestResponse.WriteStringAsync("    \"Birthday\": \"1993-11-08\",\n");
                await badRequestResponse.WriteStringAsync("    \"Address\": \"clyde lives in miami\",\n");
                await badRequestResponse.WriteStringAsync("    \"Major\": \"computer\",\n");
                await badRequestResponse.WriteStringAsync("    \"FirstYearEnrolled\": 2001,\n");
                await badRequestResponse.WriteStringAsync("    \"HighSchoolAttended\": \"Bane Bay\",\n");
                await badRequestResponse.WriteStringAsync("    \"UndergraduateSchoolAttended\": \"homeschool o ya\"\n");
                await badRequestResponse.WriteStringAsync("}\n");
                return badRequestResponse;
            }

            // Call on the data processor and return the Id
            int retVal;
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
                retVal = await StudentProcessor.CreateStudentAndReturnIdAsync(connectionString, student);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var conflictResponse = req.CreateResponse(HttpStatusCode.Conflict);
                await conflictResponse.WriteStringAsync("Conflict when inserting into the database");
                return conflictResponse;
            }


            // Successfully added the Student to the database
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(retVal);
            return response;
        }

        /**
         * 
         * 
Example request body:

{
    "Id": 4,
    "StaffId": 3,
    "StudentIdNumber": 23,
    "FirstName": "Anthony",
    "LastName": "Anthony",
    "Birthday": "1995-07-23",
    "Address": "ohio",
    "Major": "computer",
    "FirstYearEnrolled": 2003,
    "HighSchoolAttended": "Bane Bay",
    "UndergraduateSchoolAttended": "homeschool o ya"
}

         * 
         */
        [Function("UpdateStudentById")]
        public static async Task<HttpResponseData> UpdateStudentById([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StudentController");

            // Get the body of the request
            string requestBody = await req.ReadAsStringAsync();

            // Get the Id and StudentModel from the request body
            int id = -1;
            StudentModel student;
            try
            {
                student = JsonSerializer.Deserialize<StudentModel>(requestBody);
                if (student is null)
                {
                    throw new JsonException();
                }

                id = student.Id;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Request body didn't meet syntax requirements. Example body:\n");
                await badRequestResponse.WriteStringAsync("\n");
                await badRequestResponse.WriteStringAsync("{\n");
                await badRequestResponse.WriteStringAsync("    \"Id\": 4,\n");
                await badRequestResponse.WriteStringAsync("    \"StaffId\": 3,\n");
                await badRequestResponse.WriteStringAsync("    \"StudentIdNumber\": 23,\n");
                await badRequestResponse.WriteStringAsync("    \"FirstName\": \"Anthony\",\n");
                await badRequestResponse.WriteStringAsync("    \"LastName\": \"Anthony\",\n");
                await badRequestResponse.WriteStringAsync("    \"Birthday\": \"1995-07-23\",\n");
                await badRequestResponse.WriteStringAsync("    \"Address\": \"ohio\",\n");
                await badRequestResponse.WriteStringAsync("    \"Major\": \"computer\",\n");
                await badRequestResponse.WriteStringAsync("    \"FirstYearEnrolled\": 2003,\n");
                await badRequestResponse.WriteStringAsync("    \"HighSchoolAttended\": \"Bane Bay\",\n");
                await badRequestResponse.WriteStringAsync("    \"UndergraduateSchoolAttended\": \"homeschool o ya\"\n");
                await badRequestResponse.WriteStringAsync("}\n");
                return badRequestResponse;
            }

            // Call on the data processor
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
                await StudentProcessor.UpdateStudentByIdAsync(connectionString, id, student);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var conflictResponse = req.CreateResponse(HttpStatusCode.Conflict);
                await conflictResponse.WriteStringAsync("Conflict when inserting into the database");
                return conflictResponse;
            }


            // Successfully updated the Student in the database
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
        [Function("DeleteStudentById")]
        public static async Task<HttpResponseData> DeleteStudentById([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StudentController");

            // Get the body of the request and deserialize it to json
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id of the Student to delete from the request body
            int id;
            try
            {
                id = jsonBody.GetProperty("Id").GetInt32();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Request body didn't meet syntax requirements. Example body:\n");
                await badRequestResponse.WriteStringAsync("\n");
                await badRequestResponse.WriteStringAsync("{\n");
                await badRequestResponse.WriteStringAsync("    \"Id\": 1,\n");
                await badRequestResponse.WriteStringAsync("}\n");
                return badRequestResponse;
            }

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

                await StudentProcessor.DeleteStudentByIdAsync(connectionString, id);
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
         * Get all Students
         */
        [Function("GetStudents")]
        public static async Task<HttpResponseData> GetStudents([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StudentController");

            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            IEnumerable<StudentModel> Students = await StudentProcessor.GetStudentsAsync(connectionString);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<StudentModel>>(Students);
            return response;
        }

        /**
         * 
         * 
Example query parameters:

?loginId=4
         * 
         */
        [Function("GetStudentByLoginId")]
        public static async Task<HttpResponseData> GetStudentByLoginId([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext, int loginId)
        {
            ILogger logger = executionContext.GetLogger("StudentController");


            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            StudentModel student = await StudentProcessor.GetStudentByLoginIdAsync(connectionString, loginId);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<StudentModel>(student);
            return response;
        }

        /**
         * 
         * 
Example query parameters:

?staffId=2
         * 
         */
        [Function("GetStudentsByStaffId")]
        public static async Task<HttpResponseData> GetStudentsByStaffId([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext, int staffId)
        {
            ILogger logger = executionContext.GetLogger("StudentController");

            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            IEnumerable<StudentModel> Students = await StudentProcessor.GetStudentsByStaffIdAsync(connectionString, staffId);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<StudentModel>>(Students);
            return response;
        }


    }
}
