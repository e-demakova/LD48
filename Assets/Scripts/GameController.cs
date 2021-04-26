using UnityEngine;

namespace Deblue.LD48
{
    public class GameController : UniqMono<GameController>
    {
        [SerializeField] private TomatoPlant[]   _tomatoesPlants;
        [SerializeField] private OldMan _oldMan;
        [SerializeField] private GameModel       _game;

        private int[] _tomatoesStatesCounters = new int[4];

        protected override void MyAwake()
        {
            for (int i = 0; i < _tomatoesPlants.Length; i++)
            {
                _tomatoesPlants[i].TomatoGrown.Subscribe(RegistrGrownTomato);
            }
        }

        private void RegistrGrownTomato(Tomato_Grown context)
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
    }
}