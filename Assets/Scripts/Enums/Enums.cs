

namespace Enums
{
    public enum Orientation
    {
        east,
        west,
        south,
        north,
        none
    }
    public enum GameState
    {
        gameStarted,
        playingLevel,
        engagingEnemies,
        bossStage,
        engagingBoss,
        levelCompleted,
        gameWon,
        gameLost,
        gamePaused,
        dungeonOverviewMap,
        restartGame
    }
}