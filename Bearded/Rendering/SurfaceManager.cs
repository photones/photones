using System;
using amulware.Graphics;
using amulware.Graphics.ShaderManagement;
using Bearded.Photones.Particles;
using Bearded.Photones.Utilities;

namespace Bearded.Photones.Rendering
{
    class SurfaceManager
    {
        private readonly ShaderManager shaders = new ShaderManager();

        public Matrix4Uniform ViewMatrix { get; } = new Matrix4Uniform("view");
        public Matrix4Uniform ProjectionMatrix { get; } = new Matrix4Uniform("projection");

        public IndexedSurface<UVColorVertexData> ParticleSurface { get; private set; }

        public IndexedSurface<UVColorVertexData> FreshmanFontSurface { get; private set; }
        public IndexedSurface<UVColorVertexData> ConsolasFontSurface { get; private set; }

        public Font FreshmanFont { get; private set; }
        public Font ConsolasFont { get; private set; }

        public SurfaceManager() {
            addShaders();

            createSprites();
            createFonts();
        }

        private void addShaders() {
            shaders.Add(
                ShaderFileLoader.CreateDefault(asset("shaders/")).Load(".")
            );
            new[]
            {
                "geometry", "uvcolor"
            }.ForEach(name => shaders.MakeShaderProgram(name));
        }

        private void createSprites() {
            ParticleSurface = createSpriteSurface("particles/particle.png", Particle.WIDTH, Particle.HEIGHT);
        }

        private void createFonts() {
            FreshmanFont = Font.FromJsonFile(font("freshman_monospaced_numbers.json"));
            FreshmanFontSurface = createFontSurface("freshman.png");

            ConsolasFont = Font.FromJsonFile(font("inconsolata.json"));
            ConsolasFontSurface = createFontSurface("inconsolata.png");
        }

        private IndexedSurface<UVColorVertexData> createSpriteSurface(string spritePath, float w, float h) {
            var t = new Texture(sprite(spritePath));

            return new IndexedSurface<UVColorVertexData>()
                .WithShader(shaders["uvcolor"])
                .AndSettings(
                    ViewMatrix, ProjectionMatrix,
                    new TextureUniform("diffuseTexture", t),
                    SurfaceBlendSetting.Alpha, SurfaceDepthMaskSetting.DontMask
                );
        }

        private IndexedSurface<UVColorVertexData> createFontSurface(string fontPath) {
            return new IndexedSurface<UVColorVertexData>()
                .WithShader(shaders["uvcolor"])
                .AndSettings(
                    ViewMatrix, ProjectionMatrix,
                    new TextureUniform("diffuse", new Texture(font(fontPath), preMultiplyAlpha: true)),
                    SurfaceBlendSetting.Alpha, SurfaceDepthMaskSetting.DontMask
                );
        }

        private static string asset(string path) => "assets/" + path;
        private static string font(string path) => asset("fonts/" + path);
        private static string sprite(string path) => asset("gfx/sprites/" + path);
    }

    static class SurfaceExtensions
    {
        public struct SurfaceWrapper<T>
            where T : Surface
        {
            private readonly T surface;

            public SurfaceWrapper(T surface) {
                this.surface = surface;
            }

            public T AndSettings(params SurfaceSetting[] settings) {
                surface.AddSettings(settings);
                return surface;
            }
        }

        public static SurfaceWrapper<T> WithShader<T>(this T surface, ISurfaceShader shader)
            where T : Surface {
            if (shader == null)
                throw new Exception("Shader not found");

            shader.UseOnSurface(surface);
            return new SurfaceWrapper<T>(surface);
        }
    }
}
