using UnityEngine;

namespace Spirinse.System.Player
{
    public class CheckPointManager
    {
        private Vector2 currentCheckPoint;

        public void SetNewCheckPoint(Vector2 checkPoint)
        {
            currentCheckPoint = checkPoint;
        }

        public Vector2 GetCurrentCheckPoint()
        {
            return currentCheckPoint;
        }

    }
}
