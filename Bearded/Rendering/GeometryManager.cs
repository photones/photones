using amulware.Graphics;
using Bearded.Photones.Particles;
using OpenTK;

namespace Bearded.Photones.Rendering
{
    class GeometryManager
    {
        private readonly SurfaceManager surfaces;

        public Sprite2DGeometry SpriteGeometry { get; private set; }
        public Photon2DGeometry PhotonGeometry { get; private set; }

        public FontGeometry FreshmanFont { get; private set; }
        public FontGeometry ConsolasFont { get; private set; }

        public GeometryManager(SurfaceManager surfaces) {
            this.surfaces = surfaces;

            createSprites();
            createFonts();
        }

        private void createSprites() {
            SpriteGeometry = createSpriteGeometry(surfaces.SpriteSurface, Particle.WIDTH, Particle.HEIGHT);
            PhotonGeometry = createParticleGeometry(surfaces.PhotonSurface);
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

        private Photon2DGeometry createParticleGeometry(ExpandingVertexSurface<PhotonVertexData> surface) {
            return new Photon2DGeometry(surface);
        }
    }
}
