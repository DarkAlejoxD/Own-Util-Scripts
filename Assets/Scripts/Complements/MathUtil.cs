using UnityEngine;

namespace UtilsComplements
{
    public static class MathUtil
    {
        #region Report
        //Last checked: idk, probably 2022
        //Last modification: idk, probably 2022

        //Commentary: Old class used in 2D projects
        #endregion

        /// <summary>
        /// Returns an angle in radians
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float Angle(Vector2 vector)
        {
            vector.Normalize();

            if (vector.y == 1)
            {
                return DegreesToRadians(90);
            }
            else if (vector.y == -1)
            {
                return DegreesToRadians(270);
            }
            else if (vector.y < 0)
            {
                return DegreesToRadians(360) + Mathf.Atan2(vector.y, vector.x);
            }
            else
            {
                return Mathf.Atan2(vector.y, vector.x);
            }
        }

        /// <summary>
        /// The input data has to be in radians
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        }

        public static float Angle(float x, float y)
        {
            return Angle(new Vector2(x, y));
        }

        public static float DegreesToRadians(float degrees)
        {
            return degrees * Mathf.PI / 180;
        }

        public static float RadiansToDegrees(float radians)
        {
            return radians * 180 / Mathf.PI;
        }

        public static bool Range(Vector2 currentDirection, Vector2 targetDirection, float range)
        {
            float targetDirectionDegree = RadiansToDegrees(Angle(targetDirection));
            float currentDegree = RadiansToDegrees(Angle(currentDirection));

            return Range(currentDegree, targetDirectionDegree, range);
        }

        /// <summary>
        ///  The input Data has to be in degrees
        /// </summary>
        /// <param name="currentDegree"></param>
        /// <param name="targetDegree"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static bool Range(float currentDegree, float targetDegree, float range)
        {
            currentDegree = DegreesToRadians(currentDegree);
            targetDegree = DegreesToRadians(targetDegree);
            range = DegreesToRadians(range);

            float maxAngle = targetDegree + range;
            float minAngle = targetDegree - range;

            if (maxAngle >= DegreesToRadians(360))
            {
                maxAngle -= DegreesToRadians(360);
            }

            return currentDegree <= maxAngle && currentDegree >= minAngle;
        }


        public static int GetClosestPoint(Vector3 from, Vector3[] points)
        {
            if (points.Length <= 0) return - 1;

            float smallestDistance = 9999.0f;
            int closestIndex = -1;
            for(int i = 0; i < points.Length; i++)
            {
                float distance = Vector3.Distance(from, points[i]);
                if(distance < smallestDistance)
                {
                    smallestDistance = distance;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

    }
}
