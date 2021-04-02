﻿using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //first iteration
            //var input = new HelloRequest {Name = "Kevin"};
            ////move to appSettings.json eventually
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greeter.GreeterClient(channel);

            //var reply = await client.SayHelloAsync(input);
            //Console.WriteLine(reply.Message);
            //Console.ReadLine();
            //second iteration
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var customerClient = new Customer.CustomerClient(channel);

            var clientRequested = new CustomerLookupModel {UserId = 2};

            var customer = await customerClient.GetCustomerInfoAsync(clientRequested);

            Console.WriteLine($"{customer.FirstName} {customer.LastName}");

            Console.WriteLine();
            Console.WriteLine("New Customer List");
            Console.WriteLine();


            using (var call = customerClient.GetNewCustomers(new NewCustomerRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine(
                        $"{currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.EmailAddress}");
                }
            }

            Console.ReadLine();
        }
    }
}
