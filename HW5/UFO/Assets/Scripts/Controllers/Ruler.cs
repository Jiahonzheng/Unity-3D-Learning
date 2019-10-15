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

        // 在用户按下空格键后被触发，发射飞碟。
        public List<GameObject> GetUFOs()
        {
            List<GameObject> ufos = new List<GameObject>();
            // 随机生成飞碟颜色。
            var index = random.Next(colors.Length);
            var color = (UFOFactory.Color)colors.GetValue(index);
            // 获取当前 Round 下的飞碟产生数。
            var count = GetUFOCount();
            for (int i = 0; i < count; ++i)
            {
                // 调用工厂方法，获取指定颜色的飞碟对象。
                var ufo = UFOFactory.GetInstance().Get(color);
                // 设置飞碟对象的分数。
                var model = ufo.GetComponent<UFOModel>();
                model.score = score[index] * (currentRound + 1);
                // 设置飞碟对象的缩放比例。
                model.SetLocalScale(scale[index], 1, scale[index]);
                // 随机设置飞碟的初始位置（左边、右边）。
                var leftOrRight = (random.Next() & 2) - 1; // 随机生成 1 或 -1 。
                model.SetSide(leftOrRight, i);
                // 设置飞碟对象的刚体属性，以及初始受力方向。
                var rigidbody = ufo.GetComponent<Rigidbody>();
                rigidbody.AddForce(0.2f * speed[index] * model.GetSpeed(), ForceMode.Impulse);
                rigidbody.useGravity = true;
                ufos.Add(ufo);
            }

            return ufos;
        }
    }
}