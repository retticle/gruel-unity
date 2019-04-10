using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FlipbookData")]
public class FlipbookData : ScriptableObject {

	public bool _loop = true;
	public Material _material;
	
	public int _framesPerSecond = 30;
	public Sprite[] _keyFrames = new Sprite[0];

}
