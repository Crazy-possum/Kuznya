using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace MAEngine.Extention
{
    [Serializable]
    [XmlRoot("Dictionary")]
    public class SerializableDictionary<TKey, TValue>
    {
        [XmlArray("Elements"), XmlArrayItem("Element")]
        public List<DictionaryElement<TKey, TValue>> Dictionary = 
            new List<DictionaryElement<TKey, TValue>>();

        public int Length => Dictionary.Count;
        public TValue this[TKey key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }


        public void Add(DictionaryElement<TKey, TValue> element)
        {
            foreach (DictionaryElement<TKey, TValue> dictionaryElement in Dictionary)
            {
                if (dictionaryElement.Key.Equals(element.Key))
                {
                    Debug.Log($"Element with key {element.Key} in {this} already exist");
                    return;
                }
            }

            Dictionary.Add(element);
        }

        public void Add(TKey key, TValue value)
        {
            var element = new DictionaryElement<TKey, TValue>();
            element.Key = key;
            element.Value = value;
            Add(element);
        }

        public void Remove(TKey key)
        {
            DictionaryElement<TKey, TValue> element = GetElementByKey(key);
            Dictionary.Remove(element);
        }

        public DictionaryElement<TKey, TValue> GetElementByKey(TKey key)
        {
            foreach (DictionaryElement<TKey, TValue> element in Dictionary)
            {
                if (element.Key.Equals(key))
                {
                    return element;
                }
            }
            Debug.Log($"No element with key {key} in {this}");
            return default;
        }
        
        public TValue GetValue(TKey key)
        {
            foreach (DictionaryElement<TKey, TValue> element in Dictionary)
            {
                if (element.Key.Equals(key))
                {
                    return element.Value;
                }
            }
            Debug.Log($"No element with key {key} in {this}");
            return default;
        }

        public void SetValue(TKey key, TValue value)
        {
            foreach (DictionaryElement<TKey, TValue> element in Dictionary)
            {
                if (element.Key.Equals(key))
                {
                    element.Value = value;
                    return;
                }
            }
            Debug.Log($"No element with key {key} in {this}");
        }

        public TValue GetValueByIndex(int index)
        {
            return Dictionary[index].Value;
        }

        public TKey GetKeyByIndex(int index)
        {
            return Dictionary[index].Key;
        }

        public static implicit operator Dictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> serializableDictionary)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            for (int i = 0; i < serializableDictionary.Dictionary.Count; i++)
            {
                dictionary.Add(serializableDictionary.Dictionary[i].Key, serializableDictionary.Dictionary[i].Value);
            }
            return dictionary;
        }

        public static explicit operator SerializableDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            var serializableDictionary = new SerializableDictionary<TKey, TValue>();
            foreach(KeyValuePair<TKey, TValue> element in dictionary)
            {
                var dictionaryElement = new DictionaryElement<TKey, TValue>(element.Key, element.Value);
                serializableDictionary.Dictionary.Add(dictionaryElement);
            }
            return serializableDictionary;
        }

        public List<TKey> GetAllKeys()
        {
            List<TKey> keys = new List<TKey>();
            foreach (DictionaryElement<TKey, TValue> element in Dictionary)
            {
                keys.Add(element.Key);
            }

            return keys;
        }

        public List<TValue> GetAllValues()
        {
            List<TValue> values = new List<TValue>();
            foreach (DictionaryElement<TKey, TValue> element in Dictionary)
            {
                values.Add(element.Value);
            }

            return values;
        }

        public bool IsContainsKey(TKey key)
        {
            return GetAllKeys().Contains(key);
        }
        
        public bool IsContainsValue(TValue value)
        {
            
            return GetAllValues().Contains(value);
        }

        public void Serialize(TextWriter writer)
        {
            List<DictionaryElement<TKey, TValue>> entries = Dictionary;
            XmlSerializer serializer = new XmlSerializer(typeof(List<DictionaryElement<TKey, TValue>>));
            serializer.Serialize(writer, entries);
        }
        
        public void Deserialize(TextReader reader)
        {
            Clear();
            XmlSerializer serializer = new XmlSerializer(typeof(List<DictionaryElement<TKey, TValue>>));
            List<DictionaryElement<TKey, TValue>> list = (List<DictionaryElement<TKey, TValue>>)serializer.
                Deserialize(reader);
            foreach (DictionaryElement<TKey, TValue> entry in list)
            {
                GetElementByKey(entry.Key).Value = entry.Value;
            }
        }

        public void Clear()
        {
            Dictionary.Clear();
        }
    }
}