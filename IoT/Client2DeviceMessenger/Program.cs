using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace Client2DeviceMessenger
{
    class Program
    {
        private static ServiceClient serviceClient;

        // az iot hub show-connection-string --hub-name {IoTHubName} --policy-name service
        private static readonly string connectionString = "";

        private static string deviceName = "";

        private static async Task InvokeMethod(string interval)
        {
            var methodInvocation = new CloudToDeviceMethod("SendMessage") { ResponseTimeout = TimeSpan.FromSeconds(30) };
            methodInvocation.SetPayloadJson(interval);

            var response = await serviceClient.InvokeDeviceMethodAsync(deviceName, methodInvocation);

            Console.WriteLine("Response status: {0}, payload:", response.Status);
            Console.WriteLine(response.GetPayloadAsJson());
        }

        private static void Main(string[] args)
        {
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            while (true)
            {
                var interval = Console.ReadLine();
                InvokeMethod(interval).GetAwaiter().GetResult();
            }
        }
    }
}
