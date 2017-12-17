using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Feedback
{
    public class FeedbackManager
    {
        static private List<IFeedback> myFeedbackings = new List<IFeedback>();

        static public void AddFeedback(IFeedback aFeedback)
        {
            myFeedbackings.Add(aFeedback);
        }

        static public void Update()
        {
            for(int i = 0; i < myFeedbackings.Count; i++)
            {
                myFeedbackings[i].Update();
                if (myFeedbackings[i].IsDone())
                {
                    myFeedbackings.RemoveAt(i);
                    i--;
                }
            }
        }

        static public void Draw(SpriteBatch aSpriteBatch)
        {
            foreach(IFeedback feedback in myFeedbackings)
            {
                feedback.Draw(aSpriteBatch);
            }
        }
    }
}
