using amulware.Graphics;
using Bearded.Photones.Particles;
using OpenTK;

namespace Bearded.Photones.Rendering
{
    class GeometryManager
    {
        private readonly SurfaceManager surfaces;

        public Sprite2DGeometry ParticleGeometry { get; private set; }

        public FontGeometry FreshmanFont { get; private set; }
        public FontGeometry ConsolasFont { get; private set; }

        public GeometryManager(SurfaceManager surfaces) {
            this.surfaces = surfaces;

            createSprites();
            createFonts();
        }

        private void createSprites() {
            ParticleGeometry = createSpriteGeometry(surfaces.ParticleSurface, Particle.WIDTH, Particle.HEIGHT);
        }

        private void createFonts() {
            FreshmanFont = new FontGeometry(surfaces.FreshmanFontSurface, surfaces.FreshmanFont);
            ConsolasFont = new FontGeometry(surfaces.ConsolasFontSurface, surfaces.ConsolasFont);
        }

        private Sprite2DGeometry createSpriteGeometry(IndexedSurface<UVColorVertexData> surface, float w, float h) {
            var geo = new Sprite2DGeometry(surface);
            geo.Size = new Vector2(w, h);

            return geo;
        }
    }
}
