using System;
using System.Collections.Generic;
using System.Linq;
using amulware.Graphics;
using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Bearded.Photones.GameUI
{
    class Beardgame
    {
        private GameState _gameState;

        public Beardgame() {
            _gameState = new GameState(new Planet[0]);
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
        }
    }
}
