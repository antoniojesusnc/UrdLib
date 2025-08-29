using UnityEngine;

namespace Urd.Character
{
    public class ProgressionData : IProgressionData
    {
        [field: SerializeField]
        public int Level { get; protected set; }

        public ProgressionData()
        {
            
        }
        protected ProgressionData(int level)
        {
            Level = level;
        }
    }
}
