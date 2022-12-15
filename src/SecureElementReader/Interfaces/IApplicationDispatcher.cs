using System;
using System.Threading.Tasks;

namespace SecureElementReader.Interfaces
{
    public interface IApplicationDispatcher
    {
        void Dispatch(Action action);

        Task DispatchAsync(Func<Task> task);
    }
}
