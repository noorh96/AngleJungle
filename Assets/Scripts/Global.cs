using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global
{
	public static bool isDragging=false;
	public static bool isPaused=false;
	public static int antiCheater=3;

	// Layer strings
	public static string LAYER_POWER_GEM = "PowerGem";

	// Tag strings
	public static string TAG_MIRROR_COLLIDER = "MirrorCollider";
	public static string TAG_DRAGGABLE = "Draggable";

	// Scene strings
	public static string SCENE_STAGE_NEW = "StageNew";
	public static string SCENE_TREASURE = "Treasure";
}
