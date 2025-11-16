using System;
using GameLibrary.Graphics;

namespace SpaceInvadersClone.GameObjects;

public class Wavey : Enemy
{
    /// <summary>
    /// Create a new Wavey using the specified sprite.
    /// </summary>
    /// <param name="sprite">
    /// The AnimatedSprite to use when drawing the wavey.
    /// </param>
    public Wavey(AnimatedSprite sprite, int layer) : base(sprite, layer)
    {
        Sprite = sprite;
        Layer = layer;
    }

    public override void Initialize(
        int desiredRow,
        int desiredColumn,
        Tilemap tilemap
    )
    {
        int enemyColumn = tilemap.Columns;
        int enemyRow = tilemap.Rows / 7;
        float enemyOffset = MathF.Sqrt(tilemap.TileWidth);

        position = new(
            ((enemyColumn - desiredColumn) * tilemap.TileWidth) + enemyOffset,
            (enemyRow + desiredRow) * tilemap.TileHeight
        );

        Tilemap = tilemap;
        Pace = (int)tilemap.TileWidth / 2;
    }
}
