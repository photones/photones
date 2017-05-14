using System;
using amulware.Graphics;
using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Bearded.Photones.GameUI
{
    class Beardgame
    {
        const float PLANET_SIZE = 0.25f;

        private GameState _gameState;

        public Beardgame() {
            _gameState = GameStateFactory.BuildDummyGameState();
        }

        public void Update(TimeSpan elapsedTime) {
            _gameState.Update(elapsedTime);
        }

        public void Draw(GeometryManager geometries) {
            var txtGeo = geometries.FreshmanFont;

            txtGeo.Color = Color.White;
            txtGeo.SizeCoefficient = Vector2.One;
            txtGeo.Height = 48;
            txtGeo.DrawString(0.5f * new Vector2(PhotonesProgram.WIDTH, PhotonesProgram.HEIGHT), "Hello world!", 0.5f, 0.5f);

            foreach (var planet in _gameState.Planets) {
                geometries.ParticleGeometry.Size = new Vector2(PLANET_SIZE);
                geometries.ParticleGeometry.DrawSprite(planet.Position.NumericValue);
            }
        }
    }
}
