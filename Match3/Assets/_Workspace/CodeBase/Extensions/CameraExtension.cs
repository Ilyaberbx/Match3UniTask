using UnityEngine;

namespace _Workspace.CodeBase.Extensions
{
    public static class CameraExtension
    {
        public static Vector3 GetLeftUpperCornerWorldPosition(this Camera camera)
            => camera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        
        public static Vector3 GetRightUpperCornerWorldPosition(this Camera camera)
            => camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        
        public static Vector3 GetRightLowerCornerWorldPosition(this Camera camera)
            => camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        
        public static Vector3 GetLeftLowerCornerWorldPosition(this Camera camera)
            => camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
    }
}