# Azure Samples

Collection of .NET projects caintaining code examples for selected Azure services (Functions, Event Hubs, Stream Analytics, IoT, AI etc.)

[Microsoft's GitHub repo with Azure-Samples](https://github.com/Azure-Samples/azure-iot-samples-csharp)

# Done:

**1. EventHubsDemo - device simulator for IoT Hub and Event Hub**:
- Time triggered (CRON) **Azure Function**
- Usage of .NET APIs for **Iot Hub** and **Event Hub**

**2. StreamAnalyticsDemo**:
  - [**Stream Analytics** quick start](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-quick-create-vs)
  - Writing historical data to **Blob** and **Table Storages**
  - Using **Key Vault** to manage secrets
  - **Logic App** to send email every time new blob is created

**3. Azure Active Directory Authentication**:
-  Set up secure API with Azure AD
-  Communication with Secured API (get token & set auth header)
-  Swagger Intergration for secured API
 
# Plans:

**IoT**:
  - TSI - How to structure data inside TSI
  - [Connect real device to IotHub](https://www.telerik.com/blogs/diy-iot-for-azure) (NodeMCU with DHT11 sensor)

**ARM**:
  - IaC - Infrastracture as a Code - version control for ARM templates 

**Mobile**:
  - Send [location data](https://developer.xamarin.com/samples/mobile/BackgroundLocationDemo/) from [Android Service](https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/services/) to Event Hub

**Data**:
  - Play with Microsoft AdventureWorks DB 
  - Data Factory based on AdventureWorks DB 

**AI**:
  - CustomVision.ai
  - LUIS.ai