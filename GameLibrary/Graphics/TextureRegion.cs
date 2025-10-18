using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Graphics;

public class TextureRegion
{
    public Texture2D Texture { get; set; }

    public Rectangle SourceRectangle { get; set; }

    public int Width => SourceRectangle.Width;

    public int Height => SourceRectangle.Height;

    /// <summary>
    /// Creates a new texture region.
    /// </summary>
    public TextureRegion() { }

    /// <summary>
    /// Creates a new texture region.
    /// </summary
    /// <param name="texture">The texture to use as a source texture.</param>
    /// <param name="x">The x-coordinate position</param>
    /// <param name="y">The y-coordinate position</param>
    /// <param name="width">The width of this texture region.</param>
    /// <param name="height">The height of this texture region.</param>
    public TextureRegion(Texture2D texture, int x, int y, int width, int height)
    {
        Texture = texture;
        SourceRectangle = new(x, y, width, height);
    }

    /// <summary>
    /// Submit this texture region for drawing in the current batch.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch instance.</param>
    /// <param name="position">The x and y location to draw this texture.</param>
    /// <param name="color">The color of this texture.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        Draw(
            spriteBatch,
            position,
            color,
            0.0f,
            Vector2.Zero,
            Vector2.One,
            SpriteEffects.None,
            0.0f
        );
    }

    /// <summary>
    /// Submit this texture region for drawing in the current batch.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch instance.</param>
    /// <param name="position">The x and y location to draw this texture.</param>
    /// <param name="color">The color of this texture.</param>
    /// <param name="rotation">The amount of rotation in radians.</param>
    /// <param name="origin">The center of rotation, scaling and position.</param>
    /// <param name="effects">The sprite effect of this texture, flip horizontally, vertically or both.</param>
    /// <param name="layerDepth">The depth of the layer of this drawing.</param>
    public void Draw(
        SpriteBatch spriteBatch,
        Vector2 position,
        Color color,
        float rotation,
        Vector2 origin,
        float scale,
        SpriteEffects effects,
        float layerDepth
    )
    {
        Draw(
            spriteBatch,
            position,
            color,
            rotation,
            origin,
            new Vector2(scale, scale),
            effects,
            layerDepth
        );
    }

    /// <summary>
    /// Submit this texture region for drawing in the current batch.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch instance.</param>
    /// <param name="position">The x and y location to draw this texture.</param>
    /// <param name="color">The color of this texture.</param>
    /// <param name="rotation">The amount of rotation in radians.</param>
    /// <param name="origin">The center of rotation, scaling and position.</param>
    /// <param name="effects">The sprite effect of this texture, flip horizontally, vertically or both.</param>
    /// <param name="layerDepth">The depth of the layer of this drawing.</param>
    public void Draw(
        SpriteBatch spriteBatch,
        Vector2 position,
        Color color,
        float rotation,
        Vector2 origin,
        Vector2 scale,
        SpriteEffects effects,
        float layerDepth
    )
    {
        spriteBatch.Draw(
            Texture,
            position,
            SourceRectangle,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );
    }
}
