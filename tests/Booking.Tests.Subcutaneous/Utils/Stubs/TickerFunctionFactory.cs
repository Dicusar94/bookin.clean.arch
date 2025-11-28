using System.Reflection;
using TickerQ.Utilities.Enums;
using TickerQ.Utilities.Models;

namespace BookingApp.Utils.Stubs;

public static class TickerFunctionFactory
{
        public static TickerFunctionContext<TRequest> Create<TRequest>(TRequest request, TickerType type = TickerType.Timer)
        {
            // Locate the internal constructor with the correct signature
            var baseCtor = typeof(TickerFunctionContext)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(c =>
                {
                    var p = c.GetParameters();
                    return p.Length == 6 &&
                           p[0].ParameterType == typeof(Guid) &&
                           p[1].ParameterType == typeof(TickerType) &&
                           p[2].ParameterType == typeof(int) &&
                           p[3].ParameterType == typeof(bool) &&
                           p[4].ParameterType == typeof(Func<Task>) &&
                           p[5].ParameterType == typeof(Action);
                });

            // Create the base context instance
            var baseContext = (TickerFunctionContext)baseCtor.Invoke([
                Guid.NewGuid(),                          // id
                type,                   // type
                0,                                       // retryCount
                true,                                    // isDue
                (Func<Task>)(() => Task.CompletedTask),  // deleteAsync
                (Action)(() => { })                      // cancelTicker
            ]);

            // Now create the generic version
            var genericCtor = typeof(TickerFunctionContext<>)
                .MakeGenericType(typeof(TRequest))
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .First(c => c.GetParameters().Length == 2);

            return (TickerFunctionContext<TRequest>)genericCtor.Invoke(new object[]
            {
                baseContext,
                request!
            });
        }
}