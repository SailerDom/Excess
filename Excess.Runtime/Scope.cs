﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excess.Runtime
{
    //very prototype
    public sealed class __Scope
    {
        IInstantiator _instantiator;
        __Scope _parent;
        Dictionary<string, object> _bindings = new Dictionary<string, object>();
        public __Scope(__Scope parent)
        {
            _parent = parent;
        }

        private string key<T>() => typeof(T).GetHashCode().ToString();

        public T get<T>() where T : class
        {
            return (T)get(key<T>());
        }

        public T get<T>(string id) where T : class
        {
            var result = (T)get(id);
            if (result == default(T))
                result = (T)get(key<T>());

            if (result == default(T))
            {
                var instantiator = getInstantiator();
                if (instantiator != null)
                    return (T)instantiator.Create(typeof(T));
            }
            return result;
        }

        public object get(string key)
        { 
            object result;
            if (_bindings.TryGetValue(key, out result))
                return result;

            return _parent?.get(key);
        }


        public T set<T>()
        {
            if (_instantiator == null)
            {
                _instantiator = get<IInstantiator>();
                if (_instantiator == null)
                    throw new InvalidOperationException("no instantiator registered");
            }

            var value = (T)_instantiator?.Create(typeof(T));
            if (value == null)
                throw new InvalidOperationException($"type not instantiable: {typeof(T)}");

            set(key<T>(), value);
            return value;
        }

        public void set<T>(T value)
        {
            if (value == null)
                throw new InvalidOperationException($"type not instantiable: {typeof(T)}");

            set(key<T>(), value);
        }

        void set(string key, object value)
        {
            if (_bindings.ContainsKey(key))
                throw new InvalidOperationException($"duplicate binding: {value?.GetType().Name}");

            _bindings[key] = value;
        }

        private IInstantiator getInstantiator()
        {
            if (_instantiator == null)
                _instantiator = get<IInstantiator>();

            return _instantiator;
        }
    }
}
