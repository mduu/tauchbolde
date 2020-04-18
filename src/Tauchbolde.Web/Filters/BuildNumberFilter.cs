using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.Web.Filters
{
    public class BuildNumberFilter : IResultFilter
    {
        private readonly ILogger logger;
        private readonly IWebHostEnvironment env;

        public BuildNumberFilter(
            ILoggerFactory loggerFactory,
            IWebHostEnvironment env)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            logger = loggerFactory.CreateLogger<BuildNumberFilter>();
            this.env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ViewResult) {
                try
                {
                    var buildNumber = "0";
                    var buildNumberFilePath = Path.Combine(env.WebRootPath, "BuildNumber.txt");
                    if (File.Exists(buildNumberFilePath))
                    {
                        buildNumber = File.ReadAllText(buildNumberFilePath);
                        var viewResult = (ViewResult)context.Result;
                        viewResult.ViewData["buildNumber"] = buildNumber;
                        
                        logger.LogTrace($"Build-Number {buildNumber}");
                    }
                    else
                    {
                        logger.LogWarning($"BuildNumber file not found in [{buildNumberFilePath}]");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error while reading build-number from file: {ex.UnwindMessage()}");
                }
            }
        }
    
        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Can't add to headers here because response has already begun.
        }
    }
}
