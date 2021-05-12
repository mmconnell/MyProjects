using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ashen.Cameras
{
    public class Camera_Menu : MonoBehaviour
    {
        [MenuItem("Ashen/Cameras/Top Down Camea")]
        public static void CreateTopDownCamera()
        {
            GameObject[] selectedGO = Selection.gameObjects;
            
            if (selectedGO.Length > 0 && selectedGO[0].GetComponent<Camera>())
            {
                if (selectedGO.Length < 2)
                {
                    AttachTopDownScript(selectedGO[0].gameObject, null);
                }
                else if(selectedGO.Length == 2)
                {
                    AttachTopDownScript(selectedGO[0].gameObject, selectedGO[1].transform);
                }
                else if(selectedGO.Length == 3)
                {
                    EditorUtility.DisplayDialog("Camera Tools", "You can only select two GameObjects in the scene " +
                        "for this to work and the first selection needs to be the camera!", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Camera Tools", "You need to select a GameObject in the Scene that has a Camera componet.", "OK");
            }
        }

        static void AttachTopDownScript(GameObject aCamera, Transform aTarget)
        {
            //Assign Top down script to the camera
            TopDown_Camera cameraScript = null;
            if (aCamera)
            {
                cameraScript = aCamera.AddComponent<TopDown_Camera>();

                //Check to see if we have a Target and we have a script reference
                if (cameraScript && aTarget)
                {
                    cameraScript.m_Target = aTarget;
                }

                Selection.activeGameObject = aCamera;
            }
        }
    }
}