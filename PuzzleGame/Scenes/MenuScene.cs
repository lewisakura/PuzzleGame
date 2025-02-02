﻿using System;
using Microsoft.Xna.Framework;
using PuzzleGame.UI;

namespace PuzzleGame.Scenes;

/// <summary>
/// The main menu scene.
/// </summary>
public class MenuScene() : Scene("Menu")
{
    public override void Init()
    {
        // i'm aware this is very awful, but i cannot think of a better way of registering UI components right now, so too bad!
        AddUIComponent(new Text(new Point(PuzzleGame.Resolution / 2, 50), "PuzzleMaster(tm)"));
        AddUIComponent(new Button(new Rectangle(PuzzleGame.Resolution / 4, PuzzleGame.Resolution / 3, PuzzleGame.Resolution / 2, 50), "Play", () =>
        {
            Console.WriteLine("click :D");
            SceneManager.SwitchScene<PuzzleScene>();
        }));
        AddUIComponent(new Button(new Rectangle(PuzzleGame.Resolution / 4, PuzzleGame.Resolution / 2, PuzzleGame.Resolution / 2, 50), "Quit",
            () =>
            {
                Environment.Exit(0);
            }));
    }
}