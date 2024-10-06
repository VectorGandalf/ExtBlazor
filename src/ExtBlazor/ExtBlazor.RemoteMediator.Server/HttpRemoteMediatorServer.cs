using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ExtBlazor.RemoteMediator.Server;
public class HttpRemoteMediatorServer
{
    public static async Task<TransportResponse> Handle([FromBody] TransportRequest transportRequest,
        HttpRemoteMediatorServerConfig config,
        IServiceProvider serviceProvider)
    {
        await Task.CompletedTask;
        _ = config.MediatorCallback ?? throw new NoMediatorCallbackException();

        var request = transportRequest.Request.ToObject();

        object? error = null;
        object? result = null;
        try
        {
            result = InvokeMediator(request, config.MediatorCallback, serviceProvider);
        }
        catch (Exception exception)
        {
            error = config.ExceptionHandler(exception);
        }
        if (result is not null)
        {
            return new(new(result), null);
        }
        else if (error is not null)
        {
            return new(null, new(error));
        }
        throw new Exception("No error object!");
    }

    private static object? InvokeMediator(object request, Delegate mediatorCallback, IServiceProvider serviceProvider)
    {
        var arguments = ResolveArguments(request, mediatorCallback, serviceProvider);
        if (mediatorCallback.Method.ReturnType.IsSubclassOf(typeof(Task)) && mediatorCallback.Method.ReturnType.IsConstructedGenericType)
        {
            var tmp = (Task<object>?)mediatorCallback.DynamicInvoke(arguments);
            return tmp?.GetAwaiter().GetResult();
        }
        else
        {
            return mediatorCallback.DynamicInvoke(arguments);
        }
    }

    private static object[] ResolveArguments(object request, Delegate requestHandler, IServiceProvider serviceProvider)
    {
        List<object> arguments = [];
        var methodInfo = requestHandler.GetMethodInfo();
        var parameters = methodInfo.GetParameters();
        arguments.Add(request);
        foreach (var parameterType in parameters.Select(p => p.ParameterType).Skip(1).ToList())
        {
            var argument = serviceProvider.GetService(parameterType);
            arguments.Add(argument ?? new ArgumentNullException("Could not resolve dependency " + parameterType.Name));
        }

        return arguments.ToArray();
    }
}
