using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

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
            var message = new
            {
                device = "Device" + DateTime.Now.Minute,
                timestamp = DateTime.Now.ToString(),
                temperature = rand.Next(),
                humidity = rand.Next()
            };
            var json = JsonConvert.SerializeObject(message);
            var bytes = Encoding.UTF8.GetBytes(json);

            await SendMessageToEventHub(bytes);
        }

        private static async Task SendMessageToEventHub(byte[] bytes)
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(Environment.GetEnvironmentVariable("EventHubConnectionString"))
            {
                EntityPath = "demo-tsi-hub"
            };
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            await eventHubClient.SendAsync(new EventData(bytes));
            Logger.LogWarning("Sent message to EventHub");
        }
    }
}
