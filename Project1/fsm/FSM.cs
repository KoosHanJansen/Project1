using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Project1
{
    class FSM<T> where T : AbstractObject
    {
        private Dictionary<Type, AbstractState<T>> stateCache = new Dictionary<Type, AbstractState<T>>();
        private AbstractState<T> currentState;
        private T target;

        public FSM(T target)
        {
            this.target = target;
        }

        public void ChangeState<U>() where U : AbstractState<T>
        {
            if (!stateCache.ContainsKey(typeof(U)))
            {
                U state = Activator.CreateInstance(typeof(U), target) as U;
                stateCache[typeof(U)] = state;
            }

            AbstractState<T> newState = stateCache[typeof(U)];

        }

        private void changeState(AbstractState<T> newState)
        {
            if (currentState == newState) return;

            if (currentState != null) currentState.Exit();

            currentState = newState;
            if (currentState != null) currentState.Enter();
        }

        public void Update(GameTime gameTime)
        {
            if (currentState != null)
                currentState.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentState != null)
                currentState.Draw(spriteBatch);
        }

        public AbstractState<T> GetCurrentState()
        {
            return currentState;
        }
    }
}
