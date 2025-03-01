//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game states for stopped, countdown, paused, and started.
/// </summary>
public class CCDS_GameStates {

    /// <summary>
    /// Game state.
    /// </summary>
    public enum GameState { Stopped, Countdown, Paused, Started }
    public GameState gameState = GameState.Stopped;

}
