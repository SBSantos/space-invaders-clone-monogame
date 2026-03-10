using Microsoft.Xna.Framework;

namespace SpaceInvadersClone.Entities;

public class Entity
{
    public Vector2 Position;

    public bool IsActive;

    public void Remove()
    {
        IsActive = false;
    }
}