using System;
using UnityEngine;

namespace Assets.Streamline.Scripts
{
    public class CubemapSingleton
    {
        private Material[] _otherMaterials;
        private Material _levelTwoMaterial, _levelThreeMaterial, _finalMaterial;

        private CubemapSingleton()
        {
            _otherMaterials = Resources.LoadAll<Material>("Worlds/Other/");

            _levelTwoMaterial = Resources.Load<Material>("Worlds/LevelTwo");
            _levelThreeMaterial = Resources.Load<Material>("Worlds/LevelThree");
            _finalMaterial = Resources.Load<Material>("Worlds/Final");
        }

        private static CubemapSingleton _cubemapSingleton;
        public static CubemapSingleton GetInstance()
        {
            if (_cubemapSingleton == null)
            {
                _cubemapSingleton = new CubemapSingleton();
            }

            return _cubemapSingleton;
        }

        public Material GetByNextScene(string nextScene)
        {
            switch (nextScene)
            {
                case "LevelTwoScene":
                    return _levelTwoMaterial;
                case "LevelThreeScene":
                    return _levelThreeMaterial;
                case "FinalScene":
                    return _finalMaterial;
            }

            throw new Exception();
        }

        public Material GetAnotherMaterialById(int id)
        {
            return _otherMaterials[id];
        }

        public int GetNumberOfOtherMaterials()
        {
            return _otherMaterials.Length;
        }
    }
}
