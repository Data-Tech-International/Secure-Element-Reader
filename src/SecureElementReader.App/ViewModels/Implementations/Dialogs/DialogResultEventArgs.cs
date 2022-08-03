using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.ViewModels.Implementations.Dialogs
{
    public class DialogResultEventArgs<TResult> : EventArgs
    {
        public TResult Result { get; }

        public DialogResultEventArgs(TResult result)
        {
            Result = result;
        }
    }
}
