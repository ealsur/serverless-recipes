using System.Net;
using System.Net.Http.Headers;
using System.IO;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using System.Linq;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using Microsoft.Azure.WebJobs.Host;
using MimeTypes;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using Microsoft.AspNetCore.Http;

namespace cosmosdbtrigger
{
    public static class cosmosdbtrigger
    {
        const string staticFilesFolder = "wwwroot";
        static string defaultPage =
            string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEFAULT_PAGE")) ?
            "index.html" : Environment.GetEnvironmentVariable("DEFAULT_PAGE");

        /// <summary>
        /// This HttpTriggered function acts as static file server. 
        /// Based on https://github.com/anthonychu/azure-functions-static-file-server
        /// </summary>
        [FunctionName("FileServer")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, TraceWriter log, ExecutionContext context)
        {
            var filePath = GetFilePath(req, log, context);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(filePath, FileMode.Open);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(GetMimeType(filePath));
            return response;
        }

        private static string GetFilePath(HttpRequest req, TraceWriter log, ExecutionContext context)
        {
            var pathValue = req.Query["file"];

            var path = pathValue.FirstOrDefault() ?? "";

            var staticFilesPath = Path.GetFullPath(Path.Combine(context.FunctionAppDirectory, staticFilesFolder));
            var fullPath = Path.GetFullPath(Path.Combine(staticFilesPath, path));

            if (!IsInDirectory(staticFilesPath, fullPath))
            {
                throw new ArgumentException("Invalid path");
            }

            var isDirectory = Directory.Exists(fullPath);
            if (isDirectory)
            {
                fullPath = Path.Combine(fullPath, defaultPage);
            }

            return fullPath;
        }

        private static bool IsInDirectory(string parentPath, string childPath)
        {
            var parent = new DirectoryInfo(parentPath);
            var child = new DirectoryInfo(childPath);

            var dir = child;
            do
            {
                if (dir.FullName == parent.FullName)
                {
                    return true;
                }
                dir = dir.Parent;
            } while (dir != null);

            return false;
        }

        private static string GetMimeType(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return MimeTypeMap.GetMimeType(fileInfo.Extension);
        }
    }
}