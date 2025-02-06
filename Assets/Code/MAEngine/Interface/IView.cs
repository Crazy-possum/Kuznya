using UnityEngine;

namespace MAEngine
{
    public interface IView
    {
        public GameObject Object { get; }
        public string ViewID { get; set; }
    }
}
