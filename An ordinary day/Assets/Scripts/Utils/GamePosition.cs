﻿using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class GamePosition
{
	public string ScenePath;
	public Vector2 Position;


    public GamePosition() { }


    public GamePosition(string scenePath, Vector2 position)
    {
        ScenePath = scenePath;
        Position = position;
    }


    public GamePosition(GamePosition other)
    {
        ScenePath = other.ScenePath;
        Position = new Vector2(other.Position.x, other.Position.y);
    }

    public GamePosition(SpawnData other)
    {
        ScenePath = other.ScenePath;
        Position = new Vector2(other.Position.x, other.Position.y);
    }

    public GamePosition(GamePositionReference other)
    {
        ScenePath = other.Scene.Path;
        Position = new Vector2(other.Position.x, other.Position.y);
    }


    public bool IsInCurrentScene() => SceneManager.GetActiveScene().path.Equals(ScenePath);
}
