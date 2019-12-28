/**
 * Copyright (c) 2017 The Campfire Union Inc - All Rights Reserved.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for
 * full license information.
 *
 * Email:   info@campfireunion.com
 * Website: https://www.campfireunion.com
 */

using UnityEngine;
using System.Collections;
using TMPro;

namespace VRKeys {

	/// <summary>
	/// An individual key in the VR keyboard.
	/// </summary>
	public class Key : MonoBehaviour {
		public Keyboard keyboard;

		public TextMeshPro label;

		public Material inactiveMat;
		public Material hoveredMat;
		public Material activeMat;

		public Material disabledMat;

		public Vector3 defaultPosition;

		public Vector3 pressedPosition;

		public Vector3 pressDirection = Vector3.down;

		public float pressMagnitude = 0.1f;

		public bool autoInit = false;

		private bool isPressing = false;

		private bool disabled = false;

		protected MeshRenderer meshRenderer;

		private IEnumerator _ActivateFor,_HighlightFor;

		private IEnumerator _Press,_Hover;

		private void Awake () 
		{
			meshRenderer = GetComponent<MeshRenderer> ();

			if (autoInit) 
			{
				Init (transform.localPosition);
			}
		}

		/// <summary>
		/// Initialize the key with a default position and pressed position.
		/// </summary>
		/// <param name="defaultPos">Default position.</param>
		public void Init (Vector3 defaultPos) 
		{
			defaultPosition = defaultPos;
			pressedPosition = defaultPos + (Vector3.down * 0.01f);
			
		}

		private void OnEnable () 
		{
			isPressing = false;
			disabled = false;
			transform.localPosition = defaultPosition;
			meshRenderer.material = inactiveMat;

			OnEnableExtras ();
		}

		/// <summary>
		/// Override this to add custom logic on enable.
		/// </summary>
		protected virtual void OnEnableExtras () 
		{
			// Override me!
		}

		public virtual void OnPress () 
		{
//			Debug.Log("key OnPress "+name);
			if (isPressing || disabled || keyboard.disabled || !keyboard.initialized)
			{
				return;
			}
			//Debug.Log("key OnPress okay "+name);
			if (_Press != null && _Press.MoveNext ()) 
			{
				StopCoroutine (_Press);
			}
			
			_Press = Press ();
			StartCoroutine (_Press);
		}

		public void OnHover()
		{

			meshRenderer.material = hoveredMat;
		}

		public void OnUnHover () 
		{
			
			meshRenderer.material = inactiveMat;
		}
		
		private IEnumerator Press ()
		{
			isPressing = true;
			transform.localPosition = pressedPosition;
			yield return new WaitForSeconds (0.125f);
			transform.localPosition = defaultPosition;
			meshRenderer.material = inactiveMat;
			isPressing = false;
				
		}

		/// <summary>
		/// Override this to handle trigger events. Only fires when
		/// a downward trigger event occurred from the collider
		/// matching keyboard.colliderName.
		/// </summary>
		/// <param name="other">Collider.</param>

	
		
		/// <summary>
		/// Show the active material for the specified length of time.
		/// </summary>
		/// <param name="seconds">Seconds.</param>
		public void ActivateFor (float seconds) {
			if (_ActivateFor != null && _ActivateFor.MoveNext ()) {
				StopCoroutine (_ActivateFor);
			}
			_ActivateFor = DoActivateFor (seconds);
			StartCoroutine (_ActivateFor);
		}

		private IEnumerator DoActivateFor (float seconds) {
			meshRenderer.material = activeMat;

			yield return new WaitForSeconds (seconds);

			meshRenderer.material = inactiveMat;
		}

		/// <summary>
		/// Disable the key.
		/// </summary>
		public virtual void Disable () {
			disabled = true;

			if (meshRenderer != null) {
				meshRenderer.material = disabledMat;
			}
		}

		/// <summary>
		/// Re-enable a disabled key.
		/// </summary>
		public virtual void Enable () {
			disabled = false;

			if (meshRenderer != null) {
				meshRenderer.material = inactiveMat;
			}
		}

		/// <summary>
		/// Update the key's label from a new language.
		/// </summary>
		/// <param name="translation">Translation object.</param>
		public virtual void UpdateLayout (Layout translation) {
			// Override me!
		}
	}
}