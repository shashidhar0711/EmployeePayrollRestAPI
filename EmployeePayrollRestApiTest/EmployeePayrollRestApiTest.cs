using EmployeePayrollRestApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
    }
}
