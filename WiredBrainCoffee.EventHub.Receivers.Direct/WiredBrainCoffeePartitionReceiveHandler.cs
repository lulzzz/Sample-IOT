﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using WiredBrainCoffee.EventHub.Model;

namespace WiredBrainCoffee.EventHub.Receivers.Direct
{
    public class WiredBrainCoffeePartitionReceiveHandler : IPartitionReceiveHandler
    {
        public WiredBrainCoffeePartitionReceiveHandler(string partitionId)
        {
            PartitionId = partitionId;
        }

        public string PartitionId { get; }
        public int MaxBatchSize { get; set; }

        public Task ProcessEventsAsync(IEnumerable<EventData> events)
        {
            if (events != null)
            { 
                foreach (var eventData in events)
                {
                    var dataAsJson = Encoding.UTF8.GetString(eventData.Body.Array);
                    Console.WriteLine(dataAsJson);
                    var coffeeMachineData = JsonConvert.DeserializeObject<CoffeeMachineData>(dataAsJson);
                    Console.WriteLine($"{coffeeMachineData} | PartionId: {PartitionId} | Offset: { eventData.SystemProperties.Offset}");
                }
            }

            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(Exception error)
        {
            Console.WriteLine($"Exception: {error.Message}");
            return Task.CompletedTask;
        }
    }
}