using UnityEngine;

namespace DefaultNamespace {

    /// <summary>
    /// 타겟을 중심으로 마우스 입력에 따라 카메라가 회전하고 줌인/아웃이 가능한 스크립트입니다.
    /// </summary>
    public class Test : MonoBehaviour {

        [Header("Target Settings")]
        [SerializeField] private Transform target; // 추적할 타겟
        [SerializeField] private float distance = 5.0f; // 타겟과의 거리
        [SerializeField] private float minDistance = 2.0f; // 최소 거리
        [SerializeField] private float maxDistance = 10.0f; // 최대 거리

        [Header("Speed Settings")]
        [SerializeField] private float xSpeed = 120.0f; // 마우스 X축 회전 속도
        [SerializeField] private float ySpeed = 120.0f; // 마우스 Y축 회전 속도
        [SerializeField] private float zoomSpeed = 5.0f; // 줌 속도

        [Header("Angle Constraints")]
        [SerializeField] private float yMinLimit = -20f; // Y축 회전 최소 제한
        [SerializeField] private float yMaxLimit = 80f; // Y축 회전 최대 제한

        private float _x = 0.0f;
        private float _y = 0.0f;

        private void Start() {
            Vector3 angles = transform.eulerAngles;
            _x = angles.y;
            _y = angles.x;

            // Rigidbody가 있는 경우 회전이 충돌하지 않도록 설정
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) {
                rb.freezeRotation = true;
            }
        }

        private void LateUpdate() {
            if (target == null) return;

            // 마우스 오른쪽 버튼을 누르고 있을 때만 회전하도록 설정 (원하는 경우 조건 삭제 가능)
            if (Input.GetMouseButton(1)) {
                _x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                _y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                _y = ClampAngle(_y, yMinLimit, yMaxLimit);
            }

            // 마우스 휠을 이용한 줌 기능
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minDistance, maxDistance);

            // 회전 및 위치 계산
            Quaternion rotation = Quaternion.Euler(_y, _x, 0);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }

        /// <summary>
        /// 각도를 지정된 범위 내로 제한합니다.
        /// </summary>
        private static float ClampAngle(float angle, float min, float max) {
            if (angle < -360F) angle += 360F;
            if (angle > 360F) angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }
    }

}