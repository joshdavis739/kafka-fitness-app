using Confluent.Kafka;
using System;
using System.Threading;
using Microsoft.Extensions.Configuration;

class Consumer {

    static void Main(string[] args)
    {
        if (args.Length != 1) {
            Console.WriteLine("Please provide the configuration file path as a command line argument");
        }

        Console.WriteLine("Enter the name of the application");
        var appName = Console.ReadLine();

        IConfiguration configuration = new ConfigurationBuilder()
            .AddIniFile(args[0])
            .Build();

        configuration["group.id"] = $"kafka--{appName}";
        configuration["auto.offset.reset"] = "earliest";

        const string topic = "activities";

        CancellationTokenSource cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => {
            e.Cancel = true; // prevent the process from terminating.
            cts.Cancel();
        };

        using (var consumer = new ConsumerBuilder<string, string>(
            configuration.AsEnumerable()).Build())
        {
            consumer.Subscribe(topic);
            try {
                while (true) {
                    var cr = consumer.Consume(cts.Token);
                    Console.WriteLine($"Activity {cr.Message.Value} posted to {appName}!");
                }
            }
            catch (OperationCanceledException) {
                // Ctrl-C was pressed.
            }
            finally{
                consumer.Close();
            }
        }
    }
}