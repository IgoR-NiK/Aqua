﻿using Aqua.Core.Utils;
using NUnit.Framework;

namespace Aqua.Core.Tests
{
    [TestFixture]
    public class RaisableObjectTest
    {
        [Test]
        public void SimpleTest()
        {
            var dog = new Dog();

            dog.Name = "Rocky";
            
            Assert.AreEqual("Rocky", dog.Name);
        }

        [Test]
        public void OnValueChangedTest()
        {
            var dog = new Dog();

            dog.Age = 7;
            
            Assert.AreEqual("I am 7 old", dog.Name);
        }

        [Test]
        public void RaisePropertyChangedTest()
        {
            var x = 0;
            
            var dog = new Dog();
            dog.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Dog.Age))
                    x = (sender as Dog)?.Age ?? default;
            };

            dog.Age = 7;
            
            Assert.AreEqual(7, x);
        }
        
        private class Dog : RaisableObject
        {
            private string _name;
            public string Name
            {
                get => _name;
                set => SetProperty(ref _name, value);
            }

            private int _age;
            public int Age
            {
                get => _age;
                set => SetProperty(ref _age, value, it => Name = $"I am {it} old");
            }
        }
    }
}