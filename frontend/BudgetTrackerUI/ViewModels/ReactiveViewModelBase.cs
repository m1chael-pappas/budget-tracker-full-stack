using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;

namespace BudgetTrackerUI.ViewModels
{
    public class ReactiveViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            Dispatcher.UIThread.Post(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected ReactiveCommand<Unit, Unit> CreateCommand(Action action)
        {
            return ReactiveCommand.Create(() =>
            {
                Dispatcher.UIThread.Post(action);
            });
        }

        protected ReactiveCommand<Unit, Unit> CreateAsyncCommand(Func<Task> action)
        {
            return ReactiveCommand.CreateFromTask(async () =>
            {
                await Dispatcher.UIThread.InvokeAsync(async () => await action());
            });
        }
    }
}