using System;
using System.Collections.Generic;
using UnityEngine;

namespace HitUFO
{
    public class Ruler
    {
        private readonly int currentRound;
        private System.Random random;

        private static Array colors = Enum.GetValues(typeof(UFOFactory.Color));
        private static int[] UFOCount = { 1, 3, 4, 5, 6, 6, 8, 8, 8, 9 };
        private static int[] score = { 1, 5, 10 };
        private static float[] speed = { 0.5f, 0.6f, 0.7f };
        private static float[] scale = { 2f, 1.5f, 1f };

        public Ruler(int currentRound)
        {
            this.currentRound = currentRound;
            this.random = new System.Random();
        }

        public int GetUFOCount()
        {
            return UFOCount[currentRound];
        }

        public List<GameObject> GetUFOs()
        {
            List<GameObject> ufos = new List<GameObject>();

            var index = random.Next(colors.Length);
            var color = (UFOFactory.Color)colors.GetValue(index);

            var count = GetUFOCount();
            for (int i = 0; i < count; ++i)
            {
                var ufo = UFOFactory.GetInstance().Get(color);
                var model = ufo.GetComponent<UFOModel>();
                var rigidbody = ufo.GetComponent<Rigidbody>();

                model.score = score[index] * (currentRound + 1);

                var leftOrRight = (random.Next() & 2) - 1;
                ufo.transform.position = new Vector3(-3 * leftOrRight, 2 + i, -15);
                ufo.transform.localScale = new Vector3(scale[index], 0.08f, scale[index]);
                rigidbody.AddForce(0.2f * speed[index] * new Vector3(3 * leftOrRight, 11, 8), ForceMode.Impulse);
                rigidbody.useGravity = true;
                ufos.Add(ufo);
            }

            return ufos;
        }
    }
}