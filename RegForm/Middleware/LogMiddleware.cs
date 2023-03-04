using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Microsoft.VisualStudio.Web.CodeGeneration;
using RegForm.Interfaces;
using RegForm.Models;
using Serilog;
using static System.Net.Mime.MediaTypeNames;

namespace RegForm.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;
        private readonly IDBInsert _dBInsert;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private string ApiUrl;
        private Int64 ApiLogId;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger, IDBInsert dBInsert)
        {
            _next = next;
            _logger = logger;
            _dBInsert = dBInsert;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            ApiUrl = string.Empty;
            ApiLogId = 0;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await LogRequest(context);
                await LogResponse(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorResponse
            {
                Success = false
            };
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            errorResponse.Message = "Internal server error!";

            var res = _dBInsert.UpdateResponseLog(ApiLogId, response.StatusCode, exception.Message);
            var errRes = _dBInsert.InsertErrorLog(ApiLogId, ApiUrl, exception.Message, exception.StackTrace);
            _logger.LogInformation(exception.Message);

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            var Body = ReadStreamInChunks(requestStream);
            _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                                   $"Method: {context.Request.Method}" +
                                   $"Schema: {context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Request Body: {Body}");
            ApiUrl = context.Request.Scheme + "://" + context.Request.Host + context.Request.Path;
            ApiLogId = _dBInsert.InsertRequestLog(context.Request.Method, ApiUrl, context.Request.QueryString.ToString(), Body);
            context.Request.Body.Position = 0;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"Status Code:{context.Response.StatusCode} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Response Body: {text}");
            var res = _dBInsert.UpdateResponseLog(ApiLogId, context.Response.StatusCode, text);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}

