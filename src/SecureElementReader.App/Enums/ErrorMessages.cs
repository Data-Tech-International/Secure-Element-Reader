using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.Enums
{
    public enum ErrorMessages 
    {
        CannotGetPendingCommands,
        AllCommandsExecutedSuccessfully,
        AllCommandsExecutedButFailedToNotifyTaxCoreSystem,
        NotAllCommandEexecutedSuccessfully,
        CommandsNotExecuted,
        ThereIsNoPendingCommandsForThisCard
    }
}
