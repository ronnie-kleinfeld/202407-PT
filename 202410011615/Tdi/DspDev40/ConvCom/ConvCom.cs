using System.Text;

namespace Comtec.Tis
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class ConvCom
	{
		//07.20
		//'$' moved from neutral to English
		//'/' moved from neutral to number

		private bool isNeutral(char s) //vk 07.20
		{
			//return "!@#%^&*()_+|{}\"<>?-=\\[];',. ".IndexOf(s) >= 0;
			return "!@#%^&*()_+|{}:\"<>?-=\\[];',. ".IndexOf(s) >= 0;
			//"!@#$%^&*()_+|{}:\"<>?-=\\[];',./ " - original list by ms
		}
		private bool isEnglish(char s) //vk 07.20
		{
			return (s >= 'a' && s <= 'z') || (s >= 'A' && s <= 'Z') || s == '$'; //'$' ib 07.20
		}
		private bool isHebrew(char s) //vk 07.20
		{
			long n = System.Convert.ToInt32(s);
			return (n >= 1488) && (n <= 1514);
		}
		private bool isDigit(char s) //vk 07.20
		{
			return char.IsDigit(s) || s == '/'; //|| s == ':';
		}
		public ConvCom()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public string RevHeb(string sStr, ref string sLang)
		{


			StringBuilder sMiddle = new StringBuilder("", 100);
			StringBuilder sRet = new StringBuilder("", 100);
			string sTemp;

			int iCondNow;   //' 1 - Heb, 2 - neutral, 3 - Eng, 4 - numeric
			int iCondWas;   //' 1 - Heb, 2 - neutral, 3 - Eng, 4 - numeric

			char s;

			int i;
			int l;

			l = sStr.Length;

			iCondWas = 2;

			sLang = "e";

			for (i = 0; i < l; i++)
			{

				s = sStr[i];

				//if (s >= 'à' && s <= 'ú')
				if (isHebrew(s))
				{
					//' Hebrew
					sLang = "h";
					iCondNow = 1;
				}
				else
				{
					//if (char.IsDigit(s))
					//{
					//    // Numeric
					//    iCondNow = 4;
					//}
					//else
					//if (mconNeutral.IndexOf(s) >= 0)
					if (isNeutral(s))
					{
						//' Neutral
						iCondNow = 2;

						if (("({[<".IndexOf(s) >= 0 && (iCondWas == 3 || iCondWas == 4)) ||
							(")}]>".IndexOf(s) >= 0 && (iCondWas == 1 || iCondWas == 4)))
						{
							iCondNow = RevModeForword(sStr, i);
						}
						else
							if (".,".IndexOf(s) >= 0 && iCondWas == 4)
						{
							iCondNow = iCondWas;
						}
						//if (s == '$')
						//{
						//    iCondNow = 3; //ib 07.20
						//}
					}
					else
					{
						//' English
						iCondNow = 3;
					}
				}

				if (iCondWas == 2)
				{
					iCondWas = iCondNow;
				}


				if ((iCondNow == iCondWas) || (iCondNow == 2) || (iCondWas == 2))
					//' we still in the same mode
					sMiddle.Append(s);
				else
				{
					//' mode is changed

					sTemp = Rev(sMiddle.ToString(), iCondWas, sLang);
					sRet.Insert(0, sTemp);

					//sMiddle = "";
					sMiddle.Remove(0, sMiddle.Length);
					sMiddle.Append(s);
					iCondWas = iCondNow;

				}

			}  // next for i

			sTemp = Rev(sMiddle.ToString(), iCondWas, sLang);
			sRet.Insert(0, sTemp);

			return sRet.ToString();

		} // RevHeb()

		private int RevModeForword(string sStr, int iP)
		{
			int iCondNow;   //' 1 - Heb, 2 - neutral, 3 - Eng
			char s;

			int i;
			int l;

			l = sStr.Length;

			iCondNow = 2;

			for (i = iP + 1; i < l; i++)
			{

				s = sStr[i];

				//if (s >= 'à' && s <= 'ú')
				if (isHebrew(s))
				{

					//' Hebrew
					iCondNow = 1;
					break;
				}
				else
					//if (char.IsDigit(s))
					if (isDigit(s))
				{
					// Numeric
					iCondNow = 4;
					break;
				}
				else
				{
					//if (mconNeutral.IndexOf(s) >= 0)
					if (isNeutral(s))
						//' Neutral
						iCondNow = 2;
					else
					{
						//' English
						iCondNow = 3;
						break;
					}
				}
			} // next for        

			return iCondNow;

			// RevModeForword()
		}

		private string Rev(string sStr, int iCond, string gLang)
		{

			System.Text.StringBuilder sMiddle = new StringBuilder("", 100);
			System.Text.StringBuilder sFirst = new StringBuilder("", 100);
			System.Text.StringBuilder sLast = new StringBuilder("", 100);
			System.Text.StringBuilder sBuf = new StringBuilder("", 100);

			char s;

			int i, j, k;


			string paren = "(){}[]<>";
			string parenh = ")(}{][><";

			// -------------------
			// Turn part first
			// -------------------
			k = 0;

			for (i = 0; i < sStr.Length; i++)
			{
				s = sStr[i];
				//if (mconNeutral.IndexOf(s) >= 0)
				if (isNeutral(s))
				{
					j = paren.IndexOf(s);

					if (j >= 0)
						s = parenh[j];
					//sFirst = s + sFirst;
					sFirst.Insert(0, s);
					k = i + 1;
				}
				else
				{
					k = i;
					break;
				}
			}   //next for	

			if (k > 0)
				sStr = sStr.Substring(k);

			//' -------------------
			//' Turn part last
			//' -------------------

			k = -1;

			for (i = sStr.Length - 1; i >= 0; i--)
			{
				s = sStr[i];
				//if (mconNeutral.IndexOf(s) >= 0)
				if (isNeutral(s))
				{

					j = paren.IndexOf(s);

					if (j >= 0)
						s = parenh[j];

					//sLast = sLast + s;
					sLast.Append(s);
					k = i - 1;
				}
				else
				{
					k = i;
					break;
				}
			} //next for


			if (k >= 0)
				sStr = sStr.Substring(0, k + 1);
			else
				sStr = "";

			//' -------------------
			//' Turn part middle
			//' -------------------

			int iCondWas;

			if (iCond == 3 && gLang == "h")
			{
				// define if digital
				for (i = 0; i < sStr.Length; i++)
				{
					//if (char.IsDigit(sStr[i]))
					if (isDigit(sStr[i]))
					{
						iCond = 4;
					}
					else
						//if ((sStr[i] >= 'a' && sStr[i] <= 'z') ||
						//(sStr[i] >= 'A' && sStr[i] <= 'Z') || sStr[i] == '$') //'$' ib 07.20
						if (isEnglish(sStr[i]))
					{
						iCond = 3;
						break;
					}
				}
			}

			if (iCond == 1)
			{
				// sLang == "h"
				for (i = 0; i < sStr.Length; i++)
				{

					s = sStr[i];

					j = paren.IndexOf(s);

					if (j >= 0)
						s = parenh[j];


					//sMiddle = s + sMiddle;
					sMiddle.Insert(0, s);
				} //next for
			}
			else
				if (iCond == 4 && gLang == "h")
			{
				// sLang == "n"
				// numeric and hebrew
				iCondWas = 2;
				for (i = 0; i < sStr.Length; i++)
				{
					s = sStr[i];
					//if ((char.IsDigit(s) && (iCondWas == 4)) ||
					//	((",.".IndexOf(s) >= 0) && (iCondWas == 4)))
					if ((isDigit(s) && (iCondWas == 4)) ||
						((",.:".IndexOf(s) >= 0) && (iCondWas == 4))) //: ib 04.21
					{
						// proceed numeric
						sBuf.Append(s);
					}
					else
						//if (char.IsDigit(s) && (iCondWas == 2))
						if (isDigit(s) && (iCondWas == 2))
					{
						// start numeric
						//sBuf.Remove(0, sBuf.Length);
						sBuf.Append(s);
						iCondWas = 4;
					}
					else
					{
						// not numeric
						if (sBuf.Length > 0)
						{
							sMiddle.Insert(0, sBuf.ToString());
							sBuf.Remove(0, sBuf.Length);
						}

						j = paren.IndexOf(s);

						if (j >= 0)
							s = parenh[j];

						sMiddle.Insert(0, s);
						iCondWas = 2;
					}


				}  //for (i = 0; i < sStr.Length; i++)

				if (iCondWas == 4)
				{
					sMiddle.Insert(0, sBuf.ToString());
				}
			}
			else
			{
				// sLang == "e"
				sMiddle.Append(sStr);
			}

			return (sLast.ToString() + sMiddle.ToString() + sFirst.ToString());

		} // Rev()

	}  // public class ConvCom
}  // namespace Comtec.Tis
