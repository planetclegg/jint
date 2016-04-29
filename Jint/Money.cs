using System;
using System.Globalization;

namespace Jint
{


	public struct Money
	{
		private decimal? _value;
		public static readonly Money NaN = new Money(null);
		public static readonly Money MaxValue = new Money(decimal.MaxValue);
		public static readonly Money MinValue = new Money(decimal.MinValue);
		public static readonly Money PositiveInfinity = new Money(decimal.MaxValue); // FIXME
		public static readonly Money NegativeInfinity = new Money(decimal.MinValue); // FIXME


		public Money(decimal? v)
		{
			_value = v;
		}

		public static bool IsNaN(Money m)
		{
			return m._value == null;
		}
		public static bool IsInfinity(Money m)
		{
			return false; // fixme
		}

		public static bool IsPositiveInfinity(Money m)
		{
			return false; // FIXME
		}

		public static bool IsNegativeInfinity(Money m)
		{
			return false; // FIXME
		}



		private static bool IsNumber(object value)
		{
			return value is sbyte
				|| value is byte
				|| value is short
				|| value is ushort
				|| value is int
				|| value is uint
				|| value is long
				|| value is ulong
				|| value is float
				|| value is double
				|| value is decimal;
		}


		public bool Equals(Money obj)
		{
			return obj._value == this._value;
		}


		public bool Equals(int l)
		{
			return _value == l;
		}
		public bool Equals(uint l)
		{
			return _value == l;
		}

		public override bool Equals(object obj)
		{
			if ((obj.GetType() == typeof(Money)))
			{
				var d = (Money)obj;
				return d._value == _value;
			}
			return false;
//			if (IsNumber(obj))
//			{
//				if (IsNaN(this))
//				{
//					if (obj is double)
//					{
//						if (double.NaN == (double)obj)
//							return true;
//					}
//					if (obj is float)
//					{
//						if (float.NaN == (float)obj)
//							return true;
//					}
//
//				}
//				var d = Convert.ToDecimal(obj);
//			}
//			return false;
		}
		public override int GetHashCode()
		{
			return _value == null ? 0 : _value.GetHashCode();
		}

		static public implicit operator Money(decimal? value)
		{
			return new Money(value);
		}

//		static public implicit operator Money(int value)
//		{
//			return new Money(value);
//		}

		public static Money operator +(Money x, Money y)
		{
			if (Money.IsNaN(x) || Money.IsNaN(y))
				return Money.NaN;
			return x._value + y._value;
		}
		public static Money operator -(Money x, Money y)
		{
			if (Money.IsNaN(x) || Money.IsNaN(y))
				return Money.NaN;
			return x._value - y._value;
		}
		public static Money operator *(Money x, Money y)
		{
			if (Money.IsNaN(x) || Money.IsNaN(y))
				return Money.NaN;
			return x._value * y._value;
		}
		public static Money operator /(Money x, Money y)
		{
			if (Money.IsNaN(x) || Money.IsNaN(y))
				return Money.NaN;
			return x._value / y._value;
		}
		public static Money operator %(Money x, Money y)
		{
			if (Money.IsNaN(x) || Money.IsNaN(y))
				return Money.NaN;
			return x._value % y._value;
		}


		public static bool operator >(Money x, Money y)
		{
			if (Money.IsNaN(x) || Money.IsNaN(y))
				return false;
			return x._value > y._value;
		}

		public static bool operator <(Money x, Money y)
		{
			if (Money.IsNaN(x) || Money.IsNaN(y))
				return false;
			return x._value < y._value;
		}
		public static bool operator >=(Money x, Money y)
		{
			if (Money.IsNaN(x) || Money.IsNaN(y))
				return false;
			return x._value >= y._value;
		}

		public static bool operator <=(Money x, Money y)
		{
			if (Money.IsNaN(x) || Money.IsNaN(y))
				return false;
			return x._value <= y._value;
		}

		public static bool operator ==(Money x, Money y)
		{
			return x._value == y._value;
		}

		public static bool operator !=(Money x, Money y)
		{
			return x._value != y._value;
		}
			

		public static Money operator -(Money a)
		{
			if (IsNaN(a))
				return a;
			return -a._value.Value;
		}

		public static Money operator ++(Money a)
		{
			if (!IsNaN(a))				
				a._value += 1;
			return a;
		}

		public static Money operator --(Money a)
		{
			if (!IsNaN(a))				
				a._value -= 1;
			return a;
		}

		public static explicit operator int(Money m)
		{
			if (m._value == null) return 0;
			return (int)m._value;
		}

		public static explicit operator uint(Money m)
		{
			if (m._value == null) return 0;
			return (uint)m._value;
		}
		public static explicit operator long(Money m)
		{
			if (m._value == null) return 0;
			return (long)m._value;
		}

		public static explicit operator ulong(Money m)
		{
			if (m._value == null) return 0;
			return (ulong)m._value;
		}



		public static Money Min(Money x, Money y)
		{
			if (IsNaN(x) || IsNaN(y))
				return NaN;
			return Math.Min(x._value.Value, y._value.Value);
		}

		public static Money Max(Money x, Money y)
		{
			if (IsNaN(x) || IsNaN(y))
				return NaN;
			return Math.Max(x._value.Value, y._value.Value);

		}

		public static Money Abs(Money a)
		{
			if (IsNaN(a))
				return NaN;
			return Math.Abs(a._value.Value);
		}

		public static Money Floor(Money a)
		{
			if (IsNaN(a))
				return NaN;
			return Decimal.Floor(a._value.Value);
		}

		public static Money Ceiling(Money a)
		{
			if (IsNaN(a))
				return NaN;
			
			return Decimal.Ceiling(a._value.Value);
		}

		public static Money Round(Money a)
		{
			if (IsNaN(a))
				return NaN;

			return Math.Round(a._value.Value);
		}


		public double ToDouble()
		{
			if (IsNaN(this))
				return double.NaN;
			return (double)_value.Value;
		}

		public static Money FromDouble(double d)
		{
			if (double.IsNaN(d))
				return new Money (null);
			return new Money ((decimal)d);
		}


		public static bool IsPositiveZero(Money x)
		{
			return x == 0; // FIXME
		}

		public static bool IsNegativeZero(Money x)
		{
			return false; // FIXME
		}

		public string ToString(string format, CultureInfo cultureInfo)
		{
			if (IsNaN(this))
				return "NaN";
			return this._value.Value.ToString(format, cultureInfo);
		}

		public override string ToString()
		{
			return string.Format("{0}", _value);
		}
	}
}

