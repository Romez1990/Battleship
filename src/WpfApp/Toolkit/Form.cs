using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using static LanguageExt.Prelude;

namespace WpfApp.Toolkit {
    public abstract class Form : Validatable {
        public Form() {
            Submit = new(OnSubmitCommand);
        }

        protected void ValidateAllProperties() {
            var baseProperties = typeof(Validatable)
                .GetProperties();
            var properties = GetType()
                .GetProperties()
                .Except(baseProperties, new PropertyInfoEqualityComparer())
                .Filter(not((Func<PropertyInfo, bool>)IsCommandProperty));
            foreach (var property in properties) {
                var propertyName = property.Name;
                var value = property.GetValue(this);
                ValidateProperty(propertyName, value);
            }
        }

        private bool IsCommandProperty(PropertyInfo property) =>
            typeof(ICommand).IsAssignableFrom(property.PropertyType);

        public RelayCommand Submit { get; }

        private void OnSubmitCommand() {
            if (!HasErrors) {
                OnSubmit();
            }
        }

        protected abstract void OnSubmit();
    }
}
