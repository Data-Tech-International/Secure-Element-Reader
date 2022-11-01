using System;

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
