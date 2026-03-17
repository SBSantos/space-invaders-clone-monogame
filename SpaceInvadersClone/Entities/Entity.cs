using Microsoft.Xna.Framework;

namespace SpaceInvadersClone.Entities;

public class Entity
{
    public Vector2 Position;

    /// <summary>
    /// Define whether an entity is active or not.
    /// The value is true by default.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Deactivate an entity by setting IsActive to false.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }
}