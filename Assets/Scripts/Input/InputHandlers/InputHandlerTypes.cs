
using System;

namespace InputHandler
{
    public abstract class IHT 
    {
        public static Type KEY_UP { get { return typeof(KEY_UP); } }
        public static Type MOUSE_HOVER { get { return typeof(MOUSE_HOVER); } }
        public static Type MOUSE_PRIMARY_CLICK { get { return typeof(MOUSE_PRIMARY_CLICK); } }
        public static Type MOUSE_SECONDARY_CLICK { get { return typeof(MOUSE_SECONDARY_CLICK); } }
        public static Type TOUCH { get { return typeof(TOUCH); } }

    }
}