using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMQP_FederationSimulator
{
    public class MyConnectionFactory : ConnectionFactory
    {
        private const string _UserName = "guest";
        private const string _Password = "guest";
        private const string _HostName = "localhost";

        public MyConnectionFactory()
            : base()
        {
            base.UserName = _UserName;
            base.Password = _Password;
            base.HostName = _HostName;
        }
    }
}
