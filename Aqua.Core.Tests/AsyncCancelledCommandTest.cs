using System;
using System.Collections.Generic;
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
            var logger = new Logger();
            var command = new AsyncCancelledCommand(Execute, Cancelled);

            // Команда выполняется 10 секунд и что-то асинхронно делает
            // По окончании пишем в логгер об успехе
            Task Execute(CancellationToken token)
                => Task.Run(async () =>
                {
                    for (var i = 0; i < 10; i++)
                        await Task.Delay(1000, token);

                    logger.Log("Command completed successfully");
                }, token);

            // Пишем в логгер, если команда отменилась
            Task Cancelled(OperationCanceledException exception)
                => Task.Run(() => logger.Log("Command canceled"));

            // Запускаем команду
            command.ExecuteAsync();
            
            // Ждем 4 секунды
            Thread.Sleep(4000);
            
            // Недождались, отменяем
            command.Cancel();
            
            // Ждем еще несколько секунд, чтобы успело сгенерироваться и обработаться исключение
            Thread.Sleep(2000);
            
            Assert.AreEqual(1, logger.Logs.Count);
            Assert.AreEqual("Command canceled", logger.Logs[0]);
        }
        
        [Test]
        public async Task TimeoutCrashCancelCommandTest()
        {
            var logger = new Logger();
            
            // Даем команде 4 секунды на выполнение
            var command = new AsyncCancelledCommand(Execute, Cancelled).WithTimeout(4000);

            // Команда выполняется 10 секунд и что-то асинхронно делает
            // По окончании пишем в логгер об успехе
            Task Execute(CancellationToken token)
                => Task.Run(async () =>
                {
                    for (var i = 0; i < 10; i++)
                        await Task.Delay(1000, token);

                    logger.Log("Command completed successfully");
                }, token);

            // Пишем в логгер, если команда отменилась
            Task Cancelled(OperationCanceledException exception)
                => Task.Run(() => logger.Log("Command canceled"));

            // Запускаем команду и ждем завершения
            await command.ExecuteAsync();
            
            Assert.AreEqual(1, logger.Logs.Count);
            Assert.AreEqual("Command canceled", logger.Logs[0]);
        }
        
        [Test]
        public async Task TimeoutSuccessCancelCommandTest()
        {
            var logger = new Logger();
            
            // Даем команде 15 секунд на выполнение
            var command = new AsyncCancelledCommand(Execute, Cancelled).WithTimeout(15000);

            // Команда выполняется 10 секунд и что-то асинхронно делает
            // По окончании пишем в логгер об успехе
            Task Execute(CancellationToken token)
                => Task.Run(async () =>
                {
                    for (var i = 0; i < 10; i++)
                        await Task.Delay(1000, token);

                    logger.Log("Command completed successfully");
                }, token);

            // Пишем в логгер, если команда отменилась
            Task Cancelled(OperationCanceledException exception)
                => Task.Run(() => logger.Log("Command canceled"));

            // Запускаем команду и ждем завершения
            await command.ExecuteAsync();
            
            Assert.AreEqual(1, logger.Logs.Count);
            Assert.AreEqual("Command completed successfully", logger.Logs[0]);
        }

        private class Logger
        {
            public List<string> Logs { get; } = new();

            public void Log(string message)
            {
                Logs.Add(message);
            }
        }
    }
}