using UnityEngine;

namespace InGame.VFX
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] protected float m_dieTime = 2f;

        protected ParticleSystem particleSystem => GetComponentInChildren<ParticleSystem>();

        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, m_dieTime);
        }


    }
}
