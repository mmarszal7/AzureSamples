using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

namespace EventHubsDemo
{
    public static class IotDeviceSimulator
    {
        private static readonly Random rand = new Random();
        private static ILogger Logger;

        [FunctionName("IotDeviceSimulator")]
        public static async Task RunAsync([TimerTrigger("*/15 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            Logger = log;
            var messages = new List<TelemetryMessage>()
            {
                new TelemetryMessage(){ SensorName = "Temperature", Value = rand.Next()},
                new TelemetryMessage(){ SensorName = "Humidity", Value = rand.Next()},
            };

            messages.ForEach(async m =>
            {
                var json = JsonConvert.SerializeObject(m);
                var bytes = Encoding.UTF8.GetBytes(json);

                //await SendMessageToEventHub(bytes);
                await SendMessageToIotHub(bytes);
            });
        }

        private static async Task SendMessageToEventHub(byte[] bytes)
        {
            var hubName = "demo-tsi-hub";
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(Environment.GetEnvironmentVariable("EventHubConnectionString"))
            {
                EntityPath = hubName
            };
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            await eventHubClient.SendAsync(new EventData(bytes));
            Logger.LogWarning("Sent message to EventHub");
        }

        private static async Task SendMessageToIotHub(byte[] bytes)
        {
            var iotHubClient = DeviceClient.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHubConnectionString"), TransportType.Mqtt);
            await iotHubClient.SetMethodHandlerAsync("SendMessage", SendMessage, null);
            await iotHubClient.SendEventAsync(new Message(bytes));
            Logger.LogWarning("Sent message to IotHub");
        }

        private static Task<MethodResponse> SendMessage(MethodRequest methodRequest, object userContext) =>
            Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes("Message: " + Encoding.UTF8.GetString(methodRequest.Data)), 200));
    }
}
