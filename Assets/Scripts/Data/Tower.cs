using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "New Tower", menuName = "Game assets/Tower")]
    public class Tower : ScriptableObject
    {
        public string Name;

        public int Cost;

        public bool IsBought;
        public bool IsSelected;

        [Header("Materials")]
        public Material BlockMaterial;
        public Material PlatformMaterial;

        [Header("Sprites")]
        public Sprite ShopImage;

        [Header("Physic materials")]
        public PhysicMaterial Bounciness;

        [Header("Audio")]
        public AudioClip BlockSound;
    }

}