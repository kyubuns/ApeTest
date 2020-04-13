using UnityEngine;

namespace ApeTest.Sample
{
    public class SampleCube : MonoBehaviour
    {
        public bool Right { get; set; }
        public bool Left { get; set; }

        public void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow)) Right = true;
            if (Input.GetKey(KeyCode.LeftArrow)) Left = true;

            var p = transform.localPosition;
            p.x += Right ? Time.deltaTime * 1.0f : 0.0f;
            p.x -= Left ? Time.deltaTime * 1.0f : 0.0f;
            transform.localPosition = p;

            Right = false;
            Left = false;
        }
    }
}
