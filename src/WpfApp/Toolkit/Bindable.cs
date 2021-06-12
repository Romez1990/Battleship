using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp.Toolkit {
    public abstract class Bindable : INotifyPropertyChanged {
        protected virtual void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null) {
            if (Equals(member, val)) {
                throw new();
            }

            member = val;
            PropertyChanged?.Invoke(this, new(propertyName));
        }

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
