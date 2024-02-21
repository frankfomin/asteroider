using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace asteroider
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D backgroundTexture;
        Player player;
        KeyboardState previousKbState;

        List<Shot> shots = new List<Shot>();
        Texture2D laserTexture;

        List<Meteor> meteors = new List<Meteor>();
        Texture2D meteorBigTexture;
        Texture2D meteorMediumTexture;
        Texture2D meteorSmallTexture;
        Random random = new Random();

        SoundEffect laserSound;
        SoundEffect explosionSound;
        Texture2D explosionTexture;
        List<Explosion> explosions = new List<Explosion>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = Globals.ScreenHeight;
            _graphics.PreferredBackBufferWidth = Globals.ScreenWidth;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player(this);
            Components.Add(player);
            ResetMeteors();
            base.Initialize();
        }

        public void ResetMeteors()
        {
            while (meteors.Count < 10)
            {
                var angle = random.Next() * MathHelper.TwoPi;
                var m = new Meteor(MeteorType.Big)
                {
                    Position = new Vector2(Globals.GameArea.Left + (float)random.NextDouble() * Globals.GameArea.Width,
                        Globals.GameArea.Top + (float)random.NextDouble() * Globals.GameArea.Height),
                    Rotation = angle,
                    Speed = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * random.Next(20, 60) / 30.0f
                };

                if (!Globals.RespawnArea.Contains(m.Position))
                    meteors.Add(m);
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("background");
            laserTexture = Content.Load<Texture2D>("laser");
            meteorBigTexture = Content.Load<Texture2D>("meteorBrown_big4");
            meteorMediumTexture = Content.Load<Texture2D>("meteorBrown_med1");
            meteorSmallTexture = Content.Load<Texture2D>("meteorBrown_tiny1");


            laserSound = Content.Load<SoundEffect>("laserSound");
            explosionSound = Content.Load<SoundEffect>("explosionSound");
            explosionTexture = Content.Load<Texture2D>("explosion");



            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Up)) player.Accelerate();

            if (state.IsKeyDown(Keys.Left)) player.Rotation -= 0.05f;
            if(state.IsKeyDown(Keys.Right)) player.Rotation += 0.05f;

            if (state.IsKeyDown(Keys.Space))
            {
                Shot shot = player.Shoot();
                if(shot != null)
                {
                    laserSound.Play();
                    shots.Add(shot);

                }

            }

            foreach (Shot shot in shots)
            {
                shot.Update(gameTime);
                Meteor meteor = meteors.FirstOrDefault(m => m.CollidesWith(shot));

                if (meteor != null)
                {
                    meteors.Remove(meteor);
                    meteors.AddRange(Meteor.BreakMeteor(meteor));
                    explosions.Add(new Explosion()
                    {
                        Position = meteor.Position,
                        Scale = meteor.ExplosionScale
                    });
                    shot.IsDead = true;
                    explosionSound.Play(0.7f, 0f, 0f);
                }
            }

            foreach (Explosion explosion in explosions)
                explosion.Update(gameTime);

            foreach (Meteor metor in meteors)
                metor.Update(gameTime);

            shots.RemoveAll(s => s.IsDead || !Globals.GameArea.Contains(s.Position));
            explosions.RemoveAll(e => e.IsDead);

            player.Update(gameTime);
            previousKbState = state;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            for(int y = 0; y < Globals.ScreenHeight; y += backgroundTexture.Width)
            {
                for(int x = 0; x < Globals.ScreenWidth; x += backgroundTexture.Width)
                {
                    _spriteBatch.Draw(backgroundTexture, new Vector2(x, y), Color.White);
                }
            }
            player.Draw(_spriteBatch);

            foreach (Shot s in shots)
            {
                _spriteBatch.Draw(laserTexture, s.Position, null, Color.White, s.Rotation,
                new Vector2(laserTexture.Width / 2, laserTexture.Height / 2), 1.0f, SpriteEffects.None, 0f);
            }

            foreach (Meteor meteor in meteors)
            {
                Texture2D meteorTexture = meteorSmallTexture;
                switch(meteor.Type)
                {
                    case MeteorType.Big: meteorTexture = meteorBigTexture; break;
                    case MeteorType.Medium: meteorTexture = meteorMediumTexture; break;
                }
                _spriteBatch.Draw(meteorTexture, meteor.Position, null, Color.White, meteor.Rotation,
                new Vector2(meteorTexture.Width / 2, meteorTexture.Height / 2), 1.0f, SpriteEffects.None, 0f);
            }

            foreach (Explosion explosion in explosions)
            {
                _spriteBatch.Draw(explosionTexture, explosion.Position, null, explosion.Color, explosion.Rotation,
                    new Vector2(explosionTexture.Width / 2, explosionTexture.Height / 2), explosion.Scale, SpriteEffects.None, 0f);
            }
            player.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
