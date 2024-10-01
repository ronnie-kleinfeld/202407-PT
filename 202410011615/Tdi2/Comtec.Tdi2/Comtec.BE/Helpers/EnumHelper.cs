namespace Comtec.BE.Helpers {
    public static class EnumHelper<T> where T : System.Enum {
        public static bool IsEnum => typeof(T).IsEnum;

        public static bool Contains(int value) {
            if (!IsEnum) {
                throw new ArgumentException("T must be an enumerated type");
            }

            return System.Enum.GetValues(typeof(T)).Cast<int>().Contains(value);
        }

        public static int Count {
            get {
                if (!IsEnum) {
                    throw new ArgumentException("T must be an enumerated type");
                }

                return System.Enum.GetNames(typeof(T)).Length;
            }
        }
    }
}