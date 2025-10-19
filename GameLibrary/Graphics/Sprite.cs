using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Graphics;

public class Sprite
{
    #region Properties
    /// <summary>
    /// The texture region of this sprite.
    /// </summary>
    public TextureRegion Region { get; set; }

    /// <summary>
    /// The color mask to apply when rendering this sprite.
    /// </summary>
    /// <remarks>
    /// The default value is Color.White.
    /// </remarks>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    /// The amount of rotaion, in radians.
    /// </summary>
    /// <remarks>
    /// The default value is 0.0f.
    /// </remarks>
    public float Rotation { get; set; } = 0.0f;

    /// <summary>
    /// The scale of this sprite.
    /// </summary>
    /// <remarks>
    /// The default value is Vector2.One.
    /// </remarks>
    public Vector2 Scale { get; set; } = Vector2.One;

    /// <summary>
    /// The xy-coordinate origin point of this sprite.
    /// </summary>
    /// <remarks>
    /// The default value is Vector2.Zero.
    /// </remarks>
    public Vector2 Origin { get; set; } = Vector2.Zero;

    /// <summary>
    /// The sprite effect of this sprite.
    /// </summary>
    /// <remarks>
    /// The default value is SpriteEffects.None.
    /// </remarks>
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    /// <summary>
    /// The layer depth of this sprite.
    /// </summary>
    /// <remarks>
    /// The default value is 0.0f.
    /// </remarks>
    public float LayerDepth { get; set; } = 0.0f;

    /// <summary>
    /// Gets the width, in pixels, of this sprite.
    /// </summary>
    /// <remarks>
    /// Width is calculated by multiplying the width of the source texture refion by the x-axis scale factor.
    /// </remarks>
    public float Width => Region.Width * Scale.X;

    /// <summary>
    /// Gets the height, in pixels, of this sprite.
    /// </summary>
    /// <remarks>
    /// Height is calculated by multiplying the height of the source texture refion by the y-axis scale factor.
    /// </remarks>
    public float Height => Region.Height * Scale.X;
    #endregion

    /// <summary>
    /// Create a new sprite.
    /// </summary>
    public Sprite() { }

    /// <summary>
    /// Create a new sprite using a specified source texture region.
    /// </summary>
    /// <param name="region">The texture region to use as the source texture region of this sprite.</param>
    public Sprite(TextureRegion region)
    {
        Region = region;
    }

    /// <summary>
    /// Sets the origin of this sprite to the center.
    /// </summary>
    public void CenterOrigin()
    {
        const float HALF = 0.5f;
        Origin = new Vector2(Region.Width, Region.Height) * HALF; // divides by 2
    }

    /// <summary>
    /// Submits this sprite for drawing to the current batch.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch instance used for batching draw calls.</param>
    /// <param name="position">The xy-coordinate postiion to render this sprite at.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        Region.Draw(
            spriteBatch,
            position,
            Color,
            Rotation,
            Origin,
            Scale,
            Effects,
            LayerDepth
        );
    }
}

