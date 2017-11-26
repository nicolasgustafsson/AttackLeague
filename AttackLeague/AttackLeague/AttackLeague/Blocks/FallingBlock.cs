using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AttackLeague.AttackLeague.Grid;

namespace AttackLeague.AttackLeague
{

    class FallingBlock : AbstractColorBlock
    {
        private const float MyBaseSpeed = 0.15f;
        private const float MyAdditionalSpeed = 0.1f;
        private bool myCanChain = false;
        protected float myYOffset = 0.0f;

        public FallingBlock(GridBundle aGridBundle, EBlockColor aColor)
            :base(aGridBundle)
        {
            myColor = aColor;
        }

        public FallingBlock(GridBundle aGridBundle, ColorBlock aBlock)
            :base(aGridBundle)
        {
            myColor = aBlock.GetColor();
            myCanChain = aBlock.CanChain;
        }

        public override void Update(float aGameSpeed)
        {
            base.Update(aGameSpeed);
            myYOffset -= GetMagicalSpeed(aGameSpeed);

            Point position = GetPosition();

            //Speed, might pass through many tiles?
            if (WillPassTile(aGameSpeed))
            {
                if (position.Y == 0 ||
                    myGridBundle.Container.myGrid[position.Y - 1][position.X].IsEmpty() == false)
                {
                    myGridBundle.Container.InitializeBlock(position, new ColorBlock(myGridBundle, this)); // MOVE TO GENERATOR I GUESS, or blocks who tell GENERATOR
                }
                else
                {
                    PassTile();
                    //passtile changes position, update
                    position = GetPosition();
                    myGridBundle.Container.InitializeBlock(position + new Point(0, 1), new EmptyBlock(myGridBundle));

                    myGridBundle.Container.SetBlock(position, this);
                }
            }

        }

        public bool WillPassTile(float aGameSpeed)
        {
            return (myYOffset - GetMagicalSpeed(aGameSpeed)) < 0.0f;
        }

        public void PassTile()
        {
            myGridArea.Y--;
            myYOffset += 1.0f;
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
            Vector2 position = GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset);
            position.Y -= myYOffset * mySprite.GetSize().Y;
            mySprite.SetPosition(position);
            mySprite.Draw(aSpriteBatch);

            myIcon.SetPosition(position);
            SetIconAnimation();
            myIcon.Draw(aSpriteBatch);
        }

        public bool CanChain
        {
            get { return myCanChain; }
            set { myCanChain = value; }
        }
    }
}
