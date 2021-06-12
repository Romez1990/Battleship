using System.Reflection;

namespace WpfApp {
    public class PropertyInfoEqualityComparer : IPropertyInfoEqualityComparer {
        public bool Equals(PropertyInfo x, PropertyInfo y) {
            if (x == null || y == null) return false;
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(PropertyInfo property) =>
            property.MetadataToken;
    }
}
