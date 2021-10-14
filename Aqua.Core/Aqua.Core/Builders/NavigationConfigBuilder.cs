using System;
using Aqua.Core.Configs;
using Aqua.Core.Services;

namespace Aqua.Core.Builders
{
    public sealed class NavigationConfigBuilder : IBuilder<NavigationConfig, NavigationConfigBuilder>
    {
        public NavigationConfig Instance { get; private set; }

        public NavigationConfigBuilder()
        {
        }

        public NavigationConfigBuilder(Action<NavigationConfigBuilder> action)
            : this(new NavigationConfig(), action)
        {
        }

        public NavigationConfigBuilder(
            NavigationConfig defaultConfig,
            Action<NavigationConfigBuilder> action)
        {
            Instance = defaultConfig;
            action?.Invoke(this);
        }

        public NavigationConfigBuilder Clear()
        {
            Instance = new NavigationConfig();
            return this;
        }

        public NavigationConfigBuilder In<TStack>()
            where TStack : IStack
        {
            Instance.StackType = typeof(TStack);
            return this;
        }

        public NavigationConfigBuilder WithAnimation(bool withAnimation = true)
        {
            Instance.WithAnimation = withAnimation;
            return this;
        }
    }
}