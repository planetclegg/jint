using System;
using Jint.Native.Number;
using Jint.Native.Object;
using Jint.Runtime;
using Jint.Runtime.Interop;

namespace Jint.Native.Math
{
    public sealed class MathInstance : ObjectInstance
    {
        private static Random _random = new Random();
        
        private MathInstance(Engine engine):base(engine)
        {
        }

        public override string Class
        {
            get
            {
                return "Math";
            }
        }

        public static MathInstance CreateMathObject(Engine engine)
        {
            var math = new MathInstance(engine);
            math.Extensible = true;
            math.Prototype = engine.Object.PrototypeObject;

            
            return math;
        }

        public void Configure()
        {
            FastAddProperty("abs", new ClrFunctionInstance(Engine, Abs), true, false, true);
            FastAddProperty("acos", new ClrFunctionInstance(Engine, Acos), true, false, true);
            FastAddProperty("asin", new ClrFunctionInstance(Engine, Asin), true, false, true);
            FastAddProperty("atan", new ClrFunctionInstance(Engine, Atan), true, false, true);
            FastAddProperty("atan2", new ClrFunctionInstance(Engine, Atan2), true, false, true);
            FastAddProperty("ceil", new ClrFunctionInstance(Engine, Ceil), true, false, true);
            FastAddProperty("cos", new ClrFunctionInstance(Engine, Cos), true, false, true);
            FastAddProperty("exp", new ClrFunctionInstance(Engine, Exp), true, false, true);
            FastAddProperty("floor", new ClrFunctionInstance(Engine, Floor), true, false, true);
            FastAddProperty("log", new ClrFunctionInstance(Engine, Log), true, false, true);
            FastAddProperty("max", new ClrFunctionInstance(Engine, Max, 2), true, false, true);
            FastAddProperty("min", new ClrFunctionInstance(Engine, Min, 2), true, false, true);
            FastAddProperty("pow", new ClrFunctionInstance(Engine, Pow, 2), true, false, true);
            FastAddProperty("random", new ClrFunctionInstance(Engine, Random), true, false, true);
            FastAddProperty("round", new ClrFunctionInstance(Engine, Round), true, false, true);
            FastAddProperty("sin", new ClrFunctionInstance(Engine, Sin), true, false, true);
            FastAddProperty("sqrt", new ClrFunctionInstance(Engine, Sqrt), true, false, true);
            FastAddProperty("tan", new ClrFunctionInstance(Engine, Tan), true, false, true);

			FastAddProperty("E", (Money)(decimal)System.Math.E, false, false, false);
			FastAddProperty("LN10", (Money)(decimal)System.Math.Log(10), false, false, false);
			FastAddProperty("LN2", (Money)(decimal)System.Math.Log(2), false, false, false);
			FastAddProperty("LOG2E", (Money)(decimal)System.Math.Log(System.Math.E, 2), false, false, false);
			FastAddProperty("LOG10E", (Money)(decimal)System.Math.Log(System.Math.E, 10), false, false, false);
			FastAddProperty("PI", (Money)(decimal)System.Math.PI, false, false, false);
			FastAddProperty("SQRT1_2",(Money)(decimal) System.Math.Sqrt(0.5), false, false, false);
			FastAddProperty("SQRT2", (Money)(decimal)System.Math.Sqrt(2), false, false, false);

        }

        private static JsValue Abs(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
            return Money.Abs(x);
        }

        private static JsValue Acos(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
			return (Money)(decimal)System.Math.Acos(x.ToDouble());
        }

        private static JsValue Asin(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
			return (Money)(decimal)System.Math.Asin(x.ToDouble());
        }

        private static JsValue Atan(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
			return (Money)(decimal)System.Math.Atan(x.ToDouble());
        }

        private static JsValue Atan2(JsValue thisObject, JsValue[] arguments)
        {
            var y = TypeConverter.ToNumber(arguments.At(0));
            var x = TypeConverter.ToNumber(arguments.At(1));

            // If either x or y is NaN, the result is NaN.
            if (Money.IsNaN(x) || Money.IsNaN(y))
            {
                return Money.NaN;
            }

            if (y > 0 && x.Equals(0))
            {
				return (Money)(decimal)(System.Math.PI/2);
            }
            
			if (NumberInstance.IsPositiveZero(y))
            {
                // If y is +0 and x>0, the result is +0.
                if (x > 0)
                {
                    return +0;
                }

                // If y is +0 and x is +0, the result is +0.
				if (NumberInstance.IsPositiveZero(x))
                {
                    return +0;
                }

                // If y is +0 and x is −0, the result is an implementation-dependent approximation to +π.
				if (NumberInstance.IsNegativeZero(x))
                {
					return (Money)(decimal)System.Math.PI;
                }
                
                // If y is +0 and x<0, the result is an implementation-dependent approximation to +π.
                if (x < 0)
                {
					return (Money)(decimal) System.Math.PI;
                }
            }

			if (NumberInstance.IsNegativeZero(y))
            {
                // If y is −0 and x>0, the result is −0.
                if (x > 0)
                {
                    return -0;
                }

                // If y is −0 and x is +0, the result is −0.
				if (NumberInstance.IsPositiveZero(x))
                {
                    return -0;
                }

                // If y is −0 and x is −0, the result is an implementation-dependent approximation to −π.
				if (NumberInstance.IsNegativeZero(x))
                {
					return (Money)(decimal)-System.Math.PI;
                }

                // If y is −0 and x<0, the result is an implementation-dependent approximation to −π.
                if (x < 0)
                {
					return (Money)(decimal)-System.Math.PI;
                }
            }

            // If y<0 and x is +0, the result is an implementation-dependent approximation to −π/2.
            // If y<0 and x is −0, the result is an implementation-dependent approximation to −π/2.
            if (y < 0 && x.Equals(0))
            {
				return (Money)(decimal)-System.Math.PI/2;
            }

            // If y>0 and y is finite and x is +∞, the result is +0.
            if (y > 0 && !Money.IsInfinity(y))
            {
                if (Money.IsPositiveInfinity(x))
                {
                    return +0;
                }

                // If y>0 and y is finite and x is −∞, the result if an implementation-dependent approximation to +π.
                if (Money.IsNegativeInfinity(x))
                {
					return (Money) (decimal)System.Math.PI;
                }
            }


            // If y<0 and y is finite and x is +∞, the result is −0.
            // If y<0 and y is finite and x is −∞, the result is an implementation-dependent approximation to −π.
            if (y < 0 && !Money.IsInfinity(y))
            {
                if (Money.IsPositiveInfinity(x))
                {
                    return -0;
                }

                // If y>0 and y is finite and x is −∞, the result if an implementation-dependent approximation to +π.
                if (Money.IsNegativeInfinity(x))
                {
					return (Money)(decimal)-System.Math.PI;
                }
            }
            
            // If y is +∞ and x is finite, the result is an implementation-dependent approximation to +π/2.
            if (Money.IsPositiveInfinity(y) && !Money.IsInfinity(x))
            {
				return (Money)(decimal)System.Math.PI/2;
            }

            // If y is −∞ and x is finite, the result is an implementation-dependent approximation to −π/2.
            if (Money.IsNegativeInfinity(y) && !Money.IsInfinity(x))
            {
				return (Money)(decimal)-System.Math.PI / 2;
            }

            // If y is +∞ and x is +∞, the result is an implementation-dependent approximation to +π/4.
            if (Money.IsPositiveInfinity(y) && Money.IsPositiveInfinity(x))
            {
				return (Money)(decimal)System.Math.PI/4;
            }
            
            // If y is +∞ and x is −∞, the result is an implementation-dependent approximation to +3π/4.
            if (Money.IsPositiveInfinity(y) && Money.IsNegativeInfinity(x))
            {
				return (Money)(decimal)(3 * System.Math.PI / 4);
            }
            
            // If y is −∞ and x is +∞, the result is an implementation-dependent approximation to −π/4.
            if (Money.IsNegativeInfinity(y) && Money.IsPositiveInfinity(x))
            {
				return (Money)(decimal)-System.Math.PI / 4;
            }
            
            // If y is −∞ and x is −∞, the result is an implementation-dependent approximation to −3π/4.
            if (Money.IsNegativeInfinity(y) && Money.IsNegativeInfinity(x))
            {
				return (Money)(decimal)(- 3 * System.Math.PI / 4);
            }
            
			return (Money)(decimal)System.Math.Atan2(y.ToDouble(), x.ToDouble());
        }

        private static JsValue Ceil(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
            return Money.Ceiling(x);
        }

        private static JsValue Cos(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
			return (Money)(decimal)System.Math.Cos(x.ToDouble());
        }

        private static JsValue Exp(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
			return (Money)(decimal)System.Math.Exp(x.ToDouble());
        }

        private static JsValue Floor(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
            return Money.Floor(x);
        }

        private static JsValue Log(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
			return (Money)(decimal)System.Math.Log(x.ToDouble());
        }

        private static JsValue Max(JsValue thisObject, JsValue[] arguments)
        {
            if (arguments.Length == 0)
            {
                return Money.NegativeInfinity;
            }

            Money max = TypeConverter.ToNumber(arguments.At(0));
            for (int i = 0; i < arguments.Length; i++)
            {
				max = Money.Max(max, TypeConverter.ToNumber(arguments[i]));
            }
            return max;
        }

        private static JsValue Min(JsValue thisObject, JsValue[] arguments)
        {
            if (arguments.Length == 0)
            {
                return Money.PositiveInfinity;
            }

            Money min = TypeConverter.ToNumber(arguments.At(0));
            for (int i = 0; i < arguments.Length; i++)
            {
                min = Money.Min(min, TypeConverter.ToNumber(arguments[i]));
            }
            return min;
        }

        private static JsValue Pow(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
            var y = TypeConverter.ToNumber(arguments.At(1));

            if (Money.IsNaN(y))
            {
                return Money.NaN;
            }

            if (y.Equals(0))
            {
                return 1;
            }

            if (Money.IsNaN(x) && !y.Equals(0))
            {
                return Money.NaN;
            }

            if (Money.Abs(x) > 1)
            {
                if (Money.IsPositiveInfinity(y))
                {
                    return Money.PositiveInfinity;
                }

                if (Money.IsNegativeInfinity(y))
                {
                    return +0;
                }
            }

            if (Money.Abs(x).Equals(1))
            {
                if (Money.IsInfinity(y))
                {
                    return Money.NaN;
                }
            }

            if (Money.Abs(x) < 1)
            {
                if (Money.IsPositiveInfinity(y))
                {
                    return 0;
                }

                if (Money.IsNegativeInfinity(y))
                {
                    return Money.PositiveInfinity;
                }
            }

            if (Money.IsPositiveInfinity(x))
            {
                if (y > 0)
                {
                    return Money.PositiveInfinity;
                }

                if (y < 0)
                {
                    return +0;
                }
            }

            if (Money.IsNegativeInfinity(x))
            {
                if (y > 0)
                {
                    if (Money.Abs(y % 2).Equals(1))
                    {
                        return Money.NegativeInfinity;
                    }

                    return Money.PositiveInfinity;
                }

                if (y < 0)
                {
                    if (Money.Abs(y % 2).Equals(1))
                    {
                        return -0;
                    }

                    return +0;
                }
            }

            if (NumberInstance.IsPositiveZero(x))
            {
                // If x is +0 and y>0, the result is +0.
                if (y > 0)
                {
                    return 0;
                }

                // If x is +0 and y<0, the result is +∞.
                if (y < 0)
                {
                    return Money.PositiveInfinity;
                }
            }


            if (NumberInstance.IsNegativeZero(x))
            {
                if (y > 0)
                {
                    // If x is −0 and y>0 and y is an odd integer, the result is −0.
                    if (Money.Abs(y % 2).Equals(1))
                    {
                        return -0;
                    }

                    // If x is −0 and y>0 and y is not an odd integer, the result is +0.
                    return +0;
                }

                if (y < 0)
                {
                    // If x is −0 and y<0 and y is an odd integer, the result is −∞.
                    if (Money.Abs(y % 2).Equals(1))
                    {
                        return Money.NegativeInfinity;
                    }

                    // If x is −0 and y<0 and y is not an odd integer, the result is +∞.
                    return Money.PositiveInfinity;
                }
            }

            // If x<0 and x is finite and y is finite and y is not an integer, the result is NaN.
            if (x < 0 && !Money.IsInfinity(x) && !Money.IsInfinity(y) && !y.Equals((int)y))
            {
                return Money.NaN;
            }

			return (Money)(decimal)System.Math.Pow(x.ToDouble(), y.ToDouble());
        }

        private static JsValue Random(JsValue thisObject, JsValue[] arguments)
        {
			return (Money)(decimal)_random.NextDouble();
        }

        private static JsValue Round(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
            var round = Money.Round(x);
            if (round.Equals(x - 0.5m))
            {
                return round + 1;
            }

            return round;
        }

        private static JsValue Sin(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
			return (Money)(decimal)System.Math.Sin(x.ToDouble());
        }

        private static JsValue Sqrt(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
			return (Money)(decimal)System.Math.Sqrt(x.ToDouble());
        }

        private static JsValue Tan(JsValue thisObject, JsValue[] arguments)
        {
            var x = TypeConverter.ToNumber(arguments.At(0));
			return (Money)(decimal)System.Math.Tan(x.ToDouble());
        }


    }
}
