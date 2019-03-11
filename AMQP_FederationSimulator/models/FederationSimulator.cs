using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQP_FederationSimulator.models
{
    public class FederationSimulator
    {
        public void RouteMessages()
        {
            var connectionfactory = new MyConnectionFactory();

            //create a connection
            using (var connection = connectionfactory.CreateConnection())
            {
                //create a channel
                using (var channel = connection.CreateModel())
                {
                    //Declare/Create a queue
                    channel.QueueDeclare(queue: "S.Giorgio",
                                            durable: true,
                                            autoDelete: false,
                                            exclusive: false,
                                            arguments: null);

                    channel.QueueDeclare(queue: "scadenziario_operational",
                                            durable: true,
                                            autoDelete: false,
                                            exclusive: false,
                                            arguments: null);

                    Subscription subscription = new Subscription(model: channel, queueName: "S.Giorgio", autoAck: false);

                    while (true)
                    {
                        BasicDeliverEventArgs deliveryArguments = subscription.Next();      //queue consumption
                        StringBuilder messageBuilder = new StringBuilder();
                        string message = Encoding.UTF8.GetString(deliveryArguments.Body);
                        messageBuilder.Append("Message from queue: ").Append(message).Append(". ");
                        foreach (string headerKey in deliveryArguments.BasicProperties.Headers.Keys)
                        {
                            byte[] value = deliveryArguments.BasicProperties.Headers[headerKey] as byte[];
                            messageBuilder.Append("Header key: ").Append(headerKey).Append(", value: ").Append(Encoding.UTF8.GetString(value)).Append("; ");
                        }
                        Console.WriteLine(messageBuilder.ToString()+"...arrived");

                        //route message
                        string exchangePlant = "Plant";
                        channel.ExchangeDeclare(exchange: exchangePlant, type: "headers", durable: true, autoDelete: false, arguments: null);

                        Dictionary<string, object> bindingArgs = new Dictionary<string, object>();
                        bindingArgs.Add("x-match", "all");
                        bindingArgs.Add("object", "deadline");
                        channel.QueueBind(queue: "scadenziario_operational", exchange: "Plant", routingKey: "", arguments: bindingArgs);

                        channel.BasicPublish(exchange: "Plant",
                                                routingKey: "",
                                                basicProperties: deliveryArguments.BasicProperties,
                                                body: deliveryArguments.Body);
                        Console.WriteLine("[x] message '{0}' routed to {1} exchange \n", message, exchangePlant);

                        subscription.Ack(deliveryArguments);
                    }
                }
            }
        }
    }
}
