using SecureElementReader.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecureElementReader.App.Interfaces
{
    public interface ITaxCoreApiProxy
    {
        Task<bool> SendInternalData(SecureElementAuditRequest request);
        Task<List<Command>> GetPendingCommands();
        void Configure(string uniqueIdentifier, string commonName, string apiUrl);
        Task<List<CommandsStatusResult>> CommandStatusUpdate(List<CommandsStatusResult> commandResult);
    }
}
