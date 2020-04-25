using IotGatewayServer.Impl;
using IotGatewayServer.Jobs;
using IotGatewayServer.Services;
using JT808.Protocol;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Union.Gateway;
using Union.Gateway.Abstractions;
using Union.Gateway.MsgLogging;
using Union.Gateway.SessionNotice;
using Union.Gateway.Traffic;
using Union.Gateway.Transmit;

namespace IotGatewayServer
{
    public class Startup
    {
        private IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddSingleton<IDeviceService, DefaultDeviceService>();
            //使用内存队列实现会话通知
            services.AddSingleton<UnionSessionService>();
            services.AddSingleton<IUnionSessionProducer, UnionSessionProducer>();
            services.AddSingleton<IUnionSessionConsumer, UnionSessionConsumer>();
            services.AddJT808Configure()
                    .AddNormalGateway(Configuration)
                    .ReplaceNormalReplyMessageHandler<UnionNormalReplyMessageHandlerImpl>()
                    .AddMsgLogging<UnionMsgLogging>()
                    .AddTraffic()
                    .AddSessionNotice()
                    .AddTransmit(Configuration)
                    .AddTcp()
                    .AddUdp()
                    .AddGrpc()
                    .Builder();
            //流量统计
            services.AddHostedService<TrafficJob>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
