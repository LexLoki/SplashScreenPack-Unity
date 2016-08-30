/*  The MIT License (MIT)
**  Copyright © 2016 Pietro Ribeiro Pepe.
**  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
**  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
**  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SplashScreenScript : MonoBehaviour {

	public enum State{
		SceneByIndex,
		SceneByName
	}

	[SerializeField] private float fadeInDuration=1;
	[SerializeField] private float stayDuration=1;
	[SerializeField] private float fadeOutDuration=1;
	[SerializeField] private bool scaleToFill=false;
	[SerializeField] public State sceneFormat;
	[HideInInspector] public string sceneName="";
	[HideInInspector] public int sceneIndex=0;

	private float timer = 0f;
	private SpriteRenderer spRend;

	private delegate void FadeAction();
	private FadeAction handler;

	private static Vector2 getSize(){
		float height = Camera.main.GetComponent<Camera>().orthographicSize*2;
		return new Vector2 (height/Screen.height*Screen.width, height);
	}

	void Awake() {
		spRend = gameObject.GetComponent<SpriteRenderer> ();
		Vector2 size = SplashScreenScript.getSize (), rSize = spRend.bounds.size;
		Vector3 scaleToApply;
		if (!scaleToFill) {
			float scale = Mathf.Min (size.x / rSize.x, size.y / rSize.y);
			scaleToApply = new Vector3 (scale,scale,1);
		}
		else
			scaleToApply = new Vector3(size.x/rSize.x,size.y/rSize.y,1);
		transform.localScale = Vector3.Scale (transform.localScale,scaleToApply);
	}

	void Start () {
		handler = updateFadeIn;
	}

	void Update () {
		timer += Time.deltaTime;
		handler ();
	}

	private void updateFadeIn(){
		float aux;
		if (timer < fadeInDuration) {
			colorWithAlpha (timer/fadeInDuration);
		} else {
			colorWithAlpha (1);
			timer -= fadeInDuration;
			handler = updateStay;
			handler ();
		}
	}

	private void updateStay(){
		if (timer > stayDuration) {
			timer -= stayDuration;
			handler = updateFadeOut;
			handler ();
		}
	}

	private void updateFadeOut(){
		if (timer < fadeOutDuration)
			colorWithAlpha (1-timer/fadeInDuration);
		else {
			colorWithAlpha (0);
			loadScene ();
		}
	}

	private void loadScene(){
		if (sceneFormat == State.SceneByIndex)
			SceneManager.LoadScene (sceneIndex);
		else
			SceneManager.LoadScene (sceneName);
	}

	private void colorWithAlpha(float a){
		spRend.color = new Color (spRend.color.r, spRend.color.g, spRend.color.b, a);
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(SplashScreenScript))]
[CanEditMultipleObjects]
public class MyEditor : Editor{

	SplashScreenScript script;

	void OnEnable(){
		script = (SplashScreenScript)target;
	}

	override public void OnInspectorGUI(){
		//serializedObject.Update ();
		DrawDefaultInspector ();
		//SplashScreenScript script = target as SplashScreenScript;
		if (script.sceneFormat == SplashScreenScript.State.SceneByIndex)
			script.sceneIndex = EditorGUILayout.IntField ("Scene Index:",script.sceneIndex);
		else
			script.sceneName = EditorGUILayout.TextField("Scene Name:",script.sceneName);
		EditorUtility.SetDirty(target);
	}
}
#endif