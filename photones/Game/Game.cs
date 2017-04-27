using System;
using System.Collections.Generic;
using System.Linq;
using amulware.Graphics;
using Bearded.Photones.Game.GameObjects;
using Bearded.Photones.Rendering;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Bearded.Photones.Game
{
    class Game
    {
        public Game() { }

        public void Update(TimeSpan elapsedTime) { }

        public void Draw(GeometryManager geometries) {
            var txtGeo = geometries.FreshmanFont;

            txtGeo.Color = Color.White;
            txtGeo.SizeCoefficient = Vector2.One;
            txtGeo.Height = 48;
            txtGeo.DrawString(0.5f * new Vector2(PhotonesProgram.WIDTH, PhotonesProgram.HEIGHT), "Hello world!", 0.5f, 0.5f);
        }
    }
}
