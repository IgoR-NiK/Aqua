using System;
using DryIoc;
using NUnit.Framework;

namespace Aqua.Core.Tests
{
    [TestFixture]
    public class CreatingAndDisposingContainerDryIocTest
    {
        [Test]
        public void CreatingContainer()
        {
            var container = new Container();
            
            // Start using the container..
            
            Assert.NotNull(container);
            Assert.That(container, Is.InstanceOf<IDisposable>());
        }

        [Test]
        public void DisposingContainer()
        {
             // Disposing container will:
             // - Dispose resolved Singletons.
             // - Remove all registrations.
             // - Set Rules to Rules.Empty.
            
            Logger logger;
            using (var container = new Container())
            {
                // Logger is registered with singleton lifetime
                container.Register<Logger>(Reuse.Singleton); 
                
                logger = container.Resolve<Logger>();
            }

            Assert.IsTrue(logger.IsDisposed);
        }
        
        private class Logger : IDisposable
        {
            public bool IsDisposed { get; private set; }
            
            public void Dispose() => IsDisposed = true;
        }
    }
}