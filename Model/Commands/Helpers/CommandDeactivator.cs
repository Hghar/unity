using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Battle;
using Cysharp.Threading.Tasks;
using Model.Commands.Parts;
using Model.Commands.Types;
using Units;
using UnityEngine;

namespace Model.Commands.Helpers
{
    public class CommandDeactivator : IDisposable
    {
        private readonly List<ICommand> _commands = new();
        private readonly IBattleFinishPublisher _finishPublisher;
        private readonly Dictionary<
            KeyValuePair<IMinion, CommandClass>, 
            KeyValuePair<ICommand, CancellationTokenSource>> 
            _commands1 = new();
        private CancellationTokenSource _cancellationToken = new();

        public CommandDeactivator(IBattleFinishPublisher finishPublisher)
        {
            _finishPublisher = finishPublisher;
            _finishPublisher.BattleFinished += ClearCommands;
        }

        private void ClearCommands()
        {
            foreach (var command in _commands)
            {
                command.Undo();
            }
            _commands.Clear();
            _cancellationToken.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }

        public void Add(ICommand command, CommandClass commandClass,  IMinion caster = null)
        {
            if(_commands.Contains(command))
                return;
            _commands.Add(command);
            
            var internalToken = new CancellationTokenSource();
            var token = CancellationTokenSource.CreateLinkedTokenSource(_cancellationToken.Token, internalToken.Token);
            var key = new KeyValuePair<IMinion, CommandClass>(caster, commandClass);
            _commands1[key] = new KeyValuePair<ICommand, CancellationTokenSource>(command, internalToken);
            
            WaitCanceling(command, key, token);
        }

        public void UpdateCommand(CommandClass commandClass,  IMinion caster = null)
        {
            var key = new KeyValuePair<IMinion, CommandClass>(caster, commandClass);
            if (_commands1.ContainsKey(key))
            {
                var command = _commands1[key].Key;
                _commands1[key].Value.Cancel();

                var internalToken = new CancellationTokenSource();
                var token = CancellationTokenSource.CreateLinkedTokenSource(_cancellationToken.Token, internalToken.Token);
                _commands1[key] = new KeyValuePair<ICommand, CancellationTokenSource>(command, internalToken);
                    
                WaitCanceling(command, key, token);
            }
        }

        private async void WaitCanceling(ICommand command, KeyValuePair<IMinion, CommandClass> key,
            CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                Debug.Log($"start waiting {command.Duration}");
                await Task.Delay(TimeSpan.FromSeconds(command.Duration), cancellationToken: cancellationTokenSource.Token);
                Debug.Log($"time ended {command.Duration}");
            }
            catch (OverflowException e)
            {
                while (cancellationTokenSource.IsCancellationRequested == false)
                {
                    await Task.Yield();
                }

                Debug.Log($"infinity time ended {command.Duration}");
            }
            catch (TaskCanceledException e)
            {
                Debug.LogWarning(e);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                if (cancellationTokenSource.IsCancellationRequested == false)
                {
                    command.Undo();
                    _commands.Remove(command);
                    Debug.Log($"undo command {command.Duration}");
                }

                _commands1.Remove(key);
            }
        }
        
        public void Dispose()
        {
            _finishPublisher.BattleFinished -= ClearCommands;
            _commands.Clear();
            _cancellationToken.Cancel();
        }

        public bool HasWorking(CommandClass commandClass, IMinion caster)
        {
            if (commandClass.Duration == float.MaxValue)
                return false;
            
            return _commands1.ContainsKey(new KeyValuePair<IMinion, CommandClass>(caster, commandClass));
        }
    }
}