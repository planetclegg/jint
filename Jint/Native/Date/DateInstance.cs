using System;
using Jint.Native.Object;
using Jint.Runtime;

namespace Jint.Native.Date
{
    public class DateInstance : ObjectInstance
    {
        // Maximum allowed value to prevent DateTime overflow
		internal static readonly Money Max = (decimal)(DateTime.MaxValue - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

        // Minimum allowed value to prevent DateTime overflow
		internal static readonly Money Min = (decimal)-(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) - DateTime.MinValue).TotalMilliseconds;

        public DateInstance(Engine engine)
            : base(engine)
        {
        }

        public override string Class
        {
            get
            {
                return "Date";
            }
        }

        public DateTime ToDateTime()
        {
            if (Money.IsNaN(PrimitiveValue) || PrimitiveValue > Max || PrimitiveValue < Min)
            {
                throw new JavaScriptException(Engine.RangeError);
            }
            else
            {
				return DateConstructor.Epoch.AddMilliseconds(PrimitiveValue.ToDouble());
            }
        }

        public Money PrimitiveValue { get; set; }
    }
}
