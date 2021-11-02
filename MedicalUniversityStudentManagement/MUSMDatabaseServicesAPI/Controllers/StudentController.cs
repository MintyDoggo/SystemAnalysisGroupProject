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
    public static class StudentController
    {
        /**
         * 
         * 
Example request body:

{
    "Student": {
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
}

         * 
         */
        [Function("CreateStudentAndReturnId")]
        public static async Task<HttpResponseData> CreateStudentAndReturnId([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StudentController");

            // Get the body of the request
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the StudentModel from the request body
            StudentModel student;
            try
            {
                student = JsonSerializer.Deserialize<StudentModel>(jsonBody.GetProperty("Student").GetRawText());
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
    "Student": {
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
}

         * 
         */
        [Function("UpdateStudentById")]
        public static async Task<HttpResponseData> UpdateStudentById([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StudentController");

            // Get the body of the request
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id and StudentModel from the request body
            int id;
            StudentModel student;
            try
            {
                id = jsonBody.GetProperty("Id").GetInt32();
                student = JsonSerializer.Deserialize<StudentModel>(jsonBody.GetProperty("Student").GetRawText());
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

        [Function("DeleteStudentById")]
        public static async Task<HttpResponseData> DeleteStudentById([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StudentController");

            // Get the body of the request and deserialize it to json
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id of the Student to delete from the request body
            int id = jsonBody.GetInt32();

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


        [Function("GetStudents")]
        public static async Task<HttpResponseData> GetStudents([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            IEnumerable<StudentModel> Students = await StudentProcessor.GetStudentsAsync(connectionString);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<StudentModel>>(Students);
            return response;
        }

        [Function("GetStudentsByStaffId")]
        public static async Task<HttpResponseData> GetStudentsByStaffId([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StudentController");

            // Get the body of the request and deserialize it to json
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the Id of the Staff from the request body
            int staffId = jsonBody.GetInt32();


            string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
            IEnumerable<StudentModel> Students = await StudentProcessor.GetStudentsByStaffIdAsync(connectionString, staffId);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<StudentModel>>(Students);
            return response;
        }


    }
}
