using System;
using System.Reflection;

namespace CardinalEngine {

    [AttributeUsage(AttributeTargets.Method)]
    internal class RemoteAttribute : Attribute {
        private int _methodID;

        public RemoteAttribute(int commandID) {
            _methodID = commandID;
        }

        public static MethodInfo GetMethodById(NetComponent component, int methodID) {
            var methods = component.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in methods) {
                var attributes = method.GetCustomAttributes(typeof(RemoteAttribute), true);

                if (attributes.Length > 0) {
                    foreach (RemoteAttribute attribute in attributes) {
                        if (attribute._methodID == methodID) {
                            return method;
                        }
                    }
                }
            }

            return null;
        }
    }
}