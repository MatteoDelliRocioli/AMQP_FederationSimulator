using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMQP_FederationSimulator.models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AMQP_FederationSimulator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Please wait for the first message to arrive");

            FederationSimulator messageReceiver = new FederationSimulator();
            messageReceiver.RouteMessages();

            Console.ReadLine();
        }
    }
}
