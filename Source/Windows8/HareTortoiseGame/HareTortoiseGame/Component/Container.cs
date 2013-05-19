using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HareTortoiseGame.State;
using HareTortoiseGame.Manager;

namespace HareTortoiseGame.Component
{
    public class Container : GraphComponent
    {
        #region Field

        List<BasicComponent> _contain;
        
        #endregion

        #region Constructor

        public Container(Game game, Texture2D background, DrawState state)
            : base(game, background,  state)
        {
            _contain = new List<BasicComponent>();
        }
        #endregion

        #region Method
        public override void Update(GameTime gameTime)
        {
            foreach (var bc in _contain)
            {
                bc.PreviousBounds =
                    new Viewport((int)(PreviousBounds.X + PreviousBounds.Width * _state.CurrentState.Bounds.X),
                        (int)(PreviousBounds.Y + PreviousBounds.Height * _state.CurrentState.Bounds.Y),
                        (int)(PreviousBounds.Width * _state.CurrentState.Bounds.Z),
                        (int)(PreviousBounds.Height * _state.CurrentState.Bounds.W));
            }
            base.Update(gameTime);
        }

        public void AddComponent(BasicComponent component)
        {
            _contain.Add(component);
            component.PreviousBounds =
                new Viewport((int)(PreviousBounds.X + PreviousBounds.Width * _state.CurrentState.Bounds.X),
                    (int)(PreviousBounds.Y + PreviousBounds.Height * _state.CurrentState.Bounds.Y),
                    (int)(PreviousBounds.Width * _state.CurrentState.Bounds.Z),
                    (int)(PreviousBounds.Height * _state.CurrentState.Bounds.W));
        }

        public void RemoveComponent(BasicComponent component)
        {
            _contain.Remove(component);
        }

        public override void Start()
        {
            base.Start();
            foreach (var bc in _contain)
            {
                bc.Start();
            }
        }

        public override void End()
        {
            base.End();
            foreach (var bc in _contain)
            {
                bc.End();
            }
        }

        #endregion
    }
}
