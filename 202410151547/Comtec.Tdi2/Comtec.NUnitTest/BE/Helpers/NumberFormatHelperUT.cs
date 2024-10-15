using Comtec.BE.Helpers;

namespace Comtec.NUnitTest.BE.Helpers {
    public class NumberFormatHelperUT {
        // int
        [TestCase("0", 0)]
        [TestCase("12,345", 12345)]
        public void IntToString(string expected, int i) {
            Assert.AreEqual(expected, NumberFormatHelper.ToString(i));
        }

        [TestCase("₪0", 0)]
        [TestCase("₪12,345", 12345)]
        public void IntToCurrency(string expected, int i) {
            Assert.AreEqual(expected, NumberFormatHelper.ToCurrency(i));
        }

        [TestCase("0%", 0)]
        [TestCase("1,234,500%", 12345)]
        public void IntToPercent(string expected, int i) {
            Assert.AreEqual(expected, NumberFormatHelper.ToPercent(i));
        }

        [TestCase("0", 0)]
        [TestCase("3039", 12345)]
        public void IntToHex(string expected, int i) {
            Assert.AreEqual(expected, NumberFormatHelper.ToHex(i));
        }

        // long
        [TestCase("12,345,678,901,234,567", 12345678901234567)]
        public void LongToString(string expected, long l) {
            Assert.AreEqual(expected, NumberFormatHelper.ToString(l));
        }

        [TestCase("₪12,345,678,901,234,567", 12345678901234567)]
        public void LongToCurrency(string expected, long l) {
            Assert.AreEqual(expected, NumberFormatHelper.ToCurrency(l));
        }

        [TestCase("1,234,567,890,123,456,700%", 12345678901234567)]
        public void LongToPercent(string expected, long l) {
            Assert.AreEqual(expected, NumberFormatHelper.ToPercent(l));
        }

        [TestCase("2BDC545D6B4B87", 12345678901234567)]
        public void LongToHex(string expected, long i) {
            Assert.AreEqual(expected, NumberFormatHelper.ToHex(i));
        }

        // double
        [TestCase("0", 0.001, 0)]
        [TestCase("0.000", 0.000001, 3)]
        [TestCase("12,346", 12345.6789, 0)]
        [TestCase("12,345.679", 12345.6789, 3)]
        public void DoubleToString(string expected, decimal d, int decimalDigits) {
            Assert.AreEqual(expected, NumberFormatHelper.ToString((double)d, decimalDigits));
        }

        [TestCase("₪0", 0.001, 0)]
        [TestCase("₪0.000", 0.000001, 3)]
        [TestCase("₪12,346", 12345.6789, 0)]
        [TestCase("₪12,345.679", 12345.6789, 3)]
        public void DoubleToCurrency(string expected, decimal d, int decimalDigits) {
            Assert.AreEqual(expected, NumberFormatHelper.ToCurrency((double)d, decimalDigits));
        }

        [TestCase("0%", 0.001, 0)]
        [TestCase("0.000%", 0.000001, 3)]
        [TestCase("1,234,568%", 12345.6789, 0)]
        [TestCase("1,234,567.890%", 12345.6789, 3)]
        public void DoubleToPercent(string expected, decimal d, int decimalDigits) {
            Assert.AreEqual(expected, NumberFormatHelper.ToPercent((double)d, decimalDigits));
        }

        // float
        [TestCase("0", 0.001, 0)]
        [TestCase("0.000", 0.000001, 3)]
        [TestCase("12,346", 12345.6789, 0)]
        [TestCase("12,345.680", 12345.680, 3)]
        public void FloatToString(string expected, decimal d, int decimalDigits) {
            Assert.AreEqual(expected, NumberFormatHelper.ToString((float)d, decimalDigits));
        }

        [TestCase("₪0", 0.001, 0)]
        [TestCase("₪0.000", 0.000001, 3)]
        [TestCase("₪12,346", 12345.6789, 0)]
        [TestCase("₪12,345.680", 12345.680, 3)]
        public void FloatToCurrency(string expected, decimal d, int decimalDigits) {
            Assert.AreEqual(expected, NumberFormatHelper.ToCurrency((float)d, decimalDigits));
        }

        [TestCase("0%", 0.001, 0)]
        [TestCase("0.000%", 0.000001, 3)]
        [TestCase("1,234,568%", 12345.6789, 0)]
        [TestCase("123,456,787.500%", 1234567.871, 3)]
        public void FloatToPercent(string expected, decimal d, int decimalDigits) {
            Assert.AreEqual(expected, NumberFormatHelper.ToPercent((float)d, decimalDigits));
        }

        // decimal
        [TestCase("0", 0.001, 0)]
        [TestCase("0.000", 0.000001, 3)]
        [TestCase("12,346", 12345.6789, 0)]
        [TestCase("12,345.679", 12345.6789, 3)]
        public void DecimalToString(string expected, decimal d, int decimalDigits) {
            Assert.AreEqual(expected, NumberFormatHelper.ToString(d, decimalDigits));
        }

        [TestCase("₪0", 0.001, 0)]
        [TestCase("₪0.000", 0.000001, 3)]
        [TestCase("₪12,346", 12345.6789, 0)]
        [TestCase("₪12,345.679", 12345.6789, 3)]
        public void DecimalToCurrency(string expected, decimal d, int decimalDigits) {
            Assert.AreEqual(expected, NumberFormatHelper.ToCurrency(d, decimalDigits));
        }

        [TestCase("0%", 0.001, 0)]
        [TestCase("0.000%", 0.000001, 3)]
        [TestCase("1,234,568%", 12345.6789, 0)]
        [TestCase("1,234,567.890%", 12345.6789, 3)]
        public void DecimalToPercent(string expected, decimal d, int decimalDigits) {
            Assert.AreEqual(expected, NumberFormatHelper.ToPercent(d, decimalDigits));
        }
    }
}