using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AttackLeague.Utility.GUI
{
    class GUIEvent
    {
        public float ProgressStamp { get; set; }
        public virtual void FireEvent()
        {
        }
    }

    class BaseFrame
    {
        public Betweenxt.Betweenxt Interpolation { get; set; }
        public float ProgressStamp { get; set; }
    }

    class PositionFrame : BaseFrame
    {
        public Vector2 Position { get; set; }
    }

    class ScaleFrame : BaseFrame
    {
        public Vector2 Position { get; set; }
    }

    class RotationFrame : BaseFrame
    {
        public float Rotation { get; set; }
    }

    class AlphaFrame : BaseFrame
    {
        public float Alpha { get; set; }
    }

    class ColorFrame : BaseFrame
    {
        public Vector2 Position { get; set; }
    }

    class GUIAnimation
    {
        protected List<GUIEvent> myGUIEvents;
        protected List<PositionFrame> myPositionFrames;
        protected List<ScaleFrame> myScaleFrames;
        protected List<RotationFrame> mRotationFrames;
        protected List<AlphaFrame> myAlphaFrames;
        protected List<ColorFrame> myColorFrames;
        public Betweenxt.Betweenxt myProgress;

        public void Step(float aStep)
        {
            float previousProgress = myProgress.GetProgress();
            myProgress.Update(aStep);
            float currentProgress = myProgress.GetProgress();
            foreach (var guiEvent in myGUIEvents)
            {
                if (guiEvent.ProgressStamp >= previousProgress && guiEvent.ProgressStamp < currentProgress)
                    guiEvent.FireEvent();
            }
        }
    }
}
