using EmployeePayrollRestApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayrollRestApi
{
    [TestClass]
    public class EmployeePayrollRestApiTest
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }

        /// <summary>
        /// UC1
        /// Ability to retrieve all employees data in json server.
        /// </summary>
        [TestMethod]
        public void OnCallingList_ReturnEmployeeList()
        {
            IRestResponse response = GetEmployeeList();
            /// Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<EmployeePayrollRestApi> dataResponse = JsonConvert.DeserializeObject<List<EmployeePayrollRestApi>>(response.Content);
            Assert.AreEqual(4, dataResponse.Count);
            foreach (EmployeePayrollRestApi employee in dataResponse)
            {
                Console.WriteLine("EmpId:" + employee.Id + "\nEmpName:" + employee.Name + "\nSalary:" + employee.Salary);
            }
        }

        /// <summary>
        /// Using Interface to get all list of employees.
        /// </summary>
        /// <returns></returns>
        public IRestResponse GetEmployeeList()
        {
            RestRequest request = new RestRequest("/employee", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// UC2
        /// Givens the employee on post should return added employees.
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnPost_ShouldReturnAddedEmployees()
        {
            /// Arrange
            RestRequest request = new RestRequest("/employee", Method.POST);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", "Vidhya");
            jObjectBody.Add("salary", "27000");
            /// Act
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            /// Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            EmployeePayrollRestApi dataResponse = JsonConvert.DeserializeObject<EmployeePayrollRestApi>(response.Content);
            Assert.AreEqual("Vidhya", dataResponse.Name);
            Assert.AreEqual("27000", dataResponse.Salary);
            Console.WriteLine(response.Content);
        }

        /// <summary>
        /// UC3
        /// Givens the multiple employee on post should return employees.
        /// </summary>
        [TestMethod]
        public void GivenMultipleEmployee_OnPost_ShouldReturnEmployees()
        {
            /// Arrange
            List<EmployeePayrollRestApi> employeePayrollListRestApi = new List<EmployeePayrollRestApi>();
            employeePayrollListRestApi.Add(new EmployeePayrollRestApi { Name = "Varsha", Salary = "29000" });
            employeePayrollListRestApi.Add(new EmployeePayrollRestApi { Name = "Sushma", Salary = "31000" });

            employeePayrollListRestApi.ForEach(employeeData =>
            {
                RestRequest request = new RestRequest("/employee", Method.POST);
                JObject jObjectBody = new JObject();
                jObjectBody.Add("Name", employeeData.Name);
                jObjectBody.Add("Salary", employeeData.Salary);
                /// Act
                request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                /// Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                EmployeePayrollRestApi dataResponse = JsonConvert.DeserializeObject<EmployeePayrollRestApi>(response.Content);
                Assert.AreEqual(employeeData.Name, dataResponse.Name);
                Assert.AreEqual(employeeData.Salary, dataResponse.Salary);
                Console.WriteLine(response.Content);
            });
            IRestResponse response = GetEmployeeList();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<EmployeePayrollRestApi> dataResponse = JsonConvert.DeserializeObject<List<EmployeePayrollRestApi>>(response.Content);
            Assert.AreEqual(6, dataResponse.Count);
        }

        /// <summary>
        /// UC4
        /// Givens the employee on update should return updated employee.
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnUpdate_ShouldReturnUpdatedEmployee()
        {
            /// Arrange
            RestRequest request = new RestRequest("/employee/1", Method.PUT);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", "Shashi");
            jObjectBody.Add("salary", "27800");
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);
            /// Act
            IRestResponse response = client.Execute(request);
            /// Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            EmployeePayrollRestApi dataResponse = JsonConvert.DeserializeObject<EmployeePayrollRestApi>(response.Content);
            Assert.AreEqual("Shashi", dataResponse.Name);
            Assert.AreEqual("27800", dataResponse.Salary);
            Console.WriteLine(response.Content);
        }
    }
}
