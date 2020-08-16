using System;

namespace CjGameDevFrame.Common
{
    [MonoSingletonPath("[CjFrame]/[MonoAgent]")]
    public class MonoAgent : MonoSingleton<MonoAgent>
    {
        public event Action OnUpdate;
        public event Action OnFixedUpdate;
        public event Action OnGui;

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        
        private void OnGUI()
        {
            OnGui?.Invoke();
        }
    }
}