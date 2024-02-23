using Confluent.Kafka;
using System;
using Microsoft.Extensions.Configuration;

class Producer {
    static void Main(string[] args)
    {
        if (args.Length != 1) {
            Console.WriteLine("Please provide the configuration file path as a command line argument");
        }

        IConfiguration configuration = new ConfigurationBuilder()
            .AddIniFile(args[0])
            .Build();

        const string topic = "activities";

        using (var producer = new ProducerBuilder<string, string>(
            configuration.AsEnumerable()).Build())
        {
            Console.WriteLine("Enter the activity name, or 'stop' to stop");
            while (true) {
                var activityName = Console.ReadLine();
                if (activityName == "stop") {
                    Console.WriteLine("Stopping");
                    break;
                }

                var key = Guid.NewGuid().ToString();

                producer.Produce(topic, new Message<string, string> { Key = key, Value = activityName },
                    (deliveryReport) =>
                    {
                        if (deliveryReport.Error.Code != ErrorCode.NoError) {
                            Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                        }
                        else {
                            Console.WriteLine($"Produced event to topic {topic}: key = {key}, value = {activityName}");
                        }
                    });

                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}