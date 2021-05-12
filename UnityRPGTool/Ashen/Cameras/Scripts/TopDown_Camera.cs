using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.Cameras
{
    public class TopDown_Camera : Base_Camera
    {
        #region varaibles 
        
        public float m_Height = 10f;
        public float m_Distance = 20f;
        public float m_Angle = 45f;
        public float m_SmoothSpeed = 0.5f;
        public bool followRotation = false;

        private Vector3 refVelocity;
        #endregion

        #region Main Methods
        #endregion


        #region Helper Methods
        protected override void HandleCamera()
        {
            base.HandleCamera();

            //Build world position
            Vector3 worldPosition = (Vector3.forward * -m_Distance) + (Vector3.up * m_Height);
            Debug.DrawLine(m_Target.position, worldPosition, Color.red);

            //Build our Rotated Vector
            Vector3 rotatedVector = Quaternion.AngleAxis(m_Angle, Vector3.up) * worldPosition;
            Debug.DrawLine(m_Target.position, rotatedVector, Color.green);

            //Move our position
            Vector3 flatTargetPosition = m_Target.position;
            flatTargetPosition.y = 0f;
            Vector3 finalPosition = flatTargetPosition + rotatedVector;
            Debug.DrawLine(m_Target.position, finalPosition, Color.blue);

            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, m_SmoothSpeed);
            transform.LookAt(flatTargetPosition);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
            if(m_Target)
            {
                Gizmos.DrawLine(transform.position, m_Target.position);
                Gizmos.DrawSphere(m_Target.position, 1.5f);
            }
            Gizmos.DrawSphere(transform.position, 1.5f);
        }
        #endregion
    }
}