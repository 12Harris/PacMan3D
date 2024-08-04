using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPC
{
	public class Game
	{
		public enum State { idle, loading, loaded, gameStarting, gameStarted, gameRunning,  levelStarting, levelStarted, levelEnding, levelEnded, gameEnding, gameEnded, gamePausing, gameUnPausing, showingLevelResults, showingGameResults, restartingLevel, restartingGame };
	}
}