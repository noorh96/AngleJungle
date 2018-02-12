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
	public const string TAG_DRAGGABLE = "Draggable";
	public const string TAG_GO_BUTTON = "GoButton";
	public const string TAG_HAND_TUTORIAL = "HandClickTutorial";
	public const string TAG_MIRROR_COLLIDER = "MirrorCollider";
	public const string TAG_MIRROR_RECEIVER = "MirrorReceiver";
	public const string TAG_MUSIC_MANAGER = "MusicManager";
	public const string TAG_PLAYER = "Player";
	public const string TAG_POWER_GEM = "PowerGem";
	public const string TAG_PROTRACTOR = "Protractor";

	// Scene strings
	public const string SCENE_MAP = "StageNew";
	public const string SCENE_TREASURE = "Treasure";
	public const string SCENE_START = "Start";

    // Animation strings
    public const string ANIMATION_HAPPY = "Happy";
    public const string ANIMATION_WALK = "Walk";
    public const string ANIMATION_IDLE = "Idle";
    public const string ANIMATION_CELEBRATE = "Cel";

	// Font strings
	public const string FONT_ASAP_MEDIUM = "fonts/Asap-Medium";

	// Analytics event strings
	public const string ANALYTICS_ACTION_REMOVED = "action_removed";
	public const string ANALYTICS_ACTION_PLACED = "action_placed";
	public const string ANALYTICS_PROTRACTOR_OPENED = "protractor_opened";
    public const string ANALYTICS_PROTRACTOR_CLOSED = "protractor_closed";
	public const string ANALYTICS_LEVEL_ENDED = "levelEnded";
	public const string ANALYTICS_LEVEL_TIME = "levelTime";
    public const string ANALYTICS_ACTIONS = "Actions";

	public enum Interface {Touch = 0, Mouse = 1};
}
