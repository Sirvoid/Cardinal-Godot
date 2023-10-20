using System;
using System.Reflection;

namespace CardinalEngine {

    internal class SyncAttribute : Attribute {
        private int _fieldID;

        public SyncAttribute(int fieldID) {
            _fieldID = fieldID;
        }

        internal static void SetFieldValueByNetworkId(NetComponent component, int fieldId, object value) {
            var fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields) {
                var attributes = field.GetCustomAttributes(typeof(SyncAttribute), true);

                foreach (var attribute in attributes) {
                    SyncAttribute syncAttribute = (SyncAttribute)attribute;

                    if (syncAttribute._fieldID == fieldId) {
                        if (field.FieldType.IsAssignableFrom(value.GetType())) {
                            field.SetValue(component, value);
                        } else {
                            throw new ArgumentException($"Type mismatch. Expected: {field.FieldType}. Received: {value.GetType()}");
                        }

                        return;
                    }
                }
            }

            throw new ArgumentException("No field matching networkId found.");
        }
    }
}