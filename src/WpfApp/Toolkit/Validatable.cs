using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using LanguageExt;

namespace WpfApp.Toolkit {
    public abstract class Validatable : ViewModel, INotifyDataErrorInfo {
        private readonly IDictionary<string, ImmutableArray<string>> _errors =
            new Dictionary<string, ImmutableArray<string>>();

        public IEnumerable GetErrors(string propertyName) =>
            _errors.ContainsKey(propertyName)
                ? _errors[propertyName]
                : null;

        public bool HasErrors =>
            _errors.Any();

        protected override void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null) {
            base.SetProperty(ref member, val, propertyName);
            ValidateProperty(propertyName, val);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void ValidateProperty<T>(string propertyName, T value) {
            var context = new ValidationContext(this) {
                MemberName = propertyName,
            };
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(value, context, results);
            var oldErrors = _errors.TryGetValue(propertyName);
            var newErrors = results
                .Select(result => result.ErrorMessage)
                .ToImmutableArray();
            if (newErrors.Any()) {
                _errors[propertyName] = newErrors;
            } else {
                _errors.Remove(propertyName);
            }

            var isErrorsChanged = oldErrors
                .Some(oldErrorsValue => !oldErrorsValue.SequenceEqual(newErrors))
                .None(() => true);
            if (isErrorsChanged) {
                ErrorsChanged?.Invoke(this, new(propertyName));
            }
        }
    }
}
