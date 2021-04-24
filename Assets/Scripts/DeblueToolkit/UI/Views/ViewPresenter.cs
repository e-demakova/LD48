using System;
using UnityEngine;

using Deblue.Models;

namespace Deblue.UiView
{
    public interface IViewPresenter
    {
        Type ModelType { get; }

        void Init(IModel model);

        void Show();
        void Hide();
    }

    public abstract class BaseViewPresenter : MonoBehaviour, IViewPresenter
    {
        public abstract Type ModelType { get; }

        public abstract void Init(IModel model);

        public abstract void Hide();

        public abstract void Show();
    }

    public abstract class ViewPresenter<T> : BaseViewPresenter where T : IModel
    {
        protected T _model;
        public override sealed Type ModelType => typeof(T);

        public override sealed void Init(IModel model)
        {
            if (model.GetType() != typeof(T))
            {
                throw new ArgumentException($"Unexcpected mode type {model.GetType().FullName}. Expecting {typeof(T).FullName}.");
            }
            _model = (T)model;

            Init();
        }

        public override void Hide()
        {
            throw new NotImplementedException($"Hiding for {GetType()} not implemented.");
        }

        public override void Show()
        {
            throw new NotImplementedException($"Showing for {GetType()} not implemented.");
        }

        public abstract void DeInit();

        protected abstract void Init();
    }
}
