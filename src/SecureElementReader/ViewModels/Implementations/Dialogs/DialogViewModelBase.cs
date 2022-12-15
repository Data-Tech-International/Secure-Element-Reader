using ReactiveUI;
using SecureElementReader.Extensions;
using SecureElementReader.ViewModels.Services;
using System;
using System.Windows.Input;

namespace SecureElementReader.ViewModels.Implementations.Dialogs
{
    public class DialogViewModelBase<TResult> : ViewModelBase
    where TResult : DialogResultBase
    {
        public event EventHandler<DialogResultEventArgs<TResult>> CloseRequested;

        public ICommand CloseCommand { get; }

        protected DialogViewModelBase()
        {
            CloseCommand = ReactiveCommand.Create(Close);
        }

        protected void Close() => Close(default);

        protected void Close(TResult result)
        {
            var args = new DialogResultEventArgs<TResult>(result);

            CloseRequested.Raise(this, args);
        }
    }

    public class DialogViewModelBase : DialogViewModelBase<DialogResultBase>
    {

    }
}
