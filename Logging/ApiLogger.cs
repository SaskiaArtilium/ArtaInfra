﻿using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Arta.Infrastructure.Logging
{
    public class ApiLogger : IApiLogger
    {
        private readonly ILogger<ApiLogger> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiLogger(ILogger<ApiLogger> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public void LogInformation(string message, LoggingType loggingType)
        {
            var traceIdentifier = _httpContextAccessor.HttpContext.TraceIdentifier;
            var fileLogging = $"[{loggingType}] - {traceIdentifier} - {message}";

            _logger.LogInformation(fileLogging);
        }

        public void LogError(string message, LoggingType loggingType)
        {
            var traceIdentifier = _httpContextAccessor.HttpContext.TraceIdentifier;
            var fileLogging = $"[{loggingType}] - {traceIdentifier} - {message}";

            _logger.LogError(fileLogging);
        }
    }
}
