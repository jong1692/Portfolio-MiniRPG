using UnityEngine;


namespace HotMountain
{
    public class Camera : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        float xOrbit;
        float yOrbit;

        float sensibility = 2f;

        private float minAngleY = -90.0f;
        private float maxAngleY = 90.0f;

        private float maxDistance = 10;
        private float minDistance = 0f;

        private float _distanceFromCamera;
        private float _tempDistance;




        private void Awake()
        {
            _distanceFromCamera = 5f;
            _tempDistance = _distanceFromCamera;

            xOrbit = _target.eulerAngles.y;
            yOrbit = 30f;
        }

        void Update()
        {
            CameraSetting();
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F) { angle += 360F; }
            if (angle > 360F) { angle -= 360F; }
            return Mathf.Clamp(angle, min, max);
        }

        void CameraSetting()
        {
            float timeScaleSpeed = Mathf.Clamp(1.0f / Time.timeScale, 0.01f, 1);

            RaycastHit hitCamOrbital;

            int layerMaskPlayer = 1 << 15 | 1 << 8 | 1 << 9 | 1 << 10;
            if (Physics.Linecast(_target.position, transform.position, out hitCamOrbital, ~layerMaskPlayer))
            {
                _distanceFromCamera = Vector3.Distance(_target.position,
                    hitCamOrbital.point);

                if (hitCamOrbital.transform.tag != "Terrain")
                    _distanceFromCamera -= 0.2f;
                //_distanceFromCamera = Mathf.Clamp(_distanceFromCamera, minDistance, maxDistance);
            }
            else if (!Physics.Linecast(_target.position,
                transform.position - transform.forward * 0.5f - transform.up * 0.5f,
                out hitCamOrbital, ~layerMaskPlayer))
            {
                _distanceFromCamera = _tempDistance;
            }

            _distanceFromCamera = Mathf.Clamp(_distanceFromCamera
                    - Input.GetAxis("Mouse ScrollWheel") * 10.0f, minDistance, maxDistance);

            _tempDistance = Mathf.Clamp(_tempDistance
                    - Input.GetAxis("Mouse ScrollWheel") * 10.0f, minDistance, maxDistance);

            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = Input.GetAxis("Mouse Y");

            if (Input.GetMouseButton(1))
            {
                Vector3 rot = _target.eulerAngles;
                rot.y = xOrbit;

                _target.eulerAngles = rot;
            }

            if (Input.GetMouseButton(0))
            {
                xOrbit += mouseInputX * sensibility / 0.5f;
                yOrbit -= mouseInputY * sensibility * 10;

                xOrbit %= 360;
                yOrbit %= 360;
            }
            else if (Input.GetKey("w"))
            {
                xOrbit = _target.eulerAngles.y;
                yOrbit = 30f;
            }
            yOrbit = ClampAngle(yOrbit, minAngleY, maxAngleY);

            Quaternion quatToEuler = Quaternion.Euler(yOrbit, xOrbit, 0);

            Vector3 zPosition = new Vector3(0.0f, 0.0f, -_distanceFromCamera);
            Vector3 nextPosCam = quatToEuler * zPosition + _target.position + _target.up * 1f;

            Vector3 currentPosCam = transform.position;
            Quaternion camRotation = transform.rotation;

            transform.rotation =
                Quaternion.Lerp(camRotation, quatToEuler, Time.deltaTime * 5.0f * timeScaleSpeed);

            transform.position =
                      Vector3.Lerp(currentPosCam, nextPosCam, Time.deltaTime * 5.0f * timeScaleSpeed);
        }
    }
}