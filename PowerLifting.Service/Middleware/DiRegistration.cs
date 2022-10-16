using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Infrastructure.DataContext;
using PowerLifting.Infrastructure.Setup.Generic.AppActions;
using System.Reflection;

namespace PowerLifting.Service.Middleware
{
    public static class DiRegistration
    {
        /// <summary>
        /// Регистрация доступа к контексту
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddConnectionProvider(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SportContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IContextProvider, SqlContextProvider>();

            return services;
        }

        /// <summary>
        /// Регистрация стандартных репозиториев на  основе ICrudRepo
        /// </summary>
        /// <typeparam name="TAssembly"></typeparam>
        /// <param name="services"></param>
        public static void AddRepositoriesFromAssemblyOf<TAssembly>(this IServiceCollection services) => AddOperations<TAssembly>(services, typeof(ICrudRepo<>));

        /// <summary>
        /// Регистрация команд на базе ICommand
        /// </summary>
        /// <typeparam name="TAssembly"></typeparam>
        /// <param name="services"></param>
        public static void AddCommandsFromAssemblyOf<TAssembly>(this IServiceCollection services) => AddOperations<TAssembly>(services, typeof(ICommand<,>), typeof(Command<,>));

        private static void AddOperations<TAssembly>(IServiceCollection services, Type openGenericOperationInterface, Type? openGenericDecorator = null)
        {
            foreach ((Type operationType, Type operationInterface) in Assembly.GetAssembly(typeof(TAssembly))
                .GetTypes()
                .Select(type => (operationType: type, operationInterface: GetOperationInterface(type, openGenericOperationInterface)))
                .Where(t => t.operationInterface != null)
                .ToList())
            {
                // Регистрируем операцию, обернутую в указанный декоратор под интерфейсом, который она реализует
                if (openGenericDecorator == typeof(Command<,>) && operationType.Name != "Command`2")
                {
                    Type paramType = operationInterface.GetGenericArguments()[0];
                    Type resultType = operationInterface.GetGenericArguments()[1];
                    services.InvokeAddCommand(paramType, resultType, operationType);
                    continue;
                }

                // Регистрируем операцию без декоратора
                services.AddScoped(operationInterface, sp => ActivatorUtilities.CreateInstance(sp, operationType));
            }
        }

        private static Type? GetOperationInterface(Type operationCandidateType, Type openGenericOperationInterface) =>
            operationCandidateType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericOperationInterface);

        private static IServiceCollection InvokeAddCommand(this IServiceCollection services, Type param, Type result, Type command)
        {
            MethodInfo addCommandMethodOpenGeneric = typeof(DiRegistration).GetMethod(nameof(AddCommand), BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo addCommandMethodClosedGeneric = addCommandMethodOpenGeneric.MakeGenericMethod(param, result, command);

            return (IServiceCollection)addCommandMethodClosedGeneric.Invoke(null, new[] { services });
        }

        private static void AddCommand<TParam, TResult, TCommand>(this IServiceCollection services)
            where TCommand : ICommand<TParam, TResult>
        {
            services.AddScoped<ICommand<TParam, TResult>>(sp =>
                ActivatorUtilities.CreateInstance<Command<TParam, TResult>>(sp, new Func<TCommand>(() => ActivatorUtilities.CreateInstance<TCommand>(sp))));
        }
    }
}
