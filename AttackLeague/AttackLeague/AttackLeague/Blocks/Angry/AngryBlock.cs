using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.AttackLeague.Grid;
using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AttackLeague.AttackLeague.Blocks.Angry;

namespace AttackLeague.AttackLeague.Blocks
{
    public class AngryBlock : AbstractColorBlock
    {
        // essentially how high up this block is in the whole angry block. At 0 we convert this block to a ColorBlock!

        // Kind of a index which is unique for each angry bundle. We need something to denote which angry blocks belong together. 
        //Could/Should increase for each new angry block bundle. 
        // We probably just want to tell block generator "CreateAngryBlock(aWidth, aHeight) or CreateAngryBlock(aTetrisPattern)"
        // and then BlockGenerator send in an increased index for every time it creates a new Angry block bundle

        private Sprite myTileSetSprite;
        private Sprite myFrozenSprite;

        private AngryBlockBundle myBundle;
        private int myLife;
        protected float myYOffset = 0.0f;

        bool myIsFriendAware = false;
        bool myCanFall = false;
        bool myIsFrozen = false;
        bool myIsContaminated = false;

        public AngryBlock(GridBundle aGridBundle, int aLife, AngryBlockBundle aBundle) : base(aGridBundle)
        {
            myLife = aLife;
            myBundle = aBundle;
            myBundle.AddBlock(this);
            myColor = EBlockColor.Blue;
        }

        public override void LoadContent()
        {
            // load my tilesprite
            base.LoadContent();

            myFrozenSprite = new Sprite("tiley");
        }

        public override void Update(float aGameSpeed)
        {
            base.Update(aGameSpeed);

            myYOffset -= GetMagicalSpeed(aGameSpeed);

            myYOffset = Math.Max(myYOffset, 0.0f);

            if (myCanFall)
            {
                Fall(aGameSpeed);
            }
        }

        public void Freeze()
        {
            myIsFrozen = true;
        }

        public void UnFreeze()
        {
            myIsFrozen = false;
        }

        public void Fall(float aGameSpeed)
        {
            if (myYOffset < 0f)
                myYOffset = 0f;
            Point position = GetPosition();

            if (WillPassTile(aGameSpeed)) 
            {
                PassTile();
                position = GetPosition();
                myGridBundle.Container.InitializeBlock(position + new Point(0, 1), new EmptyBlock(myGridBundle));

                myGridBundle.Container.SetBlock(position, this);
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

            if (myIsFrozen)
            {
                myFrozenSprite.SetPosition(position);
                myFrozenSprite.Draw(aSpriteBatch);
            }
            else
            {
                mySprite.SetPosition(position);
                mySprite.Draw(aSpriteBatch);
            }

            myIcon.SetPosition(position);
            SetIconAnimation();
            myIcon.Draw(aSpriteBatch);
        }

        public void SetFriendlyAwareness(bool aValue)
        {
            myIsFriendAware = aValue;
        }

        public void SetCanFall(bool aValue)
        {
            myCanFall = aValue;
        }

        public override bool AllowsFalling()
        {
            if (myIsFriendAware)
                return myCanFall;
            else
                return CheckCanFall();
        }

        public int GetIndex()
        {
            return myBundle.Index;
        }

        public bool CheckCanFall()
        {
            if (myIsFrozen)
                return false;
            Rectangle rectangleCopy = GetRectangle();
            if (rectangleCopy.Y != 0)
            {
                rectangleCopy.Y--;
                if (myGridBundle.Behavior.RectangleIntersectsForFallingPurposes(rectangleCopy) == true)
                    return false;

                return true;
            }
            return false;
        }

        public virtual void Contaminate()
        {
            if (myIsContaminated)
                return;

            Freeze();

            myBundle.OnHitByMatch();

            myIsContaminated = true;
            var blocks = myGridBundle.Container.GetAdjacentBlocks(GetPosition());   
            foreach (AbstractBlock block in blocks)
            {
                if (block is AngryBlock angryBlock) // iron block needs them to be of its index as well!
                {
                    angryBlock.Contaminate();
                }
            }
        }

        public bool DiedFromContamination()
        {
            if (myIsContaminated)
            {
                //change sprite to single AngryTile
                myLife--;
            }
            myIsContaminated = false;

            return myLife <= 0;
        }

        // on destruction!
        // check all block to my sides
        // I have been checked
        // if block is AngryBlock and myTypeColorRaceThingy  then 
        //      that block is contaminated and should check its neighbors too! unless it has been checked
        // all contaminated blocks should decrease their lives.

    }
}
