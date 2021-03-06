﻿using System.Collections.Generic;
using Sisc.RestApi.Models;

namespace Sisc.RestApi.Services
{
    public class MockCreator : IMockCreator
    {
        private List<Value> _allValues;

        public List<Value> AllValues
        {
            get
            {
                if (_allValues == null)
                {
                    _allValues = CreateValueMocks();
                    
                }
                return _allValues;
            }
        }
        private List<Value> CreateValueMocks()
        {
            var allValues = new List<Value>();
            for (var i = 0; i < 100; i++)
            {
                allValues.Add(new Value { Id = i, Name = $"Name {i}" });
            }

            return allValues;
        }
    }
}
