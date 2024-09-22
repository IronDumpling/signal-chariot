using UnityEngine;

namespace InGame.VFX
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float m_dieTime = 2f;
        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, m_dieTime);
        }


    }
}
