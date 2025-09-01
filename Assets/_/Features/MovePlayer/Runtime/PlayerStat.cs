using UnityEngine;

namespace MovePlayer.Runtime
{
    public class PlayerStat : MonoBehaviour
    {
        public int m_maxLife = 3;
        private int m_life = 0;
        public int CurrentLife()
        {
            return m_life;
        }


        public void HurtPlayer(int val)
        {
            m_life -= val;
        }

        private void Awake()
        {
            ResetLife();
        }

        public void ResetLife()
        {
            m_life = m_maxLife;
        }
    }
}