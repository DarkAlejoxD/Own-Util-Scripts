using System;
using UnityEngine;
using UtilsComplements;

namespace CharacterMovement.ThirdPerson.Attached
{
//    public partial class CameraController : MonoBehaviour
//    {
//        private enum CameraStates
//        {
//            FreeMove, RecolocatingQuick, RecolocatingSlow
//        }

//        [Header("Recolocate Values")]
//        [SerializeField, Range(0, 1)] private float _smoothToRecolocateQuick = 0.01f;
//        [SerializeField, Range(0, 1)] private float _smoothToRecolocateSlow = 0.001f;
//        [Tooltip("How the angle of the camera is equal to the forward of the player and so change the state")]
//        [SerializeField, Range(0, 1)] private float _dotNearForward = 0.996f;
//        [SerializeField] private float _timeBeingIdle = 4;
//        private float _idleControl;

//        private FSM<CameraStates> _cameraBrain;

//        #region Private Methods
//        partial void FSMInit()
//        {
//            _cameraBrain = new(CameraStates.FreeMove);

//            _cameraBrain.SetOnStay(CameraStates.FreeMove, () =>
//            {
//                if (true)
//                {
//                    _idleControl = 0;
//                }
//                else
//                {
//                    _idleControl += Time.deltaTime;
//                }

//                if (_idleControl > _timeBeingIdle)
//                {
//                    _cameraBrain.ChangeState(CameraStates.RecolocatingSlow);
//                }
//            });

//            _cameraBrain.SetOnStay(CameraStates.RecolocatingQuick, () =>
//            {
//                RotateAutoUpdate(_smoothToRecolocateQuick);
//            });

//            _cameraBrain.SetOnStay(CameraStates.RecolocatingSlow, () =>
//            {
//                RotateAutoUpdate(_smoothToRecolocateSlow);

//                if (true)
//                {
//                    _cameraBrain.ChangeState(CameraStates.FreeMove);
//                }
//            });
//        }

//        partial void FSMUpdate()
//        {
//            _cameraBrain.Update();
//            DEBUG_ChangeState();
//        }

//        private void RotateCameraTo(Vector3 direction, float smooth = 1)
//        {
//            float distance = Vector3.Distance(transform.position, _lookAt.position);
//            direction.Normalize();

//            Vector3 forward = transform.forward;

//            Vector3 desiredForward = Vector3.Lerp(forward, direction, smooth);

//            transform.position = _lookAt.position + -desiredForward * distance;
//            transform.LookAt(_lookAt);
//        }

//        private void RotateAutoUpdate(float smooth = 1)
//        {
//            Vector3 targetForward = _lookAt.forward;
//            Vector3 forward = transform.forward;
//            RotateCameraTo(targetForward, smooth);

//            float dot = Vector3.Dot(forward, targetForward);
//            if (dot > _dotNearForward)
//            {
//                _cameraBrain.ChangeState(CameraStates.FreeMove);
//            }
//        }

//        public void RecolocateCameraQuick()
//        {
//            _cameraBrain.ChangeState(CameraStates.RecolocatingQuick);
//        }

//        #endregion

//        #region DEBUG
//        private void DEBUG_ChangeState()
//        {
//#if UNITY_EDITOR
//            if (Input.GetKeyDown(KeyCode.M))
//            {
//                _cameraBrain.ChangeState(CameraStates.RecolocatingQuick);
//            }

//            if (Input.GetKeyDown(KeyCode.N))
//            {
//                _cameraBrain.ChangeState(CameraStates.RecolocatingSlow);
//            }
//#endif
//        }
//        #endregion
//    }
}
