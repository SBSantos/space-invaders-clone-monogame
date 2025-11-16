using GameLibrary.Graphics;

namespace SpaceInvadersClone.GameObjects;

public class BigCrimson : Enemy
{
    /// <summary>
    /// Create a new Big Crimson using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the big crimson.
    /// </param>
    public BigCrimson(AnimatedSprite sprite, int layer) : base(sprite, layer)
    {
        Sprite = sprite;
        Layer = layer;
    }
}
