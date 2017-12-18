using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global
{
	public static bool isDragging=false;
	public static bool isPaused=false;
	public static int antiCheater=3;

	// Layer strings
	public const string LAYER_POWER_GEM = "PowerGem";

	// Tag strings
	public const string TAG_MIRROR_COLLIDER = "MirrorCollider";
	public const string TAG_DRAGGABLE = "Draggable";
	public const string TAG_HAND_TUTORIAL = "HandClickTutorial";
	public const string TAG_PROTRACTOR = "Protractor";
	public const string TAG_MUSIC_MANAGER = "MusicManager";
	public const string TAG_GO_BUTTON = "GoButton";
    public const string TAG_PLAYER = "Player";

	// Scene strings
	public const string SCENE_MAP = "StageNew";
	public const string SCENE_TREASURE = "Treasure";

    // Animation strings
    public const string ANIMATION_HAPPY = "Happy";
    public const string ANIMATION_WALK = "Walk";
    public const string ANIMATION_IDLE = "Idle";
    public const string ANIMATION_CELEBRATE = "Cel";
}
