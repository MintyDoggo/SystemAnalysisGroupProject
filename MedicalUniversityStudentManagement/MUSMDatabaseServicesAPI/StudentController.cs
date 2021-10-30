using System.Collections.Generic;
using System.Net;
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
        public static HttpResponseData CreateStudentAndReturnId([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("StudentController");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
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
        public static async Task<HttpResponseData> GetStudents([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            IEnumerable<StudentModel> Students = await StudentProcessor.GetStudentsAsync("CONECTION STRING HERE");

            await response.WriteAsJsonAsync<IEnumerable<StudentModel>>(Students);


            return response;
        }

    }
}
