/* /////////////////////////////////////////////////////////////////////////////////////////////////
SuperResolution v0.2 by MacGruber.
Makes the VaM internal screenshot tool do screenshots in 8K resolution.

Version 0.2 2019-08-18
	Adding preview camera to allow use in VR mode without performance issues.
	Adding slider with resolutions to select from.

Version 0.1 2019-08-16
	Initial release.

///////////////////////////////////////////////////////////////////////////////////////////////// */

using UnityEngine;
using System.Collections.Generic;

namespace MacGruber
{	
	public class SuperResolution : MVRScript
	{	
		private const int READY = 0;
		private const int SCREENSHOT = 1;
		private const int CLEANUP = 2;
		
		private const int resolutionDefaultIdx = 2;		
		private readonly int[] resolutionWidth = new int[] { 9600, 8192, 7680, 5760, 3840 };
		private readonly int[] resolutionHeight = new int[] { 5400, 4608, 4320, 3240, 2160 };
		private readonly string[] resolutionName = new string[] {
			"9600x5400 (5x FullHD)",
			"8192x4608                   ",
			"7680x4320 (8K UHD)   ",
			"5760x3240 (3x FullHD)",
			"3840x2160 (4K UHD)   "
		};
		
		private RenderTexture screenshotTexture;
		private Camera previewCamera;
		private Camera screenshotCamera;
		private int state = 0;
		private int resolutionIdx = resolutionDefaultIdx;
		
		public override void Init()
		{	
			List<string> modes = new List<string>(resolutionName);
			JSONStorableStringChooser chooser = new JSONStorableStringChooser("Resolution", modes, modes[resolutionDefaultIdx], "Resolution");
			chooser.setCallbackFunction += (string modeName) => {
				resolutionIdx = modes.FindIndex((string entry) => { return entry == modeName; });
				if (resolutionIdx < 0 || resolutionIdx >= resolutionWidth.Length  || resolutionIdx >= resolutionHeight.Length)
					resolutionIdx = resolutionDefaultIdx;
			};
			CreateScrollablePopup(chooser, false);
			RegisterStringChooser(chooser);
		}

		private void OnEnable()
		{
			SuperController sc = SuperController.singleton;
			screenshotCamera = sc.hiResScreenshotCamera;
			previewCamera = Instantiate<Camera>(screenshotCamera, screenshotCamera.transform);
			previewCamera.targetTexture = screenshotCamera.targetTexture;			
			previewCamera.enabled = false;
						
			screenshotCamera.targetTexture = null; // prevent regular VaM screenshot
			screenshotCamera.enabled = false; // prevent expensive rendering in the background
			
			state = READY;
		}
		
		private void Update()
		{	
			SuperController sc = SuperController.singleton;
			
			switch (state)
			{
				case READY:
				{
					// enable cameras as needed
					bool active = sc.hiResScreenshotPreview.gameObject.activeSelf;
					screenshotCamera.enabled = false;
					previewCamera.enabled = active;
					if (!active)
						return;
										
					// ensure RenderTexture size
					if (screenshotTexture == null || screenshotTexture.width != resolutionWidth[resolutionIdx])
					{
						if (screenshotTexture != null)
						{
							screenshotTexture.Release();
							Destroy(screenshotTexture);
						}
						RenderTextureDescriptor descriptor = previewCamera.targetTexture.descriptor;			
						descriptor.width = resolutionWidth[resolutionIdx];
						descriptor.height = resolutionHeight[resolutionIdx];
						screenshotTexture = new RenderTexture(descriptor);
						screenshotTexture.Create();
					}
					
					// check for user input
					if (!sc.GetRightSelect() && !sc.GetLeftSelect() && !sc.GetMouseSelect())
						return;
					
					// init screenshot sequence
					screenshotCamera.targetTexture = screenshotTexture;			
					screenshotCamera.enabled = true;
					state = SCREENSHOT;
					break;
				}
				case SCREENSHOT:
				{
					// fake user input to cause SuperController.ProcessHiResScreenshot do run
					sc.SetLeftSelect(); 
					state = CLEANUP;
					break;
				}
				case CLEANUP:
				{
					// cleanup
					screenshotCamera.targetTexture = null;
					screenshotCamera.enabled = false;
					state = READY;
					break;
				}
			}
		}
		
		private void OnDisable()
		{
			if (screenshotTexture != null)
			{
				screenshotTexture.Release();
				Destroy(screenshotTexture);
				screenshotTexture = null;
			}
			
			screenshotCamera.targetTexture = previewCamera.targetTexture;
			screenshotCamera.enabled = SuperController.singleton.hiResScreenshotPreview.gameObject.activeSelf;
			screenshotCamera = null;
			
			Destroy(previewCamera);
			previewCamera = null;
		}
	}
}