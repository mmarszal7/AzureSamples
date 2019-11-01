using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;

namespace ReadDeviceMessages
{
    class Program
    {
        // az iot hub show --query properties.eventHubEndpoints.events.endpoint --name {IoTHubName}
        private static readonly string eventHubsCompatibleEndpoint = "";

        // az iot hub show --query properties.eventHubEndpoints.events.path --name {IoTHubName}
        private static readonly string eventHubsCompatiblePath = "";

        // az iot hub policy show --name service --query primaryKey --hub-name {IoTHubName}
        private static readonly string iotHubSasKey = "";
        private static readonly string iotHubSasKeyName = "service";
        private static EventHubClient eventHubClient;

        private static async Task Main(string[] args)
        {
            var connectionString = new EventHubsConnectionStringBuilder(new Uri(eventHubsCompatibleEndpoint), eventHubsCompatiblePath, iotHubSasKeyName, iotHubSasKey);
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString.ToString());
            var runtimeInfo = await eventHubClient.GetRuntimeInformationAsync();

            var tasks = new List<Task>();
            foreach (string partition in runtimeInfo.PartitionIds)
            {
                tasks.Add(ReceiveMessagesFromDeviceAsync(partition, (new CancellationTokenSource()).Token));
            }

            Task.WaitAll(tasks.ToArray());
        }

        private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            var eventHubReceiver = eventHubClient.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.Now));
            while (true)
            {
                var events = await eventHubReceiver.ReceiveAsync(100);
                if (events == null) continue;

                foreach (EventData eventData in events)
                {
                    string data = Encoding.UTF8.GetString(eventData.Body.Array);
                    Console.WriteLine($"Message received: {data}");

                    //Console.WriteLine("Application properties (set by device):");
                    //foreach (var prop in eventData.Properties)
                    //{
                    //  Console.WriteLine("  {0}: {1}", prop.Key, prop.Value);
                    //}

                    //Console.WriteLine("System properties (set by IoT Hub):");
                    //foreach (var prop in eventData.SystemProperties)
                    //{
                    //  Console.WriteLine("  {0}: {1}", prop.Key, prop.Value);
                    //}
                }
            }
        }
    }
}
