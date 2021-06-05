using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aqua.Core.Commands;
using NUnit.Framework;

namespace Aqua.Core.Tests
{
    [TestFixture]
    public class AsyncCancelledCommandTest
    {
        [Test]
        public void ManualCancelCommandTest()
        {
            var logs = new List<string>();
            var command = new AsyncCancelledCommand(Execute, Cancelled);

            // Команда выполняется 4 секунды и что-то асинхронно делает
            // По окончании пишем в логгер об успехе
            Task Execute(CancellationToken token)
                => Task.Run(async () =>
                {
                    for (var i = 0; i < 40; i++)
                        await Task.Delay(100, token);

                    logs.Add("Command completed successfully");
                }, token);

            // Пишем в логгер, если команда отменилась
            Task Cancelled(OperationCanceledException exception)
                => Task.Run(() => logs.Add("Command canceled"));

            // Запускаем команду
            command.ExecuteAsync();
            
            // Ждем 1 секунду
            Thread.Sleep(1000);
            
            // Недождались, отменяем
            command.Cancel();
            
            // Ждем еще немного, чтобы успело сгенерироваться и обработаться исключение
            Thread.Sleep(500);
            
            Assert.AreEqual(1, logs.Count);
            Assert.AreEqual("Command canceled", logs.Single());
            Assert.False(command.IsExecuting);
        }
        
        [Test]
        public async Task TimeoutCrashCancelCommandTest()
        {
            var logs = new List<string>();
            
            // Даем команде 2 секунды на выполнение
            var command = new AsyncCancelledCommand(Execute, Cancelled).WithTimeout(2000);

            // Команда выполняется 4 секунды и что-то асинхронно делает
            // По окончании пишем в логгер об успехе
            Task Execute(CancellationToken token)
                => Task.Run(async () =>
                {
                    for (var i = 0; i < 40; i++)
                        await Task.Delay(100, token);

                    logs.Add("Command completed successfully");
                }, token);

            // Пишем в логгер, если команда отменилась
            Task Cancelled(OperationCanceledException exception)
                => Task.Run(() => logs.Add("Command canceled"));

            // Запускаем команду и ждем завершения
            await command.ExecuteAsync();
            
            Assert.AreEqual(1, logs.Count);
            Assert.AreEqual("Command canceled", logs.Single());
            Assert.False(command.IsExecuting);
        }
        
        [Test]
        public async Task TimeoutSuccessCancelCommandTest()
        {
            var logs = new List<string>();
            
            // Даем команде 5 секунд на выполнение
            var command = new AsyncCancelledCommand(Execute, Cancelled).WithTimeout(5000);

            // Команда выполняется 4 секунды и что-то асинхронно делает
            // По окончании пишем в логгер об успехе
            Task Execute(CancellationToken token)
                => Task.Run(async () =>
                {
                    for (var i = 0; i < 40; i++)
                        await Task.Delay(100, token);

                    logs.Add("Command completed successfully");
                }, token);

            // Пишем в логгер, если команда отменилась
            Task Cancelled(OperationCanceledException exception)
                => Task.Run(() => logs.Add("Command canceled"));

            // Запускаем команду и ждем завершения
            await command.ExecuteAsync();
            
            Assert.AreEqual(1, logs.Count);
            Assert.AreEqual("Command completed successfully", logs.Single());
            Assert.False(command.IsExecuting);
        }
    }
}