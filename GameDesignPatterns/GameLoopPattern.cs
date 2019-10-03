using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/*
 * A game loop runs continuously during gameplay.
 * Each turn of the loop, it processes user input without blocking, updates the game state, and renders the game.
 * It tracks the passage of time to control the rate of gameplay.
 */
namespace HappyUnity.GamePatterns
{
    public class GameLoopPattern : EverlastingSingleton<GameLoopPattern>
    {
        public enum GameState
        {
            Pause,
            Playing,
            End,
            Quit
        }

        public void SetGameState(GameState state)
        {
            switch (state)
            {
                case GameState.Quit:
                    Application.Quit();
                    break;
                case GameState.Pause:
                    // Pause set
                    break;
                case GameState.Playing:
                    // Pause unset
                    break;
                case GameState.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            CurrentGameState = state;
        }

        bool requestTitleScreen = true;
        public GameState CurrentGameState;

        IEnumerator Background_Game_Workflow()
        {
            while (true)
            {
                if (requestTitleScreen)
                {
                    requestTitleScreen = false;
                    yield return StartCoroutine(ShowingPreGameTitle());
                }

                yield return StartCoroutine(StartLevel());
                yield return StartCoroutine(PlayLevel());
                yield return StartCoroutine(EndLevel());
                GC.Collect();
            }
        }

        IEnumerator ShowingPreGameTitle()
        {
            while (CurrentGameState != GameState.Playing) yield return null;
        }

        IEnumerator StartLevel()
        {
            yield return new WaitForSeconds(2);
            // Executing some functions that start our level
        }

        IEnumerator PlayLevel()
        {
            SetGameState(GameState.Playing);
            while (CurrentGameState == GameState.Playing)
            {
                yield return null;
            }

            DefineNewGame();
        }

        IEnumerator EndLevel()
        {
            SetGameState(GameState.End);
            yield return new WaitForSeconds(2);
        }

        void DefineNewGame()
        {
            requestTitleScreen = true;
        }
    }
}