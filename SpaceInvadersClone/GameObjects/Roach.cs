using Microsoft.Xna.Framework;
using GameLibrary;
using GameLibrary.Graphics;

namespace SpaceInvadersClone.GameObjects;

public class Roach : Enemy
{
    /// <summary>
    /// Create a new Roach using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the roach.
    /// </param>
    public Roach(AnimatedSprite sprite) : base(sprite)
    {
        Sprite = sprite;
    }

    public override void Initialize(Vector2 startingPosition, Tilemap tilemap)
    {
        position = startingPosition;
        Tilemap = tilemap;
        Pace = (int)tilemap.TileWidth / 2;
    }

    public override void Update(GameTime gameTime)
    {
        Time += gameTime.ElapsedGameTime;

        if (Time >= Threshold)
        {
            // Update the animation
            Sprite.Update();

            // Will move the enemies backward or forward
            if (IsMovingBackward) { position.X -= Pace; }
            else { position.X += Pace; }

            // Always reset the time to zero
            Time -= Threshold;
        }
    }

    public override void Draw()
    {
        Sprite.Draw(Core.SpriteBatch, position);
    }
}
