using UnityEngine;

using Deblue.ObservingSystem;
using System.Collections;

namespace Deblue.LD48
{
    public class GameProcessDescription : UniqMono<GameProcessDescription>
    {
        [SerializeField] private Trigger       _insideTrigget;
        [SerializeField] private Trigger       _outsideTrigget;
        [SerializeField] private TomatoPlant[] _tomatoesPlants;
        [SerializeField] private TomatoBox     _tomatoBox;
        [SerializeField] private TeaStation    _teaStantion;
        [SerializeField] private OldMan        _oldMan;
        [SerializeField] private LD48GameModel     _game;

        private int[] _tomatoesStatesCounters = new int[4];

        protected override void MyAwake()
        {
            for (int i = 0; i < _tomatoesPlants.Length; i++)
            {
                _tomatoesPlants[i].TomatoGrown.Subscribe(RegistrGrownTomato);
            }
            _tomatoBox.BoxFull.Subscribe(RegistrBoxFulled);
            _teaStantion.CupTaken.Subscribe(RegistrTekenCup);
            _oldMan.CurrentDialog.SubscribeOnChanging(RegistrNewDilaog);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _tomatoesPlants.Length; i++)
            {
                _tomatoesPlants[i].TomatoGrown.Unsubscribe(RegistrGrownTomato);
            }
            _tomatoBox.BoxFull.Unsubscribe(RegistrBoxFulled);
            _oldMan.CurrentDialog.UnsubscribeOnChanging(RegistrNewDilaog);
            _teaStantion.CupTaken.Unsubscribe(RegistrTekenCup);
        }

        private void RegistrBoxFulled(Box_Full context)
        {
            _tomatoBox.BoxFull.Unsubscribe(RegistrBoxFulled);
            _oldMan.UnlockDialog();
            _game.ChangeStateToNext();
        }

        private void RegistrGrownTomato(Tomato_Start_Grow context)
        {
            _tomatoesStatesCounters[context.State]++;
            if (_tomatoesStatesCounters[context.State] == _tomatoesPlants.Length)
            {
                _oldMan.UnlockDialog();
                if (context.State == 2)
                {
                    _game.ChangeStateToNext();
                }
            }
        }

        private void RegistrNewDilaog(Limited_Property_Changed<int> context)
        {
            switch (context.NewValue)
            {
                case 4:
                    _outsideTrigget.PlayerInTrigger.Subscribe(RegistrOutsideTrigger);
                    break;

                case 5:
                    _teaStantion.IsAvalible = true;
                    break;

                case 6:
                    _game.ChangeStateToNext();
                    _insideTrigget.PlayerInTrigger.Subscribe(RegistrInsideTrigger);
                    break;
                
                case 7:
                    StartCoroutine(Final());
                    break;
            }
        }

        private void RegistrInsideTrigger(Player_Trigger context)
        {
            _insideTrigget.PlayerInTrigger.Unsubscribe(RegistrInsideTrigger);
            _oldMan.UnlockDialog();
        }
        
        private void RegistrOutsideTrigger(Player_Trigger context)
        {
            _outsideTrigget.PlayerInTrigger.Unsubscribe(RegistrOutsideTrigger);
            _oldMan.UnlockDialog();
        }

        private void RegistrTekenCup(Cup_Taken context)
        {
            _teaStantion.CupTaken.Unsubscribe(RegistrTekenCup);
            _oldMan.UnlockDialog();
        }

        private IEnumerator Final()
        {
            var startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < startTime + 32f)
            {
                yield return new WaitForFixedUpdate();
            }
            _game.ChangeStateToNext();
        }
    }
}