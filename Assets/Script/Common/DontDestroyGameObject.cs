namespace UnityEngine
{
    sealed class DontDestroyGameObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}