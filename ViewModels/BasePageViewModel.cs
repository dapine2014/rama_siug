using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SIUGJ.ViewModels
{
    public class BasePageViewModel : INotifyPropertyChanged
    {
        public BasePageViewModel()
        { }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<TData>(ref TData storage, TData value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<TData>.Default.Equals(storage, value))
                return false;

            storage = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
