using UnityEngine;

using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace InGame.Cores
{
    public class CameraInfo
    {
        public Camera camera;
        public GameObject gameObject;
        public Transform transform;

        public CameraInfo(Camera cam)
        {
            this.camera = cam;
            this.gameObject = cam.gameObject;
            this.transform = cam.transform;
        }
    }

    public class CameraManager: MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField]
        private Camera[] m_cameras = new Camera[4];
        private CameraInfo m_board, m_boardThumbnail, m_battle, m_battleTest;
        
        [Header("VCam Settings")]
        [SerializeField]
        private CinemachineVirtualCamera m_battleVirtualCamera;

        [Header("Postprocessing")]
        [SerializeField]
        private Volume m_redMask;

        public void Init()
        {
            m_board = new CameraInfo(m_cameras[0]);
            m_boardThumbnail = new CameraInfo(m_cameras[1]);
            m_battle = new CameraInfo(m_cameras[2]);
            m_battleTest = new CameraInfo(m_cameras[3]);
        }

        #region Getter
        public Camera boardCamera => m_board.camera;
        public Camera boardThumbnailCamera => m_boardThumbnail.camera;
        public Camera battleCamera => m_battle.camera;
        public Camera battleSimulationCamera => m_battleTest.camera;
        public Vector2 GetBattleSize()
        {
            float height = 2f * m_battle.camera.orthographicSize;
            float width = height * m_battle.camera.aspect;
            return new Vector2(width, height);
        }
        #endregion

        #region Setter
        public void SetBoardActive(bool active) => m_board.gameObject.SetActive(active);
        public void SetBoardThumbnailActive(bool active) => m_boardThumbnail.gameObject.SetActive(active);
        public void SetBoardThumbnailSize(float size) => m_boardThumbnail.camera.orthographicSize = size;
        public void SetBattleActive(bool active) => m_battle.gameObject.SetActive(active);
        public void SetBattleCameraFollow(GameObject target) => m_battleVirtualCamera.Follow = target?.transform;
        
        public void SetBoardPosition(Vector2 pos)
        {
            var z = m_board.transform.position.z;
            var position = new Vector3(pos.x, pos.y, z);
            m_board.transform.position = position;
        }
        
        public void SetBoardThumbnailPosition(Vector2 pos)
        {
            var z = m_boardThumbnail.transform.position.z;
            var position = new Vector3(pos.x, pos.y, z);
            m_boardThumbnail.transform.position = position;
        }

        public void SetBattleConfiner(string goName)
        {
            CinemachineConfiner confiner = m_battleVirtualCamera.GetComponent<CinemachineConfiner>();
            PolygonCollider2D bounds = GameObject.Find(goName)?.GetComponent<PolygonCollider2D>();
            if(confiner == null || bounds == null) return;
            confiner.m_BoundingShape2D = bounds;
            confiner.InvalidatePathCache();
        }

        public void SetRedMaskIntensity(float intensity)
        {
            if(!m_redMask.profile.TryGet(out Vignette vignette)) return;
            const float maxIntensity = 0.9f;
            vignette.intensity.value = Mathf.Lerp(0, maxIntensity, intensity);
        }
        #endregion
    }
}