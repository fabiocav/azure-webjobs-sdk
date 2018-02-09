// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SampleHost.Filters;
using SampleHost.Models;

namespace SampleHost
{
    [ErrorHandler]
    public static class Functions
    {
        public static void BlobTrigger(
            [BlobTrigger("test")] string blob, ILogger logger)
        {
            logger.LogInformation("Processed blob: " + blob);
        }

        public static void BlobPoisonBlobHandler(
            [QueueTrigger("webjobs-blobtrigger-poison")] JObject blobInfo, ILogger logger)
        {
            string container = (string)blobInfo["ContainerName"];
            string blobName = (string)blobInfo["BlobName"];

            logger.LogInformation($"Poison blob: {container}/{blobName}");
        }

        [WorkItemValidator]
        public static void ProcessWorkItem(
            [QueueTrigger("test")] WorkItem workItem, ILogger logger)
        {
            logger.LogInformation($"Processed work item {workItem.ID}");
        }
    }
}
