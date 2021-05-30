using UnityEngine;


/// <summary>
/// THIS GAME CONTROLL MAKE THE GAME START, PAUSE AND STOP
/// </summary>
public class GameController
{
	//  Events ----------------------------------------


	//  Properties ------------------------------------
	public static GameController Instance
    {
		get
        {
			if (_instance == null) _instance = new GameController();
			return _instance;
        }
    }
	public bool IsPlaying { get => _isPlaying; }


	//  Fields ----------------------------------------
	private static GameController _instance = null;
	private bool _isPlaying;


	//  Initialization --------------------------------


	//  Methods ---------------------------------------
	public void Play() { _isPlaying = true; }
	public void Pause() { _isPlaying = false; }

	//  Event Handlers --------------------------------
}
