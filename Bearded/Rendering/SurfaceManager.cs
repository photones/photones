using System;
using amulware.Graphics;
using amulware.Graphics.ShaderManagement;
using Bearded.Photones.Particles;
using Bearded.Photones.Utilities;

namespace Bearded.Photones.Rendering {
    class SurfaceManager {
        private readonly ShaderManager _shaders = new ShaderManager();

        public Matrix4Uniform ViewMatrix { get; } = new Matrix4Uniform("view");
        public Matrix4Uniform ProjectionMatrix { get; } = new Matrix4Uniform("projection");

        public IndexedSurface<UVColorVertexData> SpriteSurface { get; private set; }
        public ExpandingVertexSurface<PhotonVertexData> PhotonSurface { get; private set; }

        public IndexedSurface<UVColorVertexData> FreshmanFontSurface { get; private set; }
        public IndexedSurface<UVColorVertexData> ConsolasFontSurface { get; private set; }

        public Font FreshmanFont { get; private set; }
        public Font ConsolasFont { get; private set; }

        public SurfaceManager() {
            addShaders();
            createSurfaces();
        }

        private void addShaders() {
            _shaders.Add(
                ShaderFileLoader.CreateDefault(asset("shaders/")).Load(".")
            );
            new[] {
                "uvcolor", "photon"
            }.ForEach(name => _shaders.MakeShaderProgram(name));
        }

        private void createSurfaces() {
            SpriteSurface = createSpriteSurface("particles/particle.png",
                Particle.WIDTH,
                Particle.HEIGHT);
            PhotonSurface = createPhotonSurface();

            FreshmanFont = Font.FromJsonFile(font("freshman_monospaced_numbers.json"));
            FreshmanFontSurface = createFontSurface("freshman.png");
            ConsolasFont = Font.FromJsonFile(font("inconsolata.json"));
            ConsolasFontSurface = createFontSurface("inconsolata.png");
        }

        private ExpandingVertexSurface<PhotonVertexData> createPhotonSurface() {
            return new ExpandingVertexSurface<PhotonVertexData>(
                    OpenTK.Graphics.OpenGL.PrimitiveType.Points)
                .WithShader(_shaders["photon"])
                .AndSettings(
                    ViewMatrix, ProjectionMatrix,
                    SurfaceBlendSetting.Add,
                    SurfaceDepthMaskSetting.DontMask
                );
        }

        private IndexedSurface<UVColorVertexData> createSpriteSurface(string spritePath, float w,
                float h) {
            var t = new Texture(sprite(spritePath));
            if (t.Width != w || t.Height != h) {
                throw new ArgumentException($"Sprite size is incorrect ({spritePath}).");
            }

            return new IndexedSurface<UVColorVertexData>()
                .WithShader(_shaders["uvcolor"])
                .AndSettings(
                    ViewMatrix,
                    ProjectionMatrix,
                    new TextureUniform("diffuseTexture", t),
                    SurfaceBlendSetting.Alpha, SurfaceDepthMaskSetting.DontMask
                );
        }

        private IndexedSurface<UVColorVertexData> createFontSurface(string fontPath) {
            return new IndexedSurface<UVColorVertexData>()
                .WithShader(_shaders["uvcolor"])
                .AndSettings(
                    ViewMatrix,
                    ProjectionMatrix,
                    new TextureUniform("diffuse", new Texture(font(fontPath), true)),
                    SurfaceBlendSetting.Alpha, SurfaceDepthMaskSetting.DontMask
                );
        }

        private static string asset(string path) => "assets/" + path;
        private static string font(string path) => asset("fonts/" + path);
        private static string sprite(string path) => asset("gfx/sprites/" + path);
    }

    static class SurfaceExtensions {
        public struct SurfaceWrapper<T> where T : Surface {
            private readonly T _surface;

            public SurfaceWrapper(T surface) {
                _surface = surface;
            }

            public T AndSettings(params SurfaceSetting[] settings) {
                _surface.AddSettings(settings);
                return _surface;
            }
        }

        public static SurfaceWrapper<T> WithShader<T>(this T surface, ISurfaceShader shader)
                where T : Surface {
            if (shader == null) {
                throw new Exception("Shader not found");
            }

            shader.UseOnSurface(surface);
            return new SurfaceWrapper<T>(surface);
        }
    }
}
