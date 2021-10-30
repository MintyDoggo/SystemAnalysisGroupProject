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
        [Function("CreateStudentAndReturnId")]
        public static async Task<HttpResponseData> CreateStudentAndReturnId([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("StudentController");

            // Get the body of the request and deserialize it to json
            string requestBody = await req.ReadAsStringAsync();
            JsonElement jsonBody = JsonSerializer.Deserialize<JsonElement>(requestBody);

            // Get the StudentModel from the request body
            StudentModel student = new StudentModel();
            try
            {
                student = JsonSerializer.Deserialize<StudentModel>(requestBody);
                //student.StudentIdNumber = jsonBody.GetProperty("StudentIdNumber").GetInt32();
                //student.FirstName = jsonBody.GetProperty("FirstName").GetString();
                //student.LastName = jsonBody.GetProperty("LastName").GetString();
                //student.Birthday = jsonBody.GetProperty("Birthday").GetDateTime();
                //student.Address = jsonBody.GetProperty("Address").GetString();
                //student.Major = jsonBody.GetProperty("Major").GetString();
                //student.FirstYearEnrolled = jsonBody.GetProperty("FirstYearEnrolled").GetInt32();
                //student.HighSchoolAttended = jsonBody.GetProperty("HighSchoolAttended").GetString();
                //student.UndergraduateSchoolAttended = jsonBody.GetProperty("UndergraduateSchoolAttended").GetString();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteStringAsync("Request didn't meet syntax requirements (make sure you include everything and have the correct property types)");
                return response;
            }

            // Call on the data processor and return the Id
            try
            {
                // Hard-coded development connection string for now
                const string connectionString = "Data Source=(localdb)/MSSQLLocalDB;Initial Catalog=MedicalUniversityStudentManagementDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                int id = await StudentProcessor.CreateStudentAndReturnIdAsync(connectionString, student);
                
                // Successfully added the Student to the database
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.WriteString(id.ToString());
                return response;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                var response = req.CreateResponse(HttpStatusCode.Conflict);
                response.WriteString("Conflict when inserting into the database");
                return response;
            }


        }

        [Function("DeleteStudentById")]
        public static HttpResponseData DeleteStudentById([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req,
    FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("StudentController");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }

        [Function("GetStudents")]
        public static HttpResponseData GetStudents([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("StudentController");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }

    }
}
