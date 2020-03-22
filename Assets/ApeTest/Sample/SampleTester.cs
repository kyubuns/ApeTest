using ApeTest.Action;
using UnityEngine;

namespace ApeTest.Sample
{
    public class SampleTester : MonoBehaviour
    {
        private Ape _ape;

        public void Start()
        {
            DontDestroyOnLoad(gameObject);

            var logger = new DefaultLogger();
            _ape = new Ape(new IApeAction[]
            {
                new RandomButtonClick(),
                new KeyInputAction(),
                new StopWhenError(logger),
            }, logger);
        }

        public void Update()
        {
            if (!_ape.Update())
            {
                Destroy(gameObject);
            }
        }

        public void OnDestroy()
        {
            _ape?.Dispose();
        }
    }
}
