using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace MAEngine.Extention
{
    [Serializable]
    public class DictionaryElement<TKey, TValue>
    {
        [SerializeField][FormerlySerializedAs("Key")]
        private TKey _key;
        [SerializeField][FormerlySerializedAs("Value")]
        private TValue _value;
        
        [XmlElement("Key")]
        public TKey Key 
        {
            get => _key;
            set => _key = value;
        }
        [XmlElement("Value")]
        public TValue Value 
        {
            get => _value;
            set => _value = value;
        }

        public DictionaryElement()
        {

        }

        public DictionaryElement(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}