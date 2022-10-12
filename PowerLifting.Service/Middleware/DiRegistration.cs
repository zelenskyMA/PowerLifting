using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Infrastructure;
using PowerLifting.Infrastructure.Setup;
using SportAssist.Common.Commands;
using System.Reflection;

namespace PowerLifting.Service.Middleware
{
    public static class DiRegistration
    {
        public static void AddRepositoriesFromAssemblyOf<TAssembly>(this IServiceCollection services) => AddOperations<TAssembly>(services, typeof(ICrudRepo<>));
        public static void AddCommandsFromAssemblyOf<TAssembly>(this IServiceCollection services) => AddOperations<TAssembly>(services, typeof(ICommand<,>), typeof(Command<,>));

        private static void AddOperations<TAssembly>(IServiceCollection services, Type openGenericOperationInterface, Type openGenericDecorator = null)
        {
            foreach ((Type operationType, Type operationInterface) in Assembly.GetAssembly(typeof(TAssembly))
                .GetTypes()
                .Select(type => (operationType: type, operationInterface: GetOperationInterface(type, openGenericOperationInterface)))
                .Where(t => t.operationInterface != null)
                .ToList())
            {
                // Регистрируем операцию, обернутую в указанный декоратор под интерфейсом, который она реализует
                if (openGenericDecorator != null)
                {
                    Type decoratorType = openGenericDecorator.MakeGenericType(operationInterface.GetGenericArguments());
                    services.AddScoped(operationInterface, sp => ActivatorUtilities.CreateInstance(sp, decoratorType, ActivatorUtilities.CreateInstance(sp, operationType)));
                    continue;
                }

                // Регистрируем операцию без декоратора
                services.AddScoped(operationInterface, sp => ActivatorUtilities.CreateInstance(sp, operationType));
            }
        }

        private static Type? GetOperationInterface(Type operationCandidateType, Type openGenericOperationInterface) =>
            operationCandidateType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericOperationInterface);


        public static IServiceCollection AddConnectionProvider(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<LiftingContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IContextProvider, SqlContextProvider>();

            return services;
        }
    }

    /* public static IServiceCollection AddCommand<TParam, TResult, TCommand>(this IServiceCollection services)
     where TCommand : ICommand<TParam, TResult>
 {
     services.AddScoped<ICommand<TParam, TResult>>(sp =>
     ActivatorUtilities.CreateInstance<Command<TParam, TResult>>(sp, ActivatorUtilities.CreateInstance<TCommand>(sp)));

     return services;
 }
            // AddOperations<T>(services, typeof(ICommand<,>), typeof(Command<,>));

 */


    /*
    public static IServiceCollection AddCommand<TParam, TResult, TCommand>(this IServiceCollection services)
        where TCommand : ICommand<TParam, TResult>
    {
        services.AddScoped<ICommand<TParam, TResult>>(sp => 
            ActivatorUtilities.CreateInstance<Command<TParam, TResult>>(sp, ActivatorUtilities.CreateInstance<TCommand>(sp)));

        return services;
    }


    private static void AddCommands<T>(this IServiceCollection services)
    {
        foreach ((Type commandImplementation, Type commandInterface) in Assembly.GetAssembly(typeof(T))
            .GetTypes()
            .Select(type => (operationType: type, operationInterface: GetOperationInterface(type, typeof(ICommand<,>))))
            .Where(t => t.operationInterface != null)
            .ToList())
        {
            Type param = commandInterface.GetGenericArguments()[0];
            Type result = commandInterface.GetGenericArguments()[1];

            services.InvokeAddSingleCommand(param, result, commandImplementation);
        }
    }

    private static IServiceCollection InvokeAddSingleCommand(this IServiceCollection services, Type param, Type result, Type implementation)
    {
        MethodInfo openGenericMethod = typeof(RetryOperationsServiceCollectionExtensions).GetMethod(nameof(AddSingleCommand), BindingFlags.Static | BindingFlags.NonPublic);
        MethodInfo closedGenericMethod = openGenericMethod.MakeGenericMethod(param, result, implementation);

        return (IServiceCollection)closedGenericMethod.Invoke(null, new[] { services });
    }
    private static IServiceCollection AddSingleCommand<TParam, TResult, TImplementation>(IServiceCollection services)
    where TImplementation : ICommand<TParam, TResult>
    {
        services.AddScoped<ICommand<TParam, TResult>>(sp =>
        {
            OperationFeatures operationFeatures = sp.GetRequiredService<OperationFeatures>();

            if (operationFeatures.DisableOperationRetries)
            {
                return ActivatorUtilities.CreateInstance<Command<TParam, TResult>>(
                sp, new Func<TImplementation>(() => ActivatorUtilities.CreateInstance<TImplementation>(sp)));
            }
            else
            {
                return ActivatorUtilities.CreateInstance<RetryAdapter<TParam, TResult>>(
                sp, new Func<TParam, Task<TResult>>(async param =>
                {
                    using IServiceScope scope = sp.CreateScope();
                    var serviceProvider = scope.ServiceProvider;

                    Command<TParam, TResult> transactionalCommand = ActivatorUtilities.CreateInstance<Command<TParam, TResult>>(
serviceProvider, new Func<TImplementation>(() => ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider)));

                    return await transactionalCommand.ExecuteAsync(param);
                }));
            }
        });

        services.AddScoped<IOperation<TParam, TResult>>(sp => ActivatorUtilities.CreateInstance<TImplementation>(sp));

        return services;
    }

    private static Type GetOperationInterface(Type operationCandidateType, Type openGenericCommandInterface)
    {
        return operationCandidateType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericCommandInterface);
    }*/
}
