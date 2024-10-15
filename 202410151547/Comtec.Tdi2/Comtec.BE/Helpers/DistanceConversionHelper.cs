namespace Comtec.BE.Helpers {
    public static class DistanceConversionHelper {
        public static double ToMiles(double meters) {
            return ToYards(meters) / 1760;
        }
        public static double ToYards(double meters) {
            return ToFeet(meters) / 3;
        }
        public static double ToFeet(double meters) {
            return ToInches(meters) / 12;
        }
        public static double ToInches(double meters) {
            return meters / 0.0254;
        }
    }
}