using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.Betweenxt
{
    public delegate float BetweenxtingFunction(float aStartKey, float aEndKey, float aCurrent);

    public class Betweenxt
    {
        public Betweenxt(BetweenxtingFunction aBetweenxtingFunction, float aStartValue, float aEndValue, float aStartKey = 0.0f, float aEndKey = 1.0f)
        {
            myStartValue = aStartValue;
            myEndValue = aEndValue;

            myStartKey = aStartKey;
            myEndKey = aEndKey;

            myCurrentKey = myStartKey;

            myBetweenxtingFunction = aBetweenxtingFunction;
        }

        public void Update(float aKeyIncrement)
        {
            myCurrentKey += aKeyIncrement;
        }

        public bool IsFinished()
        {
            return (GetProgress() >= 1.0f);
        }

        public float GetValue()
        {
            return GetProgress() * (myEndValue - myStartValue);
        }

        public float GetProgress()
        {
            return Math.Max(Math.Min(myBetweenxtingFunction(myStartKey, myEndKey, myCurrentKey), 1.0f), 0.0f);
        }

        private readonly float myStartValue;
        private readonly float myEndValue;

        private readonly float myStartKey;
        private readonly float myEndKey;

        private float myCurrentKey = 0.0f;

        private BetweenxtingFunction myBetweenxtingFunction;


        public static BetweenxtingFunction Lerp = delegate(float aStartKey, float aEndKey, float aCurrentKey)
        {
            if (aStartKey == aEndKey)
                return 1.0f;

            float total = aEndKey - aStartKey;

            return (aCurrentKey - aStartKey) / total;
        };

        //Nicos is not sure if dis works but it totally should!!
        public static BetweenxtingFunction Quadratic = delegate (float aStartKey, float aEndKey, float aCurrentKey)
        {
            return (float)Math.Pow(Lerp(aStartKey, aEndKey, aCurrentKey), 2.0f);
        };
    }
}
