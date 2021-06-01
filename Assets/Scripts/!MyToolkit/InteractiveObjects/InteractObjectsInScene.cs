using UnityEngine;

using System.Collections.Generic;

using Deblue.InputSystem;
using Deblue.ObservingSystem;

namespace Deblue.Interactive
{
    public interface IObjectTaker
    {
        bool IsCanTakeObject { get; }
        string TakenObject { get; }

        bool TryGetObject(string objId, out TakebleObject obj);
        bool IsContainObject(string objId);
        bool TryPutObject();
        bool TryTakeObject(TakebleObject obj);
        TakebleObject ReturnObject();
    }

    public class InteractObjectsInScene : UniqMono<InteractObjectsInScene>
    {
        private List<InteractObject> _objects = new List<InteractObject>(30);
        private List<IObserver>      _observers = new List<IObserver>(30);
        private IInteractObject      _highlightedObj;
        private IObjectTaker         _taker;

        protected override void MyAwake()
        {
            _objects.AddRange(FindObjectsOfType<InteractObject>());
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].Updated.Subscribe(OnUpdate, _observers);
            }
            InputReciver.SubscribeOnInput<On_Button_Down>(Interact, KeyCode.F, _observers);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _observers.Count; i++)
            {
                _observers[i].Dispose();
            }
            _observers.Clear();
        }

        public void Init(IObjectTaker taker)
        {
            _taker = taker;
        }

        private void OnUpdate(Interact_Object_Updated context)
        {
            TryHilight();
        }

        private void TryHilight()
        {
            if (!TryHighlightTakebeleObjects())
            {
                if (!TryHighlightReactionObjects())
                {
                    if (!TryHighlightObjectsContainers())
                    {
                        _highlightedObj = null;
                    }
                }
            }
        }

        private bool TryHighlightTakebeleObjects()
        {
            TakebleObject obj;
            for (int i = 0; i < _objects.Count; i++)
            {
                obj = _objects[i] as TakebleObject;
                if (TryHighlightObject(obj))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TryHighlightReactionObjects()
        {
            IReactionObject obj;
            for (int i = 0; i < _objects.Count; i++)
            {
                obj = _objects[i] as IReactionObject;
                if (TryHighlightObject(obj))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TryHighlightObjectsContainers()
        {
            TakebleObjectContainer obj;
            for (int i = 0; i < _objects.Count; i++)
            {
                obj = _objects[i] as TakebleObjectContainer;
                if (TryHighlightObject(obj))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TryHighlightObject(IInteractObject obj)
        {
            if (obj != null)
            {
                if (obj.TryHighlight(_taker))
                {
                    _highlightedObj = obj;
                    return true;
                }
            }
            return false;
        }

        private void Interact(On_Button_Down context)
        {
            if (_highlightedObj == null)
            {
                _taker.TryPutObject();
                return;
            }
            if (!TryInteractWithTakebleObject())
            {
                if (!TryInteractWithReactObject())
                {
                    TryInteractWithObjectContainer();
                }
            }
        }

        private bool TryInteractWithTakebleObject()
        {
            var takebleObj = _highlightedObj as StandartTakebleObject;
            if (takebleObj != null)
            {
                if (_taker.TryTakeObject(takebleObj))
                {
                    takebleObj.Take();
                    return true;
                }
            }
            return false;
        }

        private bool TryInteractWithObjectContainer()
        {
            TakebleObject obj;
            var objContainer = _highlightedObj as TakebleObjectContainer;
            if (objContainer != null)
            {
                if (objContainer.CanReturn(_taker.TakenObject))
                {
                    obj = _taker.ReturnObject();
                    objContainer.Return(obj);
                    obj.Put();
                    return true;
                }
                if (_taker.IsCanTakeObject && objContainer.TryTake(_taker, out obj))
                {
                    if (_taker.TryTakeObject(obj))
                    {
                        obj.Take();

                        if (!_objects.Contains(obj))
                        {
                            _objects.Add(obj);
                            obj.Updated.Subscribe(OnUpdate, _observers);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TryInteractWithReactObject()
        {
            var reactObj = _highlightedObj as IReactionObject;
            if (reactObj != null)
            {
                if (reactObj.TryReact(_taker))
                {
                    return true;
                }
            }
            return false;
        }
    }
}