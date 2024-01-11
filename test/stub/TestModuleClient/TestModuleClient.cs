using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using TICO.GAUDI.Commons;

namespace ApplicationController
{
    public class TestModuleClient : IModuleClient
    {
        public IotMessage _message = null;
        public Dictionary<string,IotMessage> _allMessage = new Dictionary<string, IotMessage>();
        
        public async Task CloseAsync()
        {
            await Task.CompletedTask;
        }

        public async Task<Twin> GetTwinAsync()
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task OpenAsync()
        {
            await Task.CompletedTask;
        }

        public async Task SendEventAsync(string outputName, IotMessage message)
        {
            await SendEventAsync(outputName, message, TransportTopic.Iothub);
        }

        public async Task SendEventAsync(string outputName, IotMessage message, TransportTopic transportTopic)
        {
            if( string.IsNullOrEmpty(outputName) ){
                throw new Exception("outputName is null or Empty.");
            }
            _message = message;
            _allMessage[outputName] = message;

            await Task.CompletedTask;
        }

        public async Task SetDesiredPropertyUpdateCallbackAsync(DesiredPropertyUpdateCallback callback, object userContext)
        {
            await Task.CompletedTask;
        }

        public async Task SetDesiredPropertyUpdateCallbackAsync(DesiredPropertyUpdateCallback callback, object userContext, TransportTopic transportTopic)
        {
            await Task.CompletedTask;
        }

        public async Task SetInputMessageHandlerAsync(string inputName, IotMessageHandler handler, object userContext)
        {
            await Task.CompletedTask;
        }

        public async Task SetInputMessageHandlerAsync(string inputName, IotMessageHandler handler, object userContext, TransportTopic transportTopic)
        {
            await Task.CompletedTask;
        }

        public async Task SetMethodHandlerAsync(string methodName, MethodCallback methodHandler, object userContext)
        {
            await Task.CompletedTask;
        }

        public async Task SetMethodHandlerAsync(string methodName, MethodCallback methodHandler, object userContext, TransportTopic transportTopic)
        {
            await Task.CompletedTask;
        }

        public async Task UpdateReportedPropertiesAsync(TwinCollection reportedProperties)
        {
            await Task.CompletedTask;
        }

        public async Task UpdateReportedPropertiesAsync(TwinCollection reportedProperties, TransportTopic transportTopic)
        {
            await Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }

}