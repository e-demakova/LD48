using System;
using UnityEngine;

using Deblue.Models;

namespace Deblue.UiViews
{
    public interface IView
    {
        void Show();
        void Hide();
    }

    public interface IModelView : IView
    {
        Type ModelType { get; }

        void Init(IModel model);
    }

    public abstract class View : MonoBehaviour, IView
    {
        public abstract void Hide();
        public abstract void Show();
    }

    public abstract class ModelView<T> : MonoBehaviour, IModelView where T : IModel
    {
        protected T _model;
        public Type ModelType => typeof(T);

        public void Init(IModel model)
        {
            if (model.GetType() != typeof(T))
            {
                throw new ArgumentException($"Unexcpected mode type {model.GetType().FullName}. Expecting {typeof(T).FullName}.");
            }
            _model = (T)model;

            MyInit();
        }

        public abstract void Hide();

        public abstract void Show();

        public abstract void DeInit();

        protected abstract void MyInit();
    }
}
