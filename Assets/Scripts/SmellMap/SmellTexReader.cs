using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace OlfactoryMap
{
    [Serializable]
    public class Smell
    {
        public SmellType associatedSmell;
        public Texture2D texture2D;
        public bool enabled;
        [Range(0f, 1)] public float maxAlpha;
        [Range(0f, 1)] public float minAlpha;
        [Range(0f, 1f)] public float globalAlphaMultiplier;
    }

    public class SmellTexReader : MonoBehaviour
    {
        public int texDimension = 1024;
        [SerializeField] public Vector2 TerrainDimension;
        [SerializeField] public List<Smell> AvailableSmells;

        public float GetSmellValueById(int id, Vector2 position)
            => GetSmellValue(AvailableSmells[id].associatedSmell, position);

        public float GetSmellValue(SmellType smellType, Vector2 position)
        {
            var smell = GetSmell(smellType);
            if(smell == null) 
                return 0f;

            if (!smell.enabled)
                return 0f;

            if(smell.texture2D == null)
                return 0f;

            var texSize = smell.texture2D.Size();
            //Debug.Log($"texture size : {texSize}");
            //Debug.Log($"player position : {position}");
            var mappedPosition = MapPosition(position, TerrainDimension, texSize);
            //Debug.Log($"mapped position : {mappedPosition}");
            var pixelColor = smell.texture2D.GetPixel((int)mappedPosition.x, (int)mappedPosition.y);
            //Debug.LogError($"{pixelColor} tex alpha : {pixelColor.a}");
            float finalAlpha = pixelColor.a * smell.globalAlphaMultiplier;

            if (pixelColor.a >= smell.minAlpha)
                finalAlpha = Math.Clamp(pixelColor.a, smell.minAlpha, smell.maxAlpha);

            return finalAlpha;
        }

        private Smell GetSmell(SmellType type)
        {
            if (AvailableSmells == null)
                return null;

            if (AvailableSmells.Count == 0)
                return null;

            return AvailableSmells.Where(x => x.associatedSmell == type).FirstOrDefault();
        }

        private Vector2 MapPosition(Vector2 position, Vector2 maxMapSize, Vector2 maxTexSize)
        {
            var ratio = maxTexSize / maxMapSize;
            var result = position * ratio;

            return result;
        }
    }
}


