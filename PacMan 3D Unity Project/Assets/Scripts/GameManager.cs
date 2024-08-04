using UnityEngine;
using UnityEngine.UI;
using GPC;

using System.Collections;
using System.Collections.Generic;

//[DefaultExecutionOrder(-100)]
public class GameManager : BaseGameManager
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private List<Transform> pellets;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;

    private int ghostMultiplier = 1;

    private void Awake()
    {
		SetTargetState(Game.State.loading);

		/*if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }*/

        Instance = this;
    }

    public override void Loading()
    {
		if (Instance != null)
		{
			DestroyImmediate(gameObject);
		}
		else
		{
			Instance = this;
		}

		SetTargetState(Game.State.loaded);
	}

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

	public override void Loaded()
    {
        OnLoaded.Invoke();
	}

	public override void GameStarting()
    {
		NewGame();
		OnGameStarting.Invoke();
        SetTargetState(Game.State.gameStarted);
	}

	public override void GameStarted() 
    { 
        OnGameStarted.Invoke();
        SetTargetState(Game.State.gameRunning);

	}

    public override void RestartGame()
    {
		SetTargetState(Game.State.gameStarting);
	}

	private void Start()
    {
		//NewGame();
		SetTargetState(Game.State.gameStarting);
	}

    public override void UpdateCurrentState()
    {
        switch (currentGameState)
        {
            case Game.State.gameRunning:
                GameLoop();
                break;
        }
	}

    private void GameLoop()
    {
		if (lives <= 0 && Input.anyKeyDown)
		{
		    SetTargetState(Game.State.restartingGame);
		    //NewGame();
		}
	}

	private void Update()
    {
        UpdateCurrentState();
    }

    public void NewGame()
    {
        //SetScore(0);
        //SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        //gameOverText.enabled = false;

        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        //for (int i = 0; i < ghosts.Length; i++) {
            //ghosts[i].ResetState();
        //}

        pacman.ResetState();
    }

    private void GameOver()
    {
        gameOverText.enabled = true;

        //for (int i = 0; i < ghosts.Length; i++) {
            //ghosts[i].gameObject.SetActive(false);
        //}

        pacman.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();

        SetLives(lives - 1);

        if (lives > 0) {
            Invoke(nameof(ResetState), 3f);
        } else {

			SetTargetState(Game.State.gameEnding);
			//GameOver();
        }
    }

	public override void GameEnding()
    {
		SetTargetState(Game.State.gameEnded);
	}

	public override void GameEnded()
	{
		GameOver();
    }


	public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        //SetScore(score + points);

        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        //SetScore(score + pellet.points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            //Invoke(nameof(NewRound), 3f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf) {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

}
