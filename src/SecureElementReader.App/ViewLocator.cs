using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SecureElementReader.App.ViewModels;
using System;

namespace SecureElementReader.App
{
    public class ViewLocator : IDataTemplate
    {
        public IControl Build(object data)
        {
            var fullName = data.GetType().FullName;
            if (fullName is null)
            {
                throw new InvalidOperationException("Full name for type was not found");
            }

            var name = fullName.Replace("ViewModel", "View");

            var type = Type.GetType(name);
            if (type is null)
            {
                throw new InvalidOperationException($"Type {name} was not found");
            }

            return (Control)Activator.CreateInstance(type);
        }

        public bool Match(object data) => data is ViewModelBase;
    }
}
