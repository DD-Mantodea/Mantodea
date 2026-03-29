using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mantodea.Contents.Graphics;

namespace Mantodea.Contents.Extensions
{
    public static class SpriteBatchExt
    {
        private static Texture2D _pixel;
        public static Texture2D Pixel
        {
            get
            {
                if (_pixel == null)
                {
                    _pixel = new Texture2D(Core.Graphics?.GraphicsDevice, 1, 1);

                    _pixel.SetData([Color.White]);
                }

                return _pixel;
            }
        }

        public static void Rebegin(this SpriteBatch spriteBatch, SpriteSortMode? sortMode = null, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            var state = spriteBatch.SaveState();

            spriteBatch.End();

            spriteBatch.Begin(
                sortMode ?? state.SpriteSortMode,
                blendState ?? state.BlendState,
                samplerState ?? state.SamplerState,
                depthStencilState ?? state.DepthStencilState,
                rasterizerState ?? state.RasterizerState,
                effect ?? state.Effect,
                transformMatrix ?? state.SpriteEffect.TransformMatrix
            );
        }

        public static void EnableScissor(this SpriteBatch spriteBatch)
        {
            var type = spriteBatch.GetType();

            var rState = (RasterizerState)type.GetField("rasterizerState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);
            
            spriteBatch.Change(rasterizerState: new() { ScissorTestEnable = true, CullMode = rState.CullMode });
        }

        public static void Change(this SpriteBatch spriteBatch, SpriteSortMode? sortMode = null, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            var type = spriteBatch.GetType();

            var sMode = sortMode ?? (SpriteSortMode)type.GetField("sortMode", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            var bState = blendState ?? (BlendState)type.GetField("blendState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            var sState = samplerState ?? (SamplerState)type.GetField("samplerState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            var dsState = depthStencilState ?? (DepthStencilState)type.GetField("depthStencilState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            var rState = rasterizerState ?? (RasterizerState)type.GetField("rasterizerState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            var efct = effect ?? (Effect)type.GetField("_effect", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            var sprEffect = (SpriteEffect)type.GetField("_spriteEffect", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            var matrix = transformMatrix ?? sprEffect.TransformMatrix;

            spriteBatch.Rebegin(sMode, bState, sState, dsState, rState, efct, matrix);
        }

        public static SpriteBatchState SaveState(this SpriteBatch spriteBatch)
        {
            var state = new SpriteBatchState();

            var type = spriteBatch.GetType();

            state.SpriteSortMode = (SpriteSortMode)type.GetField("_sortMode", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            state.BlendState = (BlendState)type.GetField("_blendState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            state.SamplerState = (SamplerState)type.GetField("_samplerState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            state.DepthStencilState = (DepthStencilState)type.GetField("_depthStencilState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            state.RasterizerState = (RasterizerState)type.GetField("_rasterizerState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            state.Effect = (Effect)type.GetField("_effect", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            state.SpriteEffect = (SpriteEffect)type.GetField("_spriteEffect", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);

            return state;
        }

        public static void LoadState(this SpriteBatch spriteBatch, SpriteBatchState state)
        {
            spriteBatch.Rebegin(state.SpriteSortMode, state.BlendState, state.SamplerState, state.DepthStencilState, state.RasterizerState, state.Effect, state.SpriteEffect.TransformMatrix);
        }

        public static void DrawLine(this SpriteBatch batch, Line line, Color color)
        {
            float radian = line.ToVector2().GetRadian();
            if (Pixel is not null)
                batch.Draw(Pixel, line.Start, null, color, radian, Vector2.Zero, new Vector2(Vector2.Distance(line.Start, line.End), 1f), SpriteEffects.None, 0);
        }

        public static void DrawLine(this SpriteBatch batch, Vector2 start, Vector2 end, Color color)
        {
            batch.DrawLine(new Line(start, end), color);
        }

        public static void DrawRectangle(this SpriteBatch batch, Rectangle rect, Color color)
        {
            batch.Draw(Pixel, rect, color);
        }

        public static void DrawRectangle(this SpriteBatch batch, Rectangle rect, Color color, float rotation = 0f, Vector2 origin = default, SpriteEffects effects = SpriteEffects.None, float layerDepth = 1)
        {
            batch.Draw(Pixel, rect, null, color, rotation, origin, effects, layerDepth);
        }

        public static void Draw(this SpriteBatch batch, TextureRegion region, Vector2 position, Color color)
        {
            region.Draw(batch, position, color);
        }

        public static void Draw(this SpriteBatch batch, TextureRegion region, Vector2 position, Color color, float rotation = 0f, Vector2 origin = default, Vector2 scale = default, SpriteEffects effects = SpriteEffects.None, float layerDepth = 1)
        {
            region.Draw(batch, position, color, rotation, origin, scale == default ? Vector2.One : scale, effects, layerDepth);
        }
    }

    public struct Line
    {
        public Vector2 Start;
        public Vector2 End;
        public Line(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }
        public Vector2 ToVector2()
        {
            return End - Start;
        }
    }

    public class SpriteBatchState
    {
        public SpriteBatchState() { }

        public SpriteSortMode SpriteSortMode { get; set; }
        public BlendState BlendState { get; set; }
        public SamplerState SamplerState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public RasterizerState RasterizerState { get; set; }

        public Effect Effect { get; set; }
        public SpriteEffect SpriteEffect { get; set; }

        public void Begin(SpriteBatch spriteBatch, SpriteSortMode spriteSortMode, Effect effect = null, Matrix? matrix = null)
        {
            spriteBatch.Begin(spriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, effect, matrix ?? SpriteEffect.TransformMatrix);
        }
    }
}
