using System.Threading;
using System.Threading.Tasks;
using ApeTest.Utils;
using UnityEngine;

namespace ApeTest.Sample
{
    public class KeyInputAction : IApeAction
    {
        private SampleCube _targetCube;

        public State CheckState()
        {
            var cubes = Object.FindObjectsOfType<SampleCube>();
            _targetCube = cubes.RandomPick();
            return _targetCube != null ? State.Execute : State.DontExecute;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            var leftOrRight = Random.Range(0, 2);
            for (var i = 0; i < 30; ++i)
            {
                if (leftOrRight == 0) _targetCube.Left = true;
                else _targetCube.Right = true;
                await Task.Delay(1, cancellationToken);
            }
        }
    }
}
