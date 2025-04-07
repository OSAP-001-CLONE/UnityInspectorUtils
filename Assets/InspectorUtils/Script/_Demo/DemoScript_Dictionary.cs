using IUtil.CustomContainer;
using System.Collections.Generic;
using UnityEngine;


namespace IUtil._Demo
{
    public class DemoScript_Dictionary : MonoBehaviour
    {
        [SerializeField]
        public SerializableDictionary<string, string> strDict = new SerializableDictionary<string, string>();
    }
}