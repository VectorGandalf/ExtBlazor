using System.Reflection;

namespace ExtBlazor.RemoteMediator.Server;

public static class MediatorInvoker
{
    public static async Task<object?> Invoke(
        object request,
        Delegate mediatorCallback,
        IServiceScopeFactory serviceScopeFactory,
        CancellationToken ct = default)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();

        var arguments = ResolveArguments(request, mediatorCallback, serviceScope.ServiceProvider, ct);
        if (mediatorCallback.Method.ReturnType.IsSubclassOf(typeof(Task)) && mediatorCallback.Method.ReturnType.IsConstructedGenericType)
        {
            var task = (Task<object?>?)mediatorCallback.DynamicInvoke(arguments);
            return await (task ?? throw new NullReferenceException("MediatorInvoker: Task<object?> is null"));
        }
        else if (mediatorCallback.Method.ReturnType.IsSubclassOf(typeof(Task)) && !mediatorCallback.Method.ReturnType.IsConstructedGenericType)
        {
            var task = (Task?)mediatorCallback.DynamicInvoke(arguments);
            await (task ?? throw new NullReferenceException("MediatorInvoker: Task is null"));
            return null;
        }
        else
        {
            return mediatorCallback.DynamicInvoke(arguments);
        }
    }

    public static object[] ResolveArguments(
        object request,
        Delegate requestHandler,
        IServiceProvider serviceProvider,
        CancellationToken ct = default)
    {
        List<object> arguments = [];
        var methodInfo = requestHandler.GetMethodInfo();
        var parameters = methodInfo.GetParameters();
        arguments.Add(request);
        for (int i = 1; i < parameters.Length; i++)
        {
            var parameterType = parameters[i].ParameterType;
            if (parameterType == typeof(CancellationToken))
            {
                arguments.Add(ct);
            }
            else
            {
                var argument = serviceProvider.GetService(parameterType);
                arguments.Add(argument ?? new ArgumentNullException("Could not resolve dependency " + parameterType.Name));
            }
        }

        return arguments.ToArray();
    }
}
