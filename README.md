Entirely based on Confluent's dotnet quickstart guide: https://developer.confluent.io/get-started/dotnet/.

# Running locally

1. Clone the repo locally
1. Ensure the confluent CLI is installed (instructions very nicely detailed in https://developer.confluent.io/get-started/dotnet/#kafka-setup).
1. Start the local Kafka broker: `confluent local kafka start`
1. Create an `activities` topic: `confluent local kafka topic create activities`
1. Run the producer: `cd producer; dotnet run "$(pwd)/../getting-started.properties"`
1. Run as many consumers as you want: `cd ../consumer; dotnet run "$(pwd)/../getting-started.properties"`

# About

Imitates a _very_ simple fitness app, where there is a fitness tracker that publishes events (i.e. fitness exercises) onto the Kafka topic. Consumers can be set up by providing a name (e.g. web app, mobile), and the app name is the consumer group key for the topic. This means that, for example, if two "web app" consumers are set up, a single ice skating session will only be processed by one of them.