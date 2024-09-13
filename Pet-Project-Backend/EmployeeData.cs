using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;
using Pet_Project_Backend.Utilities;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Pet_Project_Backend.Calculations;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Azure;
using Microsoft.Azure.Cosmos.Linq;

namespace Pet_Project_Backend
{
    public class EmployeeData
    {
        public static readonly string cosmosConnectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");
        public static readonly string databaseName = "Employee_Mgmt";
        public static readonly string containerName = "Employee";

        // PUT
        [FunctionName("UpdateEmployee")]
        public static async Task<Responses> UpdateEmployee([HttpTrigger(AuthorizationLevel.Function, HttpTriggers.HttpTriggerUpdate, Route = Routes.UpdateEmployeeById)] HttpRequest req, ILogger log, string id)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Employee data = JsonConvert.DeserializeObject<Employee>(requestBody);
                var validator = new Validations();
                List<string> validationErrors = validator.validation(data);
                if (validationErrors.Count > 0)
                {
                    return new EntityData<dynamic> { Data = validationErrors };
                }
                var age = new CalculateAge();
                data.Age = age.AgeCalculation(data.DOB);
                using (var cosmosClient = new CosmosClient(cosmosConnectionString))
                {
                    var container = cosmosClient.GetContainer(databaseName, containerName);
                    ItemResponse<Employee> response = await container.ReadItemAsync<Employee>(id, new PartitionKey(id));

                    Employee existingItem = response.Resource;

                    existingItem.Name = data?.Name;
                    existingItem.DOB = data?.DOB;
                    existingItem.Phone = data?.Phone;
                    existingItem.Email = data?.Email;
                    existingItem.Age = data.Age;

                    var result = await container.ReplaceItemAsync(existingItem, id, new PartitionKey(id));
                    return new EntityData<dynamic> { Message = "Item updated Successfully.", Success = true, Data = result.Resource };
                }
            }
            catch (Exception ex)
            {
                return new Responses { Message = ex.Message, Success = false };
            }
        }

        // POST
        [FunctionName("CreateEmployee")]
        public static async Task<Responses> CreateEmployee([HttpTrigger(AuthorizationLevel.Function, HttpTriggers.HttpTriggerCreate, Route = Routes.CreateEmployee)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Employee data = JsonConvert.DeserializeObject<Employee>(requestBody);

                var validator = new Validations();
                List<string> validationErrors = validator.validation(data);
                if (validationErrors.Count > 0)
                {
                    return new EntityData<dynamic> { Success = false, Data = validationErrors };
                }

                var age = new CalculateAge();
                data.Age = age.AgeCalculation(data.DOB);

                using (var cosmosClient = new CosmosClient(cosmosConnectionString))
                {
                    var container = cosmosClient.GetContainer(databaseName, containerName);
                    var queryIterator = container.GetItemLinqQueryable<Employee>().Where(x => x.Id == data.Id).ToFeedIterator();
                    while (queryIterator.HasMoreResults)
                    {
                        var result = await queryIterator.ReadNextAsync();
                        if (result.Resource.Count() > 0)
                        {
                            return new Responses { Message = "ID already exists.", Success = false };
                        }
                    }
                    ItemResponse<Employee> response = await container.CreateItemAsync(data);
                    return new EntityData<Employee> { Message = "Employee is created successfully", Success = true, Data = response.Resource };
                }
            }
            catch (Exception ex)
            {
                return new Responses { Message = ex.Message, Success = false };
            }
        }

        // DELETE BY ID
        [FunctionName("DeleteEmployeeById")]
        public static async Task<Responses> DeleteEmployeeById([HttpTrigger(AuthorizationLevel.Function, HttpTriggers.HttpTriggerDelete, Route = Routes.DeleteEmployeeById)] HttpRequest req, ILogger log, string id)
        {
            try
            {
                using (var cosmosClient = new CosmosClient(cosmosConnectionString))
                {
                    var container = cosmosClient.GetContainer(databaseName, containerName);
                    ItemResponse<Employee> existingEmployee = await container.ReadItemAsync<Employee>(id, new PartitionKey(id));
                    var response = await container.DeleteItemAsync<Employee>(id, new PartitionKey(id));
                }
                return new Responses() { Message = $"Employee with ID : {id} is successfully deleted.", Success = true };
            }
            catch (Exception)
            {
                return new Responses() { Message = "ID not found. To Delete the Employee you need to mention existing ID.", Success = false };
            }
        }

        // GET
        [FunctionName("GetAllEmployees")]
        public static async Task<Responses> GetAllEmployees([HttpTrigger(AuthorizationLevel.Function, HttpTriggers.HttpTriggetGet, Route = Routes.GetAllEmployees)] HttpRequest req, ILogger log)
        {
            try
            {
                using (var cosmosClient = new CosmosClient(cosmosConnectionString))
                {
                    var container = cosmosClient.GetContainer(databaseName, containerName);

                    var queryIterator = container.GetItemQueryIterator<Employee>();
                    List<Employee> employees = new List<Employee>();
                    while (queryIterator.HasMoreResults)
                    {
                        var response = await queryIterator.ReadNextAsync();
                        employees.AddRange(response.ToList());
                    }
                    return new EntityData<List<Employee>>() { Message = "Retrieved data of all the Employees", Success = true, Data = employees };
                }
            }
            catch (Exception ex)
            {
                return new Responses() { Message = ex.Message, Success = false };
            }
        }

        // GET BY ID
        [FunctionName("GetEmployeeById")]
        public static async Task<Responses> GetEmployeeById([HttpTrigger(AuthorizationLevel.Function, HttpTriggers.HttpTriggetGet, Route = Routes.GetEmployeeById)] HttpRequest req, ILogger log, string id)
        {
            var cosmosClient = new CosmosClient(cosmosConnectionString);
            var container = cosmosClient.GetContainer(databaseName, containerName);

            try
            {
                ItemResponse<Employee> employee = await container.ReadItemAsync<Employee>(id, new PartitionKey(id));
                log.LogInformation($"ID Found. Proceeds with the retrival of the Employee data with ID : {id} .");

                return new EntityData<Employee>() { Message = $"Retrieved the data of the ID : {id}.", Success = true, Data = employee.Resource };
            }
            catch (Exception)
            {
                return new Responses() { Message = "ID not Found. Enter the ID which is already present in the database to retrive the Employee data.", Success = false };
            }
        }
    }
}