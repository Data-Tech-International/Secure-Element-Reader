using Avalonia.Threading;
using SecureElementReader.Interfaces;
using System;
using System.Threading.Tasks;

namespace SecureElementReader.Services
{
    public class AvaloniaDispatcher : IApplicationDispatcher
    {
        private static Dispatcher Dispatcher => Dispatcher.UIThread;

        public void Dispatch(Action action) => Dispatcher.Post(action);

        public Task DispatchAsync(Func<Task> task) => Dispatcher.InvokeAsync(task);
    }
}
