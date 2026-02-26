using GameLibrary;
using SpaceInvadersClone.Scenes;

namespace SpaceInvadersClone;

public class GameLoop : Core
{
    public GameLoop() : base(
        title: "Space Invaders (My ver.)",
        width: 640,
        height: 480,
        fullscreen: false
    )
    { }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        // Start the game with the title scene.
        ChangeScene(new TitleScene());
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
    }
}
