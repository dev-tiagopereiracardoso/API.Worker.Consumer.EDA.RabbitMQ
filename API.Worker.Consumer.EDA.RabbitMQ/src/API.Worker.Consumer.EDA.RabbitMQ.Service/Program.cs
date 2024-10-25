using API.Worker.Consumer.EDA.RabbitMQ.Domain.Implementations.Interfaces;
using API.Worker.Consumer.EDA.RabbitMQ.Domain.Implementations.Services;
using API.Worker.Consumer.EDA.RabbitMQ.Models.Events;
using API.Worker.Consumer.EDA.RabbitMQ.Service.Consumers;
using MassTransit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

var rabbitMQ_Queue = builder.Configuration["RabbitMQ_Queue"]!;
if (string.IsNullOrEmpty(rabbitMQ_Queue))
    throw new ArgumentException($"Error {assemblyName}. Parameter RabbitMQ_Queue not found.");

var rabbitMQ_Host = builder.Configuration["RabbitMQ_Host"]!;
if (string.IsNullOrEmpty(rabbitMQ_Host))
    throw new ArgumentException($"Error {assemblyName}. Parameter RabbitMQ_Host not found.");

var rabbitMQ_Port = builder.Configuration["RabbitMQ_Port"]! ?? "";

var rabbitMQ_VHost = builder.Configuration["RabbitMQ_VHost"]!;
if (string.IsNullOrEmpty(rabbitMQ_VHost))
    throw new ArgumentException($"Error {assemblyName}. Parameter rabbitMQ_VHost not found.");

var rabbitMQ_Username = builder.Configuration["RabbitMQ_Username"]!;
if (string.IsNullOrEmpty(rabbitMQ_Username))
    throw new ArgumentException($"Error {assemblyName}. Parameter RabbitMQ_Username not found.");

var rabbitMQ_Password = builder.Configuration["RabbitMQ_Password"]!;
if (string.IsNullOrEmpty(rabbitMQ_Password))
    throw new ArgumentException($"Error {assemblyName}. Parameter RabbitMQ_Password not found.");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ServiceConsumer>();
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(new Uri($"amqp://{rabbitMQ_Username}:{rabbitMQ_Password}@{rabbitMQ_Host + rabbitMQ_Port}"), rabbitMQ_VHost);

        cfg.Message<RegisterUserEvent>(message =>
        {
            message.SetEntityName(rabbitMQ_Queue);
        });

        cfg.ReceiveEndpoint(rabbitMQ_Queue, ep =>
        {
            ep.Bind(rabbitMQ_Queue, x =>
            {
                x.RoutingKey = typeof(RegisterUserEvent).Name;
            });

            ep.PrefetchCount = 10;
            ep.UseMessageRetry(r => r.Interval(5, 100));
            ep.ConfigureConsumer<ServiceConsumer>(ctx);
        });

        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddMassTransitHostedService(true);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IQueueService, QueueService>();

builder.Services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());

var app = builder.Build();

app.UseCors("AllowAnyOrigin");

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
