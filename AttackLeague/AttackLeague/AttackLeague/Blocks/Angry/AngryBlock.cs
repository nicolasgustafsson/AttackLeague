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
        private Sprite myTempToDeleteSprite;

        private AngryBlockBundle myBundle;
        private int myLife;
        public bool CanFall { get; private set; }
        protected float myYOffset = 0.0f;

        public AngryBlock(GridBundle aGridBundle, int aLife, AngryBlockBundle aBundle) : base(aGridBundle)
        {
            myLife = aLife;
            myBundle = aBundle;
            myBundle.AddBlock(this);
            myColor = EBlockColor.Blue;
        }

        //public override void LoadContent()
        //{
        //    // load my tilesprite
        //    base.LoadContent();
        //}

        public override void Update(float aGameSpeed)
        {
            base.Update(aGameSpeed);

            CanFall = CheckCanFall();

            myYOffset -= GetMagicalSpeed(aGameSpeed);

            myYOffset = Math.Max(myYOffset, 0.0f);
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
            mySprite.SetPosition(position);
            mySprite.Draw(aSpriteBatch);

            myIcon.SetPosition(position);
            SetIconAnimation();
            myIcon.Draw(aSpriteBatch);
        }

        private bool CheckCanFall()
        {
            if (GetPosition().Y <= 1)
                return false;

            var blockBeneathMe = myGridBundle.Container.GetBlockAtPosition(new Point(GetPosition().X, GetPosition().Y - 1));
            if ((blockBeneathMe is AngryBlock angryBlock && angryBlock.myBundle.Index == myBundle.Index) || blockBeneathMe is EmptyBlock)
                return true;
            return false;
        }

        // on destruction!
        // check all block to my sides
        // I have been checked
        // if block is AngryBlock and myTypeColorRaceThingy  then 
        //      that block is contaminated and should check its neighbors too! unless it has been checked
        // all contaminated blocks should decrease their lives.

    }
}
