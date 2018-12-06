﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppThirdAuth.Middlewares
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITestProvider _testProvider;

        public TestMiddleware(RequestDelegate next, ITestProvider testProvicer)
        {
            _next = next;
            _testProvider = testProvicer;
        }

        // IMyScopedService is injected into Invoke
        public async Task Invoke(HttpContext httpContext)
        {
            System.Diagnostics.Debug.WriteLine($"TestMiddleware call. {_testProvider.GetName()}");
            await _next(httpContext);
        }
    }

    public static class TestMiddlewareExtensions
    {
        public static IApplicationBuilder UseTestMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TestMiddleware>();
        }
    }

    public interface ITestProvider
    {
        string GetName();
    }

    public class TestProvider : ITestProvider
    {
        public TestProvider() { }

        public string GetName()
        {
            return nameof(TestProvider);
        }
    }

    public static class TestServicesExtensions
    {
        public static IServiceCollection AddTest(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<ITestProvider, TestProvider>();
            return services;
        }
    }
}
