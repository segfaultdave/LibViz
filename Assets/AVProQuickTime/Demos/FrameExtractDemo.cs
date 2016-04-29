using UnityEngine;
using System.Collections;
using System.IO;

public class FrameExtractDemo : MonoBehaviour 
{	
	public string _folder;
	public string _filename;
	public GUISkin _guiSkin;
	private static GUIStyle _gridStyle;
	
	private AVProQuickTime _movie;
	private GUIContent[] _contents;
	private Texture2D[] _textures;
	private bool _isExtracting;
	private int _textureIndex;
	private uint _targetFrame;
	private uint _frameStep;
	
	private void DestroyTextures()
	{
		if (_textures != null)
		{
			for (int i = 0; i < _textures.Length; i++)
			{
				if (_textures[i])
				{
					Texture2D.Destroy(_textures[i]);
					_textures[i] = null;
				}
			}
		}
	}
	
	private bool StartExtractFrames(string filePath, uint numSamples)
	{
		DestroyTextures();

		_textureIndex = 0;
		_targetFrame = 0;
		_frameStep = 0;
		
		if (_movie.StartFromFile(filePath, false, true, false, false))
		{
			_textures = new Texture2D[numSamples];
			_contents = new GUIContent[numSamples];
			for (int i = 0; i < numSamples; i++)
			{
				_contents[i] = new GUIContent(" ");
			}
						
			return true;
		}
		
		return false;
	}
	
	void Start()
	{
		_movie = new AVProQuickTime();
		_movie.IsActive = true;
	}
	
	void Update()
	{
		if (_isExtracting)
		{
			if (_frameStep > 0)
			{
				UpdateExtracting();
			}
			else
			{
				// Wait until movie properties have loaded
				_movie.Update(false);
				if (_movie.PlayState == AVProQuickTime.PlaybackState.Loaded)
				{
					uint numFrames = _movie.FrameCount;
					_frameStep = numFrames / 24;
					_targetFrame = 0;
					_textureIndex = 0;				
				}
			}
		}
	}
	
	private Texture2D CopyRenderTexture(RenderTexture rt)
	{
		RenderTexture prevRT = RenderTexture.active;
		RenderTexture.active = rt;

		Texture2D texture = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
		texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
		texture.Apply(false, false);

		RenderTexture.active = prevRT;		

		return texture;
	}
	
	private void UpdateExtracting()
	{
		_movie.Update(false);
		if (_movie.DisplayFrame == _targetFrame)
		{
			if (_textureIndex < _textures.Length)
			{			
				Texture2D texture = CopyRenderTexture((RenderTexture)_movie.OutputTexture);
				_contents[_textureIndex] = new GUIContent("Frame " + _targetFrame.ToString(), texture);
				_textures[_textureIndex++] = texture;
			}
			
			NextFrame();
		}		
	}
	
	private void NextFrame()
	{
		_targetFrame += _frameStep;
		if (_targetFrame < _movie.FrameCount)
		{
			// Seek to frame
			_movie.Frame = _targetFrame;
		}
		else
		{
			_isExtracting = false;
		}
	}
	
	void OnDestroy()
	{
		DestroyTextures();
		if (_movie != null)
		{
			_movie.Dispose();
			_movie = null;
		}
	}
	
	void OnGUI()
	{
		GUI.skin = _guiSkin;
		
		if (_gridStyle == null)
		{
			_gridStyle = GUI.skin.GetStyle("ExtractFrameGrid");
		}
		
		GUI.enabled = !_isExtracting;
		
		GUILayout.BeginVertical(GUILayout.Width(Screen.width));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Folder: ", GUILayout.Width(80));
		_folder = GUILayout.TextField(_folder, 192, GUILayout.ExpandWidth(true));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("File: ", GUILayout.Width(80));
		_filename = GUILayout.TextField(_filename, 128, GUILayout.MinWidth(440), GUILayout.ExpandWidth(true));
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();		
		if (GUILayout.Button("Extract Frames"))
		{
			string filePath = Path.Combine(_folder, _filename);
			
			// If we're running outside of the editor we may need to resolve the relative path
			// as the working-directory may not be that of the application EXE.
			if (!Application.isEditor && !Path.IsPathRooted(filePath))
			{
				string rootPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
				filePath = Path.Combine(rootPath, filePath);
			}
			
			_isExtracting = StartExtractFrames(filePath, 24);
		}
		GUILayout.EndHorizontal();
	
		GUILayout.EndVertical();
		
		GUI.enabled = true;
		
		if (_textures != null)
		{
			if (_gridStyle != null)
				GUILayout.SelectionGrid(-1, _contents, 6, _gridStyle, GUILayout.Height(Screen.height-96));
			else
				GUILayout.SelectionGrid(-1, _contents, 6, GUILayout.Height(Screen.height-96));
		}
	}
}