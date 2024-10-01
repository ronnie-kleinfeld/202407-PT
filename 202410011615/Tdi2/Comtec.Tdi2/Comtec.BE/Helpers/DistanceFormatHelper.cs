using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comtec.BE.Helpers {
    public static class DistanceFormatHelper {
        // 3.45 Km
        // 450 Meters
        // 0.67 Meters
        public static string ToMetricFormat(double meters) {
            if (meters > 1000) {
                return $"{meters / 1000} Km";
            } else {
                return $"{meters} Meters";
            }
        }

        // miles = 1760 yards = 5280 feet
        // yards = 3 feet = 36 inches
        // foot = 12 inches
        // inches = 25.4 mm = 2.54 cm = 0.0254 meter
        public static string ToMiles(double meters) {
            double mile = DistanceConversionHelper.ToMiles(meters);
            return $"{mile} {(mile < 2 ? "Mile" : "Miles")}";
        }

        public static string ToYards(double meters) {
            double yard = DistanceConversionHelper.ToYards(meters);
            return $"{yard} {(yard < 2 ? "Yard" : "Yards")}";
        }

        public static string ToFeet(double meters) {
            double foot = DistanceConversionHelper.ToFeet(meters);
            return $"{foot}'";
        }

        public static string ToInches(double meters) {
            double inch = DistanceConversionHelper.ToInches(meters);
            return $"{inch}\"";
        }

        public static string ToMilesFeetInches(double meters) {
            double mile = DistanceConversionHelper.ToMiles(meters);
            double yard = DistanceConversionHelper.ToYards(meters);
            double foot = DistanceConversionHelper.ToFeet(meters);
            double inch = DistanceConversionHelper.ToInches(meters);

            int miles = (int)mile;
            int a = miles * 63360;
            double b = inch - a;
            double c = b / 12;
            int feets = (int)Math.Floor(c);
            int d = feets * 12;
            double inches = b - d;

            if (miles > 1) {
                return $"{miles} Miles {feets}' {NumberFormatHelper.ToString(b - d, 2)}\"";
            } else if (miles == 1) {
                return $"{miles} Mile {feets}' {NumberFormatHelper.ToString(b - d, 2)}\"";
            } else if (feets >= 1) {
                return $"{feets}' {NumberFormatHelper.ToString(b - d, 2)}\"";
            } else {
                return $"{inches}\"";
            }
        }
    }
}