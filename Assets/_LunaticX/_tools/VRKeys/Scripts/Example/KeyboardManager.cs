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
using System;
using System.Text.RegularExpressions;
using System.Collections;
using CurioAssets;

namespace VRKeys {

	/// <summary>
	/// Example use of VRKeys keyboard.
	/// </summary>
	public class KeyboardManager : MonoBehaviour 
	{
		public Keyboard keyboard;
		private void OnEnable ()
		{
		//	keyboard.Enable ();
		//		keyboard.SetPlaceholderMessage ("Please enter your email address");

			keyboard.OnUpdate.AddListener (HandleUpdate);
			keyboard.OnSubmit.AddListener (HandleSubmit);
			keyboard.OnCancel.AddListener (HandleCancel);
		}

		private void OnDisable () {
			keyboard.OnUpdate.RemoveListener (HandleUpdate);
			keyboard.OnSubmit.RemoveListener (HandleSubmit);
			keyboard.OnCancel.RemoveListener (HandleCancel);

	//		keyboard.Disable ();
		}



		/// <summary>
		/// Hide the validation message on update. Connect this to OnUpdate.
		/// </summary>
		public void HandleUpdate (string text) 
		{
			keyboard.HideValidationMessage ();
		}

		public EasyPlayFabIntegration playFab;
		/// <summary>
		/// Validate the email and simulate a form submission. Connect this to OnSubmit.
		/// </summary>
		public void HandleSubmit (string text) 
		{
				//StartCoroutine (SubmitEmail (text));
			playFab.ValidateNewAccountCreation();
			keyboard.Disable ();

		}

		public void HandleCancel ()
		{
			Debug.Log ("Cancelled keyboard input!");
			keyboard.Disable ();
		}
	}
}