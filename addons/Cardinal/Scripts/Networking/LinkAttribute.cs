using System;
using System.Reflection;

namespace CardinalEngine {

    [AttributeUsage(AttributeTargets.Class)]
    public class LinkAttribute : Attribute {
        public static Type[] ComponentTypes = new Type[ushort.MaxValue];

        private int _componentTypeID;

        public LinkAttribute(int componentTypeID) {
            _componentTypeID = componentTypeID;
        }

        public static void CompileComponentTypes() {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types) {
                if (type.IsSubclassOf(typeof(NetComponent))) {
                    object[] attributes = type.GetCustomAttributes(typeof(LinkAttribute), false);

                    foreach (LinkAttribute attribute in attributes) {
                        ComponentTypes[attribute._componentTypeID] = type;
                    }
                }
            }
        }
    }
}