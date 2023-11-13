using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 AddZ(this Vector3 vector, float value)
            => new Vector3(vector.x, vector.y, vector.z + value);

        public static Vector3 AddY(this Vector3 vector, float value)
            => new Vector3(vector.x, vector.y + value, vector.z);

        public static Vector3 AddX(this Vector3 vector, float value)
            => new Vector3(vector.x + value, vector.y, vector.z);
    }
}