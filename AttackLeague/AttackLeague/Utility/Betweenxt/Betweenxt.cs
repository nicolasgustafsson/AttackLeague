using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.Betweenxt
{
    public delegate float BetweenxtingFunction(float aStartKey, float aEndKey, float aCurrent);

    public class Betweenxt
    {
        private float myStartValue;
        private float myEndValue;

        private float myStartKey;
        private float myEndKey;

        private float myCurrentKey = 0.0f;

        private BetweenxtingFunction myBetweenxtingFunction;

        public Betweenxt(BetweenxtingFunction aBetweenxtingFunction, float aStartValue, float aEndValue, float aStartKey = 0.0f, float aEndKey = 1.0f)
        {
            myStartValue = aStartValue;
            myEndValue = aEndValue;

            myStartKey = aStartKey;
            myEndKey = aEndKey;
            Debug.Assert(myStartKey < myEndKey, "Start key must be less than end key!\nStart value on the other hand may be greater than end value.");

            myCurrentKey = myStartKey;

            myBetweenxtingFunction = aBetweenxtingFunction;
        }

        public void Update(float aKeyIncrement)
        {
            myCurrentKey += aKeyIncrement;
            myCurrentKey = Math.Max(Math.Min(myCurrentKey, myEndKey), myStartKey); // clampedy clamp
        }

        public bool IsFinished()
        {
            return (GetProgress() >= 1.0f);
        }

        public float GetValue()
        {
            return GetProgress() * (myEndValue - myStartValue) + myStartValue;
        }

        public float GetValueAtKey(float aKey)
        {
            return GetProgressAtKey(aKey) * (Math.Max(myEndValue, myStartValue) - Math.Min(myEndValue, myStartValue)) + Math.Min(myEndValue, myStartValue);
        }

        public float GetValueAtKeyReverse(float aKey)
        {
            return GetProgressAtKeyReverse(aKey) * (Math.Max(myEndValue, myStartValue) - Math.Min(myEndValue, myStartValue)) + Math.Min(myEndValue, myStartValue);
        }

        public void Reset()
        {
            myCurrentKey = myStartKey;
        }

        public void ResetReverse()
        {
            float tempStart = myStartKey;
            myCurrentKey = myEndKey;
            myStartKey = myEndKey;
            myEndKey = tempStart;
        }

        public float GetEndKey()
        {
            return myEndKey;
        }

        public float GetProgress()
        {
            return Math.Max(Math.Min(AddEpsilon(myBetweenxtingFunction(myStartKey, myEndKey, myCurrentKey)), 1.0f), 0.0f);
        }

        private float AddEpsilon(float aValue)
        {
            return aValue + 0.001f;
        }

        public float GetProgressAtKey(float aKey)
        {
            return Math.Max(Math.Min(myBetweenxtingFunction(myStartKey, myEndKey, aKey), 1.0f), 0.0f);
        }

        public float GetProgressAtKeyReverse(float aKey)
        {
            return Math.Max(Math.Min(myBetweenxtingFunction(myEndKey, myStartKey, aKey), 1.0f), 0.0f);
        }

        public static BetweenxtingFunction Lerp = delegate(float aStartKey, float aEndKey, float aCurrentKey)
        {
            if (aStartKey == aEndKey)
                return 1.0f;


            float total = (Math.Max(aEndKey, aStartKey) - Math.Min(aEndKey, aStartKey));

            if (aCurrentKey >= aEndKey)
                return (aEndKey - aStartKey) / total;

            return (aCurrentKey - aStartKey) / total;
        };

        //Nicos is not sure if dis works but it totally should!!
        public static BetweenxtingFunction Quadratic = delegate (float aStartKey, float aEndKey, float aCurrentKey)
        {
            return (float)Math.Pow(Lerp(aStartKey, aEndKey, aCurrentKey), 2.0f);
        };

        public static BetweenxtingFunction EaseOutQuadratic = delegate (float aStartKey, float aEndKey, float aCurrentKey)
        {
            return (float)(-1 * (Math.Pow(Lerp(aStartKey, aEndKey, aCurrentKey) - 1, 2) - 1));
        };

        /*
        {
            var sign = power % 2 == 0 ? -1 : 1;
            return (float)(sign * (Math.Pow(s - 1, power) + sign)); (-1 * (Math.Pow(step -1, power) -1))
        }
        */

        public static BetweenxtingFunction Quintic = delegate (float aStartKey, float aEndKey, float aCurrentKey)
        {
            return (float)Math.Pow(Lerp(aStartKey, aEndKey, aCurrentKey), 5.0f);
        };
    }
}
