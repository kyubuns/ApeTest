using ApeTest.Action;
using UnityEngine;

namespace ApeTest.Sample
{
    public class SampleTester : MonoBehaviour
    {
        private readonly Ape _ape = new Ape(new IApeAction[]
        {
            new RandomButtonClick(),
            new KeyInputAction(),
        });

        public void Update()
        {
            _ape.Update();
        }
    }
}
