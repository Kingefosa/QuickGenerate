using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using QuickGenerate.Implementation;
using QuickGenerate.Reflect;

namespace QuickGenerate.Modifying
{
    public class DomainModifier<T>
    {
        private readonly T value;
        private readonly DomainGenerator generator;

        public DomainModifier(T value, DomainGenerator generator)
        {
            this.value = value;
            this.generator = generator;
        }

        // return value means does not need to be changed
        private bool ChangeProperty(PropertyInfo propertyInfo)
        {
            if (!generator.IsWritable(propertyInfo))
                return false;
            if (generator.NeedsToBeIgnored(propertyInfo))
                return false;
            if (generator.IsSimpleProperty(value, propertyInfo))
                return true;
            return false;
        }

        private bool TryChangeProperty(PropertyInfo propertyInfo)
        {
            var before = propertyInfo.GetValue(value, null);
            if (ChangeProperty(propertyInfo))
            {
                var after = propertyInfo.GetValue(value, null);
                if (before == null)
                {
                    if (after != null)
                        return true;
                }
                else if (!before.Equals(after))
                    return true;
                return false;
            }
            return true;
        }

        private void TryChangePropertyFiftyTimes(PropertyInfo propertyInfo)
        {
            for (int i = 0; i < 50; i++)
            {
                if (TryChangeProperty(propertyInfo)) break;
                if (i == 49) throw new HeyITriedFiftyTimesButCouldNotGetADifferentValue(string.Format("{0}.{1}", propertyInfo.DeclaringType.Name, propertyInfo.Name));
            }
        }

        public DomainModifier<T> ChangeAll()
        {
            foreach (var propertyInfo in typeof(T).GetProperties(MyBinding.Flags))
            {
                TryChangePropertyFiftyTimes(propertyInfo);
            }
            return this;
        }

        public DomainModifier<T> Change<TProperty>(Expression<Func<T, TProperty>> propertyFunc)
        {
            var property = propertyFunc.AsPropertyInfo();
            TryChangePropertyFiftyTimes(property);
            return this;
        }
    }
}